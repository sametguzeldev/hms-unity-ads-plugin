using NUnit.Framework;
using UnityEngine;
using SametGuzelDev.HMS.Ads;

namespace SametGuzelDev.HMS.Ads.Tests.Editor
{
    [TestFixture]
    public class HMSAdsSettingsTests
    {
        [Test]
        public void TestInterstitialAdUnitId_IsNotEmpty()
        {
            Assert.IsFalse(string.IsNullOrEmpty(HMSAdsSettings.TestInterstitialAdUnitId));
        }

        [Test]
        public void TestBannerAdUnitId_IsNotEmpty()
        {
            Assert.IsFalse(string.IsNullOrEmpty(HMSAdsSettings.TestBannerAdUnitId));
        }

        [Test]
        public void TestRewardedAdUnitId_IsNotEmpty()
        {
            Assert.IsFalse(string.IsNullOrEmpty(HMSAdsSettings.TestRewardedAdUnitId));
        }

        [Test]
        public void CreateInstance_Succeeds()
        {
            var settings = ScriptableObject.CreateInstance<HMSAdsSettings>();

            Assert.IsNotNull(settings);

            Object.DestroyImmediate(settings);
        }

        [Test]
        public void UseTestAds_DefaultsToTrue()
        {
            var settings = ScriptableObject.CreateInstance<HMSAdsSettings>();

            Assert.IsTrue(settings.UseTestAds);

            Object.DestroyImmediate(settings);
        }

        [Test]
        public void EffectiveInterstitialId_ReturnsTestId_WhenUseTestAdsTrue()
        {
            var settings = ScriptableObject.CreateInstance<HMSAdsSettings>();

            Assert.AreEqual(HMSAdsSettings.TestInterstitialAdUnitId, settings.EffectiveInterstitialId);

            Object.DestroyImmediate(settings);
        }

        [Test]
        public void EffectiveBannerId_ReturnsTestId_WhenUseTestAdsTrue()
        {
            var settings = ScriptableObject.CreateInstance<HMSAdsSettings>();

            Assert.AreEqual(HMSAdsSettings.TestBannerAdUnitId, settings.EffectiveBannerId);

            Object.DestroyImmediate(settings);
        }

        [Test]
        public void EffectiveRewardedId_ReturnsTestId_WhenUseTestAdsTrue()
        {
            var settings = ScriptableObject.CreateInstance<HMSAdsSettings>();

            Assert.AreEqual(HMSAdsSettings.TestRewardedAdUnitId, settings.EffectiveRewardedId);

            Object.DestroyImmediate(settings);
        }
    }
}
