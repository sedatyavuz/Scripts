using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [SerializeField] private Achievement[] achievements;
    [SerializeField] private PlayerData playerData;

    private void Awake()
    {
        foreach (Achievement achievement in achievements)
        {
            AchievementDataManager.LoadAchievementData(achievement);
            achievement.OnLevelUp += OnAchievementLevelUp;
        }
    }

    private void OnAchievementLevelUp(Achievement achievement)
    {
        playerData.PlayerBalance += achievement.levels[achievement.currentLevel-1].reward;
        PlayerDataManager.SavePlayerData(playerData);
        AchievementDataManager.SaveAchievementData(achievement);
    }
}
