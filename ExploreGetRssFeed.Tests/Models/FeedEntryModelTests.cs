using ExploreGetRssFeed.Models;

namespace ExploreGetRssFeed.Tests.Models
{
    public class FeedEntryModelTests
    {
        [Fact]
        public void DefaultDisplayFeedBaseUrlIsCorrect()
        {
            string expectedBaseUrl = "/displayfeed/"; // hard-coded in the model

            Assert.Equal(expectedBaseUrl, FeedEntryModel.DefaultDisplayFeedBaseUrl);
        }

        [Fact]
        public void GetRouteNameCorrectlyConcatenatesUrlAndTitle()
        {
            string expectedTitle = "Feed Title";
            string expectedRouteName = "/displayfeed/Feed Title";

            Assert.Equal(expectedRouteName, FeedEntryModel.GetRouteName(expectedTitle));
        }

        [Theory]
        [InlineData("Feed Title", "https://somefeed.net/rss", "/displayfeed/Feed Title", true)]
        [InlineData("Another Feed", "https://anotherfeed.net/rss", "/displayfeed/Another Feed", false)]
        public void CreateReturnsInstanceWithCorrectProperties(string expectedTitle, string expectedWebAddress, string expectedRouteName, bool expectedOpenInNewTab)
        {
            FeedEntryModel sut = FeedEntryModel
                .Create(
                    expectedTitle,
                    expectedWebAddress,
                    expectedOpenInNewTab
                );

            Assert.NotNull(sut);
            Assert.NotNull(sut.Title);
            Assert.NotEmpty(sut.Title);
            Assert.NotNull(sut.WebAddress);
            Assert.NotEmpty(sut.WebAddress);
            Assert.Equal(expectedTitle, sut.Title);
            Assert.Equal(expectedWebAddress, sut.WebAddress);
            Assert.Equal(expectedRouteName, FeedEntryModel.GetRouteName(sut.Title!));
            Assert.Equal(expectedOpenInNewTab, sut.OpenInNewTab);
        }
    }
}
