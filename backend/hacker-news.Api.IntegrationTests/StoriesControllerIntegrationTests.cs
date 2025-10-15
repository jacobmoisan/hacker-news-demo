using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using hacker_news.Api.Models;
using Xunit;

namespace hacker_news.Api.IntegrationTests
{
    public class StoriesControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public StoriesControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetLatestStories_ReturnsOkAndStories()
        {
            var response = await _client.GetAsync("/stories/latest");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var stories = JsonSerializer.Deserialize<List<Story>>(content);

            Assert.NotNull(stories);
            Assert.NotEmpty(stories);
            foreach (var story in stories)
            {
                Assert.False(
                    string.IsNullOrWhiteSpace(story.Title),
                    "All story titles should be populated."
                );
            }
        }

        [Fact]
        public async Task GetLatestStories_ReturnsBadRequest_ForInvalidPageSize()
        {
            var response = await _client.GetAsync("/stories/latest?pageSize=0&offset=0");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}