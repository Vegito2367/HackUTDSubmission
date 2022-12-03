using UnityEngine.Events;
using GoogleMobileAds.Api;
using System;
using System.Collections;
using UnityEngine;

public class AdsMidLevel : MonoBehaviour
{
    [SerializeField] GameManager gm;
    
    [SerializeField] ScoreDisplayer sd;
    public UnityEvent AfterAdEvent;
    [SerializeField] int restartFreq;
    [SerializeField] int MidlevelFreq;
    
    #region Google Variables
    RewardedAd rewardBasedVideoAd;
    InterstitialAd RestartLevel;
    InterstitialAd NextLevelAdThing;
    string actualReward = "ca-app-pub-1607183366490722/7067295983";
    string actualRestart = "ca-app-pub-1607183366490722/4113188957";
    string actualMidlevel = "ca-app-pub-1607183366490722/2608874903";
    //string test= "ca-app-pub-3940256099942544/5224354917";
    //string testInt= "ca-app-pub-3940256099942544/1033173712";

    #endregion

    private void Start()
    {
        MobileAds.Initialize(initstatus => { });
        InitializeAds();
        InitializeAdsInterstitialMidLevel();
        InitializeAdsInterstitialRestart();
    }
    public void NextLevelAd()
    {
        
        if (gm.levelplayed % MidlevelFreq == 0)
        {
            AfterAdEvent.Invoke();
            NextLevelAdThing.Show();
        }
        else
        {
            AfterAdEvent.Invoke();
        }
    }
    
    public void RestartLevelAd()
    {
        //ShowRestartAd();
        
        gm.RestartTheFuckingLevel();
        if(gm.Restarts%restartFreq==0)
        {
            ShowGoogleAdRestart();
        }
    }
    

    #region GoogleAds
    void ShowGoogleAdRestart()
    {
        RestartLevel.Show();
    }
    public void InitializeAds()
    {
        rewardBasedVideoAd = new RewardedAd(actualReward);

       
        rewardBasedVideoAd.OnAdLoaded += HandleRewardedAdLoaded;

        rewardBasedVideoAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
       
        rewardBasedVideoAd.OnAdOpening += HandleRewardedAdOpening;

        rewardBasedVideoAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        
        rewardBasedVideoAd.OnUserEarnedReward += HandleUserEarnedReward;
        
        rewardBasedVideoAd.OnAdClosed += HandleRewardedAdClosed;

        
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        rewardBasedVideoAd.LoadAd(request);
    }
    public void InitializeAdsInterstitialRestart()
    {
        RestartLevel = new InterstitialAd(actualRestart);

        // Called when an ad request has successfully loaded.
        RestartLevel.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        RestartLevel.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        RestartLevel.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        RestartLevel.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        RestartLevel.OnAdLeavingApplication += HandleOnAdLeavingApplication;


        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        RestartLevel.LoadAd(request);
    }
    public void InitializeAdsInterstitialMidLevel()
    {
        NextLevelAdThing = new InterstitialAd(actualMidlevel);

            // Called when an ad request has successfully loaded.
            NextLevelAdThing.OnAdLoaded += HandleOnAdLoadedNextLevel;
            // Called when an ad request failed to load.
            NextLevelAdThing.OnAdFailedToLoad += HandleOnAdFailedToLoadNextLevel;
            // Called when an ad is shown.
            NextLevelAdThing.OnAdOpening += HandleOnAdOpenedNextLevel;
            // Called when the ad is closed.
            NextLevelAdThing.OnAdClosed += HandleOnAdClosedNextLevel;
            // Called when the ad click caused the user to leave the application.
            NextLevelAdThing.OnAdLeavingApplication += HandleOnAdLeavingApplicationNextLevel;


            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the rewarded ad with the request.
            NextLevelAdThing.LoadAd(request);
        
    }

    #region MidLevel Events
    private void HandleOnAdLeavingApplicationNextLevel(object sender, EventArgs e)
    {
        NextLevelAdThing.Destroy();
        InitializeAdsInterstitialMidLevel();
    }

    private void HandleOnAdClosedNextLevel(object sender, EventArgs e)
    {
        AudioManager.instance.UnpauseMusic(AudioManager.instance.ThemeMusic);
        NextLevelAdThing.Destroy();
        InitializeAdsInterstitialMidLevel();
    }

    private void HandleOnAdOpenedNextLevel(object sender, EventArgs e)
    {
        AudioManager.instance.PauseMusic(AudioManager.instance.ThemeMusic);
    }

    private void HandleOnAdFailedToLoadNextLevel(object sender, AdFailedToLoadEventArgs e)
    {
        NextLevelAdThing.Destroy();
        InitializeAdsInterstitialMidLevel();
    }

    private void HandleOnAdLoadedNextLevel(object sender, EventArgs e)
    {
        
    }
    #endregion

    #region Restart Events
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        RestartLevel.Destroy();
        InitializeAdsInterstitialRestart();
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        AudioManager.instance.PauseMusic(AudioManager.instance.ThemeMusic);
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        AudioManager.instance.UnpauseMusic(AudioManager.instance.ThemeMusic);
        RestartLevel.Destroy();
        InitializeAdsInterstitialRestart();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {

    }
    #endregion
    
    #region Reward Video Events
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        print("HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
        InitializeAds();
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        AudioManager.instance.PauseMusic(AudioManager.instance.ThemeMusic);
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
        InitializeAds();
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        isAdClosed = true;

    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        isRewarded = true;
    }
    IEnumerator DelayMaker()
    {
        yield return new WaitForSeconds(.1f);
        AudioManager.instance.UnpauseMusic(AudioManager.instance.ThemeMusic);
        sd.UpdateAfterAd();
        InitializeAds();
    }
    #endregion

    public void IncreaseCoins()
    {
        if (rewardBasedVideoAd.IsLoaded())
        {
            rewardBasedVideoAd.Show();
        }
    }
    bool isAdClosed = false;
    bool isRewarded = false;
    void Update()
    {
        if (isAdClosed)
        {
            if (isRewarded)
            {
                AudioManager.instance.UnpauseMusic(AudioManager.instance.ThemeMusic);
                StartCoroutine(DelayMaker());
                isRewarded = false;
            }
            else
            {
                AudioManager.instance.UnpauseMusic(AudioManager.instance.ThemeMusic);
                InitializeAds();
            }
            isAdClosed = false;  // to make sure this action will happen only once.
        }
    }
    #endregion





}
