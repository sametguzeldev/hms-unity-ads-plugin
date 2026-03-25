#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;

namespace SametGuzelDev.HMS.Ads
{
    /// <summary>
    /// Helper for retrieving the current Unity Android Activity/Context.
    /// </summary>
    internal static class AndroidContext
    {
        /// <summary>
        /// Returns the current Unity player <c>Activity</c> as an <c>AndroidJavaObject</c>.
        /// The caller is responsible for disposing the returned object.
        /// </summary>
        public static AndroidJavaObject Activity
        {
            get
            {
                using var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                return unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            }
        }
    }
}
#endif
