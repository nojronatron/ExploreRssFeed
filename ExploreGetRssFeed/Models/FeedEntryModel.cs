using System.ComponentModel.DataAnnotations;

namespace ExploreGetRssFeed.Models
{
    public class FeedEntryModel
    {
        // tied to DisplayFeed page
        public static string DefaultDisplayFeedBaseUrl => "/displayfeed/";

        [Required(ErrorMessage = "Make up a short title for this RSS Feed.")]
        [StringLength(maximumLength: 30, MinimumLength = 3)]
        public string? Title { get; set; }

        [Required]
        [Url(ErrorMessage = "The url does not match an expected format.")]
        public string? WebAddress { get; set; }

        public static FeedEntryModel Create(string title, string webAddress)
        {
            return new FeedEntryModel
            {
                Title = title,
                WebAddress = webAddress,
            };
        }

        public static string GetRouteName(string title)
        {
            return string.Concat(DefaultDisplayFeedBaseUrl, title.Trim());
        }
    }
}
