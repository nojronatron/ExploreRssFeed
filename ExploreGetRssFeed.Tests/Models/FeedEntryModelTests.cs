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

        [Fact]
        public void CreateReturnsInstanceWithCorrectProperties()
        {
            string expectedTitle = "Feed Title";
            string expectedWebAddress = "https://somefeed.net/rss";
            string expectedRouteName = "/displayfeed/Feed Title";

            FeedEntryModel sut = FeedEntryModel
                .Create(
                    expectedTitle,
                    expectedWebAddress
                );

            Assert.NotNull(sut);
            Assert.NotNull(sut.Title);
            Assert.NotEmpty(sut.Title);
            Assert.NotNull(sut.WebAddress);
            Assert.NotEmpty(sut.WebAddress);
            Assert.Equal(expectedTitle, sut.Title);
            Assert.Equal(expectedWebAddress, sut.WebAddress);
            Assert.Equal(expectedRouteName, FeedEntryModel.GetRouteName(sut.Title!));
        }
    }
}
