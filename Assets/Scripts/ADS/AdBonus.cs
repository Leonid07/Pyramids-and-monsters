using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdBonus : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string adPlacementIdIOS = "Rewarded_iOS";
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    string _adUnitId = null;

    public Button buttonWatchAdsContinueLevel;
    public Button buttonWatchAdsPlusCount;

    private void Awake()
    {
#if UNITY_IOS
        _adUnitId = adPlacementIdIOS;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif
    }

    private void Start()
    {
        LoadAd(); // ��������� ������� ��� ������
        buttonWatchAdsContinueLevel.onClick.AddListener(ShowAdAndRestartLevel);
        buttonWatchAdsPlusCount.onClick.AddListener(ShowAdAndReward);
    }
    public void LoadAd()
    {
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }
    int i = 0;//////////////////// 0 ������������ ������ 1 ��������� �����
    public void ShowAdAndRestartLevel()// ������������ ������
    {
        i = 0;
        Advertisement.Show(_adUnitId, this);
        LoadAd();
    }
    public void ShowAdAndReward()// ��������� ����� �� 1000
    {
        i = 1;
        Advertisement.Show(_adUnitId, this);
        LoadAd();
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            if (i == 0)
            {
                DataManager.InstanceData.mapNextLevel.mapNextLevel.LoadLevel();
                GameManager.InstanceGame.LowLevelGame();
                PanelManager.InstancePanel.panelWin.ClosePanel();
                Debug.Log("Unity Ads Rewarded Ad Completed");
            }
            if (i == 1)
            {
                GameManager.InstanceGame.gold += 500;
                GameManager.InstanceGame.ApplyGold();
                DataManager.InstanceData.SaveGold();
            }
        }
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy()
    {

    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Ad Loaded: {placementId}");
    }
}