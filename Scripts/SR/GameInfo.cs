using Boo.Lang;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameInfo")]
public class GameInfo : CustomScriptableObject
{
    public string gameResult;

    public Character mainPlayer;
    public Team mainTeam;
    private GameObject focusedCharacter;

    [SerializeField] private bool gameOnWait = false;
    [SerializeField] private bool gameStarted = false;
    [SerializeField] private bool gameFinished = false;
    [SerializeField] private bool gameResulted = false;

    public int currentCoinRewards = 0;
    public int currentExpRewards = 0;
    public int currentKills = 0;

    public List<Character> characters;
    public List<Team> teams;

    #region Events
    public Action<Character, Character> OnCharacterPreDeath;
    public Action<Character, Character> OnCharacterPostDeath;
    public Action<GameObject> OnFocusedCharacterChanged;
    public Action OnGamePreWait;
    public Action OnGamePostWait;
    public Action OnGameStart;
    public Action OnGameFinish;
    public Action OnGameResult;
    #endregion

    #region Properties
    public bool GameOnWait {
        get { return gameOnWait; }
        set {
            gameOnWait = value;
            if (value)
            {
                OnGamePreWait?.Invoke();
                OnGamePostWait?.Invoke();
            }
        }
    }
    public bool GameStarted {
        get { return gameStarted; }
        set {
            gameStarted = value;
            if (value)
                OnGameStart?.Invoke();
        }
    }
    public bool GameFinished {
        get { return gameFinished; }
        set {
            gameFinished = value;
            if (value)
                OnGameFinish?.Invoke();
        }
    }
    public bool GameResulted {
        get { return gameResulted; }
        set {
            gameResulted = value;
            if (value)
                OnGameResult?.Invoke();
        }
    }
    public GameObject FocusedCharacter {
        get { return focusedCharacter; }
        set {
            focusedCharacter = value;
            OnFocusedCharacterChanged?.Invoke(value);
        }
    }
    #endregion

    public override void Reset()
    {
        gameResult = null;
        mainPlayer = null;
        mainTeam = null;

        focusedCharacter = null;

        characters = new List<Character>();
        teams = new List<Team>();

        OnCharacterPreDeath = null;
        OnCharacterPostDeath = null;
        OnFocusedCharacterChanged = null;
        OnGamePreWait = null;
        OnGamePostWait = null;
        OnGameStart = null;
        OnGameFinish = null;
        OnGameResult = null;

        currentCoinRewards = 0;
        currentExpRewards = 0;
        currentKills = 0;

        GameOnWait = false;
        GameFinished = false;
        GameStarted = false;
        GameResulted = false;
    }
}
