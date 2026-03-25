using NUnit.Framework;
using SametGuzelDev.HMS.Ads;

namespace SametGuzelDev.HMS.Ads.Tests.Editor
{
    [TestFixture]
    public class HMSAdsManagerTests
    {
        [Test]
        public void Initialize_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => HMSAdsManager.Initialize());
        }

        [Test]
        public void Initialize_CalledTwice_DoesNotThrow()
        {
            HMSAdsManager.Initialize();
            Assert.DoesNotThrow(() => HMSAdsManager.Initialize());
        }

        [Test]
        public void CreateInterstitialAd_ReturnsNonNull()
        {
            HMSAdsManager.Initialize();
            var ad = HMSAdsManager.CreateInterstitialAd("test-id");

            Assert.IsNotNull(ad);
            ad.Dispose();
        }

        [Test]
        public void CreateBannerAd_ReturnsNonNull()
        {
            HMSAdsManager.Initialize();
            var ad = HMSAdsManager.CreateBannerAd("test-id");

            Assert.IsNotNull(ad);
            ad.Dispose();
        }

        [Test]
        public void CreateBannerAd_WithSizeAndPosition_ReturnsNonNull()
        {
            HMSAdsManager.Initialize();
            var ad = HMSAdsManager.CreateBannerAd("test-id", BannerAdSize.SmartBanner, BannerPosition.Top);

            Assert.IsNotNull(ad);
            ad.Dispose();
        }

        [Test]
        public void CreateRewardedAd_ReturnsNonNull()
        {
            HMSAdsManager.Initialize();
            var ad = HMSAdsManager.CreateRewardedAd("test-id");

            Assert.IsNotNull(ad);
            ad.Dispose();
        }
    }
}
