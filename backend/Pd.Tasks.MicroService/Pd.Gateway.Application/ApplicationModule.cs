using Microsoft.Extensions.DependencyInjection;
using Pd.Tasks.Application.Features.IAM.Services;
using Pd.Tasks.Application.Features.TaskManagement.Services;
using Pd.Tasks.Application.Features.UsersManagement.Services;

namespace Pd.Tasks.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            services.AddScoped<ITaskManagementService, TaskManagementService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            return services;
        }
    }
}
