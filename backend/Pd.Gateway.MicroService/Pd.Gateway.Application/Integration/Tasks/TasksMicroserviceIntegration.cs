using Pd.Gateway.Application.Integration.Tasks.Services;

namespace Pd.Gateway.Application.Integration.Tasks
{
    public class TasksMicroserviceIntegration : ITasksMicroserviceIntegration
    {
        public TasksMicroserviceIntegration(IIAMIntegration iam, ITaskIntegration tasks, IUserIntegration users)
        {
            IAM = iam;
            Tasks = tasks;
            Users = users;
        }

        public IIAMIntegration IAM { get; }
        public ITaskIntegration Tasks { get; }
        public IUserIntegration Users { get; }
    }
}
