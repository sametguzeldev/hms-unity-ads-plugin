using System;
#if UNITY_ANDROID
using UnityEngine;
#endif

namespace SametGuzelDev.HMS.Ads
{
    /// <summary>
    /// Wraps the HMS <c>RewardAd</c> Java class, providing a C# event-driven API.
    /// <para>
    /// Usage:
    /// <code>
    /// var ad = HMSAdsManager.CreateRewardedAd("your-ad-unit-id");
    /// ad.OnAdLoaded  += () => ad.Show();
    /// ad.OnRewarded  += reward => GivePlayerReward(reward.Name, reward.Amount);
    /// ad.OnAdClosed  += () => ad.LoadAd();   // pre-load next ad
    /// ad.LoadAd();
    /// </code>
    /// </para>
    /// Dispose when no longer needed to release JNI resources.
    /// </summary>
    public sealed class HMSRewardedAd : IDisposable
    {
        /// <summary>Fired on the Unity main thread when the ad is loaded and ready to show.</summary>
        public event Action OnAdLoaded;

        /// <summary>Fired on the Unity main thread when the ad fails to load. HMS error code is passed.</summary>
        public event Action<int> OnAdFailedToLoad;

        /// <summary>Fired on the Unity main thread when the ad overlay is displayed.</summary>
        public event Action OnAdOpened;

        /// <summary>Fired on the Unity main thread when the ad overlay is closed.</summary>
        public event Action OnAdClosed;

        /// <summary>Fired on the Unity main thread when the user earns a reward.</summary>
        public event Action<Reward> OnRewarded;

        /// <summary>Fired on the Unity main thread when the ad fails to display. HMS error code is passed.</summary>
        public event Action<int> OnAdFailedToShow;

#if UNITY_ANDROID
        private AndroidJavaObject _rewardAd;
        private RewardAdLoadListener _loadListener;
        private RewardAdStatusListener _statusListener;
        private bool _disposed;

        /// <summary>Creates a rewarded ad wrapper for the given ad unit ID.</summary>
        /// <param name="adUnitId">Your HMS Ads rewarded ad unit ID.</param>
        public HMSRewardedAd(string adUnitId)
        {
            _loadListener = new RewardAdLoadListener();
            _loadListener.OnRewardedLoaded      += () => OnAdLoaded?.Invoke();
            _loadListener.OnRewardAdFailedToLoad += code => OnAdFailedToLoad?.Invoke(code);

            _statusListener = new RewardAdStatusListener();
            _statusListener.OnRewardAdOpened       += () => OnAdOpened?.Invoke();
            _statusListener.OnRewardAdClosed       += () => OnAdClosed?.Invoke();
            _statusListener.OnRewarded             += reward => OnRewarded?.Invoke(reward);
            _statusListener.OnRewardAdFailedToShow += code => OnAdFailedToShow?.Invoke(code);

            using var activity = AndroidContext.Activity;
            _rewardAd = new AndroidJavaObject(
                "com.huawei.hms.ads.reward.RewardAd", activity, adUnitId);
        }

        /// <summary>Returns <c>true</c> if the ad is loaded and ready to be shown.</summary>
        public bool IsLoaded => _rewardAd?.Call<bool>("isLoaded") ?? false;

        /// <summary>
        /// Starts loading the rewarded ad. Subscribe to <see cref="OnAdLoaded"/> before calling this.
        /// </summary>
        /// <param name="adParam">Optional ad targeting parameters. Uses defaults when null.</param>
        public void LoadAd(AdParam adParam = null)
        {
            adParam ??= AdParam.Default;

            // BridgeRewardAdLoadListener extends HMS RewardAdLoadListener and delegates
            // to our IRewardAdLoadCallback proxy (_loadListener). The 'using' here is safe:
            // HMS holds a reference to the Java bridge during the load request.
            using var loadBridge = new AndroidJavaObject(
                "com.sametguzeldev.hms.ads.bridge.BridgeRewardAdLoadListener", _loadListener);
            _rewardAd?.Call("loadAd", adParam.JavaObject, loadBridge);
        }

        /// <summary>
        /// Displays the rewarded ad. Only call after <see cref="OnAdLoaded"/> fires.
        /// The user must watch the ad to completion before <see cref="OnRewarded"/> fires.
        /// </summary>
        public void Show()
        {
            if (!IsLoaded)
            {
                Debug.LogWarning("[HMS Ads] Rewarded ad is not loaded yet. Call LoadAd() first.");
                return;
            }

            // BridgeRewardAdStatusListener extends HMS RewardAdStatusListener and delegates
            // to our IRewardAdStatusCallback proxy (_statusListener).
            using var activity     = AndroidContext.Activity;
            using var statusBridge = new AndroidJavaObject(
                "com.sametguzeldev.hms.ads.bridge.BridgeRewardAdStatusListener", _statusListener);
            _rewardAd?.Call("show", activity, statusBridge);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_disposed) return;
            _rewardAd?.Dispose();
            _rewardAd       = null;
            _loadListener   = null;
            _statusListener = null;
            _disposed = true;
        }
#else
        /// <summary>Creates a rewarded ad wrapper for the given ad unit ID.</summary>
        public HMSRewardedAd(string adUnitId) { }

        /// <summary>Returns <c>true</c> if the ad is loaded and ready to be shown.</summary>
        public bool IsLoaded => false;

        /// <summary>Starts loading the rewarded ad.</summary>
        public void LoadAd(AdParam adParam = null) { }

        /// <summary>Displays the rewarded ad.</summary>
        public void Show() { }

        /// <inheritdoc/>
        public void Dispose() { }
#endif
    }
}
