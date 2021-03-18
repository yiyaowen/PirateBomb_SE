using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdsButton : MonoBehaviour, IUnityAdsListener
{
#if UNITY_IOS
    private string gameID = "4053756";
#elif UNITY_ANDROID
    private string gameID = "4053757";
#endif

    Button adsButton;
    public string placementID = "rewardedVideo";

    // Start is called before the first frame update
    void Start()
    {
        adsButton = GetComponent<Button>();

        if (adsButton)
            adsButton.onClick.AddListener(ShowRewardAds);

        Advertisement.AddListener(this);
        Advertisement.Initialize(gameID, true);
    }

    public void ShowRewardAds()
    {
        Advertisement.Show(placementID);
    }

    public void OnUnityAdsDidError(string message)
    {

    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch (showResult)
        {
            case ShowResult.Failed:
                break;
            case ShowResult.Skipped:
                break;
            case ShowResult.Finished:
                var player = FindObjectOfType<PlayerController>();
                player.health = 3;
                player.isDead = false;
                UIManager.instance.UpdateHealth(player.health);
                break;
            default:
                break;
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {

    }

    public void OnUnityAdsReady(string placementId)
    {

    }
}
