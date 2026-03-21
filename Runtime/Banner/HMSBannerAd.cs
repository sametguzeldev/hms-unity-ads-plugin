using System;
#if UNITY_ANDROID
using UnityEngine;
#endif

namespace SametGuzelDev.HMS.Ads
{
    /// <summary>Screen position for banner ad placement.</summary>
    public enum BannerPosition
    {
        /// <summary>Anchors the banner to the top of the screen.</summary>
        Top,

        /// <summary>Anchors the banner to the bottom of the screen.</summary>
        Bottom
    }

    /// <summary>
    /// Wraps the HMS <c>BannerView</c> Java class, providing a C# event-driven API.
    /// The banner view is added to the activity's root layout automatically on <see cref="LoadAd"/>.
    /// <para>
    /// Usage:
    /// <code>
    /// var banner = HMSAdsManager.CreateBannerAd("your-ad-unit-id", BannerAdSize.Banner320x50);
    /// banner.OnAdLoaded += () => Debug.Log("Banner loaded");
    /// banner.LoadAd();
    /// </code>
    /// </para>
    /// Call <see cref="Dispose"/> to remove the view from the layout and release JNI resources.
    /// </summary>
    public sealed class HMSBannerAd : IDisposable
    {
        /// <summary>Fired on the Unity main thread when the banner ad loads successfully.</summary>
        public event Action OnAdLoaded;

        /// <summary>Fired on the Unity main thread when the banner ad fails to load. HMS error code is passed.</summary>
        public event Action<int> OnAdFailed;

        /// <summary>Fired on the Unity main thread when the banner ad overlay is dismissed.</summary>
        public event Action OnAdClosed;

        /// <summary>Fired on the Unity main thread when the user taps the banner ad.</summary>
        public event Action OnAdClicked;

        /// <summary>Fired on the Unity main thread when the banner ad overlay opens.</summary>
        public event Action OnAdOpened;

        /// <summary>Fired on the Unity main thread when the user leaves the app via the banner.</summary>
        public event Action OnAdLeave;

#if UNITY_ANDROID
        private AndroidJavaObject _bannerView;
        private BannerAdListener _listener;
        private bool _disposed;

        private readonly string _adUnitId;
        private readonly BannerAdSize _bannerSize;
        private readonly BannerPosition _position;

        // android.widget.FrameLayout.LayoutParams gravity constants
        private const int GravityTop              = 0x30;
        private const int GravityBottom           = 0x50;
        private const int GravityCenterHorizontal = 0x01;

        // android.view.View visibility constants
        private const int ViewVisible = 0;
        private const int ViewGone    = 8;

        // android.R.id.content (the root content FrameLayout of every Activity)
        private const int AndroidRIdContent = 0x01020014;

        /// <summary>Creates a banner ad for the given ad unit ID.</summary>
        /// <param name="adUnitId">Your HMS Ads banner ad unit ID.</param>
        /// <param name="size">Banner size. Defaults to <see cref="BannerAdSize.Banner320x50"/>.</param>
        /// <param name="position">Screen anchor position. Defaults to <see cref="BannerPosition.Bottom"/>.</param>
        public HMSBannerAd(string adUnitId, BannerAdSize size = null,
            BannerPosition position = BannerPosition.Bottom)
        {
            _adUnitId   = adUnitId;
            _bannerSize = size ?? BannerAdSize.Banner320x50;
            _position   = position;

            _listener = new BannerAdListener();
            _listener.OnAdLoaded  += () => OnAdLoaded?.Invoke();
            _listener.OnAdFailed  += code => OnAdFailed?.Invoke(code);
            _listener.OnAdClosed  += () => OnAdClosed?.Invoke();
            _listener.OnAdClicked += () => OnAdClicked?.Invoke();
            _listener.OnAdOpened  += () => OnAdOpened?.Invoke();
            _listener.OnAdLeave   += () => OnAdLeave?.Invoke();
        }

        /// <summary>
        /// Loads the banner ad and adds the <c>BannerView</c> to the activity layout.
        /// Subsequent calls to <see cref="LoadAd"/> will refresh the banner in place.
        /// </summary>
        /// <param name="adParam">Optional ad targeting parameters. Uses defaults when null.</param>
        public void LoadAd(AdParam adParam = null)
        {
            adParam ??= AdParam.Default;
            var param = adParam;    // capture for lambda

            using var activity = AndroidContext.Activity;
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                if (_bannerView == null)
                {
                    using var act = AndroidContext.Activity;
                    _bannerView = new AndroidJavaObject("com.huawei.hms.ads.banner.BannerView", act);
                    _bannerView.Call("setAdId", _adUnitId);
                    _bannerView.Call("setBannerAdSize", _bannerSize.JavaObject);

                    // BridgeAdListener extends HMS AdListener and delegates to our IAdCallback proxy.
                    using var bridge = new AndroidJavaObject(
                        "com.sametguzeldev.hms.ads.bridge.BridgeAdListener", _listener);
                    _bannerView.Call("setAdListener", bridge);

                    // Attach the view to the activity's root FrameLayout with the requested gravity.
                    int gravity = (_position == BannerPosition.Bottom)
                        ? GravityBottom | GravityCenterHorizontal
                        : GravityTop    | GravityCenterHorizontal;

                    using var layoutParams = new AndroidJavaObject(
                        "android.widget.FrameLayout$LayoutParams",
                        -2,      // WRAP_CONTENT width
                        -2,      // WRAP_CONTENT height
                        gravity);

                    using var window     = act.Call<AndroidJavaObject>("getWindow");
                    using var decorView  = window.Call<AndroidJavaObject>("getDecorView");
                    using var rootLayout = decorView.Call<AndroidJavaObject>(
                        "findViewById", AndroidRIdContent);

                    rootLayout?.Call("addView", _bannerView, layoutParams);
                }

                _bannerView.Call("loadAd", param.JavaObject);
            }));
        }

        /// <summary>Makes the banner view visible (after it has been hidden with <see cref="Hide"/>).</summary>
        public void Show()
        {
            using var activity = AndroidContext.Activity;
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _bannerView?.Call("setVisibility", ViewVisible);
            }));
        }

        /// <summary>Hides the banner view without destroying it or stopping the ad cycle.</summary>
        public void Hide()
        {
            using var activity = AndroidContext.Activity;
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                _bannerView?.Call("setVisibility", ViewGone);
            }));
        }

        /// <summary>
        /// Destroys the banner view, removes it from the layout, and releases JNI resources.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            using var activity = AndroidContext.Activity;
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                if (_bannerView == null) return;

                _bannerView.Call("destroy");

                using var parent = _bannerView.Call<AndroidJavaObject>("getParent");
                parent?.Call("removeView", _bannerView);

                _bannerView.Dispose();
                _bannerView = null;
            }));

            _listener = null;
        }
#else
        /// <summary>Creates a banner ad for the given ad unit ID.</summary>
        public HMSBannerAd(string adUnitId, BannerAdSize size = null,
            BannerPosition position = BannerPosition.Bottom) { }

        /// <summary>Loads the banner ad.</summary>
        public void LoadAd(AdParam adParam = null) { }

        /// <summary>Shows the banner view.</summary>
        public void Show() { }

        /// <summary>Hides the banner view.</summary>
        public void Hide() { }

        /// <inheritdoc/>
        public void Dispose() { }
#endif
    }
}
