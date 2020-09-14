using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;

namespace AZV3CleanArchitecture.ApplicationInsightsInitializers
{
    public class ApplicationNameTelemetryInitializer : ITelemetryInitializer
    {
        private const string ApplicationNameConfigurationKey = "HostingEnvironment:ApplicationName";

        private const string ApplicationNameKey = "ApplicationName";

        private string applicationName = "MyService";

        public ApplicationNameTelemetryInitializer(IConfiguration configuration)
        {
            applicationName = configuration[ApplicationNameConfigurationKey];
        }

        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                return;
            }

            telemetry.Context.GlobalProperties[ApplicationNameKey] = applicationName;
        }
    }
}
