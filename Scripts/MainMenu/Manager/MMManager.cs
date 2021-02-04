using Facebook.Unity;
using System.Collections;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MMManager : MonoBehaviour
{
    [SerializeField] private GameEvent rewardedAdFinished;

    [SerializeField] private IntVar gameMode;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameItems gameItems;

    [SerializeField] private Achievement killsAchievement;
    [SerializeField] private Achievement winsAchievement;

    #region Events
    [SerializeField] private GameEventObject OnCharacterItemChanged;
    [SerializeField] private GameEvent OnSelectionFinished;
    [SerializeField] private GameEvent OnReloadItems;
    #endregion
    
    [SerializeField] private GameObject mmCharacter;

    private void Awake()
    {
        #region Event Registeration
        OnCharacterItemChanged.AddListener((item) => { SetCharacterItem((BaseItem)item); } );
        OnSelectionFinished.AddListener(() => { LoadCharacterItems(playerData); } );
        OnReloadItems.AddListener(()=> { LoadCharacterItems(playerData); });
        rewardedAdFinished.AddListener(() =>
        {
            playerData.PlayerBalance += 15;
            PlayerDataManager.SavePlayerData(playerData);
        });
        #endregion
    }

    private void Start()
    {
        PlayFabManager.PostPlayerData("Kills", killsAchievement.currentPoints);
        PlayFabManager.PostPlayerData("Wins", winsAchievement.currentPoints);
    }

    private void LoadCharacterItems(PlayerData playerData)
    {
        SetCharacterItem(gameItems.characterGuns[playerData.gun_index]);
        SetCharacterItem(gameItems.characterSkins[playerData.skin_index]);

        SetCharacterItem(gameItems.characterBags[playerData.bag_index]);
        SetCharacterItem(gameItems.characterBeards[playerData.beard_index]);
        SetCharacterItem(gameItems.characterHairs[playerData.hair_index]);
        SetCharacterItem(gameItems.characterHats[playerData.hat_index]);
        SetCharacterItem(gameItems.characterMasks[playerData.mask_index]);
        SetCharacterItem(gameItems.characterPouches[playerData.pouch_index]);
        SetCharacterItem(gameItems.characterScarfs[playerData.scarf_index]);
    }
    public void SetCharacterItem(BaseItem item)
    {
        CharacterChildren characterChilds = mmCharacter.GetComponent<CharacterChildren>();

        Mesh itemPrefabMesh = null;
        
        if(item.itemType != ItemType.Skin && item.itemType != ItemType.Gun)
        {
            if (item.prefab != null)
                itemPrefabMesh = item.prefab.GetComponent<MeshFilter>().sharedMesh;
        }


        switch (item.itemType)
        {
            case ItemType.Gun:
                Transform gunsChild = characterChilds.gun.transform;
                foreach (Transform gunObj in gunsChild)
                    if(gunObj.childCount > 0)
                        Destroy(gunObj.GetChild(0).gameObject);

                Transform gunChild = null;
                switch (((GunItem)item).gunType)
                {
                    case GunType.Pistol:
                        gunChild = gunsChild.Find("Pistol");
                        break;
                    case GunType.Rifle:
                        gunChild = gunsChild.Find("Rifle");
                        break;
                    case GunType.Sniper:
                        gunChild = gunsChild.Find("Sniper");
                        break;
                    case GunType.RPG:
                        gunChild = gunsChild.Find("RPG");
                        break;
                    default:
                        break;
                }
                Instantiate(item.prefab, gunChild);
                break;
            case ItemType.Skin:
                characterChilds.skin.GetComponent<SkinnedMeshRenderer>().sharedMesh = item.prefab.GetComponent<SkinnedMeshRenderer>().sharedMesh;
                break;
            case ItemType.Bag:
                    characterChilds.bag.GetComponent<MeshFilter>().sharedMesh = itemPrefabMesh;
                break;
            case ItemType.Beard:
                    characterChilds.beard.GetComponent<MeshFilter>().sharedMesh = itemPrefabMesh;
                break;
            case ItemType.Hair:
                characterChilds.hair.GetComponent<MeshFilter>().sharedMesh = itemPrefabMesh;
                break;
            case ItemType.Hat:
                    characterChilds.hat.GetComponent<MeshFilter>().sharedMesh = itemPrefabMesh;
                break;
            case ItemType.Mask:
                    characterChilds.mask.GetComponent<MeshFilter>().sharedMesh = itemPrefabMesh;
                break;
            case ItemType.Pouch:
                    characterChilds.pouch.GetComponent<MeshFilter>().sharedMesh = itemPrefabMesh;
                break;
            case ItemType.Scarf:
                    characterChilds.scarf.GetComponent<MeshFilter>().sharedMesh = itemPrefabMesh;
                break;
            default:
                break;
        }
    }

    public void MenuPlayButtonOnClick(bool singlePlayer)
    {
        SceneManager.LoadScene("City");
    }
}
