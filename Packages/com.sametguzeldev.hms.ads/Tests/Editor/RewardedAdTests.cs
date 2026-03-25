using System;
using NUnit.Framework;
using SametGuzelDev.HMS.Ads;

namespace SametGuzelDev.HMS.Ads.Tests.Editor
{
    [TestFixture]
    public class RewardedAdTests
    {
        private HMSRewardedAd _ad;

        [SetUp]
        public void SetUp()
        {
            _ad = new HMSRewardedAd("test-rewarded-id");
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
            Action<int> onFailedToLoad = _ => { };
            Action onOpened = () => { };
            Action onClosed = () => { };
            Action<Reward> onRewarded = _ => { };
            Action<int> onFailedToShow = _ => { };

            Assert.DoesNotThrow(() =>
            {
                _ad.OnAdLoaded += onLoaded;
                _ad.OnAdFailedToLoad += onFailedToLoad;
                _ad.OnAdOpened += onOpened;
                _ad.OnAdClosed += onClosed;
                _ad.OnRewarded += onRewarded;
                _ad.OnAdFailedToShow += onFailedToShow;

                _ad.OnAdLoaded -= onLoaded;
                _ad.OnAdFailedToLoad -= onFailedToLoad;
                _ad.OnAdOpened -= onOpened;
                _ad.OnAdClosed -= onClosed;
                _ad.OnRewarded -= onRewarded;
                _ad.OnAdFailedToShow -= onFailedToShow;
            });
        }
    }
}
