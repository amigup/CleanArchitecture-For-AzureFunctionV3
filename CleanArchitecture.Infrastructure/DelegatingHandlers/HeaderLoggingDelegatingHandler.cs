using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.DelegatingHandlers
{
    public class HeaderLoggingDelegatingHandler : DelegatingHandler
    {
        private const string Headers = "Headers";
        private readonly IHttpContextAccessor httpContextAccessor;

        public HeaderLoggingDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var dependencyTelemetry = this.httpContextAccessor.HttpContext.Features.Get<DependencyTelemetry>();
            if (dependencyTelemetry != null)
            {
                var headers = string.Join(",", request.Headers.Select(h => $"Key: {h.Key}, Value: {h.Value.First()}"));
                dependencyTelemetry.Context.GlobalProperties.Add(Headers, headers);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
