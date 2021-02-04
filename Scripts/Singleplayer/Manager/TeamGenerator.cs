using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Runtime.Remoting.Messaging;

public class TeamGenerator : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameInfo gameInfo;
    [SerializeField] private IntVar gameMode;
    [SerializeField] private StringRuntimeSet devNicknames;
    [SerializeField] private StringRuntimeSet botNicknames;
    [SerializeField] private GameItems gameItems;

    [SerializeField] private GameObject teamsParentObj;
    [SerializeField] private GameObject teamPrefab;
    [SerializeField] private GameObject characterPrefab;

    [SerializeField] private List<Vector3> spawnPositions;
    private List<string> tmpBotNicknames;
    private List<string> tmpDevNicknames;

    [SerializeField] private int teamCount;

    private void Awake()
    {
        InitNickNames();
        InitSpawnPositions();

        gameInfo.OnGamePreWait += GeneratePlayer;
        gameInfo.OnGamePreWait += GenerateBots;

        gameInfo.OnGameStart += RandomizeCharactersPositions;
    }

    private void GeneratePlayer()
    {
        Vector3 spawnPos = spawnPositions[0];
        spawnPositions.RemoveAt(0);

        GameObject teamObj = Instantiate(teamPrefab, spawnPos, Quaternion.identity, teamsParentObj.transform);
        Team teamClass = teamObj.GetComponent<Team>();

        Character newCharacter = CreateCharacter(teamObj.transform, true, playerData);
        newCharacter.OnPreDeath += gameInfo.OnCharacterPreDeath;
        newCharacter.OnPostDeath += gameInfo.OnCharacterPostDeath;
        newCharacter.nickname = playerData.PlayerNickname;
        

        gameInfo.characters.Add(newCharacter);
        gameInfo.teams.Add(teamClass);

        gameInfo.mainTeam = teamClass;
        gameInfo.mainPlayer = newCharacter;
        gameInfo.FocusedCharacter = GameManager.Instance.FindReferenceCharacter().gameObject;
    }
    private void GenerateBots()
    {
        for (int i = 0; i < teamCount; i++)
        {
            Vector3 spawnPos = spawnPositions[0];
            spawnPositions.RemoveAt(0);
            string nickname = "Bot";
            if(gameMode.Value == 0)
            {
                nickname = tmpDevNicknames[0];
                tmpDevNicknames.RemoveAt(0);
            }
            else
            {
                nickname = tmpBotNicknames[0];
                tmpBotNicknames.RemoveAt(0);
            }


            GameObject teamObj = Instantiate(teamPrefab, spawnPos, Quaternion.identity, teamsParentObj.transform);
            Team teamClass = teamObj.GetComponent<Team>();

            Character newCharacter = CreateCharacter(teamObj.transform, false, GetRandomPlayerData());
            newCharacter.OnPreDeath += gameInfo.OnCharacterPreDeath;
            newCharacter.OnPostDeath += gameInfo.OnCharacterPostDeath;
            newCharacter.nickname = nickname;

            gameInfo.characters.Add(newCharacter);
            gameInfo.teams.Add(teamClass);
        }
    }
    private Character CreateCharacter(Transform parent, bool isPlayer, PlayerData settings)
    {
        GameObject newCharObj = Instantiate(characterPrefab, parent.position, Quaternion.identity, parent);
        CreateCharacterItems(newCharObj, settings);

        newCharObj.GetComponentInChildren<ProjectileController>().enabled = true;
        newCharObj.GetComponent<MMCharacterController>().enabled = false;
        newCharObj.GetComponent<CharacterAudioController>().enabled = true;
        newCharObj.GetComponent<CharacterUIController>().ActivateUIComponents();
        newCharObj.GetComponent<ParticlesHandler>().ActivateEffects();
        if (isPlayer)
        {
            newCharObj.GetComponent<BotController>().enabled = false;
        }
        else
        {
            newCharObj.GetComponent<PlayerController>().enabled = false;
            newCharObj.GetComponent<CharacterUIController>().DestroyPlayerUIComponents();
        }

        Character characterClass = newCharObj.GetComponent<Character>();
        SetCharacterStats(characterClass);

        return characterClass;
        
    }
    private void CreateCharacterItems(GameObject characterObject, PlayerData settings)
    {
        SetCharacterItem(characterObject, gameItems.characterSkins[settings.skin_index]);
        SetCharacterItem(characterObject, gameItems.characterGuns[settings.gun_index]);
        SetCharacterItem(characterObject, gameItems.characterBags[settings.bag_index]);
        SetCharacterItem(characterObject, gameItems.characterBeards[settings.beard_index]);
        SetCharacterItem(characterObject, gameItems.characterHairs[settings.hair_index]);
        SetCharacterItem(characterObject, gameItems.characterHats[settings.hat_index]);
        SetCharacterItem(characterObject, gameItems.characterMasks[settings.mask_index]);
        SetCharacterItem(characterObject, gameItems.characterPouches[settings.pouch_index]);
        SetCharacterItem(characterObject, gameItems.characterScarfs[settings.scarf_index]);

        void SetCharacterItem(GameObject character, BaseItem item)
        {
            CharacterChildren characterChilds = character.GetComponent<CharacterChildren>();

            Mesh itemPrefabMesh = null;

            if (item.itemType != ItemType.Skin && item.itemType != ItemType.Gun)
            {
                if (item.prefab != null)
                    itemPrefabMesh = item.prefab.GetComponent<MeshFilter>().sharedMesh;
            }


            switch (item.itemType)
            {
                case ItemType.Gun:
                    Transform gunsChild = characterChilds.gun.transform;
                    foreach (Transform gunObj in gunsChild)
                        if (gunObj.childCount > 0)
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
    }
    private void SetCharacterStats(Character characterClass)
    {
        characterClass.health = GameMath.characterHP;
        characterClass.movementSpeed = GameMath.characterMovementSpeed;
        characterClass.baseHealth = characterClass.health;
    }
    private PlayerData GetRandomPlayerData()
    {
        PlayerData randomPlayerData = new PlayerData();
        randomPlayerData.gender = (Gender)UnityEngine.Random.Range(0, 2);

        randomPlayerData.gun_index = GetRandomItemIndex(gameItems.characterGuns);
        randomPlayerData.skin_index = GetRandomItemIndex(gameItems.characterSkins);
        randomPlayerData.bag_index = GetRandomItemIndex(gameItems.characterBags);
        randomPlayerData.beard_index = GetRandomItemIndex(gameItems.characterBeards);
        randomPlayerData.hair_index = GetRandomItemIndex(gameItems.characterHairs);
        randomPlayerData.hat_index = GetRandomItemIndex(gameItems.characterHats);
        randomPlayerData.mask_index = GetRandomItemIndex(gameItems.characterMasks);
        randomPlayerData.pouch_index = GetRandomItemIndex(gameItems.characterPouches);
        randomPlayerData.scarf_index = GetRandomItemIndex(gameItems.characterScarfs);

        #region Few Small Functions To Keep Things Tidy
        int GetRandomItemIndex(ItemRuntimeSet originalItemList)
        {
            List<BaseItem> items = originalItemList.Items.OrderBy(x => UnityEngine.Random.value).ToList();
            for (int i = 0; i < items.Count; i++)
                if (items[i].itemGender == Gender.Unisex || items[i].itemGender == randomPlayerData.gender)
                {
                    return originalItemList.IndexOf(items[i]);
                }
            return 0;
        }
        #endregion

        return randomPlayerData;
    }

    private void RandomizeCharactersPositions()
    {
        InitSpawnPositions();
        foreach (Character item in gameInfo.characters)
        {
            item.OnPositionChanged.Invoke(spawnPositions[0]);
            spawnPositions.RemoveAt(0);
        }
    }

    private void InitSpawnPositions()
    {
        spawnPositions = new List<Vector3>();
        foreach (Transform item in transform)
            spawnPositions.Add(item.position);
        spawnPositions = spawnPositions.OrderBy((x) => { return UnityEngine.Random.Range(0, 100000); }).ToList();
    }
    private void InitNickNames()
    {
        tmpBotNicknames = botNicknames.OrderBy((x) => { return UnityEngine.Random.value; }).ToList();
        tmpDevNicknames = devNicknames.OrderBy((x) => { return UnityEngine.Random.value; }).ToList();
    }
}
