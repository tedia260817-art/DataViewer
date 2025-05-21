using Xunit;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using DataExplorer.Services;
using DataExplorerModels;
using System.Collections.Generic;

public class AlbumGraphQLServiceTests
{
    [Fact]
    public async Task FetchAlbumsAsync_ReturnsAlbums_WhenResponseIsValid()
    {
        var fakeResponse = JsonSerializer.Serialize(new
        {
            data = new
            {
                albums = new
                {
                    data = new[]
                    {
                        new
                        {
                            id = 1,
                            title = "Album One",
                            user = new
                            {
                                id = 10,
                                username = "user1",
                                company = new { name = "TestCorp" }
                            }
                        }
                    }
                }
            }
        });

        var handler = new MockHttpMessageHandler(fakeResponse, HttpStatusCode.OK);
        var httpClient = new HttpClient(handler);

        var configValues = new Dictionary<string, string?>

        {
            { "GraphQL:Endpoint", "http://fake-endpoint/graphql" },
            { "Album:DefaultThumbnailUrl", "http://default-thumbnail.com/image.png" }
        };

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();

        var service = new AlbumGraphQLService(httpClient, config);

        var albums = await service.FetchAlbumsAsync();

        Assert.Single(albums);
        Assert.Equal("Album One", albums[0].Title);
        Assert.Equal("user1", albums[0].CreatedBy);
        Assert.Equal("TestCorp", albums[0].FromCompany);
        Assert.Equal("http://default-thumbnail.com/image.png", albums[0].ThumbnailUrl);
    }

    private class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _response;
        private readonly HttpStatusCode _statusCode;

        public MockHttpMessageHandler(string response, HttpStatusCode statusCode)
        {
            _response = response;
            _statusCode = statusCode;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var msg = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_response, Encoding.UTF8, "application/json")
            };

            return Task.FromResult(msg);
        }
    }
}
