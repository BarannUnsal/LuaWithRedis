using BenchmarkDotNet.Attributes;
using RedisLua.Context;
using RedisLua.Context.Entity;
using RedisLua.Models;
using System.Data.Entity;

namespace RedisLua.Infrastructure.Sql
{
    public class SqlSortedListHelper
    {
        public void AddDb()
        {
            AppDbContext appDbContext = new();

            for (int i = 0; i < ConstantValues.TenThousand; i++)
            {
                appDbContext.SortedSetWithLuas.Add(new SortedSetWithLua
                {
                    Member = ConstantValues.MemberName.Replace("{i}", i.ToString()),
                    Score = i.ToString()
                });

                if (i % 500 == 0 && i != 0)
                    appDbContext.SaveChanges();
            }
        }

        [Benchmark]
        public IEnumerable<GetSortedSetWithLuaResponse> SortedListFromDb()
        {
            AppDbContext appDbContext = new();

            return appDbContext
                .SortedSetWithLuas
                .OrderBy(o => o.Score)
                .Take(10)
                .Select(s => new GetSortedSetWithLuaResponse
                {
                    Member = s.Member,
                    Score = s.Score
                });
        }
    }
}
