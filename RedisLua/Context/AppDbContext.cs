using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RedisLua.Context.Entity;

namespace RedisLua.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<SortedSetWithLua> SortedSetWithLuas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath("C:\\Users\\Precision\\Desktop\\RedisLua\\RedisLua\\")
                   .AddJsonFile("config.json")
                   .Build();

                string? connectionString = configuration.GetSection("ConnectionString:Default").Value ?? throw new NullReferenceException("Oops!!!");
                optionsBuilder.UseNpgsql(connectionString, opt =>
                {
                    opt.EnableRetryOnFailure();
                });
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
