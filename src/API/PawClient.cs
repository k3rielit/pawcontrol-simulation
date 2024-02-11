using API.Endpoints;
using API.Serialization;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace API
{
    public class PawClient
    {
        [JsonIgnore]
        private readonly HttpClient _client = new();
        [JsonIgnore]
        public bool IsLogging { get; set; } = true;
        public string AuthToken { get; set; } = string.Empty;

        public PawClient(bool isLogging = true)
        {
            IsLogging = isLogging;
        }

        private HttpRequestMessage ConstructRequestMessage(IEndpoint endpoint, object? body)
        {
            return new HttpRequestMessage
            {
                Method = endpoint.Method,
                RequestUri = endpoint.GetUri(),
                Headers =
                {
                    { "Accept", "application/json" },
                    { endpoint.AuthHeader, $"{endpoint.AuthTokenPrefix}{AuthToken}" },
                },
                Content = new StringContent(JsonConvert.SerializeObject(body), new MediaTypeHeaderValue("application/json")),
            };
        }

        public async Task<T?> SendRequestAsync<T>(IEndpoint endpoint, object? body)
        {
            T? result = default;
            var request = ConstructRequestMessage(endpoint, body);
            try
            {
                using (var response = await _client.SendAsync(request))
                {
                    var str = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();
                    result = JsonConvert.DeserializeObject<T>(str, Converter.Settings) ?? result;
                }
            }
            catch (Exception ex)
            {
                if (IsLogging)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return result;
        }

        public T? SendRequest<T>(IEndpoint endpoint, object? body)
        {
            T? result = default;
            var request = ConstructRequestMessage(endpoint, body);
            try
            {
                using (var response = _client.Send(request))
                {
                    var stream = response.Content.ReadAsStream();
                    var str = new StreamReader(stream).ReadToEnd();
                    response.EnsureSuccessStatusCode();
                    result = JsonConvert.DeserializeObject<T>(str, Converter.Settings) ?? result;
                }
            }
            catch (Exception ex)
            {
                if(IsLogging)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return result;
        }
    }
}
