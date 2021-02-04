using UnityEngine;
using System;
using System.Collections.Generic;
using Facebook.Unity;
using PlayFab;
using PlayFab.ClientModels;
using LoginResult = PlayFab.ClientModels.LoginResult;
using System.Collections;

public class PlayFabManager : MonoBehaviour
{
    private static Action OnSuccess;
    private static Action OnFailed;
    private static string displayName;

    private static bool isLoggingIn;

    public static bool IsLoggedIn {
        get { return PlayFabClientAPI.IsClientLoggedIn(); }
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("PreviouslyLoggedIn") == 1)
            Login(null, null);
    }

    public static void Login(Action onSuccess, Action onFailed)
    {
        if (IsLoggedIn)
        {
            onSuccess?.Invoke();
        }
        else
        {
            OnSuccess = onSuccess;
            OnFailed = onFailed;
            #if UNITY_IOS
            LoginWithGameCenter();
            #elif UNITY_ANDROID
            LoginWithFacebook();
            #endif
        }
    }

    private static IEnumerator TryLogin(Action onSucess, Action onFailed)
    {
        while (isLoggingIn)
        {
            yield return null;
        }

        if (IsLoggedIn)
        {
            OnSuccess?.Invoke();
        }
        else
        {
            Login(onSucess, onFailed);
        }
    }

    public static void LoginWithFacebook()
    {
        if (!FB.IsInitialized)
            FB.Init(OnFacebookInitialized);
        else
            OnFacebookInitialized();
    }
    private static void OnFacebookInitialized()
    {
        if (FB.IsLoggedIn)
            OnFacebookLoggedIn(null);
        else
            FB.LogInWithReadPermissions(null, OnFacebookLoggedIn);
    }
    private static void OnFacebookLoggedIn(ILoginResult result)
    {
        if (result == null || (string.IsNullOrEmpty(result.Error) && !result.Cancelled))
        {
            FB.API("/me?fields=name", HttpMethod.GET, (fbAccountInfo) => {
                if (fbAccountInfo.ResultDictionary.ContainsKey("name"))
                {
                    displayName = (string)fbAccountInfo.ResultDictionary["name"];
                    PlayFabClientAPI.LoginWithFacebook(new LoginWithFacebookRequest { CreateAccount = true, AccessToken = AccessToken.CurrentAccessToken.TokenString },
                        OnPlayFabAuthSucess, OnPlayFabAuthFailed);
                }
            });
        }
        else
        {
            Debug.Log("Error FB Login in: " + result.Error);
            OnFailed?.Invoke();
        }
    }

    public static void LoginWithGameCenter()
    {
        if (!Social.localUser.authenticated)
            Social.localUser.Authenticate(OnGameCenterLoggedIn);
        else
            OnGameCenterLoggedIn(true);
    }
    private static void OnGameCenterLoggedIn(bool success)
    {
        if (success)
        {
            displayName = Social.localUser.userName;
            PlayFabClientAPI.LoginWithGameCenter(new LoginWithGameCenterRequest { CreateAccount = true, PlayerId = Social.localUser.id },
                OnPlayFabAuthSucess, OnPlayFabAuthFailed); ;
        }
        else
        {
            OnFailed?.Invoke();
        }
    }

    private static void OnPlayFabAuthSucess(LoginResult result)
    {
        PlayerPrefs.SetInt("PreviouslyLoggedIn", 1);
        UpdateUserTitleDisplayName(displayName);
        OnSuccess?.Invoke();
    }
    private static void OnPlayFabAuthFailed(PlayFabError error)
    {
        Debug.Log("Error PlayFab Login " + error.ErrorMessage);
        OnFailed?.Invoke();
    }

    private static void UpdateUserTitleDisplayName(string name)
    {
        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = name };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, 
            (updateResult) =>
            {
                Debug.Log("Display Name is set to " + updateResult.DisplayName);
            },
            (error) =>
            {
                Debug.Log("Name Changed failed, Error is: " + error.ErrorMessage);
            }
        );
    }

    public static void PostPlayerData(string statisticsName, int value)
    {
        if (!IsLoggedIn)
            return;

        List<StatisticUpdate> updates = new List<StatisticUpdate>();
        updates.Add(new StatisticUpdate { StatisticName = statisticsName, Value = value });
        UpdatePlayerStatisticsRequest request = new UpdatePlayerStatisticsRequest { Statistics = updates };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnSuccessUpdatePlayerStatistics, OnFailedUpdatePlayerStatistics);
    }
    private static  void OnSuccessUpdatePlayerStatistics(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Success Statistics Upload");
    }
    private static void OnFailedUpdatePlayerStatistics(PlayFabError error)
    {
        Debug.Log("Failed Statistics Upload, Error Message: " + error.ErrorMessage);
    }
}
