using ExploreGetRssFeed.Models;

namespace ExploreGetRssFeed.Tests.Models
{
    public class FeedEntryModelTests
    {
        [Fact]
        public void FeedEntryModel_ExpectedInstantiationState()
        {
            string expectedTitle = "Feed name";
            string expectedRouteName = "BlazorRoute";
            string expectedWebAddress = "https://somefeed.net/rss";
            string expectedBaseUrl = "/displayfeed/"; // hard-coded in the model
            string expectedPathUrl = $"{expectedBaseUrl}{expectedRouteName}";
            string expectedToRowString = $"{expectedTitle}: {expectedPathUrl}";

            FeedEntryModel sut = FeedEntryModel
                .Create(
                    expectedTitle,
                    expectedRouteName,
                    expectedWebAddress
                );

            Assert.Equal(expectedTitle, sut.Title);
            Assert.Equal(expectedWebAddress, sut.WebAddress);
            Assert.Equal(expectedRouteName, sut.RouteName);
            Assert.Equal(expectedBaseUrl, sut.BaseUrl);
            Assert.Equal(expectedPathUrl, sut.PathUrl);
            Assert.Equal(expectedToRowString, sut.ToRow());
        }
    }
}