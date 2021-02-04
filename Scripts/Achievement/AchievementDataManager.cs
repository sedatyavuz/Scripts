using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AchievementDataManager
{
    public static void SaveAchievementData(Achievement achievement)
    {
        PlayerPrefs.SetInt("AchievementCurrentCount_" + achievement.ID, achievement.currentPoints);
        PlayerPrefs.SetInt("AchievementCurrentLevel_" + achievement.ID, achievement.currentLevel);

        PlayerPrefs.Save();
    }
    public static void LoadAchievementData(Achievement achievement)
    {
        achievement.currentPoints = PlayerPrefs.GetInt("AchievementCurrentCount_" + achievement.ID);
        achievement.currentLevel = PlayerPrefs.GetInt("AchievementCurrentLevel_" + achievement.ID);
        achievement.OnInitilize?.Invoke();
    }
}
