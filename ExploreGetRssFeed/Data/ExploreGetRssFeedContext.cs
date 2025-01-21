using Microsoft.EntityFrameworkCore;

namespace ExploreGetRssFeed.Data
{
    public class ExploreGetRssFeedContext : DbContext
    {
        public ExploreGetRssFeedContext (DbContextOptions<ExploreGetRssFeedContext> options)
            : base(options)
        {
        }

        public DbSet<FeedEntryDataModel> FeedEntryDataModels { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FeedEntryDataModel>().ToTable("RssFeedEntry");
        }
    }
}
