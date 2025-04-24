using Microsoft.AspNetCore.Mvc;
using DataExplorer.Services;
using DataExplorerModels;

namespace DataExplorer.Controllers
{
    [ApiController]
    [Route("api/albums")]
    public class AlbumsController : ControllerBase
    {
        private readonly AlbumGraphQLService _albumService;

        public AlbumsController(AlbumGraphQLService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AlbumDto>>> GetAlbums()
        {
            var albums = await _albumService.FetchAlbumsAsync();
            return Ok(albums);
        }
    }
}
