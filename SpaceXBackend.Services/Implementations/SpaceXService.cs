using SpaceXBackend.Services.DTO;
using SpaceXBackend.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace SpaceXBackend.Services.Implementations
{
    public class SpaceXService : ISpaceXService
    {
        private readonly HttpClient _client;

        public SpaceXService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("SpaceX");
        }

        public async Task<SpaceXLaunchDto?> GetLatestLaunchAsync()
        {
            var response = await _client.GetFromJsonAsync<JsonElement>("launches/latest");

            if (response.ValueKind == JsonValueKind.Undefined) return null;

            return new SpaceXLaunchDto
            {
                Id = response.GetProperty("id").GetString() ?? string.Empty,
                Name = response.GetProperty("name").GetString() ?? string.Empty,
                DateUtc = DateTime.Parse(response.GetProperty("date_utc").GetString() ?? ""),
                Success = response.TryGetProperty("success", out var successProp) ? successProp.GetBoolean() : null,
                Details = response.GetProperty("details").GetString(),
                PatchImage = response.GetProperty("links").GetProperty("patch").GetProperty("small").GetString(),
                Webcast = response.GetProperty("links").GetProperty("webcast").GetString(),
                Wikipedia = response.GetProperty("links").GetProperty("wikipedia").GetString()
            };
        }


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
