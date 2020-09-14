using Microsoft.AspNetCore.Http;
using System;

namespace AZV3CleanArchitecture.Providers
{
    public class CorrelationProvider : ICorrelationProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CorrelationProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Guid? SetCorrelationId()
        {
            if (httpContextAccessor != null && httpContextAccessor.HttpContext != null && httpContextAccessor.HttpContext.Request != null)
            {
                if (!httpContextAccessor.HttpContext.Request.Headers.ContainsKey(Constants.CorrelationIdHeader))
                {
                    Guid requestId = Guid.NewGuid();
                    httpContextAccessor.HttpContext.Request.Headers[Constants.CorrelationIdHeader] = requestId.ToString();
                    return requestId;
                }
                else
                {
                    if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue(Constants.CorrelationIdHeader, out var value) && Guid.TryParse(value, out Guid requestId))
                    {
                        return requestId;
                    }
                }
            }

            return null;
        }

        public Guid GetCorrelationId()
        {            
            Guid? correlationId = null;
            if (this.httpContextAccessor.HttpContext.Items.TryGetValue(Constants.CorrelationIdHeader, out object requestIdProperty))
            {
                if (requestIdProperty != null && Guid.TryParse(Convert.ToString(requestIdProperty), out Guid parsedCorrelationId))
                {
                    correlationId = parsedCorrelationId;
                }
            }

            if (correlationId == null)
            {
                correlationId = Guid.NewGuid();
                this.httpContextAccessor.HttpContext.Items.Add(Constants.CorrelationIdHeader, correlationId);
            }

            return correlationId.Value;
        }
    }
}
