using System;
using UnityEngine;

public class IronSourceObject : MonoBehaviour
{
    [SerializeField] private GameEvent RewardedAdFinished;

    public Action OnInterstitialAdDone;

    public static IronSourceObject Instance;

    private bool requestingInterstitialAd;
    private bool requestingBannerAd;

    private bool bannerAdLoaded;
    private bool interstitailAdLoaded;
    
    private void Awake()
    {
        if (Instance)
            Destroy(this.gameObject);
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        #if UNITY_IOS
        IronSource.Agent.init("ba2d7e95");
        #elif UNITY_ANDROID
        IronSource.Agent.init("ba2ed7c5");
        #endif

        RegisterCallbacks();
    }
    private void FixedUpdate()
    {
        if (!requestingInterstitialAd && !IronSource.Agent.isInterstitialReady())
        {
            RequestInterstitial();
        }
        if (!requestingBannerAd && !bannerAdLoaded)
        {
            RequestBannerAds();
        }
    }


    public void RequestRewardedAd()
    {

    }
    public void RequestBannerAds()
    {
        requestingBannerAd = true;
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
    }
    public void RequestInterstitial()
    {
        requestingInterstitialAd = true;
        IronSource.Agent.loadInterstitial();
    }
    
    private void RegisterCallbacks()
    {
        IronSource.Agent.shouldTrackNetworkState (true);

        IronSourceEvents.onBannerAdLoadedEvent += OnBannerAdLoaded;
        IronSourceEvents.onBannerAdLoadFailedEvent += OnBannerAdLoadFailed;

        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

        IronSourceEvents.onRewardedVideoAdRewardedEvent += (id) => { RewardedAdFinished.Invoke(); };
    }

    public void ShowRewarded()
    {
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo();
        }
    }
    public void ShowInterstitialAd()
    {
        if (IronSource.Agent.isInterstitialReady())
        {
            IronSource.Agent.showInterstitial();
        }
        else
        {
            OnInterstitialAdDone?.Invoke();
            OnInterstitialAdDone = null;
        }
    }


    #region IronSource Callbacks
    private void OnBannerAdLoaded()
    {
        requestingBannerAd = false;
        bannerAdLoaded = true;
        IronSource.Agent.displayBanner();
    }
    private void OnBannerAdLoadFailed(IronSourceError error)
    {
        Debug.Log("Error loading interstitial ad, error: " + error);
        requestingBannerAd = false;
        bannerAdLoaded = false;
    }
    private void InterstitialAdClosedEvent()
    {
        requestingInterstitialAd = false;
        OnInterstitialAdDone?.Invoke();
        OnInterstitialAdDone = null;
    }
    private void InterstitialAdShowFailedEvent(IronSourceError obj)
    {
        requestingInterstitialAd = false;
        OnInterstitialAdDone?.Invoke();
        OnInterstitialAdDone = null;
    }
    private void InterstitialAdLoadFailedEvent(IronSourceError obj)
    {
        requestingInterstitialAd = false;
        OnInterstitialAdDone?.Invoke();
        OnInterstitialAdDone = null;
    }
    #endregion

    private void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }
    
}
