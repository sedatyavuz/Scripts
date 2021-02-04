using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Achievement achievement;
    [SerializeField] private Text description;
    [SerializeField] private Text rewardText;
    [SerializeField] private Image fill;
    [SerializeField] private Image coinImage;
    [SerializeField] private Text progressText;

    private void Awake()
    {
        achievement.OnInitilize += UpdateUI;
        achievement.OnLevelUp += (x) => { UpdateUI(); };
        achievement.OnCountChanged += UpdateUI;
    }
    private void Start()
    {
        UpdateUI();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        achievement.TryLevelUp();
    }


    private void UpdateUI()
    {
        float fillAmount = 0;

        if (achievement.currentLevel == achievement.levels.Length) //If Max Level
        {
            description.text = "You've completed this Achievement !";
            rewardText.text = "";
            progressText.text = "MAX";

            fillAmount = 1;
        }
        else
        {
            description.text = string.Format("{0}", achievement.description);
            rewardText.text = achievement.levels[achievement.currentLevel].reward.ToString();

            float currentCount = achievement.GetCurrentPoints();
            float nextCount = achievement.GetNextPoints();
            fillAmount = currentCount / nextCount;

            progressText.text = string.Format("{0}/{1}", currentCount, nextCount);
        }

        fill.fillAmount = fillAmount;

        //Check if Achievement can be leveled UP and make Coin Image have different Transparency.
        if (fillAmount >= 1)
            coinImage.color = new Color(1, 1, 1, 1f);
        else
            coinImage.color = new Color(1, 1, 1, 0.5f);
    }

}
