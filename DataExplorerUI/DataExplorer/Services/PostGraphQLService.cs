using DataExplorerModels;
using System.Net.Http.Json;

namespace DataExplorer.Services
{
    
    public class PostGraphQLService
    {
        private readonly HttpClient _httpClient;
        private readonly string _graphqlEndpoint;

        public PostGraphQLService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _graphqlEndpoint = configuration["GraphQL:Endpoint"]
        ?? throw new InvalidOperationException("GraphQL endpoint is not configured.");
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

            var response = await _httpClient.PostAsJsonAsync(_graphqlEndpoint, query);

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
