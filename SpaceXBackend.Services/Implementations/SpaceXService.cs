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

            return MapLaunch(response);
        }

        public async Task<List<SpaceXLaunchDto>> GetUpcomingLaunchesAsync()
        {
            var response = await _client.GetFromJsonAsync<List<JsonElement>>("launches/upcoming");
            if (response == null)
                return new List<SpaceXLaunchDto>();

            return response.Select(MapLaunch).OrderByDescending(l => l.DateUtc).ToList();
        }

        public async Task<List<SpaceXLaunchDto>> GetPastLaunchesAsync()
        {
            var response = await _client.GetFromJsonAsync<List<JsonElement>>("launches/past");
            if (response == null)
                return new List<SpaceXLaunchDto>();

            return response.Select(MapLaunch).OrderByDescending(l => l.DateUtc).ToList(); 
        }

        private SpaceXLaunchDto MapLaunch(JsonElement item)
        {
            return new SpaceXLaunchDto
            {
                Id = item.GetProperty("id").GetString() ?? string.Empty,
                Name = item.GetProperty("name").GetString() ?? string.Empty,
                DateUtc = DateTime.Parse(item.GetProperty("date_utc").GetString() ?? ""),
                Success = item.TryGetProperty("success", out var successProp)
                    ? successProp.ValueKind switch
                    {
                        JsonValueKind.True => true,
                        JsonValueKind.False => false,
                        _ => (bool?)null
                    }
                    : null,
                Details = item.GetProperty("details").GetString(),
                PatchImage = item.GetProperty("links").GetProperty("patch").GetProperty("small").GetString(),
                Webcast = item.GetProperty("links").GetProperty("webcast").GetString(),
                Wikipedia = item.GetProperty("links").GetProperty("wikipedia").GetString()
            };
        }
    }
}
