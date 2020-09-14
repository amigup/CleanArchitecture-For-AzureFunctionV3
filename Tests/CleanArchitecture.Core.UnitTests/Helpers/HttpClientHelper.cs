using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.UnitTests
{
    public static class HttpClientHelper
    {
        public static HttpClient CreateHttpClient<T>(T payload, string baseAddress = null)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(payload?.GetType() == typeof(string) ? (string)Convert.ChangeType(payload, typeof(string)) : JsonConvert.SerializeObject(payload)),
            })
            .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            if (!string.IsNullOrEmpty(baseAddress))
            {
                httpClient.BaseAddress = new Uri(baseAddress);
            }

            return httpClient;
        }

        public static HttpClient CreateHttpClient(HttpStatusCode httpStatusCode)
        {
            return CreateHttpClient(httpStatusCode, new Dictionary<string, string>());
        }

        public static HttpClient CreateHttpClient(HttpStatusCode httpStatusCode, IDictionary<string, string> headers)
        {
            var httpResponseMessage = new HttpResponseMessage()
            {
                StatusCode = httpStatusCode,
                Content = new StringContent(httpStatusCode.ToString()),
            };

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpResponseMessage.Headers.Add(header.Key, header.Value);
                }
            }

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(httpResponseMessage)
            .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            return httpClient;
        }
    }
}
