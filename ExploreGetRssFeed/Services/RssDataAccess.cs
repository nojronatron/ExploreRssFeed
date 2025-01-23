using Microsoft.EntityFrameworkCore;
using ExploreGetRssFeed.Models;
using ExploreGetRssFeed.Data;

namespace ExploreGetRssFeed.Services
{
    public class RssDataAccess : IRssDataAccess
    {
        private readonly IDbContextFactory<ExploreGetRssFeedContext> dbFactory;
        private readonly ILogger<RssDataAccess> logger;

        public RssDataAccess(IDbContextFactory<ExploreGetRssFeedContext> dbFactory, ILogger<RssDataAccess> logger)
        {
            this.dbFactory = dbFactory;
            this.logger = logger;
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
            using var context = dbFactory.CreateDbContext();

            var dto = new FeedEntryDataModel
            {
                Title = title,
                RouteName = pathUrl,
                WebAddress = webAddress
            };

            context.FeedEntryDataModels.Add(dto);
            return await context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves all feed entries from the database.
        /// </summary>
        /// <returns>A list of FeedModel</returns>
        public async Task<IEnumerable<FeedEntryModel>> GetAllAsync()
        {
            using var context = dbFactory.CreateDbContext();
            var dtoList = await context.FeedEntryDataModels.AsNoTracking().ToListAsync();

            return dtoList.Select(
                dto => FeedEntryModel.Create(
                    dto.Title,
                    dto.RouteName,
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
            using var context = dbFactory.CreateDbContext();
            var dtoToUpdate = await context.FeedEntryDataModels
                .FirstOrDefaultAsync(feedEntry =>
                    feedEntry.Title == existingItem.Title);

            if (dtoToUpdate is null)
            {
                logger.LogWarning("No item with Title {title} was found, no update performed.", existingItem.Title);
                return 0;
            }

            if (string.IsNullOrWhiteSpace(updatedItem.RouteName) == false
                && dtoToUpdate.RouteName.Equals(updatedItem.RouteName) == false)
            {
                dtoToUpdate.RouteName = updatedItem.RouteName;
            }

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
            using var context = dbFactory.CreateDbContext();

            var itemToRemove = await context.FeedEntryDataModels
                .FirstOrDefaultAsync(feedEntry =>
                    feedEntry.Title == title);

            if (itemToRemove is null)
            {
                logger.LogWarning("No item found to remove.");
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
            using var context = dbFactory.CreateDbContext();

            var foundItem = await context.FeedEntryDataModels
                .FirstOrDefaultAsync(feedEntry =>
                feedEntry.Title == title);

            if (foundItem is not null)
            {
                logger.LogInformation("Item found with title {title}", title);

                // null coalescing operator ?? returns value of left-hand operand if not null
                // otherwise returns the right-hand operand
                var result = FeedEntryModel.Create(
                    foundItem.Title,
                    foundItem.RouteName ?? string.Empty,
                    foundItem.WebAddress ?? string.Empty
                    );

                return result;
            }
            
            logger.LogWarning("No item found with title {title}", title);
            return new FeedEntryModel();
        }

        /// <summary>
        /// Retrieves a specific item from the database using its RouteName.
        /// </summary>
        /// <param name="routeName"></param>
        /// <returns>The FeedEntryModel instance retrieved from the database, or empty if not found</returns>
        public async Task<FeedEntryModel> GetByRouteAsync(string routeName)
        {
            using var context = dbFactory.CreateDbContext();

            var foundItem = await context.FeedEntryDataModels
                .FirstOrDefaultAsync(feedEntry =>
                feedEntry.RouteName == routeName);

            if (foundItem is not null)
            {
                logger.LogInformation("Item found with route name {routename}", routeName);

                // null coalescing operator ?? returns value of left-hand operand if not null
                // otherwise returns the right-hand operand
                var result = FeedEntryModel.Create(
                    foundItem.Title,
                    foundItem.RouteName ?? string.Empty,
                    foundItem.WebAddress ?? string.Empty
                    );

                return result;
            }

            logger.LogWarning("No item found with route name {routename}", routeName);
            return new FeedEntryModel();
        }
    }
}
