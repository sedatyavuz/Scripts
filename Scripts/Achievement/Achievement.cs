using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievement", order = 1)]
public class Achievement : CustomScriptableObject
{
    public string ID;

    public AchievementLevel[] levels;
    [Space]
    public string description;
    [Space]
    [Header("Runtime Values")]
    public int currentLevel = 0;
    public int currentPoints = 0;

    [Tooltip("Reset the current value when level up.")]
    public bool pointsAddUp;
    [Tooltip("Offset the current and next level required points by previous level required points.")]
    public bool offsetable;

    #region Events
    public Action OnInitilize;
    public Action<Achievement> OnLevelUp;
    public Action OnCountChanged;
    #endregion

    public int CurrentCount {
        get { return currentPoints; }
        set {
            currentPoints = value;
            OnCountChanged?.Invoke();
        }
    }

    public int GetCurrentPoints()
    {
        int requiredPoint = levels[currentLevel].pointsRequired;
        if (pointsAddUp)
            for (int i = currentLevel - 1; i >= 0; i--)
                requiredPoint += levels[i].pointsRequired;

        int offset = 0;
        if (offsetable && currentLevel != 0)
            offset = requiredPoint - levels[currentLevel].pointsRequired;

        return currentPoints - offset;
    }
    public int GetNextPoints()
    {
        int requiredPoint = levels[currentLevel].pointsRequired;
        if (pointsAddUp)
            for (int i = currentLevel-1; i >= 0; i--)
                requiredPoint += levels[i].pointsRequired;

        int offset = 0;
        if (offsetable && currentLevel != 0)
            offset = requiredPoint - levels[currentLevel].pointsRequired;

        return requiredPoint - offset;
    }
    public void TryLevelUp()
    {
        if (currentLevel < levels.Length)
        {
            int requiredPoint = levels[currentLevel].pointsRequired;
            if (pointsAddUp)
                for (int i = currentLevel - 1; i >= 0; i--)
                    requiredPoint += levels[i].pointsRequired;

            if(currentPoints >= requiredPoint)
            {
                currentLevel++;
                OnLevelUp?.Invoke(this);
            }
        }
    }

    public override void Reset()
    {
        OnLevelUp = null;
        OnInitilize = null;
        OnCountChanged = null;
        currentPoints = 0;
        currentLevel = 0;
    }

}
