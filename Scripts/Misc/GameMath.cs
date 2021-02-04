using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameMath
{
    public const int characterHP = 100;
    public const float characterMovementSpeed = 5f;

    private static int baseExpRequired = 100;

    public static int killCoinReward = 4;
    public static int[] winCoinRewards = { 30, 20, 10, 5 };

    public static int killExpReward = 10;
    public static int winExpReward = 20;

    public static int CalculateExpRequired(int level)
    {
        level--;
        float result = baseExpRequired * ((level * 0.05f) + 1);
        return Mathf.RoundToInt(baseExpRequired);
    }

    public static void LevelUpPlayer(PlayerData playerData)
    {
        int levelUpExpRequired = CalculateExpRequired(playerData.PlayerLevel);
        while (playerData.PlayerExp >= levelUpExpRequired)
        {
            playerData.PlayerLevel++;
            playerData.PlayerExp -= levelUpExpRequired;
            levelUpExpRequired = CalculateExpRequired(playerData.PlayerLevel);
        }
    }

    public static Vector3 GetPredictedPosition(Vector3 targetPosition, Vector3 targetVelocity, float bulletSpeed)
    {
        Vector3 relativePosition = targetPosition;
        Vector3 relativeVelocity = targetVelocity;

        float a = Vector3.Dot(relativeVelocity, relativeVelocity) - bulletSpeed * bulletSpeed;
        float b = 2f * Vector3.Dot(relativeVelocity, relativePosition);
        float c = Vector3.Dot(relativePosition, relativePosition);

        float desc = b * b - 4f * a * c;

        if (desc < 0f)
            return Vector3.zero;

        float deltaTime = 2f * c / (Mathf.Sqrt(desc) - b);

        if (deltaTime < 0f)
            return Vector3.zero;

        return targetPosition + targetVelocity * deltaTime;
    }
}
