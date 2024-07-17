using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TestApp.Models;

namespace TestApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:2446/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T> PostAsync<T>(string url, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, stringContent);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(result);
            }
            else
            {
                throw new HttpRequestException($"Request to {url} failed with status code {response.StatusCode}");
            }
        }

        public async Task<IEnumerable<InputBPKB>> GetBpkbsAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<InputBPKB>>(result);
            }
            else
            {
                throw new HttpRequestException($"Request to api/bpkb failed with status code {response.StatusCode}");
            }
        }
    }
}
