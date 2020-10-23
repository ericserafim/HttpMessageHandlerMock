using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestProject1
{
  public class UnitTest1
  {
    public DummyService SUT { get; }

    public HttpMessageHandlerMock HttpMessageHandlerMock { get; set; }

    public UnitTest1()
    {
      HttpMessageHandlerMock = new HttpMessageHandlerMock();      
      SUT = new DummyService(HttpMessageHandlerMock.CreateHttpClient());
    }

    [Fact]
    public async Task GetAsync_ShouldReturnsHttp200WithContent()
    {
      //Arrange
      var responseBody = @"[{ ""id"": 1, ""title"": ""Record 1""}, { ""id"": 100, ""title"": ""Record 2""}]";
      var response = new HttpResponseMessage
      {
        StatusCode = HttpStatusCode.OK,
        Content = new StringContent(responseBody),
      };

      HttpMessageHandlerMock.WhenAnyRequest().Returns(response);      

      //Act      
      var actionResult = await SUT.GetFakeData();

      //Assert      
      HttpMessageHandlerMock.VerifyRequest(HttpMethod.Get, Times.Once());

      var itemsOrigin = JsonSerializer.Deserialize<IEnumerable<JsonElement>>(responseBody);
      Assert.All(actionResult, item => itemsOrigin.Contains(item));
    }
  }
}
