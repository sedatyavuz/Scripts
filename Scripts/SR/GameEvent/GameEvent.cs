using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SR/GameEvent")]
public class GameEvent : CustomScriptableObject
{
    private Action _event;

    public override void Reset()
    {
        _event = null;
    }

    public void AddListener(Action action)
    {
        _event += action;
    }
    public void Invoke()
    {
        _event?.Invoke();
    }
}
