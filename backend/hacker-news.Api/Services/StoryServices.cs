using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hacker_news.Api.Models;
using hacker_news.Api.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace hacker_news.Api.Services
{
    public class StoryService : IStoryService
    {
        private readonly IStoryRepository _storyRepository;

        public StoryService(IStoryRepository storyRepository)
        {
            _storyRepository = storyRepository;
        }

        public async Task<List<Story>> GetNewStoriesPaginatedAsync(int pageSize, int offset)
        {
            var newStoryIDs = await _storyRepository.GetNewStoryIdsAsync();
            var pagedIDs = newStoryIDs
                .Skip(offset)
                .Take(pageSize);

            var stories = new List<Story>();
            foreach (var id in pagedIDs)
            {
                var story = await _storyRepository.GetStoryByIdAsync(id);
                if (story != null)
                {
                    stories.Add(story);
                }
            }
            return stories;
        }
    }
}