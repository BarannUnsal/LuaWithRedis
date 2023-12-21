using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using RedisLua.Infrastructure.Extensions;
using RedisLua.Models;
using RedisLua.Service.Lua.Concrete;
using RedisLua.Service.Redis;
using StackExchange.Redis;

namespace RedisLua.Infrastructure.Lua
{
    public class SortedListHelper
    {
        public void SetThousandDataSortedSet()
        {
            RedisProviderService redisProviderService = new RedisProviderService();

            IDatabase redis = redisProviderService.GetDataBase((int)Enums.RedisDatabaseType.Ranked);

            for (int i = 0; i < ConstantValues.TenThousand; i++)
            {
                redis.SortedSetAdd(ConstantValues.RedisKey, ConstantValues.MemberName.Replace("{i}", i.ToString()), i, CommandFlags.FireAndForget);
            }
        }

        [Benchmark]
        public List<GetSortedSetWithLuaResponse> GetSortedSetWithLua()
        {
            RedisProviderService redisProviderService = new RedisProviderService();

            LuaEval eval = new LuaEval(redisProviderService);

            LoadedLuaScript luaScript = eval.GetLoadedLuaScript(Enums.LuaScript.GetSortedKey);

            IDatabase redis = redisProviderService.GetDataBase((int)Enums.RedisDatabaseType.Ranked);

            RedisResult result = redis.ScriptEvaluate(luaScript, new
            {
                key1 = ConstantValues.RedisKey,
                key2 = (int)Enums.RedisDatabaseType.Ranked
            });

            List<GetSortedSetWithLuaResponse>? responses = result.IsNull ?
                Enumerable.Empty<GetSortedSetWithLuaResponse>().ToList() :
                JsonConvert.DeserializeObject<List<GetSortedSetWithLuaResponse>>(result.ToString().GetValidateJsonResult());

            return responses;
        }
    }
}
