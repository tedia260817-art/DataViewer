using DataExplorerModels;
using System.Net.Http.Json;

namespace DataExplorer.Services
{
    public class PostGraphQLService
    {
        private readonly HttpClient _httpClient;

        public PostGraphQLService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Post>> FetchPostsAsync()
        {
            var query = new
            {
                query = @"{
                    posts {
                        data {
                            id
                            title
                            body
                        }
                    }
                }"
            };

            var response = await _httpClient.PostAsJsonAsync("https://graphqlzero.almansi.me/api", query);

            if (!response.IsSuccessStatusCode)
                return new();

            var result = await response.Content.ReadFromJsonAsync<GraphQLResponse>();
            return result?.Data?.Posts?.Data ?? new();
        }

        private class GraphQLResponse
        {
            public GraphQLData? Data { get; set; }
        }

        private class GraphQLData
        {
            public PostCollection? Posts { get; set; }
        }

        private class PostCollection
        {
            public List<Post>? Data { get; set; }
        }
    }
}
