using Moq;
using Moq.Protected;
using System.Net;

namespace TestStockConsole
{
    public static class HttpClientFactoryMock
    {
        public static List<HttpRequestMessage> ReceivedRequests { get; } = new List<HttpRequestMessage>();
        
        public static IHttpClientFactory CreateMockFactory(string jsonResponse)
        {
            return CreateMockFactory(request => jsonResponse);
        }

        public static IHttpClientFactory CreateMockFactory(Func<HttpRequestMessage, string> responseProvider)
        {
            ReceivedRequests.Clear();
            
            var handlerMock = new Mock<HttpMessageHandler>();
            
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((request, _) => 
                {
                    ReceivedRequests.Add(request);
                    
                    Console.WriteLine($"Request received: {request.Method} {request.RequestUri}");
                    
                    if (request.Content != null)
                    {
                        var content = request.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        Console.WriteLine($"Request content: {content}");
                    }
                    
                    foreach (var header in request.Headers)
                    {
                        Console.WriteLine($"Header: {header.Key} = {string.Join(", ", header.Value)}");
                    }
                })
                .ReturnsAsync((HttpRequestMessage request, CancellationToken _) => new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseProvider(request))
                });

            var httpClient = new HttpClient(handlerMock.Object);

            var factoryMock = new Mock<IHttpClientFactory>();
            
            factoryMock
                .Setup(f => f.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            return factoryMock.Object;
        }
    }
}