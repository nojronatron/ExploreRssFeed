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
        /// Creates a new entry in the database.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="webAddress"></param>
        /// <param name="pathUrl"></param>
        /// <param name="newTab"></param>
        /// <returns>1 if new item created and stored in the DB, otherwise 0.</returns>
        public async Task<int> CreateAsync(string title, string webAddress, string pathUrl, bool? newTab=false)
        {
            var tabText = newTab == true ? "true" : "false";
            _logger.LogInformation("CreateAsync received title {title}, webAddress {webAddress}, pathUrl {pathurl}, newTab {newTab}", title, webAddress, pathUrl, tabText);

            using var context = _dbFactory.CreateDbContext();

            var dto = new FeedEntryDataModel
            {
                Title = title,
                RouteName = pathUrl,
                WebAddress = webAddress,
                OpenInNewTab = newTab ?? false
            };

            tabText = dto.OpenInNewTab ? "true" : "false";
            _logger.LogInformation("New DTO to store is title {title}, webAddress {webAddress}, pathUrl {pathurl}, newTab {newTab}", title, webAddress, pathUrl, tabText);
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
                    dto.WebAddress,
                    dto.OpenInNewTab
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
            var entityToUpdate = await context.FeedEntryDataModels
                .FirstOrDefaultAsync(feedEntry =>
                    feedEntry.Title == existingItem.Title);

            if (entityToUpdate is null)
            {
                _logger.LogInformation("No item with Title {title} was found, no update performed.", existingItem.Title);
                return 0;
            }

            if (string.IsNullOrWhiteSpace(entityToUpdate.Title)){
                string newTab = entityToUpdate.OpenInNewTab ? "true" : "false";
                _logger.LogError("An item with no title was returned. This database record could be corrupt: {id}, {title}, {webAddr}, {newtab}.",
                    entityToUpdate.Id,
                    existingItem.Title,
                    existingItem.WebAddress,
                    newTab);
                return 0;
            }

            if (string.IsNullOrWhiteSpace(updatedItem.WebAddress) == false
                && entityToUpdate.WebAddress.Equals(updatedItem.WebAddress) == false)
            {
                entityToUpdate.WebAddress = updatedItem.WebAddress;
            }

            if (string.IsNullOrWhiteSpace(updatedItem.Title) == false
                && entityToUpdate.Title.Equals(updatedItem.Title) == false)
            {
                entityToUpdate.Title = updatedItem.Title;
            }

            entityToUpdate.OpenInNewTab = updatedItem.OpenInNewTab;
            entityToUpdate.RouteName = FeedEntryModel.GetRouteName(updatedItem.Title!);
            return await context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates a specific item in the database.
        /// <paramref name="updateProperties"/> is a list of tuples where the first item is the property name and the second item is the new value.
        /// </summary>
        /// <param name="existingItem"></param>
        /// <param name="updateProperties"></param>
        /// <returns>Task that returns 0 if no update, 1 if update succeeded.</returns>
        public async Task<int> UpdateAsync(FeedEntryModel existingItem, Tuple<string,string>[] updateProperties)
        {
            using var context = _dbFactory.CreateDbContext();
            var entityToUpdate = await context.FeedEntryDataModels
                .FirstOrDefaultAsync(feedEntry =>
                    feedEntry.Title == existingItem.Title);

            if (entityToUpdate is null)
            {
                _logger.LogWarning("No item with Title {title} was found, no update performed.", existingItem.Title);
                return 0;
            }

            if (updateProperties.Length < 1) { 
                _logger.LogInformation("No update to make (list of changes was empty).");
                return 0;
            }

            foreach (var item in updateProperties)
            {
                var propertyName = item.Item1;
                var propertyValue = item.Item2;
                if (string.IsNullOrWhiteSpace(propertyValue) == false)
                {
                    switch (propertyName)
                    {
                        case nameof(entityToUpdate.Title):
                            entityToUpdate.Title = propertyValue;
                            break;
                        case nameof(entityToUpdate.WebAddress):
                            entityToUpdate.WebAddress = propertyValue;
                            break;
                        case nameof(entityToUpdate.RouteName):
                            entityToUpdate.RouteName = propertyValue;
                            break;
                        case nameof(entityToUpdate.OpenInNewTab):
                            if (bool.TryParse(propertyValue, out var openInNewTab))
                            {
                                entityToUpdate.OpenInNewTab = openInNewTab;
                            }
                            else
                            {
                                _logger.LogWarning("Could not parse {propertyname} to a boolean value.", propertyName);
                            }
                            break;
                        default:
                            _logger.LogWarning("No match for {propertyname} found.", propertyName);
                            break;
                    }
                }
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
                    foundItem.WebAddress ?? string.Empty,
                    foundItem.OpenInNewTab
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
                    foundItem.WebAddress ?? string.Empty,
                    foundItem.OpenInNewTab
                    );

                return result;
            }

            _logger.LogWarning("No item found with route name {routename}", routeName);
            return new FeedEntryModel();
        }
    }
}
