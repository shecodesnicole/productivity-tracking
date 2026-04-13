using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pd.Tasks.Application.Domain.Persistence;
using Pd.Tasks.Application.Features.TaskManagement.Persistence;
using Pd.Tasks.Persistence.Accessors;

namespace Pd.Tasks.Persistence
{
    public static class PersistenceModule
    {
        public static IServiceCollection AddPersistenceModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<LocalDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DbConnection"), npgsqlOptions =>
                {
                //options.UseNpgsql(configuration["DbConnection"], npgsqlOptions =>
                
                    npgsqlOptions.EnableRetryOnFailure();
                });
            });

            return services;
        }
    }
}
  