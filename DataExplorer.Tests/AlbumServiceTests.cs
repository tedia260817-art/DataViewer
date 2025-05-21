using Xunit;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using DataExplorerUI.Services;
using DataExplorerModels;

public class AlbumServiceTests
{
    [Fact]
    public async Task GetAlbumsAsync_ReturnsAlbums_WhenResponseIsValid()
    {
        var mockAlbums = new List<AlbumDto>
        {
            new AlbumDto { Id = 1, Title = "First Album", CreatedBy = "user1", FromCompany = "company1", ThumbnailUrl = "url1" },
            new AlbumDto { Id = 2, Title = "Second Album", CreatedBy = "user2", FromCompany = "company2", ThumbnailUrl = "url2" }
        };

        var json = JsonSerializer.Serialize(mockAlbums);
        var handler = new MockHttpMessageHandler(json, HttpStatusCode.OK);
        var httpClient = new HttpClient(handler);

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Backend:AlbumsEndpoint", "http://fake-api/api/albums" }
            })
            .Build();

        var service = new AlbumService(httpClient, config);

        var result = await service.GetAlbumsAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("First Album", result[0].Title);
    }

    private class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _responseContent;
        private readonly HttpStatusCode _statusCode;

        public MockHttpMessageHandler(string responseContent, HttpStatusCode statusCode)
        {
            _responseContent = responseContent;
            _statusCode = statusCode;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_responseContent, Encoding.UTF8, "application/json")
            };

            return Task.FromResult(response);
        }
    }
}
