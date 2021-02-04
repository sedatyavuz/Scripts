using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/PlayerData")]
public class PlayerData : CustomScriptableObject
{
    private string playerNickname;
    private int playerLevel;
    private int playerExp;
    [SerializeField] private int playerBalance;

    #region Events
    public Action<string> OnNameChanged;
    public Action OnLevelChanged;
    public Action OnExpChanged;
    public Action OnBalanceChanged;
    #endregion

    #region Player Items
    public Gender gender;
    public int gun_index;
    public int skin_index;
    public int bag_index;
    public int beard_index;
    public int hair_index;
    public int hat_index;
    public int mask_index;
    public int pouch_index;
    public int scarf_index;
    #endregion

    #region Properties
    public string PlayerNickname {
        get { return playerNickname; }
        set {
            playerNickname = value;
            OnNameChanged?.Invoke(value);
        }
    }
    public int PlayerLevel {
        get { return playerLevel; }
        set {
            playerLevel = value;
            OnLevelChanged?.Invoke();
        }
    }
    public int PlayerExp {
        get { return playerExp; }
        set {
            playerExp = value;
            OnExpChanged?.Invoke();
        }
    }
    public int PlayerBalance {
        get { return playerBalance; }
        set {
            playerBalance = value;
            OnBalanceChanged?.Invoke();
        }
    }
    #endregion

    public override void Reset()
    {
        playerNickname = "Player";
        playerLevel = 1;
        playerExp = 0;
        playerBalance = 0;

        #region Events Reset
        OnLevelChanged = null;
        OnExpChanged = null;
        OnNameChanged = null;
        OnBalanceChanged = null;
        #endregion

        #region Player Items Reset
        gender = Gender.Unisex;
        gun_index = 0;
        skin_index = 0;
        bag_index = 0;
        beard_index = 0;
        hair_index = 0;
        hat_index = 0;
        mask_index = 0;
        pouch_index = 0;
        scarf_index = 0;
        #endregion
    }
}
