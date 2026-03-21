package com.sametguzeldev.hms.ads.bridge;

import com.huawei.hms.ads.AdListener;

/**
 * Extends the HMS {@code AdListener} abstract class and delegates every callback to an
 * {@link IAdCallback} instance.
 *
 * <p><b>Why this class exists:</b> Unity's {@code AndroidJavaProxy} can implement Java
 * interfaces but cannot extend abstract classes. {@code AdListener} is abstract, so we need
 * this bridge to make it accessible from C#. The C# side creates an {@code AndroidJavaProxy}
 * that implements {@link IAdCallback}, then passes it into this class via the constructor.
 *
 * <p>Usage from C# (Unity):
 * <pre>{@code
 *   var proxy  = new InterstitialAdListener();  // C# AndroidJavaProxy : IAdCallback
 *   var bridge = new AndroidJavaObject(
 *       "com.sametguzeldev.hms.ads.bridge.BridgeAdListener", proxy);
 *   interstitialAd.Call("setAdListener", bridge);
 * }</pre>
 */
public class BridgeAdListener extends AdListener {

    private final IAdCallback callback;

    /**
     * @param callback The {@link IAdCallback} instance (typically a Unity {@code AndroidJavaProxy})
     *                 to which all ad events will be forwarded.
     */
    public BridgeAdListener(IAdCallback callback) {
        this.callback = callback;
    }

    @Override
    public void onAdLoaded() {
        if (callback != null) callback.onAdLoaded();
    }

    @Override
    public void onAdFailed(int errorCode) {
        if (callback != null) callback.onAdFailed(errorCode);
    }

    @Override
    public void onAdClosed() {
        if (callback != null) callback.onAdClosed();
    }

    @Override
    public void onAdClicked() {
        if (callback != null) callback.onAdClicked();
    }

    @Override
    public void onAdOpened() {
        if (callback != null) callback.onAdOpened();
    }

    @Override
    public void onAdLeave() {
        if (callback != null) callback.onAdLeave();
    }
}
