package com.sametguzeldev.hms.ads.bridge;

/**
 * Interface mirroring the methods of the HMS
 * {@code com.huawei.hms.ads.reward.RewardAdLoadListener} abstract class.
 *
 * <p>Implement this interface on the C# side (via {@code AndroidJavaProxy}) and pass the proxy
 * to {@link BridgeRewardAdLoadListener#BridgeRewardAdLoadListener(IRewardAdLoadCallback)}.
 */
public interface IRewardAdLoadCallback {

    /** Called when the rewarded ad has finished loading and is ready to be shown. */
    void onRewardedLoaded();

    /**
     * Called when the rewarded ad fails to load.
     *
     * @param errorCode HMS error code.
     */
    void onRewardAdFailedToLoad(int errorCode);
}
