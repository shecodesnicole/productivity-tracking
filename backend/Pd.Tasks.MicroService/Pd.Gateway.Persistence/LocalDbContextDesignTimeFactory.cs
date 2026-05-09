using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Pd.Tasks.Persistence
{
    public class LocalDbContextDesignTimeFactory : IDesignTimeDbContextFactory<LocalDbContext>
    {
        public LocalDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LocalDbContext>();
            
            // Prefer an environment-driven connection string when available.
            // Host localhost:5432 is mapped to your running postgres container.
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
                ?? "Host=localhost;Port=5432;Database=dt_tasks;Username=postgres;Password=postgres";

            optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure();
            });

            return new LocalDbContext(optionsBuilder.Options);
        }
    }
}
