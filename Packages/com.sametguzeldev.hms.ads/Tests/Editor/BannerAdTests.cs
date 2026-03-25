using System;
using NUnit.Framework;
using SametGuzelDev.HMS.Ads;

namespace SametGuzelDev.HMS.Ads.Tests.Editor
{
    [TestFixture]
    public class BannerAdTests
    {
        private HMSBannerAd _ad;

        [SetUp]
        public void SetUp()
        {
            _ad = new HMSBannerAd("test-banner-id");
        }

        [TearDown]
        public void TearDown()
        {
            _ad?.Dispose();
        }

        [Test]
        public void Constructor_Succeeds()
        {
            Assert.IsNotNull(_ad);
        }

        [Test]
        public void Constructor_WithSizeAndPosition_Succeeds()
        {
            using var ad = new HMSBannerAd("test-id", BannerAdSize.Banner320x100, BannerPosition.Top);
            Assert.IsNotNull(ad);
        }

        [Test]
        public void LoadAd_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _ad.LoadAd());
        }

        [Test]
        public void Show_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _ad.Show());
        }

        [Test]
        public void Hide_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _ad.Hide());
        }

        [Test]
        public void Dispose_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _ad.Dispose());
        }

        [Test]
        public void Dispose_CalledTwice_DoesNotThrow()
        {
            _ad.Dispose();
            Assert.DoesNotThrow(() => _ad.Dispose());
        }

        [Test]
        public void Events_CanSubscribeAndUnsubscribe()
        {
            Action onLoaded = () => { };
            Action<int> onFailed = _ => { };
            Action onClosed = () => { };

            Assert.DoesNotThrow(() =>
            {
                _ad.OnAdLoaded += onLoaded;
                _ad.OnAdFailed += onFailed;
                _ad.OnAdClosed += onClosed;

                _ad.OnAdLoaded -= onLoaded;
                _ad.OnAdFailed -= onFailed;
                _ad.OnAdClosed -= onClosed;
            });
        }
    }
}
