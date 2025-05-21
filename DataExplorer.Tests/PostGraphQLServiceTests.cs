using Xunit;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using DataExplorer.Services;
using DataExplorerModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class PostGraphQLServiceTests
{
    [Fact]
    public async Task FetchPostsAsync_ReturnsPosts_WhenResponseIsValid()
    {
        var fakeResponse = JsonSerializer.Serialize(new
        {
            data = new
            {
                posts = new
                {
                    data = new[]
                    {
                        new { id = 1, title = "Test Post", body = "This is a test post." },
                        new { id = 2, title = "Another Post", body = "Another body." }
                    }
                }
            }
        });

        var handler = new MockHttpMessageHandler(fakeResponse, HttpStatusCode.OK);
        var httpClient = new HttpClient(handler);

        var configValues = new Dictionary<string, string?>
        {
            { "GraphQL:Endpoint", "http://fake-graphql/posts" }
        };

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();

        var service = new PostGraphQLService(httpClient, config);

        var posts = await service.FetchPostsAsync();

        Assert.Equal(2, posts.Count);
        Assert.Equal("Test Post", posts[0].Title);
        Assert.Equal("This is a test post.", posts[0].Body);
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
            var message = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_response, Encoding.UTF8, "application/json")
            };
            return Task.FromResult(message);
        }
    }
}
