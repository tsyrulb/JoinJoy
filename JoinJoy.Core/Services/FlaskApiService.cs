using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class FlaskApiService
{
    private readonly HttpClient _httpClient;

    public FlaskApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetGeneratedTextAsync(string userInput)
    {
        var payload = new { input = userInput };
        var content = new StringContent(JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/generate", content);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        return responseString;
    }
}
