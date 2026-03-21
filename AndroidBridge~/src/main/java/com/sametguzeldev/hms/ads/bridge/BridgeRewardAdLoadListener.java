package com.sametguzeldev.hms.ads.bridge;

import com.huawei.hms.ads.reward.RewardAdLoadListener;

/**
 * Extends the HMS {@code RewardAdLoadListener} abstract class and delegates all load
 * lifecycle callbacks to an {@link IRewardAdLoadCallback} instance.
 *
 * <p>Pass an instance of this class to {@code RewardAd.loadAd(AdParam, RewardAdLoadListener)}.
 *
 * <p>Usage from C# (Unity):
 * <pre>{@code
 *   var proxy  = new RewardAdLoadListener();   // C# AndroidJavaProxy : IRewardAdLoadCallback
 *   var bridge = new AndroidJavaObject(
 *       "com.sametguzeldev.hms.ads.bridge.BridgeRewardAdLoadListener", proxy);
 *   rewardAd.Call("loadAd", adParam.JavaObject, bridge);
 * }</pre>
 */
public class BridgeRewardAdLoadListener extends RewardAdLoadListener {

    private final IRewardAdLoadCallback callback;

    /**
     * @param callback The {@link IRewardAdLoadCallback} (typically a Unity {@code AndroidJavaProxy})
     *                 to which load events will be forwarded.
     */
    public BridgeRewardAdLoadListener(IRewardAdLoadCallback callback) {
        this.callback = callback;
    }

    @Override
    public void onRewardedLoaded() {
        if (callback != null) callback.onRewardedLoaded();
    }

    @Override
    public void onRewardAdFailedToLoad(int errorCode) {
        if (callback != null) callback.onRewardAdFailedToLoad(errorCode);
    }
}
