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
        public void CleanDescription_WithEmptyDescription_ShouldReturnAltDescription()
        {
            // Arrange
            string expectedDescription = string.Empty;
            string expectedAltDescription = "Alternate Description";

            // Act
            string result = FeedModel.CleanDescription(expectedDescription, expectedAltDescription);

            // Assert
            Assert.Equal(expectedAltDescription, result);
        }

        [Fact]
        public void CleanDescription_WithParagraphTags_ShouldReturnFirstParagraphContent()
        {
            // Arrange
            string expectedDescription = "<p>First Paragraph</p><p>Second Paragraph</p>";
            string expectedAltDescription = "Alternate Description";

            // Act
            string result = FeedModel.CleanDescription(expectedDescription, expectedAltDescription);

            // Assert
            Assert.Equal("First Paragraph", result);
        }

        [Fact]
        public void CleanDescription_WithoutParagraphTags_ShouldReturnTrimmedDescription()
        {
            // Arrange
            string spaceyDescription = "  No paragraph tags   ";
            string expectedDescription = "No paragraph tags";
            string expectedAltDescription = "Alternate Description";

            // Act
            string result = FeedModel.CleanDescription(spaceyDescription, expectedAltDescription);

            // Assert
            Assert.Equal(expectedDescription, result);
        }

        [Fact]
        public void CleanLink_WithValidLink_ShouldReturnCleanedLink()
        {
            // Arrange
            string inputLink = @"<Link>https://example.com/rss</Link>";
            string expectedLink = @"https://example.com/rss";

            // Act
            string result = FeedModel.CleanLink(inputLink);

            // Assert
            Assert.Equal(expectedLink, result);
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
