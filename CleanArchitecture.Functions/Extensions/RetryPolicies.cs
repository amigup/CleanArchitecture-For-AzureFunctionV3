using Polly;
using Polly.Extensions.Http;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace AZV3CleanArchitecture.Extensions
{
    public static class RetryPolicies
    {
        private const int MaxRetry = 6;

        public static IAsyncPolicy<HttpResponseMessage> GetTooManyRequestsRetryPolicy(string retryAfterHeaderName = "Retry-After", bool exponentialBackoffRetry = true)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests
                || msg.StatusCode == HttpStatusCode.NotFound
                || msg.StatusCode == HttpStatusCode.ServiceUnavailable
                || msg.StatusCode == HttpStatusCode.RequestTimeout)
                .WaitAndRetryAsync(
                    MaxRetry,
                    sleepDurationProvider: (retryCount, response, context) =>
                    {
                        var waitDuration = 2000; // 2 seconds
                        if (response.Result != null && response.Result.Headers != null && response.Result.Headers.TryGetValues(retryAfterHeaderName, out var headers))
                        {
                            if (headers != null)
                            {
                                Int32.TryParse(headers.First(), out waitDuration);
                            }
                        }
                        if (exponentialBackoffRetry)
                        {
                            return TimeSpan.FromSeconds(Math.Pow((waitDuration / 1000), retryCount == 0 ? 1 : retryCount));
                        }
                        else
                        {
                            return TimeSpan.FromMilliseconds(waitDuration);
                        }
                    },
                    onRetryAsync: async (response, timespan, retryCount, context) =>
                    {
                        await System.Threading.Tasks.Task.Yield();
                    });
        }

        public static IAsyncPolicy<HttpResponseMessage> GetExponentialBackoffRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(MaxRetry, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                    retryAttempt)));
        }
    }
}
