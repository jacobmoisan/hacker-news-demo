using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using hacker_news.Api.Models;
using Microsoft.Extensions.Caching.Memory;

namespace hacker_news.Api.Repositories
{
    public class StoryRepository : IStoryRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memCache;
        private const string BASE_URL = "https://hacker-news.firebaseio.com/v0/";
        private const string NEW_STORIES_URL = BASE_URL + "newstories.json";
        private const string ITEM_URL = BASE_URL + "item/";
        private const string NEW_STORIES_CACHE_KEY = "newest-stories";
        private const int CACHE_DURATION_MINUTES = 5;
        private const int RETRY_DELAY_MS = 500;
        private const int MAX_RETRIES = 3;

        public StoryRepository(IHttpClientFactory httpClientFactory, IMemoryCache memCache)
        {
            _httpClient = httpClientFactory.CreateClient();
            _memCache = memCache;
        }


        public async Task<List<long>> GetNewStoryIdsAsync()
        {
            // Check cache first
            var cachedIds = GetFromCache<List<long>>(NEW_STORIES_CACHE_KEY);
            if (cachedIds != null)
                return cachedIds;

            // If cache miss, fetch from API
            var response = await FetchWithRetryAsync(NEW_STORIES_URL);
            var responseJson = await response.Content.ReadAsStringAsync();

            var ids = JsonSerializer.Deserialize<List<long>>(responseJson);
            if (ids == null)
                return new List<long>();

            _memCache.Set(NEW_STORIES_CACHE_KEY, ids, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
            return ids;
        }

        public async Task<Story?> GetStoryByIdAsync(long id)
        {
            // Check cache first
            var cacheKey = $"story-{id}";
            var cachedStory = GetFromCache<Story>(cacheKey);
            if (cachedStory != null)
                return cachedStory;
            
            // cache miss, fetch from API
            var response = await FetchWithRetryAsync(ITEM_URL + $"{id}.json");
            var responseJson = await response.Content.ReadAsStringAsync();

            var story = JsonSerializer.Deserialize<Story>(responseJson);
            if (story != null)
                _memCache.Set(cacheKey, story, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
            
            return story;
        }

        /// <summary>
        /// Light retry logic to handle transient failures from the Hacker News API.
        /// </summary>
        private async Task<HttpResponseMessage> FetchWithRetryAsync(string url)
        {
            HttpResponseMessage response = null!;
            for (int attempt = 0; attempt < MAX_RETRIES; attempt++)
            {
                response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                if (attempt < MAX_RETRIES - 1)
                {
                    await Task.Delay(RETRY_DELAY_MS);
                }
            }
            throw new HttpRequestException($"Unable to reach hacker news API at {url} and status {response.StatusCode}");
        }

        private T? GetFromCache<T>(string cacheKey)
        {
            _memCache.TryGetValue(cacheKey, out T? cachedValue);
            return cachedValue;
        }
    }
}