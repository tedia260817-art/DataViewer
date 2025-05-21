using Xunit;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using DataExplorerUI.Services;
using DataExplorerModels;

public class PostServiceTests
{
    [Fact]
    public async Task GetPostsAsync_ReturnsPosts_WhenResponseIsValid()
    {
        var mockPosts = new List<Post>
        {
            new Post { Id = 1, Title = "Test Title 1", Body = "Test Body 1" },
            new Post { Id = 2, Title = "Test Title 2", Body = "Test Body 2" }
        };

        var json = JsonSerializer.Serialize(mockPosts);
        var handler = new MockHttpMessageHandler(json, HttpStatusCode.OK);
        var httpClient = new HttpClient(handler);

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Backend:PostsEndpoint", "http://fake-api/api/posts" }
            })
            .Build();

        var service = new PostService(httpClient, config);

        var result = await service.GetPostsAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Test Title 1", result[0].Title);
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
            return Task.FromResult(new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_responseContent, Encoding.UTF8, "application/json")
            });
        }
    }
}
