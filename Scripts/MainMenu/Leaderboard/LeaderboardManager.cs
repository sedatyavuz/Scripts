using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using Facebook.Unity;
using UnityEngine.UI;
using PlayFab.ClientModels;
using System;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private GameObject entriesContainer;
    [SerializeField] private GameObject LeaderboardEntryPrefab;
    [SerializeField] private TMP_Text informationText;

    private string defaultLeaderboard = "Kills";

    private string loadingString = "Loading...";
    private string notLoggedInString = "You're not logged in.";

    private void OnEnable()
    {
        if (!PlayFabManager.IsLoggedIn)
            PlayFabManager.Login(OnSuccessLoginIn, OnFailedLoginIn);
        else
            RequestLeaderboard(defaultLeaderboard);
    }
    private void OnDisable()
    {
        ClearLeaderboard();
    }

    private void OnSuccessLoginIn()
    {
        RequestLeaderboard(defaultLeaderboard);
    }
    private void OnFailedLoginIn()
    {
        Debug.Log(informationText.text);
        informationText.text = notLoggedInString;
        Debug.Log(informationText.text);
    }


    public void RequestLeaderboard(string leaderboardName)
    {
        informationText.text = loadingString;

        if (!PlayFabManager.IsLoggedIn)
        {
            PlayFabManager.Login(OnSuccessLoginIn, OnFailedLoginIn);
            return;
        }

        ClearLeaderboard();

        var request = new GetLeaderboardRequest { StatisticName = leaderboardName, MaxResultsCount = 50 };
        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, OnGetLeaderboardFailed);
    }

    private void OnGetLeaderboardSuccess(GetLeaderboardResult leaderboardResult)
    {
        ClearLeaderboard();
        informationText.text = "";
        foreach (var entry in leaderboardResult.Leaderboard)
        {
            GameObject newEntry = Instantiate(LeaderboardEntryPrefab, entriesContainer.transform);
            newEntry.GetComponent<LeaderboardEntry>().Init(entry.Position + 1, entry.DisplayName, entry.StatValue);
        }
    }
    private void OnGetLeaderboardFailed(PlayFabError obj)
    {

    }

    private void ClearLeaderboard()
    {
        foreach (Transform item in entriesContainer.transform)
        {
            Destroy(item.gameObject);
        }
    }

}
