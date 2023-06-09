using Moq.Protected;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HiveFS.TakeHomeAssessment.Tests.Repositories;

internal static class Helper
{
    public static Mock<IHttpClientFactory> GetClientFactory(HttpStatusCode httpStatusCode, string clientName, string data)
    {
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = httpStatusCode,
            Content = new StringContent(data, Encoding.UTF8, "application/json")
        };

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

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };

        var _httpClientFactory = new Mock<IHttpClientFactory>();
        _httpClientFactory.Setup(_ => _.CreateClient(clientName)).Returns(httpClient);
        return _httpClientFactory;
    }
}
