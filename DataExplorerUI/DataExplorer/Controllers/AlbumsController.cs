using Microsoft.AspNetCore.Mvc;
using DataExplorerModels;

namespace DataExplorer.Controllers
{
    [ApiController]
    [Route("api/albums")]
    public class AlbumsController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public AlbumsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<ActionResult<List<AlbumDto>>> GetAlbums()
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

            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://graphqlzero.almansi.me/api", query);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Error fetching albums.");
                }

                var result = await response.Content.ReadFromJsonAsync<GraphQLResponse>();

                if (result == null || result.Data == null || result.Data.Albums == null)
                {
                    return NotFound("No albums found.");
                }

#pragma warning disable CS8604 // Possible null reference argument.
                var albums = result.Data.Albums.Data.Select(album => new AlbumDto
                {
                    Id = album.Id,
                    Title = album.Title,
                    ThumbnailUrl = $"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTWh-hr3PwoUpqvsviNcV8EIxl7Gkcpkvr-JQ&s",
                    CreatedBy = album.User?.Username ?? "Unknown",
                    FromCompany = album.User?.Company?.Name ?? "Unknown"
                }).ToList();
#pragma warning restore CS8604 // Possible null reference argument.

                return Ok(albums);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GraphQL Response Classes
        private class GraphQLResponse
        {
            public AlbumData? Data { get; set; }
        }

        private class AlbumData
        {
            public AlbumCollection? Albums { get; set; }
        }

        private class AlbumCollection
        {
            public List<AlbumRaw>? Data { get; set; }
        }

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
