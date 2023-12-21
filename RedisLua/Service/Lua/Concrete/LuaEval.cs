using RedisLua.Service.Lua.Abstract;
using RedisLua.Service.Redis;
using StackExchange.Redis;
using System.Collections.Concurrent;

namespace RedisLua.Service.Lua.Concrete
{
    public class LuaEval : ILuaEval
    {
        private readonly ConcurrentDictionary<Enums.LuaScript, Lazy<LoadedLuaScript>> _luaScript;

        private readonly IRedisProviderService _redisService;

        public LuaEval(IRedisProviderService redisService)
        {
            _redisService = redisService;
            _luaScript = new ConcurrentDictionary<Enums.LuaScript, Lazy<LoadedLuaScript>>();
        }

        public LoadedLuaScript GetLoadedLuaScript(Enums.LuaScript luaScript)
        {
            Lazy<LoadedLuaScript> script = _luaScript.GetOrAdd(luaScript,
                l => new Lazy<LoadedLuaScript>(() => GetLoadedLuaScriptFactory(luaScript),
                LazyThreadSafetyMode.ExecutionAndPublication));

            return script.Value;
        }

        private LoadedLuaScript GetLoadedLuaScriptFactory(Enums.LuaScript luaScriptType)
        {
            string script = File.ReadAllText($@"C:\Users\Precision\Desktop\RedisLua\RedisLua\{luaScriptType}.lua");

            LuaScript luaScript = LuaScript.Prepare(script);
            return luaScript.Load(_redisService.GetServer());
        }
    }
}
