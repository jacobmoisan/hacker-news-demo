using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using hacker_news.Api.Models;

namespace hacker_news.Api.Repositories
{
    public class StoryApiClient : IStoryApiClient
    {
        private readonly HttpClient _httpClient;

        private const string BASE_URL = "https://hacker-news.firebaseio.com/v0/";
        private const string NEW_STORIES_URL = BASE_URL + "newstories.json";
        private const string ITEM_URL = BASE_URL + "item/";
        private const int RETRY_DELAY_MS = 500;
        private const int MAX_RETRIES = 3;

        public StoryApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<List<long>> GetNewStoryIdsAsync()
        {
            var response = await FetchWithRetryAsync(NEW_STORIES_URL);
            var json = await response.Content.ReadAsStringAsync();
            var ids = JsonSerializer.Deserialize<List<long>>(json);
            return ids ?? new List<long>();
        }

        public async Task<Story?> GetStoryByIdAsync(long id)
        {
            var response = await FetchWithRetryAsync($"{ITEM_URL}{id}.json");
            var json = await response.Content.ReadAsStringAsync();
            var story = JsonSerializer.Deserialize<Story>(json);
            return story;
        }

        /// <summary>
        /// Ensures success response or throws exception after max retries
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
    }
}