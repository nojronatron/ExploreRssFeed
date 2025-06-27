using ExploreGetRssFeed.Models;

namespace ExploreGetRssFeed.Services
{
    public interface IRssDataAccess
    {
        Task<int> CreateAsync(string title, string webAddress, string pathUrl, bool? newTab = false);
        Task<IEnumerable<FeedEntryModel>> GetAllAsync();
        Task<FeedEntryModel> GetByTitleAsync(string title);
        Task<FeedEntryModel> GetByRouteAsync(string routeName);
        Task<int> RemoveAsync(string title);
        Task<int> UpdateAsync(FeedEntryModel existingItem, FeedEntryModel updatedItem);
    }
}
