using System.Text.Json;
using hacker_news.Api.Models;
using Microsoft.Extensions.Caching.Memory;

namespace hacker_news.Api.Repositories
{
    public class StoryRepository : IStoryRepository
    {
        private readonly IStoryApiClient _storyApiClient;
        private readonly IMemoryCache _memCache;
        
        private const string NEW_STORIES_CACHE_KEY = "newest-stories";
        private const int CACHE_DURATION_MINUTES = 5;
        

        public StoryRepository(IStoryApiClient storyApiClient, IMemoryCache memCache)
        {
            _storyApiClient = storyApiClient;
            _memCache = memCache;
        }


        public async Task<List<long>> GetNewStoryIdsAsync()
        {
            var cachedIds = GetFromCache<List<long>>(NEW_STORIES_CACHE_KEY);
            if (cachedIds != null) {
                return cachedIds;
            }

            // cache miss, fetch from API
            var fetchedIds = await _storyApiClient.GetNewStoryIdsAsync();
            if (fetchedIds == null) {
                return new List<long>();
            }

            _memCache.Set(NEW_STORIES_CACHE_KEY, fetchedIds, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
            
            return fetchedIds;
        }

        public async Task<Story?> GetStoryByIdAsync(long id)
        {
            var cacheKey = $"story-{id}";
            var cachedStory = GetFromCache<Story>(cacheKey);
            if (cachedStory != null) {
                return cachedStory;
            }
            
            // cache miss, fetch from API
            var fetchedStory = await _storyApiClient.GetStoryByIdAsync(id);
            if (fetchedStory != null) {
                _memCache.Set(cacheKey, fetchedStory, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
            }

            return fetchedStory;
        }

        private T? GetFromCache<T>(string cacheKey)
        {
            _memCache.TryGetValue(cacheKey, out T? cachedValue);
            return cachedValue;
        }
    }
}