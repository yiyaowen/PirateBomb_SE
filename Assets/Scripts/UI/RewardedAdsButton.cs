using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

[RequireComponent(typeof(Button))]
public class RewardedAdsButton : MonoBehaviour, IUnityAdsListener
{
#if UNITY_ANDROID
    string gameId = "4065477";
#elif UNITY_IOS
    string gameId = "4065476";
#endif

    Button button;
    string surfacingId = "rewardedVideo";

    void Start()
    {
        button = GetComponent<Button>();
        button.interactable = Advertisement.IsReady(surfacingId);
        button.onClick.AddListener(ShowRewardedVideo);

        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, true);
    }

    public void ShowRewardedVideo()
    {
        Advertisement.Show(surfacingId);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // 奖励满血复活
        if (showResult != ShowResult.Failed)
        {
            GameManager.instance.PlayerResurge();
            transform.parent.gameObject.SetActive(false);
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        if (surfacingId == placementId)
        {
            button.interactable = true;
        }
    }

    void IUnityAdsListener.OnUnityAdsDidError(string message)
    {
        
    }

    void IUnityAdsListener.OnUnityAdsDidStart(string placementId)
    {
        
    }
}
