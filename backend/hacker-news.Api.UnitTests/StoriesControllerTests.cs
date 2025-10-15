using System.Collections.Generic;
using System.Threading.Tasks;
using hacker_news.Api.Controllers;
using hacker_news.Api.Models;
using hacker_news.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace hacker_news.Api.UnitTests
{
    public class StoriesControllerTests
    {
        [Fact]
        public async Task ReturnsBadRequest_WhenPageSizeIsInvalid()
        {
            var serviceMock = new Mock<IStoryService>();
            var controller = new StoriesController(serviceMock.Object);

            var result = await controller.GetLatestStories(0, 0);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ReturnsBadRequest_WhenOffsetIsNegative()
        {
            var serviceMock = new Mock<IStoryService>();
            var controller = new StoriesController(serviceMock.Object);

            var result = await controller.GetLatestStories(10, -1);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ReturnsBadRequest_WhenPageSizeIsTooLarge()
        {
            var serviceMock = new Mock<IStoryService>();
            var controller = new StoriesController(serviceMock.Object);

            var result = await controller.GetLatestStories(21, 0);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ReturnsOk_WithStories()
        {
            const int STORY_ID = 12345;
            var serviceMock = new Mock<IStoryService>();
            serviceMock.Setup(s => s.GetNewStoriesPaginatedAsync(10, 0))
                .ReturnsAsync(new List<Story> { new Story { Id = STORY_ID, Title = "title", Url = "url" } });

            var controller = new StoriesController(serviceMock.Object);

            var result = await controller.GetLatestStories(10, 0);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var stories = Assert.IsType<List<Story>>(okResult.Value);
            Assert.Single(stories);
            Assert.Equal(STORY_ID, stories[0].Id);
        }
    }
}