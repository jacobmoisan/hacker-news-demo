using hacker_news.Api.Models;

namespace hacker_news.Api.Repositories
{
    public interface IStoryApiClient
    {
        Task<List<long>> GetNewStoryIdsAsync();
        Task<Story?> GetStoryByIdAsync(long id);
    }
}