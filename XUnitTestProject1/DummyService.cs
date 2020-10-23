using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace XUnitTestProject1
{
  public class DummyService
  {
    private readonly HttpClient httpClient;
    private const string url = "https://fakeurl.com";

    public DummyService(HttpClient httpClient)
    {
      this.httpClient = httpClient;
    }

    public async Task<IEnumerable<JsonElement>> GetFakeData()
    {
      var response = await httpClient.GetAsync(url);
      var body = await response.Content.ReadAsStringAsync();
      var fakeData = JsonSerializer.Deserialize<IEnumerable<JsonElement>>(body);
      return fakeData;
    }
  }
}
