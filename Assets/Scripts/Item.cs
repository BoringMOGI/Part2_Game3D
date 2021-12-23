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

    // 복사 생성자.
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

        tip = string.Concat(tip, $"아이템 이름 : {itemName}\n");
        tip = string.Concat(tip, $"아이템 수량 : {itemCount}");

        return tip;
    }

}
