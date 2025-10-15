using hacker_news.Api.Models;
using hacker_news.Api.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace hacker_news.Api.UnitTests
{
    public class StoryRepositoryTests
    {
        private const string NEW_STORIES_CACHE_KEY = "newest-stories";
        
        [Fact]
        public async Task GetNewStoryIdsAsync_ReturnsFromCache_IfPresent()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var expectedIds = new List<long> { 1, 2, 3, 4, 5 };
            cache.Set(NEW_STORIES_CACHE_KEY, expectedIds);

            var apiClientMock = new Mock<IStoryApiClient>();
            var repo = new StoryRepository(apiClientMock.Object, cache);

            var result = await repo.GetNewStoryIdsAsync();

            Assert.Equal(expectedIds, result);
            apiClientMock.Verify(x => x.GetNewStoryIdsAsync(), Times.Never);
        }

        [Fact]
        public async Task GetNewStoryIdsAsync_FetchesAndCaches_IfNotInCache()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var expectedIds = new List<long> { 1, 2, 3, 4, 5 };

            var apiClientMock = new Mock<IStoryApiClient>();
            apiClientMock.Setup(x => x.GetNewStoryIdsAsync()).ReturnsAsync(expectedIds);

            var repo = new StoryRepository(apiClientMock.Object, cache);

            var result = await repo.GetNewStoryIdsAsync();

            Assert.Equal(expectedIds, result);
            Assert.True(cache.TryGetValue(NEW_STORIES_CACHE_KEY, out List<long>? cachedIds));
            Assert.Equal(expectedIds, cachedIds);
            apiClientMock.Verify(x => x.GetNewStoryIdsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetStoryByIdAsync_ReturnsFromCache_IfPresent()
        {
            const int STORY_ID = 123;
            var cache = new MemoryCache(new MemoryCacheOptions());
            var expectedStory = new Story { Id = STORY_ID, Title = "title", Url = "url" };

            cache.Set($"story-{STORY_ID}", expectedStory);

            var apiClientMock = new Mock<IStoryApiClient>();
            var repo = new StoryRepository(apiClientMock.Object, cache);

            var result = await repo.GetStoryByIdAsync(STORY_ID);

            Assert.Equal(expectedStory, result);
            apiClientMock.Verify(x => x.GetStoryByIdAsync(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async Task GetStoryByIdAsync_FetchesAndCaches_IfNotInCache()
        {
            const int STORY_ID = 123;
            var cache = new MemoryCache(new MemoryCacheOptions());
            var expectedStory = new Story { Id = STORY_ID, Title = "title", Url = "url" };

            var apiClientMock = new Mock<IStoryApiClient>();
            apiClientMock.Setup(x => x.GetStoryByIdAsync(STORY_ID)).ReturnsAsync(expectedStory);

            var repo = new StoryRepository(apiClientMock.Object, cache);

            var result = await repo.GetStoryByIdAsync(STORY_ID);

            Assert.Equal(expectedStory, result);
            Assert.True(cache.TryGetValue($"story-{STORY_ID}", out Story? cachedStory));
            Assert.Equal(expectedStory, cachedStory);
            apiClientMock.Verify(x => x.GetStoryByIdAsync(STORY_ID), Times.Once);
        }
    }
}