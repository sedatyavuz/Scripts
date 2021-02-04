using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SR/GameEventObject")]
public class GameEventObject : CustomScriptableObject
{
    private Action<object> _event;

    public override void Reset()
    {
        _event = null;
    }

    public void AddListener(Action<object> action)
    {
        _event += action;
    }
    public void Invoke(object parameter1)
    {
        _event?.Invoke(parameter1);
    }
}
