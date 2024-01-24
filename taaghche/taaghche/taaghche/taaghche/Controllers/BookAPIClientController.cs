using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using taaghche.Models;
using taaghche.RabitMQ;
using taaghche.Services;

namespace taaghche.Controllers
{
    [ApiController]
    [Route("api/book/[controller]")]
    public class BookAPIClientController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IRabitMQCunsomer _rabbitMQCunsomer;
        public BookAPIClientController(IBookService bookService ,IRabitMQCunsomer rabitMQ)
        {
            _bookService = bookService;
            _rabbitMQCunsomer = rabitMQ;
        }

        [HttpGet]
        public async Task<string> GetBook(int bookId)
        {
            return await _bookService.GetBookAsync(bookId);            
        }
        [HttpPost]
        public void BookHasChanged(int bookId)
        {
            //send the message to the queue and consumer will listening this data from queue
            //var message = $"book with id: {bookId} has changed.";
            _rabbitMQCunsomer.SendMessage(JsonConvert.SerializeObject(new BookMessage{BookId = bookId,Message= "Remove from cache" }));
        }
    }
}
