using UnityEngine;

public abstract class TVariable<T> : CustomScriptableObject
{
    [SerializeField] private T defaultValue;

    public T Value { get; set; }

    public override void Reset()
    {
        Value = defaultValue;
    }
}
