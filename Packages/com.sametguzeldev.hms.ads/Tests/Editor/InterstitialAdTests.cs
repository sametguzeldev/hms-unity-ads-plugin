using System;
using NUnit.Framework;
using SametGuzelDev.HMS.Ads;

namespace SametGuzelDev.HMS.Ads.Tests.Editor
{
    [TestFixture]
    public class InterstitialAdTests
    {
        private HMSInterstitialAd _ad;

        [SetUp]
        public void SetUp()
        {
            _ad = new HMSInterstitialAd("test-interstitial-id");
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
        public void IsLoaded_ReturnsFalse()
        {
            Assert.IsFalse(_ad.IsLoaded);
        }

        [Test]
        public void LoadAd_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _ad.LoadAd());
        }

        [Test]
        public void LoadAd_WithParam_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _ad.LoadAd(AdParam.Default));
        }

        [Test]
        public void Show_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _ad.Show());
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
            Action onClicked = () => { };
            Action onOpened = () => { };
            Action onLeave = () => { };

            Assert.DoesNotThrow(() =>
            {
                _ad.OnAdLoaded += onLoaded;
                _ad.OnAdFailed += onFailed;
                _ad.OnAdClosed += onClosed;
                _ad.OnAdClicked += onClicked;
                _ad.OnAdOpened += onOpened;
                _ad.OnAdLeave += onLeave;

                _ad.OnAdLoaded -= onLoaded;
                _ad.OnAdFailed -= onFailed;
                _ad.OnAdClosed -= onClosed;
                _ad.OnAdClicked -= onClicked;
                _ad.OnAdOpened -= onOpened;
                _ad.OnAdLeave -= onLeave;
            });
        }
    }
}
