using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace taaghche.Services
{
    public class RedisConnection
    {
        static RedisConnection()
        {
            RedisConnection.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect("redis");
            });
        }

        private static readonly Lazy<ConnectionMultiplexer> lazyConnection;

        public ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
