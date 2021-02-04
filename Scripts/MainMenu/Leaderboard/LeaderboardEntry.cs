using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private Image profilePhoto;
    [SerializeField] private TMP_Text displayNameText;
    [SerializeField] private TMP_Text scoreText;

    public void Init(int rank, string displayName, int score)
    {
        rankText.text = rank.ToString();
        displayNameText.text = displayName;
        scoreText.text = score.ToString();
    }

}
