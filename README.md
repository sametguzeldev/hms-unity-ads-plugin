# HMS Ads Kit — Unity Plugin

Unity plugin for Huawei Mobile Services (HMS) Ads Kit. Supports **Interstitial**, **Banner**, and **Rewarded** ads via a clean, event-driven C# API.

## Requirements

- Unity 2021.3 or later
- Android build target
- Huawei Mobile Services Core installed on the device
- A Huawei AppGallery Connect project with Ads Kit enabled

## Installation

Add to your project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.sametguzeldev.hms.ads": "https://github.com/sametguzeldev/hms-unity-ads-plugin.git"
  }
}
```

### Building the Native Bridge

The plugin requires a native Java bridge AAR to adapt HMS abstract listener classes to Unity-compatible interfaces. **Build it once before using the plugin:**

```bash
cd AndroidBridge~
./gradlew assembleRelease
# Output: AndroidBridge~/build/outputs/aar/hms-ads-bridge-release.aar
cp AndroidBridge~/build/outputs/aar/hms-ads-bridge-release.aar Plugins/Android/hms-ads-bridge.aar
```

> **Note:** The HMS Ads SDK (`com.huawei.hms:ads-lite`) is fetched from Huawei's Maven repository during the Gradle build. The AAR only contains the bridge classes; the HMS SDK itself must be declared as a dependency in your Unity Android project's `mainTemplate.gradle`.

### mainTemplate.gradle dependency

Add to your Unity project's `Assets/Plugins/Android/mainTemplate.gradle` under `dependencies`:

```gradle
dependencies {
    implementation 'com.huawei.hms:ads-lite:13.4.73.301'
}
```

And in `settingsTemplate.gradle`, add Huawei's Maven repo:

```gradle
maven { url 'https://developer.huawei.com/repo/' }
```

## Quick Start

```csharp
using SametGuzelDev.HMS.Ads;
using UnityEngine;

public class AdsExample : MonoBehaviour
{
    private HMSInterstitialAd _interstitial;
    private HMSRewardedAd _rewarded;

    void Start()
    {
        // 1. Initialize the SDK once
        HMSAdsManager.Initialize();

        // 2. Create and load an interstitial
        _interstitial = HMSAdsManager.CreateInterstitialAd("your-interstitial-ad-unit-id");
        _interstitial.OnAdLoaded += () => Debug.Log("Interstitial ready!");
        _interstitial.OnAdFailed += code => Debug.LogError($"Interstitial failed: {code}");
        _interstitial.OnAdClosed += () => _interstitial.LoadAd(); // reload after close
        _interstitial.LoadAd();

        // 3. Create and load a rewarded ad
        _rewarded = HMSAdsManager.CreateRewardedAd("your-rewarded-ad-unit-id");
        _rewarded.OnAdLoaded += () => Debug.Log("Rewarded ready!");
        _rewarded.OnRewarded += reward => Debug.Log($"User earned: {reward}");
        _rewarded.LoadAd();
    }

    public void ShowInterstitial()
    {
        if (_interstitial.IsLoaded)
            _interstitial.Show();
    }

    public void ShowRewarded()
    {
        if (_rewarded.IsLoaded)
            _rewarded.Show();
    }

    void OnDestroy()
    {
        _interstitial?.Dispose();
        _rewarded?.Dispose();
    }
}
```

### Banner Ad

```csharp
var banner = HMSAdsManager.CreateBannerAd(
    "your-banner-ad-unit-id",
    BannerAdSize.Banner320x50,
    BannerPosition.Bottom);

banner.OnAdLoaded += () => Debug.Log("Banner loaded");
banner.LoadAd(); // Loads and shows the banner automatically
```

## Test Ad Unit IDs

During development, use the built-in test IDs via `HMSAdsSettings` (set **Use Test Ads** to true), or pass them directly:

| Ad Type      | Test ID               |
|--------------|-----------------------|
| Interstitial | `testb4znbuh3n2`      |
| Banner       | `testw6vs28auh3`      |
| Rewarded     | `testx9dtjwj8hp`      |

## Editor Settings

Create a settings asset via **Assets > Create > HMS Ads > Settings** to manage ad unit IDs and toggle test mode from the Inspector.

## Architecture

```
C# API (HMSInterstitialAd, HMSBannerAd, HMSRewardedAd)
    │
    ├── AndroidJavaProxy  →  IAdCallback / IRewardAdLoadCallback / IRewardAdStatusCallback
    │                              (Java interfaces in the bridge AAR)
    │
    └── BridgeAdListener / BridgeRewardAdLoadListener / BridgeRewardAdStatusListener
              (Java classes that extend HMS abstract listeners and delegate to the interfaces)
```

HMS abstract listener classes cannot be directly implemented by Unity's `AndroidJavaProxy` (which only supports Java interfaces). The bridge AAR solves this by providing interface+bridge pairs for each listener type.

## License

MIT — see [LICENSE](LICENSE).
