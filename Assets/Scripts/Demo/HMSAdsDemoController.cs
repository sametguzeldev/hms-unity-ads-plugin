using System;
using UnityEngine;
using UnityEngine.UI;
using SametGuzelDev.HMS.Ads;

public class HMSAdsDemoController : MonoBehaviour
{
    // Test ad unit IDs (duplicated from HMSAdsSettings which is editor-only)
    const string TestInterstitialAdUnitId = "testb4znbuh3n2";
    const string TestBannerAdUnitId = "testw6vs28auh3";
    const string TestRewardedAdUnitId = "testx9dtjwj8hp";

    HMSInterstitialAd _interstitialAd;
    HMSBannerAd _bannerAd;
    HMSRewardedAd _rewardedAd;

    Button _showInterstitialButton;
    Button _showRewardedButton;
    Button _showBannerButton;
    Button _hideBannerButton;

    Text _logText;
    ScrollRect _scrollRect;

    void Awake()
    {
        BuildUI();

        #if UNITY_EDITOR
        Log("Running in Editor — ads will not load. Deploy to an HMS Android device to test.");
        #endif

        HMSAdsManager.Initialize();
        Log("HMS Ads SDK initialized");

        SetupInterstitialAd();
        SetupBannerAd();
        SetupRewardedAd();
    }

    void OnDestroy()
    {
        _interstitialAd?.Dispose();
        _bannerAd?.Dispose();
        _rewardedAd?.Dispose();
    }

    void SetupInterstitialAd()
    {
        _interstitialAd = HMSAdsManager.CreateInterstitialAd(TestInterstitialAdUnitId);

        _interstitialAd.OnAdLoaded += _interstitialAd_OnAdLoaded;
        _interstitialAd.OnAdFailed += code => Log($"Interstitial: failed to load (code: {code})");
        _interstitialAd.OnAdOpened += () => Log("Interstitial: opened");
        _interstitialAd.OnAdClosed += _interstitialAd_OnAdClosed;
        _interstitialAd.OnAdClicked += () => Log("Interstitial: clicked");
        _interstitialAd.OnAdLeave += () => Log("Interstitial: user left app");
    }

    void SetupBannerAd()
    {
        _bannerAd = HMSAdsManager.CreateBannerAd(
            TestBannerAdUnitId,
            BannerAdSize.Banner320x50,
            BannerPosition.Bottom
        );

        _bannerAd.OnAdLoaded += _bannerAd_OnAdLoaded;
        _bannerAd.OnAdFailed += code => Log($"Banner: failed to load (code: {code})");
        _bannerAd.OnAdOpened += () => Log("Banner: opened");
        _bannerAd.OnAdClosed += () => Log("Banner: closed");
        _bannerAd.OnAdClicked += () => Log("Banner: clicked");
        _bannerAd.OnAdLeave += () => Log("Banner: user left app");
    }

    void SetupRewardedAd()
    {
        _rewardedAd = HMSAdsManager.CreateRewardedAd(TestRewardedAdUnitId);

        _rewardedAd.OnAdLoaded += _rewardedAd_OnAdLoaded;
        _rewardedAd.OnAdFailedToLoad += code => Log($"Rewarded: failed to load (code: {code})");
        _rewardedAd.OnAdOpened += () => Log("Rewarded: opened");
        _rewardedAd.OnAdClosed += _rewardedAd_OnAdClosed;
        _rewardedAd.OnRewarded += _rewardedAd_OnRewarded;
        _rewardedAd.OnAdFailedToShow += code => Log($"Rewarded: failed to show (code: {code})");
    }

    // ─── Ad Actions ──────────────────────────────────────────────

    void LoadInterstitial()
    {
        Log("Interstitial: loading...");
        _interstitialAd.LoadAd();
        #if UNITY_EDITOR
        _interstitialAd_OnAdLoaded();
        #endif
    }

    void ShowInterstitial()
    {
        _interstitialAd.Show();
        #if UNITY_EDITOR
        Log("Interstitial: shown (simulated)");
        _interstitialAd_OnAdClosed();
        #endif
    }

    void LoadBanner()
    {
        Log("Banner: loading...");
        _bannerAd.LoadAd();
        #if UNITY_EDITOR
        _bannerAd_OnAdLoaded();
        #endif
    }

    void ShowBanner()
    {
        _bannerAd.Show();
        #if UNITY_EDITOR
        Log("Banner: shown (simulated)");
        #endif
    }

    void HideBanner()
    {
        _bannerAd.Hide();
        Log("Banner: hidden");
    }

    void LoadRewarded()
    {
        Log("Rewarded: loading...");
        _rewardedAd.LoadAd();
        #if UNITY_EDITOR
        _rewardedAd_OnAdLoaded();
        #endif
    }

    void ShowRewarded()
    {
        _rewardedAd.Show();
        #if UNITY_EDITOR
        Log("Rewarded: shown (simulated)");
        _rewardedAd_OnRewarded(new Reward("coins", 10));
        _rewardedAd_OnAdClosed();
        #endif
    }

    // ─── Event Handlers (named for reuse in Editor simulation) ──

    void _interstitialAd_OnAdLoaded()
    {
        Log("Interstitial: loaded");
        _showInterstitialButton.interactable = true;
    }

    void _interstitialAd_OnAdClosed()
    {
        Log("Interstitial: closed");
        _showInterstitialButton.interactable = false;
    }

    void _bannerAd_OnAdLoaded()
    {
        Log("Banner: loaded");
        _showBannerButton.interactable = true;
        _hideBannerButton.interactable = true;
    }

    void _rewardedAd_OnAdLoaded()
    {
        Log("Rewarded: loaded");
        _showRewardedButton.interactable = true;
    }

    void _rewardedAd_OnRewarded(Reward reward)
    {
        Log($"Rewarded: earned {reward.Name} x{reward.Amount}");
    }

    void _rewardedAd_OnAdClosed()
    {
        Log("Rewarded: closed");
        _showRewardedButton.interactable = false;
    }

    void Log(string message)
    {
        string entry = $"[{DateTime.Now:HH:mm:ss}] {message}";
        Debug.Log($"[HMS Demo] {message}");
        _logText.text += entry + "\n";
        Canvas.ForceUpdateCanvases();
        _scrollRect.verticalNormalizedPosition = 0f;
    }

    // ─── UI Construction ───────────────────────────────────────────

    void BuildUI()
    {
        // Canvas
        var canvasGo = new GameObject("Canvas");
        canvasGo.transform.SetParent(transform);
        var canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        var scaler = canvasGo.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);
        scaler.matchWidthOrHeight = 0.5f;

        canvasGo.AddComponent<GraphicRaycaster>();

        // EventSystem
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            var esGo = new GameObject("EventSystem");
            esGo.AddComponent<UnityEngine.EventSystems.EventSystem>();
            esGo.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        // Root layout
        var rootGo = CreatePanel(canvasGo.transform, "Root", new Color(0.12f, 0.12f, 0.12f));
        var rootRect = rootGo.GetComponent<RectTransform>();
        rootRect.anchorMin = Vector2.zero;
        rootRect.anchorMax = Vector2.one;
        rootRect.offsetMin = Vector2.zero;
        rootRect.offsetMax = Vector2.zero;

        var rootLayout = rootGo.AddComponent<VerticalLayoutGroup>();
        rootLayout.padding = new RectOffset(30, 30, 30, 80); // extra bottom for banner
        rootLayout.spacing = 20;
        rootLayout.childForceExpandWidth = true;
        rootLayout.childForceExpandHeight = false;
        rootLayout.childControlWidth = true;
        rootLayout.childControlHeight = true;

        // Title
        var titleGo = CreateText(rootGo.transform, "Title", "HMS Ads Demo", 48, TextAnchor.MiddleCenter, Color.white);
        var titleLayout = titleGo.AddComponent<LayoutElement>();
        titleLayout.preferredHeight = 80;

        // Controls panel
        var controlsGo = CreatePanel(rootGo.transform, "Controls", new Color(0.18f, 0.18f, 0.18f));
        var controlsLayout = controlsGo.AddComponent<VerticalLayoutGroup>();
        controlsLayout.padding = new RectOffset(20, 20, 20, 20);
        controlsLayout.spacing = 15;
        controlsLayout.childForceExpandWidth = true;
        controlsLayout.childForceExpandHeight = false;
        controlsLayout.childControlWidth = true;
        controlsLayout.childControlHeight = true;
        var controlsLayoutElem = controlsGo.AddComponent<LayoutElement>();
        controlsLayoutElem.preferredHeight = 350;

        // Interstitial row
        var interstitialRow = CreateRow(controlsGo.transform, "Interstitial");
        CreateLabel(interstitialRow.transform, "Interstitial");
        CreateButton(interstitialRow.transform, "Load", true, () => LoadInterstitial());
        _showInterstitialButton = CreateButton(interstitialRow.transform, "Show", false, () => ShowInterstitial());

        // Banner row
        var bannerRow = CreateRow(controlsGo.transform, "Banner");
        CreateLabel(bannerRow.transform, "Banner");
        CreateButton(bannerRow.transform, "Load", true, () => LoadBanner());
        _showBannerButton = CreateButton(bannerRow.transform, "Show", false, () => ShowBanner());
        _hideBannerButton = CreateButton(bannerRow.transform, "Hide", false, () => HideBanner());

        // Rewarded row
        var rewardedRow = CreateRow(controlsGo.transform, "Rewarded");
        CreateLabel(rewardedRow.transform, "Rewarded");
        CreateButton(rewardedRow.transform, "Load", true, () => LoadRewarded());
        _showRewardedButton = CreateButton(rewardedRow.transform, "Show", false, () => ShowRewarded());

        // Log area
        BuildLogArea(rootGo.transform);
    }

    void BuildLogArea(Transform parent)
    {
        // ScrollView container
        var scrollGo = new GameObject("LogScroll");
        scrollGo.transform.SetParent(parent, false);
        var scrollImage = scrollGo.AddComponent<Image>();
        scrollImage.color = new Color(0.08f, 0.08f, 0.08f);

        var scrollLayoutElem = scrollGo.AddComponent<LayoutElement>();
        scrollLayoutElem.flexibleHeight = 1;

        _scrollRect = scrollGo.AddComponent<ScrollRect>();
        _scrollRect.horizontal = false;
        _scrollRect.vertical = true;
        _scrollRect.movementType = ScrollRect.MovementType.Clamped;
        _scrollRect.scrollSensitivity = 30;

        // Viewport
        var viewportGo = new GameObject("Viewport");
        viewportGo.transform.SetParent(scrollGo.transform, false);
        var viewportRect = viewportGo.AddComponent<RectTransform>();
        viewportRect.anchorMin = Vector2.zero;
        viewportRect.anchorMax = Vector2.one;
        viewportRect.offsetMin = new Vector2(10, 5);
        viewportRect.offsetMax = new Vector2(-10, -5);
        var viewportImage = viewportGo.AddComponent<Image>();
        viewportImage.color = Color.clear;
        var mask = viewportGo.AddComponent<Mask>();
        mask.showMaskGraphic = false;

        _scrollRect.viewport = viewportRect;

        // Content
        var contentGo = new GameObject("Content");
        contentGo.transform.SetParent(viewportGo.transform, false);
        var contentRect = contentGo.AddComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0, 1);
        contentRect.anchorMax = new Vector2(1, 1);
        contentRect.pivot = new Vector2(0, 1);
        contentRect.offsetMin = Vector2.zero;
        contentRect.offsetMax = Vector2.zero;

        var contentFitter = contentGo.AddComponent<ContentSizeFitter>();
        contentFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        var contentLayout = contentGo.AddComponent<VerticalLayoutGroup>();
        contentLayout.childForceExpandWidth = true;
        contentLayout.childForceExpandHeight = false;
        contentLayout.childControlWidth = true;
        contentLayout.childControlHeight = true;

        _scrollRect.content = contentRect;

        // Log text
        var logGo = CreateText(contentGo.transform, "LogText", "", 28, TextAnchor.UpperLeft, new Color(0.8f, 0.9f, 0.8f));
        _logText = logGo.GetComponent<Text>();
    }

    // ─── UI Helpers ────────────────────────────────────────────────

    GameObject CreatePanel(Transform parent, string name, Color color)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        var image = go.AddComponent<Image>();
        image.color = color;
        return go;
    }

    GameObject CreateRow(Transform parent, string name)
    {
        var go = new GameObject($"Row_{name}");
        go.transform.SetParent(parent, false);

        var layout = go.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 10;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = true;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childAlignment = TextAnchor.MiddleLeft;

        var layoutElem = go.AddComponent<LayoutElement>();
        layoutElem.preferredHeight = 80;

        return go;
    }

    void CreateLabel(Transform parent, string text)
    {
        var go = CreateText(parent, $"Label_{text}", text, 34, TextAnchor.MiddleLeft, Color.white);
        var layoutElem = go.AddComponent<LayoutElement>();
        layoutElem.preferredWidth = 280;
    }

    Button CreateButton(Transform parent, string label, bool interactable, Action onClick)
    {
        var go = new GameObject($"Btn_{label}");
        go.transform.SetParent(parent, false);

        var image = go.AddComponent<Image>();
        image.color = new Color(0.25f, 0.47f, 0.85f);

        var button = go.AddComponent<Button>();
        button.interactable = interactable;
        button.onClick.AddListener(() => onClick());

        var colors = button.colors;
        colors.normalColor = new Color(0.25f, 0.47f, 0.85f);
        colors.highlightedColor = new Color(0.35f, 0.57f, 0.95f);
        colors.pressedColor = new Color(0.15f, 0.37f, 0.75f);
        colors.disabledColor = new Color(0.3f, 0.3f, 0.3f);
        button.colors = colors;

        var layoutElem = go.AddComponent<LayoutElement>();
        layoutElem.preferredWidth = 180;

        CreateText(go.transform, "Text", label, 32, TextAnchor.MiddleCenter, Color.white);

        return button;
    }

    GameObject CreateText(Transform parent, string name, string text, int fontSize, TextAnchor alignment, Color color)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);

        var rect = go.GetComponent<RectTransform>();
        if (rect == null)
            rect = go.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        var t = go.AddComponent<Text>();
        t.text = text;
        t.fontSize = fontSize;
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.alignment = alignment;
        t.color = color;
        t.horizontalOverflow = HorizontalWrapMode.Wrap;
        t.verticalOverflow = VerticalWrapMode.Overflow;

        return go;
    }
}
