# Building the HMS Ads Bridge AAR (Developer Reference)

The `hms-ads-bridge.aar` is **pre-built and already included** in this directory — Unity users do not
need to build it. These instructions are for plugin maintainers who need to regenerate the AAR after
modifying the Java sources in `AndroidBridge~/`.

## Prerequisites

- Java 8 or later (`java -version`)
- Android SDK with build tools 34+
- `ANDROID_HOME` environment variable set (or `local.properties` with `sdk.dir=...`)

## Build Steps

```bash
# From the repository root:
cd AndroidBridge~

# Make the Gradle wrapper executable (first time only)
chmod +x gradlew

# Build the release AAR
./gradlew assembleRelease
```

The output will be at:
```
AndroidBridge~/build/outputs/aar/app-release.aar
```

Copy it to the plugin's `Plugins/Android/` directory with the correct name:

```bash
cp AndroidBridge~/build/outputs/aar/app-release.aar Plugins/Android/hms-ads-bridge.aar
```

Then refresh your Unity project — Unity will automatically import the new AAR.

## What the AAR contains

The AAR contains only the bridge classes in `com.sametguzeldev.hms.ads.bridge`:

| Class | Purpose |
|-------|---------|
| `IAdCallback` | Java interface for `AdListener` methods |
| `BridgeAdListener` | Extends `AdListener`, delegates to `IAdCallback` |
| `IRewardAdLoadCallback` | Java interface for `RewardAdLoadListener` methods |
| `BridgeRewardAdLoadListener` | Extends `RewardAdLoadListener`, delegates to `IRewardAdLoadCallback` |
| `IRewardAdStatusCallback` | Java interface for `RewardAdStatusListener` methods |
| `BridgeRewardAdStatusListener` | Extends `RewardAdStatusListener`, delegates to `IRewardAdStatusCallback` |

The HMS Ads SDK itself (`com.huawei.hms:ads-lite`) is declared as `compileOnly` and is **not**
bundled — it must be present at runtime via your Unity project's `mainTemplate.gradle`.
