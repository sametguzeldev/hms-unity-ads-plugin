#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
#endif

namespace SametGuzelDev.HMS.Ads
{
    /// <summary>
    /// Wraps the HMS <c>BannerAdSize</c> predefined size constants.
    /// Pass an instance to <see cref="HMSBannerAd"/> when creating banner ads.
    /// </summary>
    public sealed class BannerAdSize
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        internal AndroidJavaObject JavaObject { get; }

        private BannerAdSize(AndroidJavaObject javaObject)
        {
            JavaObject = javaObject;
        }

        private static BannerAdSize GetStatic(string fieldName)
        {
            using var bannerAdSizeClass = new AndroidJavaClass("com.huawei.hms.ads.BannerAdSize");
            return new BannerAdSize(bannerAdSizeClass.GetStatic<AndroidJavaObject>(fieldName));
        }

        /// <summary>320 × 50 standard banner.</summary>
        public static BannerAdSize Banner320x50  => GetStatic("BANNER_SIZE_320_50");

        /// <summary>320 × 100 large banner.</summary>
        public static BannerAdSize Banner320x100 => GetStatic("BANNER_SIZE_320_100");

        /// <summary>300 × 250 medium rectangle (MREC).</summary>
        public static BannerAdSize Banner300x250 => GetStatic("BANNER_SIZE_300_250");

        /// <summary>468 × 60 full banner.</summary>
        public static BannerAdSize Banner468x60  => GetStatic("BANNER_SIZE_468_60");

        /// <summary>728 × 90 leaderboard.</summary>
        public static BannerAdSize Banner728x90  => GetStatic("BANNER_SIZE_728_90");

        /// <summary>360 × 57 Huawei-specific banner.</summary>
        public static BannerAdSize Banner360x57  => GetStatic("BANNER_SIZE_360_57");

        /// <summary>360 × 144 Huawei-specific large banner.</summary>
        public static BannerAdSize Banner360x144 => GetStatic("BANNER_SIZE_360_144");

        /// <summary>Dynamic-width smart banner that fits the screen width.</summary>
        public static BannerAdSize SmartBanner    => GetStatic("BANNER_SIZE_SMART");
#else
        private BannerAdSize() { }

        /// <summary>320 × 50 standard banner.</summary>
        public static BannerAdSize Banner320x50  => new BannerAdSize();
        /// <summary>320 × 100 large banner.</summary>
        public static BannerAdSize Banner320x100 => new BannerAdSize();
        /// <summary>300 × 250 medium rectangle (MREC).</summary>
        public static BannerAdSize Banner300x250 => new BannerAdSize();
        /// <summary>468 × 60 full banner.</summary>
        public static BannerAdSize Banner468x60  => new BannerAdSize();
        /// <summary>728 × 90 leaderboard.</summary>
        public static BannerAdSize Banner728x90  => new BannerAdSize();
        /// <summary>360 × 57 Huawei-specific banner.</summary>
        public static BannerAdSize Banner360x57  => new BannerAdSize();
        /// <summary>360 × 144 Huawei-specific large banner.</summary>
        public static BannerAdSize Banner360x144 => new BannerAdSize();
        /// <summary>Dynamic-width smart banner that fits the screen width.</summary>
        public static BannerAdSize SmartBanner    => new BannerAdSize();
#endif
    }
}
