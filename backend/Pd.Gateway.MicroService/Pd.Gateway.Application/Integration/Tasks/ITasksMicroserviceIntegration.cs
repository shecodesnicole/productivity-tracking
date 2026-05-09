using Pd.Gateway.Application.Integration.Tasks.Services;

namespace Pd.Gateway.Application.Integration.Tasks
{
    public interface ITasksMicroserviceIntegration
    {
        IIAMIntegration IAM { get; }
        ITaskIntegration Tasks { get; }
        IUserIntegration Users { get; }
    }
}
