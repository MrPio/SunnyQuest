using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdMobManager : MonoBehaviour
{
    [SerializeField] private string adUnitId = "ca-app-pub-4240235604287847/2200938145";
    private InterstitialAd interstitial;

    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        // Show();
    }
    
    private void RequestInterstitial()
    {
        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        var request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    public void Show()
    {
        RequestInterstitial();
        if (this.interstitial.CanShowAd()) {
            this.interstitial.Show();
        }
    }
}
