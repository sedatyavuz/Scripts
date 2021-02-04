using System.Collections.Generic;
using UnityEngine;

public class SOResetter : MonoBehaviour
{
    [SerializeField] private List<CustomScriptableObject> scriptableObjectToReset = new List<CustomScriptableObject>();
    [Space]
    [SerializeField] private List<ItemRuntimeSet> itemRuntimeSetsToReset;
    private void Awake()
    {
        foreach (CustomScriptableObject item in scriptableObjectToReset)
            item.Reset();

        foreach (ItemRuntimeSet set in itemRuntimeSetsToReset)
            foreach (BaseItem item in set)
                item.Reset();
    }
}
