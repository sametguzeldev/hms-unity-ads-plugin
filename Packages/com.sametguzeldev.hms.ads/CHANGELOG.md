# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-03-25

### Added
- Initial release
- `HMSAdsManager` static facade for SDK initialization and ad creation
- `HMSInterstitialAd` — full-screen interstitial ad support
- `HMSBannerAd` — banner ad support with Top/Bottom positioning
- `HMSRewardedAd` — rewarded ad support with `Reward` data class
- Native Java bridge (AAR) to adapt HMS abstract listener classes to C#-compatible interfaces
- `UnityMainThreadDispatcher` for safe callback marshalling from Java to Unity main thread
- `HMSAdsSettings` editor ScriptableObject for managing ad unit IDs
- `#if UNITY_ANDROID` guards for full Editor compatibility
