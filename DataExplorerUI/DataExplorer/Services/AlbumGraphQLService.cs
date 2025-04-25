using DataExplorerModels;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace DataExplorer.Services
{
    public class AlbumGraphQLService
    {
        private readonly HttpClient _httpClient;
        private readonly string _graphqlEndpoint;
        private readonly string _defaultThumbnailUrl;

        public AlbumGraphQLService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _graphqlEndpoint = configuration["GraphQL:Endpoint"]
                ?? throw new InvalidOperationException("GraphQL endpoint is not configured.");
                _defaultThumbnailUrl = configuration["Album:DefaultThumbnailUrl"]
        ?? throw new InvalidOperationException("Missing default thumbnail URL.");
        
        }

        public async Task<List<AlbumDto>> FetchAlbumsAsync()
        {
            var query = new
            {
                query = @"{
                    albums {
                        data {
                            id
                            title
                            user {
                                id
                                username
                                company {
                                    name
                                }
                            }
                        }
                    }
                }"
            };

            var response = await _httpClient.PostAsJsonAsync(_graphqlEndpoint, query);
            if (!response.IsSuccessStatusCode)
                return new();

            var result = await response.Content.ReadFromJsonAsync<GraphQLResponse>();
            if (result?.Data?.Albums?.Data == null)
                return new();

            return result.Data.Albums.Data.Select(album => new AlbumDto
            {
                Id = album.Id,
                Title = album.Title,
                ThumbnailUrl = _defaultThumbnailUrl,
                CreatedBy = album.User?.Username ?? "Unknown",
                FromCompany = album.User?.Company?.Name ?? "Unknown"
            }).ToList();
        }

        private class GraphQLResponse { public AlbumData? Data { get; set; } }
        private class AlbumData { public AlbumCollection? Albums { get; set; } }
        private class AlbumCollection { public List<AlbumRaw>? Data { get; set; } }
        private class AlbumRaw
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public UserRaw? User { get; set; }
}

private class UserRaw
{
    public string Username { get; set; } = string.Empty;
    public CompanyRaw? Company { get; set; }
}

private class CompanyRaw
{
    public string Name { get; set; } = string.Empty;
}
    }
}
