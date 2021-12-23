using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] List<Item> items = new List<Item>();

    public Item GetItem(string itemKey, int count = 1)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemKey == itemKey)
            {
                Item newItem = new Item(items[i]);      // i번째 아이템을 복사해서 생성.
                newItem.itemCount = count;              // 아이템 카운트는 count로 설정.

                return newItem;
            }
        }

        return null;
    }

}
