using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopItemButton : MonoBehaviour
{
    [SerializeField] private Shader shopItemMaskShader;

    private Image background;

    private static Color buttonColorNormal = new Color(0.784f, 0.784f, 0.784f, 1);
    private static Color buttonColorSelected = new Color(0.588f, 1, 1, 1);

    private BaseItem item;
    [SerializeField] private GameEventObject OnShopItemClicked;

    public void InitilizeShopListing(BaseItem item)
    {
        GetComponent<Button>().onClick.AddListener(HandleClick);
        background = GetComponent<Image>();
        this.item = item;
        this.item.OnItemDataChanged += UpdateDisplay;
        CreatePrefab();
        this.item.LoadData();
    }
    private void CreatePrefab()
    {
        if (item.prefab == null)
            return;

        GameObject itemObj = Instantiate(item.prefab, transform);

        switch (item.itemType)
        {
            case ItemType.Bag:
                itemObj.transform.localScale = Vector3.one * 200;
                itemObj.transform.localPosition = new Vector3(0, 0, 0);
                itemObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case ItemType.Beard:
                itemObj.transform.localScale = Vector3.one * 500;
                itemObj.transform.localPosition = new Vector3(0, 0, 0);
                itemObj.transform.localRotation = Quaternion.Euler(0, 180, 0);
                break;
            case ItemType.Hair:
                itemObj.transform.localScale = Vector3.one * 200;
                itemObj.transform.localPosition = new Vector3(0, 0, 0);
                itemObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case ItemType.Hat:
                itemObj.transform.localScale = Vector3.one * 200;
                itemObj.transform.localPosition = new Vector3(0, 0, 0);
                itemObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case ItemType.Mask:
                itemObj.transform.localScale = Vector3.one * 200;
                itemObj.transform.localPosition = new Vector3(0, 0, -40);
                itemObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case ItemType.Pouch:
                itemObj.transform.localScale = Vector3.one * 200;
                itemObj.transform.localPosition = new Vector3(0, 0, 0);
                itemObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case ItemType.Scarf:
                itemObj.transform.localScale = Vector3.one * 200;
                itemObj.transform.localPosition = new Vector3(0, 0, 0);
                itemObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            default:
                break;
        }

        return;
        Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material mat = new Material(shopItemMaskShader);
            mat.SetTexture("_MainTex", renderer.material.GetTexture("_BaseMap"));
            renderer.material = mat;
        }
    }

    private void HandleClick() => OnShopItemClicked.Invoke(item);

    private void UpdateDisplay()
    {
        if (item.Selected)
            background.color = Color.red;
        else if (item.Previewing)
            background.color = Color.green;
        else if (!item.Selected && !item.Previewing)
            background.color = buttonColorNormal;
    }
}