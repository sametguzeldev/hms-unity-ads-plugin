using UnityEngine;

namespace SametGuzelDev.HMS.Ads
{
    /// <summary>
    /// Main entry point for the HMS Ads Kit plugin.
    /// <para>
    /// Call <see cref="Initialize"/> once at app startup, then use the factory methods
    /// (<see cref="CreateInterstitialAd"/>, <see cref="CreateBannerAd"/>, <see cref="CreateRewardedAd"/>)
    /// to create and manage ad instances.
    /// </para>
    /// <example>
    /// <code>
    /// void Start()
    /// {
    ///     HMSAdsManager.Initialize();
    ///
    ///     var interstitial = HMSAdsManager.CreateInterstitialAd("your-ad-unit-id");
    ///     interstitial.OnAdLoaded += () => interstitial.Show();
    ///     interstitial.LoadAd();
    /// }
    /// </code>
    /// </example>
    /// </summary>
    public static class HMSAdsManager
    {
        private static bool _initialized;

        /// <summary>
        /// Initialises the HMS Ads SDK. Must be called once before requesting any ads,
        /// typically in your first scene's <c>Awake</c> or <c>Start</c>.
        /// Calling this more than once is safe — subsequent calls are no-ops.
        /// </summary>
        public static void Initialize()
        {
#if UNITY_ANDROID
            if (_initialized) return;

            using var hwAds    = new AndroidJavaClass("com.huawei.hms.ads.HwAds");
            using var activity = AndroidContext.Activity;
            hwAds.CallStatic("init", activity);

            _initialized = true;
            Debug.Log("[HMS Ads] SDK initialized.");
#else
            _initialized = true;
            Debug.Log("[HMS Ads] SDK initialized (Editor stub — no-op on non-Android platforms).");
#endif
        }

        /// <summary>
        /// Creates a new <see cref="HMSInterstitialAd"/> for the given ad unit ID.
        /// </summary>
        /// <param name="adUnitId">
        /// Your HMS Ads interstitial ad unit ID from AppGallery Connect.
        /// Use <c>HMSAdsSettings.TestInterstitialAdUnitId</c> during development.
        /// </param>
        public static HMSInterstitialAd CreateInterstitialAd(string adUnitId)
        {
            WarnIfNotInitialized();
            return new HMSInterstitialAd(adUnitId);
        }

        /// <summary>
        /// Creates a new <see cref="HMSBannerAd"/> for the given ad unit ID.
        /// </summary>
        /// <param name="adUnitId">Your HMS Ads banner ad unit ID.</param>
        /// <param name="size">
        /// Banner size. Defaults to <see cref="BannerAdSize.Banner320x50"/> when null.
        /// </param>
        /// <param name="position">
        /// Screen anchor. Defaults to <see cref="BannerPosition.Bottom"/>.
        /// </param>
        public static HMSBannerAd CreateBannerAd(string adUnitId,
            BannerAdSize size = null,
            BannerPosition position = BannerPosition.Bottom)
        {
            WarnIfNotInitialized();
            return new HMSBannerAd(adUnitId, size, position);
        }

        /// <summary>
        /// Creates a new <see cref="HMSRewardedAd"/> for the given ad unit ID.
        /// </summary>
        /// <param name="adUnitId">Your HMS Ads rewarded ad unit ID.</param>
        public static HMSRewardedAd CreateRewardedAd(string adUnitId)
        {
            WarnIfNotInitialized();
            return new HMSRewardedAd(adUnitId);
        }

        private static void WarnIfNotInitialized()
        {
            if (!_initialized)
                Debug.LogWarning(
                    "[HMS Ads] HMSAdsManager.Initialize() has not been called. " +
                    "Call it before creating ad instances.");
        }
    }
}
