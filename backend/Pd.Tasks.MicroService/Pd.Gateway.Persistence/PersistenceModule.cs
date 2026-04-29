using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pd.Tasks.Application.Domain.Persistence;
using Pd.Tasks.Application.Features.IAM.Persistance;
using Pd.Tasks.Application.Features.TaskManagement.Persistence;
using Pd.Tasks.Application.Features.UsersManagement.Persistence;
using Pd.Tasks.Persistence.Accessors;

namespace Pd.Tasks.Persistence
{
    public static class PersistenceModule
    {
        public static IServiceCollection AddPersistenceModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthenticationTokensRepository, AuthenticationTokensRepository>();

            services.AddDbContext<LocalDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DbConnection"), npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure();
                });
            });

            return services;
        }
    }
}
  