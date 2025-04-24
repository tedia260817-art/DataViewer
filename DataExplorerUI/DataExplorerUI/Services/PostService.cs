using DataExplorerModels;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace DataExplorerUI.Services
{
    public class PostService
    {
        private readonly HttpClient _httpClient;
        private readonly string _postsEndpoint;

        public PostService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _postsEndpoint = configuration["Backend:PostsEndpoint"]
                ?? throw new InvalidOperationException("Missing configuration: Backend:PostsEndpoint");
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            try
            {
                var posts = await _httpClient.GetFromJsonAsync<List<Post>>(_postsEndpoint);
                return posts ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error fetching posts: {ex.Message}");
                return new();
            }
        }
    }
}
