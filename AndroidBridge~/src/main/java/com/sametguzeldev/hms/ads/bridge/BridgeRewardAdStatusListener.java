package com.sametguzeldev.hms.ads.bridge;

import com.huawei.hms.ads.reward.Reward;
import com.huawei.hms.ads.reward.RewardAdStatusListener;

/**
 * Extends the HMS {@code RewardAdStatusListener} abstract class and delegates all show
 * lifecycle callbacks — including the reward grant — to an {@link IRewardAdStatusCallback} instance.
 *
 * <p>Pass an instance of this class to {@code RewardAd.show(Activity, RewardAdStatusListener)}.
 *
 * <p>Usage from C# (Unity):
 * <pre>{@code
 *   var proxy  = new RewardAdStatusListener();  // C# AndroidJavaProxy : IRewardAdStatusCallback
 *   var bridge = new AndroidJavaObject(
 *       "com.sametguzeldev.hms.ads.bridge.BridgeRewardAdStatusListener", proxy);
 *   rewardAd.Call("show", AndroidContext.Activity, bridge);
 * }</pre>
 */
public class BridgeRewardAdStatusListener extends RewardAdStatusListener {

    private final IRewardAdStatusCallback callback;

    /**
     * @param callback The {@link IRewardAdStatusCallback} (typically a Unity {@code AndroidJavaProxy})
     *                 to which show events will be forwarded.
     */
    public BridgeRewardAdStatusListener(IRewardAdStatusCallback callback) {
        this.callback = callback;
    }

    @Override
    public void onRewardAdOpened() {
        if (callback != null) callback.onRewardAdOpened();
    }

    @Override
    public void onRewardAdClosed() {
        if (callback != null) callback.onRewardAdClosed();
    }

    @Override
    public void onRewarded(Reward reward) {
        if (callback != null) callback.onRewarded(reward);
    }

    @Override
    public void onRewardAdFailedToShow(int errorCode) {
        if (callback != null) callback.onRewardAdFailedToShow(errorCode);
    }
}
