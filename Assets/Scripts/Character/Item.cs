using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemKey;
    public Sprite itemSprite;
    public string itemName;
    public int itemCount;

    // ���� ������.
    public Item(Item copy)
    {
        itemKey    = copy.itemKey;
        itemSprite = copy.itemSprite;
        itemName   = copy.itemName;
        itemCount  = copy.itemCount;
    }


    public string GetTooltip()
    {
        string tip = string.Empty;

        tip = string.Concat(tip, $"������ �̸� : {itemName}\n");
        tip = string.Concat(tip, $"������ ���� : {itemCount}");

        return tip;
    }

}
