using System;
#if UNITY_ANDROID
using UnityEngine;
#endif

namespace SametGuzelDev.HMS.Ads
{
    /// <summary>
    /// Wraps the HMS <c>InterstitialAd</c> Java class, providing a C# event-driven API.
    /// <para>
    /// Usage:
    /// <code>
    /// var ad = HMSAdsManager.CreateInterstitialAd("your-ad-unit-id");
    /// ad.OnAdLoaded += () => ad.Show();
    /// ad.LoadAd();
    /// </code>
    /// </para>
    /// Dispose when no longer needed to release JNI resources.
    /// </summary>
    public sealed class HMSInterstitialAd : IDisposable
    {
        /// <summary>Fired on the Unity main thread when the ad is loaded and ready to show.</summary>
        public event Action OnAdLoaded;

        /// <summary>Fired on the Unity main thread when the ad fails to load. HMS error code is passed.</summary>
        public event Action<int> OnAdFailed;

        /// <summary>Fired on the Unity main thread when the ad overlay is closed.</summary>
        public event Action OnAdClosed;

        /// <summary>Fired on the Unity main thread when the user taps the ad.</summary>
        public event Action OnAdClicked;

        /// <summary>Fired on the Unity main thread when the ad overlay is opened/displayed.</summary>
        public event Action OnAdOpened;

        /// <summary>Fired on the Unity main thread when the user leaves the app via the ad.</summary>
        public event Action OnAdLeave;

#if UNITY_ANDROID
        private AndroidJavaObject _interstitialAd;
        private InterstitialAdListener _listener;
        private bool _disposed;

        /// <summary>Creates an interstitial ad wrapper for the given ad unit ID.</summary>
        /// <param name="adUnitId">Your HMS Ads interstitial ad unit ID.</param>
        public HMSInterstitialAd(string adUnitId)
        {
            _listener = new InterstitialAdListener();
            _listener.OnAdLoaded  += () => OnAdLoaded?.Invoke();
            _listener.OnAdFailed  += code => OnAdFailed?.Invoke(code);
            _listener.OnAdClosed  += () => OnAdClosed?.Invoke();
            _listener.OnAdClicked += () => OnAdClicked?.Invoke();
            _listener.OnAdOpened  += () => OnAdOpened?.Invoke();
            _listener.OnAdLeave   += () => OnAdLeave?.Invoke();

            using var activity = AndroidContext.Activity;
            _interstitialAd = new AndroidJavaObject("com.huawei.hms.ads.InterstitialAd", activity);
            _interstitialAd.Call("setAdId", adUnitId);

            // BridgeAdListener extends HMS AdListener (abstract) and delegates to our IAdCallback proxy.
            // The 'using' here is safe: once setAdListener is called, the InterstitialAd Java object
            // holds the only strong reference needed. Our _listener proxy is kept alive as a field.
            using var bridge = new AndroidJavaObject(
                "com.sametguzeldev.hms.ads.bridge.BridgeAdListener", _listener);
            _interstitialAd.Call("setAdListener", bridge);
        }

        /// <summary>Returns <c>true</c> if the ad is loaded and ready to be shown.</summary>
        public bool IsLoaded => _interstitialAd?.Call<bool>("isLoaded") ?? false;

        /// <summary>
        /// Starts loading the ad. Subscribe to <see cref="OnAdLoaded"/> before calling this.
        /// </summary>
        /// <param name="adParam">Optional ad targeting parameters. Uses defaults when null.</param>
        public void LoadAd(AdParam adParam = null)
        {
            adParam ??= AdParam.Default;
            _interstitialAd?.Call("loadAd", adParam.JavaObject);
        }

        /// <summary>
        /// Displays the interstitial ad. Only call after <see cref="OnAdLoaded"/> fires.
        /// </summary>
        public void Show()
        {
            if (!IsLoaded)
            {
                Debug.LogWarning("[HMS Ads] Interstitial ad is not loaded yet. Call LoadAd() first.");
                return;
            }

            using var activity = AndroidContext.Activity;
            _interstitialAd?.Call("show", activity);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_disposed) return;
            _interstitialAd?.Dispose();
            _interstitialAd = null;
            _listener = null;
            _disposed = true;
        }
#else
        /// <summary>Creates an interstitial ad wrapper for the given ad unit ID.</summary>
        public HMSInterstitialAd(string adUnitId) { }

        /// <summary>Returns <c>true</c> if the ad is loaded and ready to be shown.</summary>
        public bool IsLoaded => false;

        /// <summary>Starts loading the ad.</summary>
        public void LoadAd(AdParam adParam = null) { }

        /// <summary>Displays the interstitial ad.</summary>
        public void Show() { }

        /// <inheritdoc/>
        public void Dispose() { }
#endif
    }
}
