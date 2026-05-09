using Microsoft.Extensions.DependencyInjection;
using Pd.Gateway.Application.Domain.Services.Http;
using Pd.Gateway.Application.Integration.Tasks;
using Pd.Gateway.Application.Integration.Tasks.Services;

namespace Pd.Gateway.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            services.AddHttpClient<IHttpService, HttpService>();

            services.AddTransient<ITasksMicroserviceIntegration, TasksMicroserviceIntegration>()
                .AddTransient<IIAMIntegration, IAMIntegration>()
                .AddTransient<ITaskIntegration, TaskIntegration>()
                .AddTransient<IUserIntegration, UserIntegration>();

            return services;
        }
    }
}
