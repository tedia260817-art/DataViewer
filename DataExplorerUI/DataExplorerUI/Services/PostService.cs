using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataExplorerModels;

namespace DataExplorerUI.Services
{
    public class PostService 
    {
        private readonly HttpClient _http;

        public PostService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Post>> GetPostsAsync()
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

            var jsonQuery = JsonSerializer.Serialize(query);
            var request = new StringContent(jsonQuery, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("https://graphqlzero.almansi.me/api", request);
            
            if (response == null || !response.IsSuccessStatusCode)
            {
                return new List<Post>();
            }

            var result = await response.Content.ReadFromJsonAsync<GraphQLResponse>();

            return result?.Data?.Posts?.Data ?? new List<Post>();
        }
    }
}
