using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SR/GameItems")]
public class GameItems : CustomScriptableObject
{
    public ItemRuntimeSet characterGuns;
    public ItemRuntimeSet characterSkins;
    public ItemRuntimeSet characterBags;
    public ItemRuntimeSet characterBeards;
    public ItemRuntimeSet characterHairs;
    public ItemRuntimeSet characterHats;
    public ItemRuntimeSet characterMasks;
    public ItemRuntimeSet characterPouches;
    public ItemRuntimeSet characterScarfs;

    public override void Reset()
    {
        
    }
}
