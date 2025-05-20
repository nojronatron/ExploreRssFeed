namespace ExploreGetRssFeed.Data
{
    public class DbInitializer
    {
        public static void Initialize(IConfiguration configuration, 
            ExploreGetRssFeedContext context,
            bool isDevelopment)
        {
            // if database is NOT ALREADY created, create and initialize it
            if (context.Database.EnsureCreated())
            {
                if (isDevelopment)
                {
                    // get values from development configuration
                    List<FeedEntryDataModel>? feedEntries = configuration.GetSection("RssFeeds").Get<List<FeedEntryDataModel>>();

                    if (feedEntries is null || feedEntries.Count == 0)
                    {
                        // nothing there or invalid so load static defaults
                        feedEntries = GetDefaultEntries();
                    }

                    // validate the entries
                    ValidateEntries(feedEntries);

                    // track the rane of new entities
                    context.FeedEntryDataModels.AddRange(feedEntries);

                    // discover changes and save to underlying database (blocking call)
                    context.SaveChanges();
                }
                else
                {
                    // production mode, no need to seed the DB
                }
            }
        }

        private static void ValidateEntries(List<FeedEntryDataModel> feedEntries)
        {
            // validate the entries captured from appsettings
            foreach (var feedEntry in feedEntries)
            {
                if (string.IsNullOrWhiteSpace(feedEntry.Title)
                    || string.IsNullOrWhiteSpace(feedEntry.WebAddress)
                    || string.IsNullOrWhiteSpace(feedEntry.RouteName))
                {
                    throw new InvalidOperationException("FeedEntryDataModel is not valid");
                }
            }
        }

        private static List<FeedEntryDataModel> GetDefaultEntries()
        {
            // null or no entries? fill with static defaults
            return new List<FeedEntryDataModel>
                {
                    new() { Title = "Dev Blogs .Net Feed", WebAddress = "https://devblogs.microsoft.com/dotnet/feed/", RouteName = "DevBlogsDotNetFeed" },
                    new() { Title = "NCEI NOAA Feed", WebAddress = "https://www.ncei.noaa.gov/news.xml", RouteName = "NceiNoaaGov" },
                    new() { Title = "Aspireify Feed", WebAddress = "https://aspireify.net/rss", RouteName = "Aspireify", OpenInNewTab = true },
                    new() { Title = "Test Broken Feed", WebAddress = "https://localhost:8080/broken", RouteName = "ErrorFeed" }
                };
        }
    }
}
