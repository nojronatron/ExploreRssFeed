using Microsoft.EntityFrameworkCore;
using ExploreGetRssFeed.Models;
using ExploreGetRssFeed.Data;

namespace ExploreGetRssFeed.Services
{
    public class RssDataAccess : IRssDataAccess
    {
        private readonly IDbContextFactory<ExploreGetRssFeedContext> _dbFactory;
        private readonly ILogger<RssDataAccess> _logger;

        public RssDataAccess(IDbContextFactory<ExploreGetRssFeedContext> dbFactory, ILogger<RssDataAccess> logger)
        {
            _dbFactory = dbFactory;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new feed entry in the database.
        /// </summary>
        /// <param name="title">The title for the new feed entry</param>
        /// <param name="webAddress">The web address (target) to the feed</param>
        /// <param name="pathUrl">The blazor page directive path</param>
        /// <returns>The number of entries written to the db</returns>
        public async Task<int> CreateAsync(string title, string webAddress, string pathUrl)
        {
            _logger.LogInformation("CreateAsync received title {title}, webAddress {webAddress}, pathUrl {pathurl}", title, webAddress, pathUrl);

            using var context = _dbFactory.CreateDbContext();

            var dto = new FeedEntryDataModel
            {
                Title = title,
                RouteName = pathUrl,
                WebAddress = webAddress
            };

            _logger.LogInformation("New DTO to store is title {title}, webAddress {webAddress}, pathUrl {pathurl}", title, webAddress, pathUrl);
            context.FeedEntryDataModels.Add(dto);
            return await context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves all feed entries from the database.
        /// </summary>
        /// <returns>A list of FeedModel</returns>
        public async Task<IEnumerable<FeedEntryModel>> GetAllAsync()
        {
            using var context = _dbFactory.CreateDbContext();
            var dtoList = await context.FeedEntryDataModels.AsNoTracking().ToListAsync();

            return dtoList.Select(
                dto => FeedEntryModel.Create(
                    dto.Title,
                    dto.WebAddress
                    ));
        }

        /// <summary>
        /// Overwrite an existing item with new values.
        /// This is an overwrite operation if all values are changed.
        /// </summary>
        /// <param name="existingItem">Existing item in the database</param>
        /// <param name="updatedItem">A new instance whose properties are used to update the existing item properties</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(FeedEntryModel existingItem, FeedEntryModel updatedItem)
        {
            using var context = _dbFactory.CreateDbContext();
            var dtoToUpdate = await context.FeedEntryDataModels
                .FirstOrDefaultAsync(feedEntry =>
                    feedEntry.Title == existingItem.Title);

            if (dtoToUpdate is null || string.IsNullOrWhiteSpace(dtoToUpdate.Title))
            {
                _logger.LogWarning("No item with Title {title} was found, no update performed.", existingItem.Title);
                return 0;
            }

            dtoToUpdate.RouteName = FeedEntryModel.GetRouteName(updatedItem.Title!);

            if (string.IsNullOrWhiteSpace(updatedItem.WebAddress) == false
                && dtoToUpdate.WebAddress.Equals(updatedItem.WebAddress) == false)
            {
                dtoToUpdate.WebAddress = updatedItem.WebAddress;
            }

            if (string.IsNullOrWhiteSpace(updatedItem.Title) == false
                && dtoToUpdate.Title.Equals(updatedItem.Title) == false)
            {
                dtoToUpdate.Title = updatedItem.Title;
            }

            return await context.SaveChangesAsync();
        }

        /// <summary>
        /// Removes a specific item from the database. This operation is permanent and not reversable.
        /// </summary>
        /// <param name="title">Title</param>
        /// <returns>The number of items removed from the database</returns>
        public async Task<int> RemoveAsync(string title)
        {
            using var context = _dbFactory.CreateDbContext();

            var itemToRemove = await context.FeedEntryDataModels
                .FirstOrDefaultAsync(feedEntry =>
                    feedEntry.Title == title);

            if (itemToRemove is null)
            {
                _logger.LogWarning("No item found to remove.");
                return 0;
            };

            context.Remove(itemToRemove);
            return await context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves a specific item from the database using its Title.
        /// </summary>
        /// <param name="title">Title</param>
        /// <returns>The FeedEntryModel instance retrieved from the database, or empty if not found</returns>
        public async Task<FeedEntryModel> GetByTitleAsync(string title)
        {
            using var context = _dbFactory.CreateDbContext();

            var foundItem = await context.FeedEntryDataModels
                .FirstOrDefaultAsync(feedEntry =>
                feedEntry.Title == title);

            if (foundItem is not null)
            {
                _logger.LogInformation("Item found with title {title}", title);

                // null coalescing operator ?? returns value of left-hand operand if not null
                // otherwise returns the right-hand operand
                var result = FeedEntryModel.Create(
                    foundItem.Title,
                    foundItem.WebAddress ?? string.Empty
                    );

                return result;
            }
            
            _logger.LogWarning("No item found with title {title}", title);
            return new FeedEntryModel();
        }

        /// <summary>
        /// Retrieves a specific item from the database using its RouteName.
        /// </summary>
        /// <param name="routeName"></param>
        /// <returns>The FeedEntryModel instance retrieved from the database, or empty if not found</returns>
        public async Task<FeedEntryModel> GetByRouteAsync(string routeName)
        {
            using var context = _dbFactory.CreateDbContext();

            var foundItem = await context.FeedEntryDataModels
                .FirstOrDefaultAsync(feedEntry =>
                feedEntry.RouteName == routeName);

            if (foundItem is not null)
            {
                _logger.LogInformation("Item found with route name {routename}", routeName);

                // null coalescing operator ?? returns value of left-hand operand if not null
                // otherwise returns the right-hand operand
                var result = FeedEntryModel.Create(
                    foundItem.Title,
                    foundItem.WebAddress ?? string.Empty
                    );

                return result;
            }

            _logger.LogWarning("No item found with route name {routename}", routeName);
            return new FeedEntryModel();
        }
    }
}
