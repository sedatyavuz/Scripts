using System;
using UnityEngine;

[CreateAssetMenu(menuName = "GameItems/BaseItem")]
public class BaseItem : CustomScriptableObject
{
    [SerializeField] public string ID;
    [SerializeField] public ItemType itemType;
    [SerializeField] public Gender itemGender; 
    [SerializeField] public GameObject prefab;
    [SerializeField] protected bool isPreUnlocked;
    [SerializeField] public int price;
    [Space]
    [Header("Runtime Set Properties")]
    [SerializeField] protected bool unlocked;
    [SerializeField] protected bool selected;
    [SerializeField] protected bool previewing;

    #region Events
    public Action OnItemDataChanged;
    #endregion

    #region Properties
    public bool Unlocked {
        get { return unlocked; }
        set {
            unlocked = value;
            if (isPreUnlocked)
                unlocked = true;
            OnItemDataChanged?.Invoke();
        }
    }
    public bool Selected {
        get { return selected; }
        set {
            selected = value;
            OnItemDataChanged?.Invoke();
        }
    }
    public bool Previewing {
        get { return previewing; }
        set {
            previewing = value;
            OnItemDataChanged?.Invoke();
        }
    }
    #endregion

    public override void Reset()
    {
        unlocked = false;
        if (isPreUnlocked)
            unlocked = true;
        selected = false;
        previewing = false;

        OnItemDataChanged = null;
    }
    public void LoadData()
    {
        Unlocked = (PlayerPrefs.GetInt("Item_Unlocked_" + ID) == 1) ? true : false ;
        Selected = (PlayerPrefs.GetInt("Item_Selected_" + ID) == 1) ? true : false ;
    }
    public void SaveData()
    {
        PlayerPrefs.SetInt("Item_Unlocked_" + ID, unlocked ? 1 : 0);
        PlayerPrefs.SetInt("Item_Selected_" + ID, selected ? 1 : 0);
        PlayerPrefs.Save();
    }
}