#if UNITY_ANDROID
using System;
using UnityEngine.Scripting;

namespace SametGuzelDev.HMS.Ads
{
    /// <summary>
    /// <c>AndroidJavaProxy</c> that implements the <c>IAdCallback</c> Java interface
    /// (defined in the bridge AAR). Receives ad lifecycle events from the native layer
    /// and dispatches them back to the Unity main thread via <see cref="UnityMainThreadDispatcher"/>.
    /// </summary>
    internal sealed class InterstitialAdListener : AndroidJavaProxy
    {
        /// <summary>Fired when the ad finishes loading.</summary>
        public event Action OnAdLoaded;

        /// <summary>Fired when the ad fails to load. The HMS error code is passed.</summary>
        public event Action<int> OnAdFailed;

        /// <summary>Fired when the ad overlay is dismissed by the user.</summary>
        public event Action OnAdClosed;

        /// <summary>Fired when the user taps the ad.</summary>
        public event Action OnAdClicked;

        /// <summary>Fired when the ad overlay is opened/displayed.</summary>
        public event Action OnAdOpened;

        /// <summary>Fired when the user leaves the app via the ad.</summary>
        public event Action OnAdLeave;

        internal InterstitialAdListener()
            : base("com.sametguzeldev.hms.ads.bridge.IAdCallback") { }

        [Preserve]
        public void onAdLoaded() =>
            UnityMainThreadDispatcher.Enqueue(() => OnAdLoaded?.Invoke());

        [Preserve]
        public void onAdFailed(int errorCode) =>
            UnityMainThreadDispatcher.Enqueue(() => OnAdFailed?.Invoke(errorCode));

        [Preserve]
        public void onAdClosed() =>
            UnityMainThreadDispatcher.Enqueue(() => OnAdClosed?.Invoke());

        [Preserve]
        public void onAdClicked() =>
            UnityMainThreadDispatcher.Enqueue(() => OnAdClicked?.Invoke());

        [Preserve]
        public void onAdOpened() =>
            UnityMainThreadDispatcher.Enqueue(() => OnAdOpened?.Invoke());

        [Preserve]
        public void onAdLeave() =>
            UnityMainThreadDispatcher.Enqueue(() => OnAdLeave?.Invoke());
    }
}
#endif
