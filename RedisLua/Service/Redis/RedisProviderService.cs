using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace RedisLua.Service.Redis
{
    public class RedisProviderService : IRedisProviderService
    {
        private readonly Lazy<ConnectionMultiplexer> _connection;

        IConfigurationBuilder builder = new ConfigurationBuilder()
                     .SetBasePath("C:\\Users\\Precision\\Desktop\\RedisLua\\RedisLua\\")
                     .AddJsonFile("config.json", false);

        public RedisProviderService()
        {
            _connection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.ConnectAsync(RedisConnectionString()).GetAwaiter().GetResult();
            });
        }

        public IDatabase GetDataBase(int database = 0)
        {
            return _connection.Value.GetDatabase(database);
        }

        public IServer GetServer()
        {
            return _connection.Value.GetServer(RedisConnectionString());
        }

        private string RedisConnectionString()
        {
            IConfiguration config = builder.Build();

            string cnnString = config.GetSection("RedisConnection:Default").Value ?? throw new ArgumentNullException("Oops!!!");

            return cnnString;
        }
    }
}
