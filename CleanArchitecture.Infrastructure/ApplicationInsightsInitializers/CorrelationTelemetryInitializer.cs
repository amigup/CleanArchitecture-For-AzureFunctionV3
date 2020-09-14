using AZV3CleanArchitecture;
using AZV3CleanArchitecture.Providers;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace CleanArchitecture.Infrastructure.ApplicationInsightsInitializers
{
    public class CorrelationTelemetryInitializer : ITelemetryInitializer
    {
        private readonly ICorrelationProvider correlationProvider;

        public CorrelationTelemetryInitializer(ICorrelationProvider correlationProvider)
        {
            this.correlationProvider = correlationProvider;
        }

        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                return;
            }

            var requestId = correlationProvider.SetCorrelationId();
            if (requestId.HasValue)
            {
                telemetry.Context.GlobalProperties[Constants.CorrelationIdHeader] = requestId.Value.ToString();
            }
        }
    }
}
