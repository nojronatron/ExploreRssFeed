using ExploreGetRssFeed.Models;

namespace ExploreGetRssFeed.Tests.Models
{
    public class FeedModelTests
    {
        [Fact]
        public void Create_WithValidParameters_ShouldReturnFeedModel()
        {
            // Arrange
            string title = "Test Title";
            string link = @"https://example.com";
            string pubDate = "2023-10-01";
            string description = "<p>Test Description</p>";

            // Act
            var feedModel = FeedModel.Create(title, link, pubDate, description);

            // Assert
            Assert.Equal("Test Title", feedModel.Title);
            Assert.Equal(@"https://example.com", feedModel.Link);
            Assert.Equal(DateTime.Parse(pubDate), feedModel.PubDate);
            Assert.Equal("Test Description", feedModel.Description);
        }

        [Fact]
        public void Create_WithMissingPubDate_ShouldUseCurrentDate()
        {
            // Arrange
            string title = "Test Title";
            string link = @"https://example.com";
            string description = "<p>Test Description</p>";

            // Act
            var feedModel = FeedModel.Create(title, link, string.Empty, description);

            // Assert
            Assert.Equal("Test Title", feedModel.Title);
            Assert.Equal(@"https://example.com", feedModel.Link);
            Assert.Equal(DateTime.Now.Date, feedModel.PubDate.Date);
            Assert.Equal("Test Description", feedModel.Description);
        }

        [Fact]
        public void CleanDescription_WithEmptyDescription_ShouldReturnDefaultDescription()
        {
            // Arrange
            string description = string.Empty;

            // Act
            string result = FeedModel.CleanDescription(description);

            // Assert
            Assert.Equal("<p>No description</p>", result);
        }

        [Fact]
        public void CleanDescription_WithParagraphTags_ShouldReturnFirstParagraphContent()
        {
            // Arrange
            string description = "<p>First Paragraph</p><p>Second Paragraph</p>";

            // Act
            string result = FeedModel.CleanDescription(description);

            // Assert
            Assert.Equal("First Paragraph", result);
        }

        [Fact]
        public void CleanDescription_WithoutParagraphTags_ShouldReturnTrimmedDescription()
        {
            // Arrange
            string description = "No paragraph tags";

            // Act
            string result = FeedModel.CleanDescription(description);

            // Assert
            Assert.Equal("No paragraph tags", result);
        }

        [Fact]
        public void CleanLink_WithValidLink_ShouldReturnCleanedLink()
        {
            // Arrange
            string link = @"<Link>https://example.com/rss</Link>";

            // Act
            string result = FeedModel.CleanLink(link);

            // Assert
            Assert.Equal(@"https://example.com/rss", result);
        }

        [Fact]
        public void CleanLink_WithEmptyLink_ShouldReturnEmptyString()
        {
            // Arrange
            string link = string.Empty;

            // Act
            string result = FeedModel.CleanLink(link);

            // Assert
            Assert.Equal(string.Empty, result);
        }
    }
}
