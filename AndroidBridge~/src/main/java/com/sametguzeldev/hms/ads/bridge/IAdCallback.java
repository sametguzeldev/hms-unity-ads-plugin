package com.sametguzeldev.hms.ads.bridge;

/**
 * Interface mirroring the methods of the HMS {@code com.huawei.hms.ads.AdListener} abstract class.
 *
 * <p>Unity's {@code AndroidJavaProxy} can only implement Java <em>interfaces</em>, not abstract
 * classes. This interface, combined with {@link BridgeAdListener}, allows C# code to receive
 * HMS AdListener callbacks via {@code AndroidJavaProxy}.
 *
 * <p>Implement this interface on the C# side (via {@code AndroidJavaProxy}) and pass the proxy
 * to {@link BridgeAdListener#BridgeAdListener(IAdCallback)}.
 */
public interface IAdCallback {

    /** Called when the ad finishes loading successfully. */
    void onAdLoaded();

    /**
     * Called when the ad fails to load.
     *
     * @param errorCode HMS error code. See HMS Ads Kit documentation for error code values.
     */
    void onAdFailed(int errorCode);

    /** Called when the user dismisses the ad overlay. */
    void onAdClosed();

    /** Called when the user taps the ad. */
    void onAdClicked();

    /** Called when the ad overlay is opened/displayed. */
    void onAdOpened();

    /** Called when the user leaves the app after clicking the ad. */
    void onAdLeave();
}
