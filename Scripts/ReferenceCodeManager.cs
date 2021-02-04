using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using PlayFab;
using PlayFab.ClientModels;

public class ReferenceCodeManager : MonoBehaviour
{
    private int requestsCount = 0;

    [SerializeField] private GameObject refCodeCanvas;
    [SerializeField] private InputField referenceCodeInputField;

    private void Awake()
    {
        requestsCount = PlayerPrefs.GetInt("RequestsCount", 0);

        if(requestsCount >= 2)
        {
            refCodeCanvas.SetActive(false);
        }
    }


    public void Close()
    {
        requestsCount++;
        PlayerPrefs.SetInt("RequestsCount", requestsCount);
        refCodeCanvas.SetActive(false);
    }

    public void SubmitButtonOnClick()
    {
        if (!PlayFabManager.IsLoggedIn)
        {
            PlayFabManager.Login(OnLoginSuccessfull, ()=> { Close(); });
        }
        else
        {
            OnLoginSuccessfull();
        }
    }

    private void OnLoginSuccessfull()
    {
        string referenceCode = referenceCodeInputField.text;
        var request = new GetAccountInfoRequest();

        PlayFabClientAPI.GetAccountInfo(request, (result) => {
            Submit(result.AccountInfo.PlayFabId, referenceCode);
        },
        (error) => {
            Debug.LogError(error.ErrorMessage);
            Close();
        });


    }

    private void Submit(string id, string referenceCode)
    {
        PlayerPrefs.SetInt("RequestsCount", 2);
        string api = $"http://game.binbirgames.com/?{id}=12&oyun_adi=DodgeShoot&yayinci_adi={referenceCode}";
        StartCoroutine(SendRequest(api));
        refCodeCanvas.SetActive(false);
    }

    private IEnumerator SendRequest(string uri)
    {
        UnityWebRequest request = UnityWebRequest.Get(uri);

        yield return request.SendWebRequest();

        string response = null;
        string message = null;
        JObject responseJson = null;

        if (!request.isNetworkError && !request.isHttpError)
        {
            response = request.downloadHandler.text;
        }

        try
        {
            responseJson = JObject.Parse(response);
            message = (string)responseJson["message"];
        }
        catch (System.Exception)
        {
            Debug.Log("Error parsing Json.");
        }
        

        if (responseJson == null)
        {
            refCodeCanvas.SetActive(false);
        }
        else
        {
            if (message.ToLower() == "true")
            {
                //Reward Player?
            }
            else
            {
                refCodeCanvas.SetActive(false);
            }
        }
    }

}
