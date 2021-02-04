using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

    [SerializeField] private PlayerData playerData;

    #region Events
    [SerializeField] private GameEvent OnItemUnlock;
    [SerializeField] private GameEventObject OnShopItemClicked;
    [SerializeField] private GameEventObject OnCharacterItemChanged;
    [SerializeField] private GameEvent OnButtonClick;
    [SerializeField] private GameEvent OnSelectionFinished;
    [SerializeField] private GameEvent OnReloadItems;

    #endregion

    [SerializeField] private GameItems gameItems;

    #region Shop Item Containers
    [Space]
    [SerializeField] private GameObject bagContainer;
    [SerializeField] private GameObject beardContainer;
    [SerializeField] private GameObject hairContainer;
    [SerializeField] private GameObject hatContainer;
    [SerializeField] private GameObject maskContainer;
    [SerializeField] private GameObject pouchContainer;
    [SerializeField] private GameObject scarfContainer;

    #endregion
    
    [Space]
    [SerializeField] private GameObject shopItemButtonPrefab;

    [Space]
    [SerializeField] private GameObject maleCharactersParent;
    [SerializeField] private GameObject femaleCharactersParnet;

    [Space]
    [SerializeField] private GameObject buyOrSelectButton;
    [SerializeField] private GameObject priceObj;

    private BaseItem currentPreviewItem;

    private void Awake()
    {
        OnShopItemClicked.AddListener((o) => { OnItemClick((BaseItem)o); });
        OnSelectionFinished.AddListener(InitilizeShop);
        OnReloadItems.AddListener(() => { if(currentPreviewItem) currentPreviewItem.Previewing = false; });
    }

    private void InitilizeShop()
    {
        if (playerData.gender == Gender.Male)
        {
            maleCharactersParent.SetActive(true);
            femaleCharactersParnet.SetActive(false);
        }
        else
        {
            maleCharactersParent.SetActive(false);
            femaleCharactersParnet.SetActive(true);
        }

        PopulateItemContainer(bagContainer, gameItems.characterBags);
        PopulateItemContainer(beardContainer, gameItems.characterBeards);
        PopulateItemContainer(hairContainer, gameItems.characterHairs);
        PopulateItemContainer(hatContainer, gameItems.characterHats);
        PopulateItemContainer(maskContainer, gameItems.characterMasks);
        PopulateItemContainer(pouchContainer, gameItems.characterPouches);
        PopulateItemContainer(scarfContainer, gameItems.characterScarfs);

        if (!PlayerPrefs.HasKey("ShopInitilised"))
        {
            //set first items as default.
            PlayerPrefs.SetInt("ShopInitilised", 1);
            PlayerPrefs.Save();
            BuyOrSelect(gameItems.characterGuns[playerData.gun_index]);
            BuyOrSelect(gameItems.characterSkins[playerData.skin_index]);
            BuyOrSelect(gameItems.characterBags[playerData.bag_index]);
            BuyOrSelect(gameItems.characterBeards[playerData.beard_index]);
            BuyOrSelect(gameItems.characterHairs[playerData.hair_index]);
            BuyOrSelect(gameItems.characterHats[playerData.hat_index]);
            BuyOrSelect(gameItems.characterMasks[playerData.mask_index]);
            BuyOrSelect(gameItems.characterPouches[playerData.pouch_index]);
            BuyOrSelect(gameItems.characterScarfs[playerData.scarf_index]);
        }
    }
    private void PopulateItemContainer(GameObject content, ItemRuntimeSet items)
    {
        RectTransform contentRect = content.GetComponent<RectTransform>();
        float yItemSize = shopItemButtonPrefab.GetComponent<RectTransform>().sizeDelta.y;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, items.Count * yItemSize);

        foreach (BaseItem item in items)
        {
            if (item.itemGender == Gender.Unisex || item.itemGender == playerData.gender)
            {
                GameObject newShopItem = Instantiate(shopItemButtonPrefab, content.transform);
                ShopItemButton buttonScript = newShopItem.GetComponent<ShopItemButton>();
                buttonScript.InitilizeShopListing(item);
            }
        }
    }

    public void BuyOrSelectButtonOnClick()
    {
        OnButtonClick.Invoke();
        BuyOrSelect(currentPreviewItem);
    }

    public void BuyOrSelect(BaseItem item)
    {
        if (!item.Unlocked)
        {
            if (playerData.PlayerBalance >= item.price)
            {
                item.Unlocked = true;
                playerData.PlayerBalance -= item.price;
                PlayerDataManager.SavePlayerData(playerData);
                item.SaveData();
            }
        }

        if(item.Unlocked)
        {
            BaseItem currentItem = null;
            switch (item.itemType)
            {
                case ItemType.Gun:
                    currentItem = gameItems.characterGuns[playerData.gun_index];
                    playerData.gun_index = gameItems.characterGuns.IndexOf(item);
                    break;
                case ItemType.Skin:
                    currentItem = gameItems.characterSkins[playerData.skin_index];
                    playerData.skin_index = gameItems.characterSkins.IndexOf(item);
                    break;
                case ItemType.Bag:
                    currentItem = gameItems.characterBags[playerData.bag_index];
                    playerData.bag_index = gameItems.characterBags.IndexOf(item);
                    break;
                case ItemType.Beard:
                    currentItem = gameItems.characterBeards[playerData.beard_index];
                    playerData.beard_index = gameItems.characterBeards.IndexOf(item);
                    break;
                case ItemType.Hair:
                    currentItem = gameItems.characterHairs[playerData.hair_index];
                    playerData.hair_index = gameItems.characterHairs.IndexOf(item);
                    break;
                case ItemType.Hat:
                    currentItem = gameItems.characterHats[playerData.hat_index];
                    playerData.hat_index = gameItems.characterHats.IndexOf(item);
                    break;
                case ItemType.Mask:
                    currentItem = gameItems.characterMasks[playerData.mask_index];
                    playerData.mask_index = gameItems.characterMasks.IndexOf(item);
                    break;
                case ItemType.Pouch:
                    currentItem = gameItems.characterPouches[playerData.pouch_index];
                    playerData.pouch_index = gameItems.characterPouches.IndexOf(item);
                    break;
                case ItemType.Scarf:
                    currentItem = gameItems.characterScarfs[playerData.scarf_index];
                    playerData.scarf_index = gameItems.characterScarfs.IndexOf(item);
                    break;
                default:
                    break;
            }

            if (currentItem)
            {
                currentItem.Selected = false;
                currentItem.SaveData();
            }

            item.Selected = true;
            item.SaveData();
            PlayerDataManager.SavePlayerData(playerData);
            
            buyOrSelectButton.SetActive(false);
            priceObj.SetActive(false);
        }

    }

    private void OnItemClick(BaseItem item)
    {
        OnButtonClick.Invoke();

        if (currentPreviewItem)
        {
            currentPreviewItem.Previewing = false;
        }

        currentPreviewItem = item;
        OnCharacterItemChanged.Invoke(currentPreviewItem);
        item.Previewing = true;

        if (item.Selected)
        {
            buyOrSelectButton.SetActive(false);
            priceObj.SetActive(false);
        }
        else if (item.Unlocked)
        {
            buyOrSelectButton.SetActive(true);
            priceObj.SetActive(false);
            buyOrSelectButton.GetComponentInChildren<Text>().text = "Select";
        }
        else
        {
            buyOrSelectButton.SetActive(true);
            priceObj.SetActive(true);
            priceObj.GetComponentInChildren<Text>().text = item.price.ToString();
            buyOrSelectButton.GetComponentInChildren<Text>().text = "Buy";
        }
    }
}
