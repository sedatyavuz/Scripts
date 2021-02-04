using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static void SavePlayerData(PlayerData playerData)
    {
        PlayerPrefs.SetString("playerNickname", playerData.PlayerNickname);
        PlayerPrefs.SetInt("playerLevel", playerData.PlayerLevel);
        PlayerPrefs.SetInt("playerExp", playerData.PlayerExp);
        PlayerPrefs.SetInt("playerBalance", playerData.PlayerBalance);

        PlayerPrefs.SetInt("PlayerGender", (int)playerData.gender);
        PlayerPrefs.SetInt("SelectedGunIndex", playerData.gun_index);
        PlayerPrefs.SetInt("SelectedSkinIndex", playerData.skin_index);
        PlayerPrefs.SetInt("SelectedBagIndex", playerData.bag_index);
        PlayerPrefs.SetInt("SelectedBeardIndex", playerData.beard_index);
        PlayerPrefs.SetInt("SelectedHairIndex", playerData.hair_index);
        PlayerPrefs.SetInt("SelectedHatIndex", playerData.hat_index);
        PlayerPrefs.SetInt("SelectedScarfIndex", playerData.scarf_index);
        PlayerPrefs.SetInt("SelectedPouchIndex", playerData.pouch_index);
        PlayerPrefs.SetInt("SelectedMaskIndex", playerData.mask_index);

        PlayerPrefs.Save();
    }
    public static void LoadPlayerData(PlayerData playerData)
    {
        playerData.PlayerNickname = PlayerPrefs.GetString("playerNickname");
        playerData.PlayerLevel = PlayerPrefs.GetInt("playerLevel");
        playerData.PlayerExp = PlayerPrefs.GetInt("playerExp");
        playerData.PlayerBalance = PlayerPrefs.GetInt("playerBalance");

        playerData.gender = (Gender)PlayerPrefs.GetInt("PlayerGender");

        if (PlayerPrefs.HasKey("SelectedGunIndex"))
            playerData.gun_index = PlayerPrefs.GetInt("SelectedGunIndex");
        if (PlayerPrefs.HasKey("SelectedSkinIndex"))
            playerData.skin_index = PlayerPrefs.GetInt("SelectedSkinIndex");
        if (PlayerPrefs.HasKey("SelectedBagIndex"))
            playerData.bag_index = PlayerPrefs.GetInt("SelectedBagIndex");
        if (PlayerPrefs.HasKey("SelectedBeardIndex"))
            playerData.beard_index = PlayerPrefs.GetInt("SelectedBeardIndex");
        if (PlayerPrefs.HasKey("SelectedHairIndex"))
            playerData.hair_index = PlayerPrefs.GetInt("SelectedHairIndex");
        if (PlayerPrefs.HasKey("SelectedHatIndex"))
            playerData.hat_index = PlayerPrefs.GetInt("SelectedHatIndex");
        if (PlayerPrefs.HasKey("SelectedScarfIndex"))
            playerData.scarf_index = PlayerPrefs.GetInt("SelectedScarfIndex");
        if (PlayerPrefs.HasKey("SelectedPouchIndex"))
            playerData.pouch_index = PlayerPrefs.GetInt("SelectedPouchIndex");
        if (PlayerPrefs.HasKey("SelectedMaskIndex"))
            playerData.mask_index = PlayerPrefs.GetInt("SelectedMaskIndex");
    }

    public static void SetToDefaultMale(PlayerData playerData)
    {
        playerData.gender = Gender.Male;
        playerData.gun_index = 0;
        playerData.skin_index = 0;
        playerData.bag_index = 0;
        playerData.beard_index = 0;
        playerData.hair_index = 0;
        playerData.hat_index = 0;
        playerData.mask_index = 0;
        playerData.pouch_index = 0;
        playerData.scarf_index = 0;
    }
    public static void SetToDefaultFemale(PlayerData playerData)
    {
        playerData.gender = Gender.Female;
        playerData.gun_index = 0;
        playerData.skin_index = 7;
        playerData.bag_index = 0;
        playerData.beard_index = 0;
        playerData.hair_index = 0;
        playerData.hat_index = 0;
        playerData.mask_index = 0;
        playerData.pouch_index = 0;
        playerData.scarf_index = 0;
    }
}
