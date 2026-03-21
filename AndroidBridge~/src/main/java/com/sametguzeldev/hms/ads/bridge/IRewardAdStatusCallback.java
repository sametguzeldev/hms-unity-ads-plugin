package com.sametguzeldev.hms.ads.bridge;

import com.huawei.hms.ads.reward.Reward;

/**
 * Interface mirroring the methods of the HMS
 * {@code com.huawei.hms.ads.reward.RewardAdStatusListener} abstract class.
 *
 * <p>Implement this interface on the C# side (via {@code AndroidJavaProxy}) and pass the proxy
 * to {@link BridgeRewardAdStatusListener#BridgeRewardAdStatusListener(IRewardAdStatusCallback)}.
 */
public interface IRewardAdStatusCallback {

    /** Called when the rewarded ad is displayed to the user. */
    void onRewardAdOpened();

    /** Called when the rewarded ad overlay is closed by the user. */
    void onRewardAdClosed();

    /**
     * Called when the user has completed the rewarded ad and earned the reward.
     *
     * @param reward The {@link Reward} object containing the reward name and amount.
     */
    void onRewarded(Reward reward);

    /**
     * Called when the rewarded ad fails to display.
     *
     * @param errorCode HMS error code.
     */
    void onRewardAdFailedToShow(int errorCode);
}
