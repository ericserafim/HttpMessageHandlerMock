using Moq;
using Moq.Language.Flow;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUnitTestProject1
{
  public sealed class HttpMessageHandlerMock : Mock<HttpMessageHandler>
  {
    public HttpClient CreateHttpClient() => new HttpClient(this.Object);

    public ISetup<HttpMessageHandler, Task<HttpResponseMessage>> WhenAnyRequest()
    {
      return this.Protected()
        .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>());
    }

    public void VerifyRequest(HttpMethod httpMethod, Times times)
    {
      this.Protected()
        .Verify<Task<HttpResponseMessage>>(
                  "SendAsync",
                  times,
                  ItExpr.Is<HttpRequestMessage>(req => req.Method == httpMethod),
                  ItExpr.IsAny<CancellationToken>());
    }
  }

  public static class MockExtensions
  {
    public static void ReturnsAsync<TFact, TResult>(this ISetup<TFact, Task<TResult>> setup, TResult result)
      where TFact : class
    {
      setup.ReturnsAsync(result);
    }

    public static void Returns<TFact, TResult>(this ISetup<TFact, Task<TResult>> setup, TResult result)
      where TFact : class
    {
      setup.Returns(result);
    }
  }
}
