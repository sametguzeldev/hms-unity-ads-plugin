using NUnit.Framework;
using SametGuzelDev.HMS.Ads;

namespace SametGuzelDev.HMS.Ads.Tests.Editor
{
    [TestFixture]
    public class BannerAdSizeTests
    {
        [Test]
        public void Banner320x50_ReturnsNonNull()
        {
            Assert.IsNotNull(BannerAdSize.Banner320x50);
        }

        [Test]
        public void Banner320x100_ReturnsNonNull()
        {
            Assert.IsNotNull(BannerAdSize.Banner320x100);
        }

        [Test]
        public void Banner300x250_ReturnsNonNull()
        {
            Assert.IsNotNull(BannerAdSize.Banner300x250);
        }

        [Test]
        public void Banner468x60_ReturnsNonNull()
        {
            Assert.IsNotNull(BannerAdSize.Banner468x60);
        }

        [Test]
        public void Banner728x90_ReturnsNonNull()
        {
            Assert.IsNotNull(BannerAdSize.Banner728x90);
        }

        [Test]
        public void Banner360x57_ReturnsNonNull()
        {
            Assert.IsNotNull(BannerAdSize.Banner360x57);
        }

        [Test]
        public void Banner360x144_ReturnsNonNull()
        {
            Assert.IsNotNull(BannerAdSize.Banner360x144);
        }

        [Test]
        public void SmartBanner_ReturnsNonNull()
        {
            Assert.IsNotNull(BannerAdSize.SmartBanner);
        }
    }
}
