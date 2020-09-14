using AZV3CleanArchitecture;
using AZV3CleanArchitecture.Providers;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.DelegatingHandlers
{
    public class CorrelationDelegatingHandler : DelegatingHandler
    {
        private readonly ICorrelationProvider correlationProvider;

        public CorrelationDelegatingHandler(ICorrelationProvider correlationProvider)
        {
            this.correlationProvider = correlationProvider;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(Constants.CorrelationIdHeader))
            {
                request.Headers.Add(Constants.CorrelationIdHeader, this.correlationProvider.GetCorrelationId().ToString());
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
