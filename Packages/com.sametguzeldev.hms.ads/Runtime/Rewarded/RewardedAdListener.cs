#if UNITY_ANDROID
using System;
using UnityEngine.Scripting;

namespace SametGuzelDev.HMS.Ads
{
    /// <summary>
    /// <c>AndroidJavaProxy</c> that implements the <c>IRewardAdLoadCallback</c> Java interface
    /// (bridge AAR). Handles ad load lifecycle callbacks and dispatches them to the Unity
    /// main thread via <see cref="UnityMainThreadDispatcher"/>.
    /// </summary>
    internal sealed class RewardAdLoadListener : AndroidJavaProxy
    {
        /// <summary>Fired when the rewarded ad finishes loading.</summary>
        public event Action OnRewardedLoaded;

        /// <summary>Fired when the rewarded ad fails to load. HMS error code is passed.</summary>
        public event Action<int> OnRewardAdFailedToLoad;

        internal RewardAdLoadListener()
            : base("com.sametguzeldev.hms.ads.bridge.IRewardAdLoadCallback") { }

        [Preserve]
        public void onRewardedLoaded() =>
            UnityMainThreadDispatcher.Enqueue(() => OnRewardedLoaded?.Invoke());

        [Preserve]
        public void onRewardAdFailedToLoad(int errorCode) =>
            UnityMainThreadDispatcher.Enqueue(() => OnRewardAdFailedToLoad?.Invoke(errorCode));
    }

    /// <summary>
    /// <c>AndroidJavaProxy</c> that implements the <c>IRewardAdStatusCallback</c> Java interface
    /// (bridge AAR). Handles ad show lifecycle callbacks — including the reward grant — and
    /// dispatches them to the Unity main thread.
    /// </summary>
    internal sealed class RewardAdStatusListener : AndroidJavaProxy
    {
        /// <summary>Fired when the rewarded ad is opened/displayed to the user.</summary>
        public event Action OnRewardAdOpened;

        /// <summary>Fired when the rewarded ad overlay is closed.</summary>
        public event Action OnRewardAdClosed;

        /// <summary>Fired when the user has earned the reward.</summary>
        public event Action<Reward> OnRewarded;

        /// <summary>Fired when the rewarded ad fails to display. HMS error code is passed.</summary>
        public event Action<int> OnRewardAdFailedToShow;

        internal RewardAdStatusListener()
            : base("com.sametguzeldev.hms.ads.bridge.IRewardAdStatusCallback") { }

        [Preserve]
        public void onRewardAdOpened() =>
            UnityMainThreadDispatcher.Enqueue(() => OnRewardAdOpened?.Invoke());

        [Preserve]
        public void onRewardAdClosed() =>
            UnityMainThreadDispatcher.Enqueue(() => OnRewardAdClosed?.Invoke());

        [Preserve]
        public void onRewarded(AndroidJavaObject rewardJava)
        {
            // Extract reward data before crossing thread boundary to avoid JNI calls on main thread
            // from a captured reference that may have been disposed.
            string name   = rewardJava?.Call<string>("getName") ?? string.Empty;
            int    amount = rewardJava?.Call<int>("getAmount")   ?? 0;
            var    reward = new Reward(name, amount);

            UnityMainThreadDispatcher.Enqueue(() => OnRewarded?.Invoke(reward));
        }

        [Preserve]
        public void onRewardAdFailedToShow(int errorCode) =>
            UnityMainThreadDispatcher.Enqueue(() => OnRewardAdFailedToShow?.Invoke(errorCode));
    }
}
#endif
