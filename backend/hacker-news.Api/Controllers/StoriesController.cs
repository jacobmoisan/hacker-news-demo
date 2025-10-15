using hacker_news.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace hacker_news.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly IStoryService _storyService;

        public StoriesController(IStoryService storyService)
        {
            _storyService = storyService;
        }

        /// <summary>
        /// Get latest stories from Hacker News public API https://github.com/HackerNews/API
        /// </summary>
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestStories([FromQuery] int pageSize = 10, [FromQuery] int offset = 0)
        {
            //validations
            if (pageSize <= 0 || offset < 0)
            {
                return BadRequest("Invalid pageSize or offset");
            }
            if (pageSize > 20)
            {
                return BadRequest("pageSize must be at most 20");
            }

            var stories = await _storyService.GetNewStoriesPaginatedAsync(pageSize, offset);
            return Ok(stories);
        }
    }
}