#if UNITY_ANDROID
using UnityEngine;
#endif

namespace SametGuzelDev.HMS.Ads
{
    /// <summary>
    /// Wraps the HMS <c>AdParam</c> Java class. Build an instance using <see cref="Builder"/>.
    /// </summary>
    public sealed class AdParam
    {
#if UNITY_ANDROID
        internal AndroidJavaObject JavaObject { get; }

        private AdParam(AndroidJavaObject javaObject)
        {
            JavaObject = javaObject;
        }
#else
        private AdParam() { }
#endif

        /// <summary>Returns a default <see cref="AdParam"/> with no targeting parameters.</summary>
        public static AdParam Default => new Builder().Build();

        /// <summary>Builder for constructing an <see cref="AdParam"/>.</summary>
        public sealed class Builder
        {
#if UNITY_ANDROID
            private readonly AndroidJavaObject _builder;

            /// <summary>Creates a new AdParam builder.</summary>
            public Builder()
            {
                _builder = new AndroidJavaObject("com.huawei.hms.ads.AdParam$Builder");
            }

            /// <summary>Builds and returns the configured <see cref="AdParam"/>.</summary>
            public AdParam Build()
            {
                var adParam = _builder.Call<AndroidJavaObject>("build");
                return new AdParam(adParam);
            }
#else
            /// <summary>Builds and returns the configured <see cref="AdParam"/>.</summary>
            public AdParam Build() => new AdParam();
#endif
        }
    }
}
