using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultUIManager : MonoBehaviour
{
    [SerializeField] private GameInfo gameInfo;

    #region Result UI Objects
    [SerializeField] private GameObject resultUI;
    [SerializeField] private GameObject resultStats;
    [SerializeField] private Text resultUIHeaderTx;
    [SerializeField] private Text resultUIRanking;
    [SerializeField] private Text resultKillsTx;
    [SerializeField] private Text resultCoinEarnsTx;
    [SerializeField] private Text resultExpEarnsTx;
    #endregion

    private void Awake()
    {
        gameInfo.OnGameResult += () => { ShowResultUI(gameInfo.gameResult); };
    }

    private void ShowResultUI(string result)
    {
        resultUI.SetActive(true);
        resultStats.SetActive(true);

        resultKillsTx.text = string.Format("Kills: {0}", gameInfo.currentKills);
        resultCoinEarnsTx.text = string.Format("Coins Earned: {0}", gameInfo.currentCoinRewards);
        resultExpEarnsTx.text = string.Format("Exp Earned: {0}", gameInfo.currentExpRewards);

        int rank = result == "Won" ? gameInfo.teams.Count : gameInfo.teams.Count + 1;
        resultUIRanking.text = "Rank #" + rank.ToString();
        resultUIHeaderTx.text = result == "Won" ? "You Won" : "You Lost";
    }

    public void ResultUIContinueButtonOnClick() => GameManager.Instance.ContinueButtonClicked();
}
