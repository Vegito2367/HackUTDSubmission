using System.Collections;
using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class AdMoneyGenerator : MonoBehaviour
{


    public UpdateCoins updatecoins;
    string actualID = "ca-app-pub-1607183366490722/9857304377";
    //string test = "ca-app-pub-3940256099942544/5224354917";
    RewardedAd GetMoreCoins;
    [SerializeField] GameObject FailedAd;
    private void Start()
    {
        InitializeAds();
    }
    public void LaunchAd()
    {
        InitializeThenShow();
        
    }
    public void InitializeThenShow()
    { 
        GetMoreCoins.Show();
        
    }
    void InitializeAds()
    {
        GetMoreCoins = new RewardedAd(actualID);
        GetMoreCoins.OnAdLoaded += HandleRewardedAdLoaded;
        GetMoreCoins.OnUserEarnedReward += HandleRewardBasedVideoRewarded;
        GetMoreCoins.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;

        GetMoreCoins.OnAdOpening += HandleRewardedAdOpening;

        GetMoreCoins.OnAdFailedToShow += HandleRewardedAdFailedToShow;


        GetMoreCoins.OnAdClosed += HandleRewardedAdClosed;
        AdRequest request = new AdRequest.Builder().Build();

        GetMoreCoins.LoadAd(request);
    }

    private void HandleRewardedAdClosed(object sender, EventArgs e)
    {   
        isAdClosed = true;
    }

    private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs e)
    {
        FailedAd.SetActive(true);
        InitializeAds();
    }

    private void HandleRewardedAdOpening(object sender, EventArgs e)
    {
        AudioManager.instance.PauseMusic(AudioManager.instance.ThemeMusic);
    }

    private void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs e)
    {
        InitializeAds();
    }

    private void HandleRewardedAdLoaded(object sender, EventArgs e)
    {

    }

    private IEnumerator DelayMaker()
    {
        yield return new WaitForSeconds(.1f);
        InitializeAds();
        GiveCoins();
    }

    private void GiveCoins()
    {
        PlayerDataStorer.totalcoins += 100;
        PlayerDataStorer.SaveProgress();
        updatecoins.UpdateCurrentCoins();
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

    

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        isRewarded = true;
    }


}
