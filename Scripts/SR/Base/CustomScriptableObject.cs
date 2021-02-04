using UnityEngine;

public abstract class CustomScriptableObject : ScriptableObject, IResetable
{
    public abstract void Reset();
}
