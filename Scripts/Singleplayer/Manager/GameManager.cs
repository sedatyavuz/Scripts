using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private IntVar gameMode;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameInfo gameInfo;

    public static GameManager Instance;

    private bool loadingLevel;

    private void Awake()
    {
        Instance = this;
        gameInfo.OnCharacterPostDeath += CharacterDied;
    }
    private void Start()
    {
        PlayerDataManager.LoadPlayerData(playerData);
        gameInfo.GameOnWait = true;
        Invoke("StartGame", 10);
    }
    private void Update()
    {
        //Return to main menu if the internet connection is lost (Cheap way, seems buggy)
/*        if(gameMode.Value == 1 && Application.internetReachability == NetworkReachability.NotReachable && !loadingLevel)
        {
            loadingLevel = true;
            LoadMainLevel();
        }*/
    }

    public void StartGame()
    {
        gameInfo.GameOnWait = false;
        gameInfo.GameStarted = true;
    }

    private void CharacterDied(Character character, Character killer = null)
    {
        if (killer && killer.team == gameInfo.mainTeam)
        {
            gameInfo.currentCoinRewards += GameMath.killCoinReward;
            gameInfo.currentExpRewards += GameMath.killExpReward;
            gameInfo.currentKills++;
        }

        gameInfo.characters.Remove(character);
        character.gameObject.SetActive(false);

        Transform focusedCharacter = FindReferenceCharacter();
        gameInfo.FocusedCharacter = focusedCharacter?.gameObject;

        int characterCount = CountCharacters(character.team);
        if (characterCount == 0)
        {
            gameInfo.teams.Remove(character.team);
            CheckGameResult();
        }
    }

    public void CheckGameResult()
    {
        if (gameInfo.GameFinished)
            return;
        
        if (gameInfo.teams.Count <= 1)
        {
            gameInfo.GameFinished = true;
            foreach (Character character in gameInfo.characters)
                character.OnWon?.Invoke();
        }

        if (!gameInfo.GameResulted)
        {
            int characterCount = CountCharacters(gameInfo.mainTeam);

            if (characterCount >= 1 && gameInfo.teams.Count <= 1) // WON
            {
                int rewardIndex = gameInfo.characters.Count - 1;
                gameInfo.currentCoinRewards += rewardIndex <= 3 ? GameMath.winCoinRewards[rewardIndex] : 0;
                gameInfo.currentExpRewards += GameMath.winExpReward;

                gameInfo.gameResult = "Won";
                gameInfo.GameResulted = true;
                RewardPlayer();
            }
            else if (characterCount == 0) // LOST
            {
                gameInfo.gameResult = "Lost";
                gameInfo.GameResulted = true;
                RewardPlayer();
            }
        }
    }

    private int CountCharacters(Team team)
    {
        int tmp = 0;
        foreach (Transform child in team.gameObject.transform)
        {
            if (!child.GetComponent<Character>().isDead)
                tmp++;
        }
        return tmp;
    }

    public Transform FindReferenceCharacter()
    {
        for (int i = 0; i < gameInfo.characters.Count; i++)
        {
            if(gameInfo.characters[i] != null && !gameInfo.characters[i].isDead)
            {
                return gameInfo.characters[i].gameObject.transform;
            }
        }
        return null;
    }


    public void ContinueButtonClicked()
    {
        IronSourceObject.Instance.OnInterstitialAdDone = () => { SceneManager.LoadScene("MainMenu"); };
        IronSourceObject.Instance.ShowInterstitialAd();//TODO: Event Based NO HARD REFERENCE.
    }

    private void RewardPlayer()
    {
        playerData.PlayerBalance += gameInfo.currentCoinRewards;
        playerData.PlayerExp += gameInfo.currentExpRewards;
        gameInfo.currentCoinRewards = 0;
        gameInfo.currentExpRewards = 0;
        GameMath.LevelUpPlayer(playerData);
        PlayerDataManager.SavePlayerData(playerData);
    }
}
