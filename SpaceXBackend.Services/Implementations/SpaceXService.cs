using SpaceXBackend.Services.Interfaces;
using System.Net.Http;

namespace SpaceXBackend.Services.Implementations
{
    public class SpaceXService : ISpaceXService
    {
        private readonly HttpClient _client;

        public SpaceXService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("SpaceX");
        }

        public async Task<string> GetLatestLaunchAsync()
            => await GetJsonAsync("launches/latest");

        public async Task<string> GetUpcomingLaunchesAsync()
            => await GetJsonAsync("launches/upcoming");

        public async Task<string> GetPastLaunchesAsync()
            => await GetJsonAsync("launches/past");

        private async Task<string> GetJsonAsync(string path)
        {
            var resp = await _client.GetAsync(path);
            var content = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
                throw new HttpRequestException($"SpaceX API error {(int)resp.StatusCode}: {content}");

            return content; // raw JSON string
        }
    }
}
