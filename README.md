# HMS Ads Kit — Unity Plugin

Unity project and UPM package for integrating Huawei Mobile Services (HMS) Ads Kit into Unity games. Supports **Interstitial**, **Banner**, and **Rewarded** ads.

## Project Structure

```
Assets/
  Scripts/Demo/         Demo scene with UI for testing all ad types
  Plugins/Android/      Gradle templates and agconnect-services.json
  Scenes/               Demo scene

Packages/
  com.sametguzeldev.hms.ads/    The UPM package (plugin source)
    Runtime/                    C# API and native bridge wrappers
    Editor/                     EDM4U dependency descriptor
    Plugins/Android/            Pre-built hms-ads-bridge.aar
    Tests/                      EditMode and PlayMode unit tests

AndroidBridge~/                 Java bridge source (Gradle project)
```

## Getting Started

### Prerequisites

- Unity 2022.3 or later
- Android build target
- [AppGallery Connect](https://developer.huawei.com/consumer/en/service/josp/agc/index.html) project with Ads Kit enabled

### Setup

1. Clone the repo and open in Unity
2. Create an app in AppGallery Connect, enable Ads Kit
3. Add your signing certificate's SHA-256 fingerprint
4. Download `agconnect-services.json` and place it in `Assets/Plugins/Android/`
5. Build and run on a Huawei/Honor device

### Running in Editor

The plugin uses `#if UNITY_ANDROID && !UNITY_EDITOR` guards — all ad calls are no-ops in the Editor. The demo scene simulates ad lifecycle events so you can verify the UI flow without a device.

### Running Tests

Open **Window > General > Test Runner** in Unity:

- **EditMode** — Tests for Reward, AdParam, BannerAdSize, ad classes (stub behavior), HMSAdsSettings, and HMSAdsManager
- **PlayMode** — Tests for UnityMainThreadDispatcher

## Releasing the UPM Package

The UPM package is published via a `upm` branch using `git subtree split`:

```bash
git subtree split --prefix=Packages/com.sametguzeldev.hms.ads -b upm
git tag v1.0.0 upm
git push origin upm --tags
```

Consumers install via:

```json
{
  "dependencies": {
    "com.sametguzeldev.hms.ads": "https://github.com/sametguzeldev/hms-unity-ads-plugin.git#upm"
  }
}
```

## Building the Bridge AAR

See [AndroidBridge~/README.md](AndroidBridge~/README.md) for instructions on rebuilding the native Java bridge.

## License

MIT — see [Packages/com.sametguzeldev.hms.ads/LICENSE](Packages/com.sametguzeldev.hms.ads/LICENSE).
