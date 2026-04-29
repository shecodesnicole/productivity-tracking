namespace Pd.Gateway.Application.Domain.Settings
{
    public class IntegrationSettings
    {
        public const string SectionName = nameof(IntegrationSettings);
        public string TasksUrl { get; set; } = string.Empty;
    }
}
