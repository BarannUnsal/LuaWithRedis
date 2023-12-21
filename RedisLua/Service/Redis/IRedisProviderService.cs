using StackExchange.Redis;

namespace RedisLua.Service.Redis
{
    public interface IRedisProviderService
    {
        IServer GetServer();

        IDatabase GetDataBase(int database = 0);
    }
}
