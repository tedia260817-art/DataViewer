using Microsoft.AspNetCore.Mvc;
using DataExplorerModels;
using DataExplorer.Services;

namespace DataExplorer.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly PostGraphQLService _postService;

        public PostsController(PostGraphQLService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetPosts()
        {
            var posts = await _postService.FetchPostsAsync();
            return Ok(posts);
        }
    }
}
