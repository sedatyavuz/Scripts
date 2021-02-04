using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementLogger : MonoBehaviour
{
    [SerializeField] private GameInfo gameInfo;

    [SerializeField] private Achievement totalKillCount;
    [SerializeField] private Achievement deathCount;
    [SerializeField] private Achievement killRecord;
    [SerializeField] private Achievement totalPlayedMatches;
    [SerializeField] private Achievement totalPlayedTime;
    [SerializeField] private Achievement totalTraveledDistance;
    [SerializeField] private Achievement totalTimeSurvived;
    [SerializeField] private Achievement winsCount;

    private float playDuration = 0;

    public static bool instanceExist;

    private void Awake()
    {
        if (instanceExist)
            Destroy(this.gameObject);
        instanceExist = true;
        DontDestroyOnLoad(this.gameObject);


        gameInfo.OnGameResult += () => {
            LogValues(gameInfo.mainPlayer); 
        };


        AchievementDataManager.LoadAchievementData(totalKillCount);
        AchievementDataManager.LoadAchievementData(deathCount);
        AchievementDataManager.LoadAchievementData(killRecord);
        AchievementDataManager.LoadAchievementData(totalPlayedMatches);
        AchievementDataManager.LoadAchievementData(totalPlayedTime);
        AchievementDataManager.LoadAchievementData(totalTraveledDistance);
        AchievementDataManager.LoadAchievementData(totalTimeSurvived);
        AchievementDataManager.LoadAchievementData(winsCount);

        StartCoroutine(SaveProgressEvery(1));
    }

    private void FixedUpdate()
    {
        playDuration += Time.deltaTime;
        if(playDuration >= 1)
        {
            int tmp = Mathf.RoundToInt(playDuration);
            totalPlayedTime.CurrentCount += tmp;
            playDuration -= tmp;
        }
    }

    private IEnumerator SaveProgressEvery(float seconds)
    {
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer > seconds)
                SaveProgress();
            yield return new WaitForEndOfFrame();
        }
    }
    private void SaveProgress()
    {
        AchievementDataManager.SaveAchievementData(totalKillCount);
        AchievementDataManager.SaveAchievementData(deathCount);
        AchievementDataManager.SaveAchievementData(killRecord);
        AchievementDataManager.SaveAchievementData(totalPlayedMatches);
        AchievementDataManager.SaveAchievementData(totalPlayedTime);
        AchievementDataManager.SaveAchievementData(totalTraveledDistance);
        AchievementDataManager.SaveAchievementData(totalTimeSurvived);
        AchievementDataManager.SaveAchievementData(winsCount);
    }

    private void LogValues(Character characterToLog)
    {
        totalKillCount.CurrentCount += characterToLog.stats.killCount;
        deathCount.CurrentCount += characterToLog.stats.deathCount;
        totalTraveledDistance.CurrentCount += (int)characterToLog.stats.travelDistance;
        totalTimeSurvived.CurrentCount += (int)characterToLog.stats.surviveTime;
        if (characterToLog.stats.killCount > killRecord.CurrentCount)
            killRecord.CurrentCount = characterToLog.stats.killCount;
        if (gameInfo.gameResult == "Won")
            winsCount.currentPoints++;

        totalPlayedMatches.CurrentCount++;

        SaveProgress();
    }
}
