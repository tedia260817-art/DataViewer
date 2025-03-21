using System.Net.Http.Json;
using DataExplorerModels;

namespace DataExplorerUI.Services
{
    public class AlbumService
    {
        private readonly HttpClient _httpClient;

        public AlbumService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AlbumDto>> GetAlbumsAsync()
        {
            try
            {
                var albums = await _httpClient.GetFromJsonAsync<List<AlbumDto>>("http://localhost:5192/api/albums");
                return albums ?? new List<AlbumDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception in GetAlbumsAsync: {ex.Message}");
                return new List<AlbumDto>();
            }
        }
    }
}
