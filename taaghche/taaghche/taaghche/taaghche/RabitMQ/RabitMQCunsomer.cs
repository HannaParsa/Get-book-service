using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;
using taaghche.Models;
using taaghche.Services;

namespace taaghche.RabitMQ
{
    public class RabitMQCunsomer : IRabitMQCunsomer
    {
        IModel channel;
        string queueName = "book";
        private readonly IBookService _bookService;
        public RabitMQCunsomer(IBookService bookService)
        {
            _bookService = bookService;
            //Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
            var factory = new ConnectionFactory
            {
                //HostName = "localhost"
                HostName = "rabbitmq"
            };
            //Create the RabbitMQ connection using connection factory details as i mentioned above
            var connection = factory.CreateConnection();
            //Here we create channel with session and model
            channel = connection.CreateModel();
            //declare the queue after mentioning name and a few property related to that
            channel.QueueDeclare(queueName, exclusive: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (ch, ea) => 
            {
                var body = ea.Body.ToArray();
                // copy or deserialise the payload
                // and process the message
                // ...
                var json = new string(body.Select(x=>(char)x).ToArray());
                var message = JsonConvert.DeserializeObject<BookMessage>(json);
                await DeleteFromCacheAsync(message.BookId);
                channel.BasicAck(ea.DeliveryTag, false);
                
            };
            // this consumer tag identifies the subscription
            // when it has to be cancelled
            string consumerTag = channel.BasicConsume(queueName, false, consumer);
        }
        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            //put the data on to the product queue
            channel.BasicPublish(exchange: "", routingKey: "book", body: body);
            //delete book from redis and memory:
        }
        public async Task DeleteFromCacheAsync(int bookId)
        {
            await _bookService.DeleteFromCacheAsync(bookId);
        }
    }
}
