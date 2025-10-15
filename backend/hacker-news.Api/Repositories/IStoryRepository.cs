using System.Collections.Generic;
using System.Threading.Tasks;
using hacker_news.Api.Models;

namespace hacker_news.Api.Repositories
{
    public interface IStoryRepository
    {
        Task<List<long>> GetNewStoryIdsAsync();
        Task<Story?> GetStoryByIdAsync(long id);
    }
}