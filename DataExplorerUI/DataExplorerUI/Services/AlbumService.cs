using System.Net.Http.Json;
using DataExplorerModels;
using Microsoft.Extensions.Configuration;

namespace DataExplorerUI.Services
{
    public class AlbumService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public AlbumService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;

            _apiUrl = configuration["Backend:AlbumsEndpoint"]
                      ?? throw new InvalidOperationException("Missing config value: Backend:AlbumsEndpoint");
        }

        public async Task<List<AlbumDto>> GetAlbumsAsync()
        {
            try
            {
                var albums = await _httpClient.GetFromJsonAsync<List<AlbumDto>>(_apiUrl);
                return albums ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception in GetAlbumsAsync: {ex.Message}");
                return new();
            }
        }
    }
}
