using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MMUIManager : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject skinUI;
    [SerializeField] private GameObject achievementUI;
    [SerializeField] private GameObject leaderboardUI;

    [SerializeField] private List<GameObject> itemContainers;
    
    [Space]
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject buyOrSelectButton;
    [SerializeField] private GameObject multiplayerButton;

    [Space]
    [SerializeField] private GameObject playerNameObject;
    [SerializeField] private GameObject playerBalanceObject;
    [SerializeField] private GameObject priceObj;

    [Space]
    [SerializeField] private GameEvent OnReloadItems;

    private bool isOnShop;
    private bool isOnAchievement;
    private bool isOnWeapons;
    private bool isOnSkins;
    private bool isOnLeaderboard;

    private bool multiplayerEnabled;

    private void Awake()
    {
        playerData.OnNameChanged += ChangeMMCharacterName;
        playerData.OnBalanceChanged += UpdateUIElements;
    }
    private void Start()
    {
        shopUI.SetActive(false); 
        UpdateUIElements();
        HideAllContainers();
    }
    private void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable && multiplayerEnabled)
        {
            multiplayerEnabled = false;
            multiplayerButton.SetActive(false);
        }
        if (Application.internetReachability != NetworkReachability.NotReachable && !multiplayerEnabled)
        {
            multiplayerEnabled = true;
            multiplayerButton.SetActive(true);
        }
    }

    private void ChangeMMCharacterName(string name)
    {
        playerNameObject.GetComponent<Text>().text = name;
    }

    private void UpdateUIElements()
    {
        playerBalanceObject.GetComponentInChildren<Text>().text = playerData.PlayerBalance.ToString();
    }

    public void ShowShopUI()
    {
        isOnShop = true;
        mainUI.SetActive(false);
        shopUI.SetActive(true);
        backButton.SetActive(true);
    }
    public void ShowWeapons()
    {
        isOnWeapons = true;
        mainUI.SetActive(false);
        shopUI.SetActive(false);
        backButton.SetActive(true);
    }
    public void ShowSkins()
    {
        isOnSkins = true;
        skinUI.SetActive(true);
        mainUI.SetActive(false);
        backButton.SetActive(true);
    }
    public void ShowAchievements()
    {
        isOnAchievement = true;
        achievementUI.SetActive(true);
        mainUI.SetActive(false);
        backButton.SetActive(true);
    }
    public void ShowLeaderboard()
    {
        isOnLeaderboard = true;
        leaderboardUI.SetActive(true);
        mainUI.SetActive(false);
        playerBalanceObject.SetActive(false);
        backButton.SetActive(true);
    }

    private void HideAllContainers()
    {
        foreach (GameObject item in itemContainers)
        {
            item.SetActive(false);
        }
    }
    public void ShowContainer(GameObject container)
    {
        buyOrSelectButton.SetActive(false);
        priceObj.SetActive(false);
        OnReloadItems.Invoke();
        HideAllContainers();
        container.SetActive(true);
    }

    public void GoBack()
    {
        OnReloadItems.Invoke();
        buyOrSelectButton.SetActive(false);
        priceObj.SetActive(false);
        if (isOnShop)
        {
            isOnShop = false;
            HideAllContainers();
            mainUI.SetActive(true);
            shopUI.SetActive(false);
        }
        else if (isOnWeapons)
        {
            isOnWeapons = false;
            mainUI.SetActive(true);
        }
        else if(isOnSkins)
        {
            isOnSkins = false;
            mainUI.SetActive(true);
            skinUI.SetActive(false);
        }
        else if (isOnAchievement)
        {
            isOnAchievement = false;
            mainUI.SetActive(true);
            achievementUI.SetActive(false);
        }
        else if (isOnLeaderboard)
        {
            isOnLeaderboard = false;
            leaderboardUI.SetActive(false);
            mainUI.SetActive(true);
            playerBalanceObject.SetActive(true);
        }

        backButton.SetActive(false);
    }

}
