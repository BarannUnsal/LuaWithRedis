using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedisLua.Context;
using RedisLua.Infrastructure.Lua;
using RedisLua.Infrastructure.Sql;
using RedisLua.Service.Lua.Abstract;
using RedisLua.Service.Lua.Concrete;
using RedisLua.Service.Redis;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((collection, services) =>
    {
        services.AddDbContext<AppDbContext>();
        services.AddSingleton<IRedisProviderService, RedisProviderService>();
        services.AddSingleton<ILuaEval, LuaEval>();
        services.AddSingleton<SortedListHelper>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    })
    .Build();


using (IServiceScope scope = host.Services.CreateScope())
{
    AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (dbContext.Database.GetMigrations().Any())
    {
        dbContext.Database.Migrate();
    }
}

Summary? sortedListResult = BenchmarkRunner.Run<SortedListHelper>();

Summary? sqlListResult = BenchmarkRunner.Run<SqlSortedListHelper>();