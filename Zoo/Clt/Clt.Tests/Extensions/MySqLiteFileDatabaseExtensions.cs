using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace Clt.Tests.Extensions
{
    public class MySqLiteFileDatabaseExtensions
    {
        public static TContext Create<TContext>(Func<DbContextOptions<TContext>, TContext> createFunc, string dbName) where TContext : DbContext
        {
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            optionsBuilder.UseSqlite($"Data Source={dbName}.db", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });

            return createFunc(optionsBuilder.Options);
        }
    }
}
