using Microsoft.Extensions.DependencyInjection;
using Pd.Tasks.Application.Features.TaskManagement.Services;

namespace Pd.Tasks.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            services.AddScoped<ITaskManagementService, TaskManagementService>();
            return services;
        }
    }
}
