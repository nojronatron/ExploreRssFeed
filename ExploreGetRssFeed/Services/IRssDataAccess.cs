using ExploreGetRssFeed.Models;

namespace ExploreGetRssFeed.Services
{
    public interface IRssDataAccess
    {
        Task<int> Create(FeedModel itemToAdd);
        Task<int> Create(string title, string webAddress, string routeName);
        Task<IEnumerable<FeedModel>> GetAllEntries();
        Task<int> UpdateTitle(FeedEntryModel feedItemToChange, string newTitle);
        Task<int> UpdateWebAddress(FeedEntryModel feedItemToChange, string newWebAddress);
        Task<int> Remove(FeedEntryModel feedItemToRemove);
    }
}
