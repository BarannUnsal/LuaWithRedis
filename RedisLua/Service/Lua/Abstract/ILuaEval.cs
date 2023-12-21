using StackExchange.Redis;

namespace RedisLua.Service.Lua.Abstract
{
    public interface ILuaEval
    {
        LoadedLuaScript GetLoadedLuaScript(Enums.LuaScript luaScript);
    }
}
