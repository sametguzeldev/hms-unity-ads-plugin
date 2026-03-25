using UnityEngine;

namespace SametGuzelDev.HMS.Ads.Editor
{
    /// <summary>
    /// ScriptableObject for configuring HMS Ads ad unit IDs and global settings.
    /// <para>
    /// Create an instance via <b>Assets &gt; Create &gt; HMS Ads &gt; Settings</b>.
    /// Place it in a <c>Resources</c> folder to load it at runtime with
    /// <c>Resources.Load&lt;HMSAdsSettings&gt;("HMSAdsSettings")</c>.
    /// </para>
    /// </summary>
    [CreateAssetMenu(fileName = "HMSAdsSettings", menuName = "HMS Ads/Settings", order = 0)]
    public sealed class HMSAdsSettings : ScriptableObject
    {
        // -----------------------------------------------------------------------------------------
        // Test ad unit IDs provided by Huawei — safe to use during development
        // -----------------------------------------------------------------------------------------

        /// <summary>Official HMS test ad unit ID for interstitial ads.</summary>
        public const string TestInterstitialAdUnitId = "testb4znbuh3n2";

        /// <summary>Official HMS test ad unit ID for banner ads.</summary>
        public const string TestBannerAdUnitId = "testw6vs28auh3";

        /// <summary>Official HMS test ad unit ID for rewarded ads.</summary>
        public const string TestRewardedAdUnitId = "testx9dtjwj8hp";

        // -----------------------------------------------------------------------------------------
        // Inspector-configurable fields
        // -----------------------------------------------------------------------------------------

        [Header("Test Mode")]
        [Tooltip("When enabled, the plugin uses the official HMS test ad unit IDs instead of " +
                 "the production IDs below. DISABLE before publishing to AppGallery.")]
        [SerializeField] private bool useTestAds = true;

        [Header("Interstitial Ad")]
        [Tooltip("Production ad unit ID for interstitial ads (AppGallery Connect > Monetization > Ads).")]
        [SerializeField] private string interstitialAdUnitId = string.Empty;

        [Header("Banner Ad")]
        [Tooltip("Production ad unit ID for banner ads.")]
        [SerializeField] private string bannerAdUnitId = string.Empty;

        [Header("Rewarded Ad")]
        [Tooltip("Production ad unit ID for rewarded ads.")]
        [SerializeField] private string rewardedAdUnitId = string.Empty;

        // -----------------------------------------------------------------------------------------
        // Public accessors — automatically switch between test and production IDs
        // -----------------------------------------------------------------------------------------

        /// <summary>Whether test ad unit IDs are currently active.</summary>
        public bool UseTestAds => useTestAds;

        /// <summary>
        /// Returns the effective interstitial ad unit ID.
        /// Returns <see cref="TestInterstitialAdUnitId"/> when <see cref="UseTestAds"/> is true.
        /// </summary>
        public string EffectiveInterstitialId =>
            useTestAds ? TestInterstitialAdUnitId : interstitialAdUnitId;

        /// <summary>
        /// Returns the effective banner ad unit ID.
        /// Returns <see cref="TestBannerAdUnitId"/> when <see cref="UseTestAds"/> is true.
        /// </summary>
        public string EffectiveBannerId =>
            useTestAds ? TestBannerAdUnitId : bannerAdUnitId;

        /// <summary>
        /// Returns the effective rewarded ad unit ID.
        /// Returns <see cref="TestRewardedAdUnitId"/> when <see cref="UseTestAds"/> is true.
        /// </summary>
        public string EffectiveRewardedId =>
            useTestAds ? TestRewardedAdUnitId : rewardedAdUnitId;

        private void OnValidate()
        {
            if (useTestAds) return;

            if (string.IsNullOrWhiteSpace(interstitialAdUnitId))
                Debug.LogWarning("[HMS Ads] Interstitial ad unit ID is empty and test mode is off.", this);
            if (string.IsNullOrWhiteSpace(bannerAdUnitId))
                Debug.LogWarning("[HMS Ads] Banner ad unit ID is empty and test mode is off.", this);
            if (string.IsNullOrWhiteSpace(rewardedAdUnitId))
                Debug.LogWarning("[HMS Ads] Rewarded ad unit ID is empty and test mode is off.", this);
        }
    }
}
