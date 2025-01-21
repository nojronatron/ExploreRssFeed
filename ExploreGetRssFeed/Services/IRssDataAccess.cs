using ExploreGetRssFeed.Models;

namespace ExploreGetRssFeed.Services
{
    public interface IRssDataAccess
    {
        Task<int> CreateAsync(string title, string webAddress, string pathUrl);
        Task<FeedEntryModel> GetAsync(string title);
        Task<IEnumerable<FeedEntryModel>> GetAllAsync();
        Task<int> RemoveAsync(string title);
        Task<int> UpdateAsync(FeedEntryModel existingItem, FeedEntryModel updatedItem);
    }
}