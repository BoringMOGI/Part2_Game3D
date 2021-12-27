using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : ObjectPool<ItemManager, ItemObject>
{
    [SerializeField] List<ItemData> items = new List<ItemData>();

    private ItemData GetItemData(string itemKey)
    {
        foreach(ItemData item in items)
        {
            if(item.itemKey == itemKey)
                return item;
        }

        return null;
    }

    public Item GetItem(string itemKey, int itemCount = 1)
    {
        itemCount = Mathf.Clamp(itemCount, 1, 9999);    // 매게변수 수량의 개수를 최소, 최대값으로 제한.

        ItemData data = GetItemData(itemKey);           // itemKey를 통해 아이템 데이터 찾기.
        if(data != null)                                // 데이터를 찾았다면.
        {
            Item item = new Item(data, itemCount);      // 데이터와 수량을 포함하는 아이템 클래스 생성.
            return item;                                // 해당 아이템 리턴.
        }

        return null;                                    // 못찾았다면 null을 리턴.
    }

    // 내가 전달해주는 Item을 담고 있는 아이템 오브젝트를 반환해달라.
    public ItemObject GetItemObject()
    {
        return GetPool();
    }
}
