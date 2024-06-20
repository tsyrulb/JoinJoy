using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using JoinJoy.Core.Interfaces;

namespace JoinJoy.Infrastructure.Services
{
    public class AIChatService : IAIChatService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public AIChatService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        public async Task<string> GetChatResponseAsync(string userInput)
        {
            var requestContent = new StringContent(JsonConvert.SerializeObject(new { inputs = userInput }), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"https://api-inference.huggingface.co/models/microsoft/phi-1_5", requestContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ChatResponse>(responseContent);
            return result.GeneratedText;
        }
    }

    public class ChatResponse
    {
        [JsonProperty("generated_text")]
        public string GeneratedText { get; set; }
    }
}
