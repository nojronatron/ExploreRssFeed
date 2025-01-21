using System.ComponentModel.DataAnnotations;

namespace ExploreGetRssFeed.Models
{
    public class FeedEntryModel
    {
        [Required(ErrorMessage = "Make up a short title for this RSS Feed.")]
        [StringLength(maximumLength: 30, MinimumLength = 3)]
        public string? Title { get; set; }

        [Required]
        [Url(ErrorMessage = "The url does not match an expected format.")]
        public string? WebAddress { get; set; }

        public string? RouteName { get; set; }

        public string BaseUrl = "/rssfeeds/";

        // Ensures a path is returned even if RouteName is null
        public string PathUrl
        {
            get
            {
                return RouteName is null
                    ? string.Concat(BaseUrl, Title!.Replace(" ", ""))
                    : string.Concat(BaseUrl, RouteName);
            }
        }

        public string ToRow()
        {
            return $"{Title}: {WebAddress}";
        }

        public static FeedEntryModel Create(string title, string routeName, string webAddress)
        {
            return new FeedEntryModel
            {
                Title = title,
                RouteName = routeName,
                WebAddress = webAddress
            };
        }
    }
}
