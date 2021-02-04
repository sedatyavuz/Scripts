using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItem3DButton : MonoBehaviour
{
    [SerializeField] private BaseItem item;

    public GameEventObject OnShopItemClicked;

    private void Awake()
    {
        item.OnItemDataChanged += UpdateDisplay;
        item.LoadData();
    }
    private void OnMouseDown()
    {
        if(!EventSystem.current.IsPointerOverGameObject())
            OnShopItemClicked.Invoke(item);
    }


    private void UpdateDisplay()
    {
        if (item.Selected)
            ShowOutline(Color.red);
        else if (item.Previewing)
            ShowOutline(Color.green);
        else if (!item.Selected && !item.Previewing)
            transform.GetComponent<Outline>().OutlineWidth = 0;
    }
    private void ShowOutline(Color color)
    {
        transform.GetComponent<Outline>().OutlineColor = color;
        transform.GetComponent<Outline>().OutlineWidth = 5;
    }
}
