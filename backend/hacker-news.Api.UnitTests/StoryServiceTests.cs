using hacker_news.Api.Models;
using hacker_news.Api.Repositories;
using hacker_news.Api.Services;
using Moq;
using Xunit;

namespace hacker_news.Api.UnitTests
{
    public class StoryServiceTests
    {
        [Fact]
        public async Task ReturnsPagedStories()
        {
            var storyIds = new List<long> { 1, 2, 3, 4, 5 };
            var stories = new List<Story>
            {
                new Story { Id = 3, Title = "Story 3", Url = "url3" },
                new Story { Id = 4, Title = "Story 4", Url = "url4" }
            };

            var repoMock = new Mock<IStoryRepository>();
            repoMock.Setup(r => r.GetNewStoryIdsAsync()).ReturnsAsync(storyIds);
            repoMock.Setup(r => r.GetStoryByIdAsync(3)).ReturnsAsync(stories[0]);
            repoMock.Setup(r => r.GetStoryByIdAsync(4)).ReturnsAsync(stories[1]);
            var service = new StoryService(repoMock.Object);

            var result = await service.GetNewStoriesPaginatedAsync(2, 2);

            Assert.Equal(2, result.Count);
            Assert.Equal(3, result[0].Id);
            Assert.Equal(4, result[1].Id);
        }
    }
}