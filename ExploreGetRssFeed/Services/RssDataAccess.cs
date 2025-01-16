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
        /// <param name="itemToAdd">The FeedModel to add.</param>
        /// <returns>The number of entries written to the db.</returns>
        public async Task<int> Create(FeedModel itemToAdd)
        {
            using var context = dbFactory.CreateDbContext();
            var dto = new FeedEntryDataModel
            {
                Title = itemToAdd.Title,
                RouteName = itemToAdd.Link,
                WebAddress = itemToAdd.Creator
            };

            context.FeedEntryDataModels.Add(dto);
            return await context.SaveChangesAsync();
        }

        /// <summary>
        /// Creates a new feed entry in the database.
        /// </summary>
        /// <param name="title">The title for the new feed entry.</param>
        /// <param name="webAddress">The web address (target) to the feed.</param>
        /// <param name="routeName">The blazor page directive path.</param>
        /// <returns>The number of entries written to the db.</returns>
        public async Task<int> Create(string title, string webAddress, string routeName)
        {
            using var context = dbFactory.CreateDbContext();

            var dto = new FeedEntryDataModel
            {
                Title = title,
                RouteName = routeName,
                WebAddress = webAddress
            };

            context.FeedEntryDataModels.Add(dto);
            return await context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves all feed entries from the database.
        /// </summary>
        /// <returns>A list of FeedModel.</returns>
        public async Task<IEnumerable<FeedModel>> GetAllEntries()
        {
            using var context = dbFactory.CreateDbContext();
            var dtoList = await context.FeedEntryDataModels.AsNoTracking().ToListAsync();

            return dtoList.Select(
                dto => FeedModel.Create(
                    dto.Title,
                    dto.RouteName,
                    dto.WebAddress
                    ));
        }

        /// <summary>
        /// Update the title of a specific item.
        /// </summary>
        /// <param name="feedItemToChange">The item to update.</param>
        /// <param name="newTitle">The new Title.</param>
        /// <returns>The number of changes written to the database.</returns>
        public async Task<int> UpdateTitle(FeedEntryModel feedItemToChange, string newTitle)
        {
            using var context = dbFactory.CreateDbContext();
            var dtoToUpdate = await context.FeedEntryDataModels
                .FirstOrDefaultAsync(feedEntry =>
                    feedEntry.Title == feedItemToChange.Title
                    && feedEntry.WebAddress == feedItemToChange.WebAddress);

            if (dtoToUpdate is null)
            {
                logger.LogWarning("No item found to update.");
                return 0;
            };

            dtoToUpdate.Title = newTitle;
            return await context.SaveChangesAsync();
        }

        /// <summary>
        /// Update the web address of a specific item.
        /// </summary>
        /// <param name="feedItemToChange">The item to update.</param>
        /// <param name="newWebAddress">The new web address.</param>
        /// <returns>The number of changes written to the database.</returns>
        public async Task<int> UpdateWebAddress(FeedEntryModel feedItemToChange, string newWebAddress)
        {
            using var context = dbFactory.CreateDbContext();
            var dtoToUpdate = await context.FeedEntryDataModels
                .FirstOrDefaultAsync(feedEntry =>
                    feedEntry.Title == feedItemToChange.Title
                    && feedEntry.WebAddress == feedItemToChange.WebAddress);

            if (dtoToUpdate is null)
            {
                logger.LogWarning("No item found to update.");
                return 0;
            };

            dtoToUpdate.WebAddress = newWebAddress;
            return await context.SaveChangesAsync();
        }


        /// <summary>
        /// Removes a specific item from the database. This operation is permanent and not reversable.
        /// </summary>
        /// <param name="itemToRemove">The item to remove.</param>
        /// <returns>The number of items removed from the database.</returns>
        public async Task<int> Remove(FeedEntryModel itemToRemove)
        {
            using var context = dbFactory.CreateDbContext();

            var dtoToRemove = await context.FeedEntryDataModels
                .FirstOrDefaultAsync(feedEntry =>
                    feedEntry.Title == itemToRemove.Title
                    && feedEntry.WebAddress == itemToRemove.WebAddress);

            if (dtoToRemove is null)
            {
                logger.LogWarning("No item found to remove.");
                return 0;
            };

            context.Remove(dtoToRemove);
            return await context.SaveChangesAsync();
        }
    }
}
