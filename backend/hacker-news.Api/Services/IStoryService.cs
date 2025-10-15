using hacker_news.Api.Models;

namespace hacker_news.Api.Services
{
    public interface IStoryService
    {
        Task<List<Story>> GetNewStoriesPaginatedAsync(int pageSize, int offset);
    }
}