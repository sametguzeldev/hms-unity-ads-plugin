# HMS Ads Kit — Unity Plugin

Unity plugin for Huawei Mobile Services (HMS) Ads Kit. Supports **Interstitial**, **Banner**, and **Rewarded** ads via a clean, event-driven C# API.

## Requirements

- Unity 2021.3 or later
- Android build target
- Huawei device with HMS Core (or Honor device)
- A [Huawei AppGallery Connect](https://developer.huawei.com/consumer/en/service/josp/agc/index.html) project with Ads Kit enabled

## Installation

### Via Git URL (UPM)

Add to your project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.sametguzeldev.hms.ads": "https://github.com/sametguzeldev/hms-unity-ads-plugin.git#upm"
  }
}
```

### Dependency Resolution

The plugin requires the HMS Ads SDK and AppGallery Connect SDK at runtime. Choose one of the following:

**Option A — EDM4U (recommended if available)**

If you have [External Dependency Manager for Unity](https://github.com/googlesamples/unity-jar-resolver) installed, the plugin's `HMSAdsDependencies.xml` will automatically resolve the dependencies.

**Option B — Manual Gradle templates**

Enable Custom Gradle Templates in **Player Settings > Publishing Settings** and add:

1. **baseProjectTemplate.gradle** — Add the `agcp` classpath:
    ```gradle
    buildscript {
        repositories {
            google()
            mavenCentral()
            maven { url 'https://developer.huawei.com/repo/' }
        }
        dependencies {
            classpath 'com.huawei.agconnect:agcp:1.9.1.301'
        }
    }
    ```

2. **settingsTemplate.gradle** — Add Huawei's Maven repo to `pluginManagement` and `dependencyResolutionManagement`:
    ```gradle
    maven { url 'https://developer.huawei.com/repo/' }
    ```

3. **mainTemplate.gradle** — Add dependencies and the agcp plugin:
    ```gradle
    dependencies {
        implementation 'com.huawei.hms:ads-lite:13.4.73.301'
        implementation 'com.huawei.agconnect:agconnect-core:1.9.1.301'
    }
    ```
    At the bottom of the file:
    ```gradle
    apply plugin: 'com.huawei.agconnect'
    ```

### AppGallery Connect Setup

1. Create an app in [AppGallery Connect](https://developer.huawei.com/consumer/en/service/josp/agc/index.html)
2. Enable **Ads Kit** under Manage APIs
3. Add your signing certificate's SHA-256 fingerprint
4. Download `agconnect-services.json` and place it in `Assets/Plugins/Android/`

## Quick Start

```csharp
using SametGuzelDev.HMS.Ads;
using UnityEngine;

public class AdsExample : MonoBehaviour
{
    private HMSInterstitialAd _interstitial;
    private HMSRewardedAd _rewarded;

    private void Start()
    {
        HMSAdsManager.Initialize();

        _interstitial = HMSAdsManager.CreateInterstitialAd("your-interstitial-ad-unit-id");
        _interstitial.OnAdLoaded += () => Debug.Log("Interstitial ready!");
        _interstitial.OnAdFailed += code => Debug.LogError($"Interstitial failed: {code}");
        _interstitial.OnAdClosed += () => _interstitial.LoadAd();
        _interstitial.LoadAd();

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

    private void OnDestroy()
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
banner.LoadAd();
```

## Settings

Create a settings asset via **Assets > Create > HMS Ads > Settings** to manage ad unit IDs and toggle test mode from the Inspector. Access the effective IDs at runtime:

```csharp
var settings = Resources.Load<HMSAdsSettings>("HMSAdsSettings");
var ad = HMSAdsManager.CreateInterstitialAd(settings.EffectiveInterstitialId);
```

## Test Ad Unit IDs

| Ad Type      | Test ID          |
|--------------|------------------|
| Interstitial | `testb4znbuh3n2` |
| Banner       | `testw6vs28auh3` |
| Rewarded     | `testx9dtjwj8hp` |

These are also available as constants: `HMSAdsSettings.TestInterstitialAdUnitId`, etc.

## Architecture

```
C# API (HMSInterstitialAd, HMSBannerAd, HMSRewardedAd)
    |
    +-- AndroidJavaProxy --> IAdCallback / IRewardAdLoadCallback / IRewardAdStatusCallback
    |                          (Java interfaces in the bridge AAR)
    |
    +-- BridgeAdListener / BridgeRewardAdLoadListener / BridgeRewardAdStatusListener
              (Java classes extending HMS abstract listeners, delegating to the interfaces)
```

HMS abstract listener classes cannot be directly implemented by Unity's `AndroidJavaProxy` (which only supports Java interfaces). The bridge AAR solves this by providing interface+bridge pairs for each listener type.

## License

MIT — see [LICENSE](LICENSE).
