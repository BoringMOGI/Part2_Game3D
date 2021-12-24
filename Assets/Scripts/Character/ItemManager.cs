using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : ObjectPool<ItemManager, ItemObject>
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

    // 내가 전달해주는 Item을 담고 있는 아이템 오브젝트를 반환해달라.
    public ItemObject GetItemObject(Item item)
    {
        ItemObject pool = GetPool();        // 풀에서 오브젝트를 하나 꺼낸다.
        pool.Push(item);
        return pool;
    }

    // 새로운 아이템 데이터를 담고 있는 아이템 오브젝트를 반환해달라.
    public ItemObject GetItemObject(string itemKey, int count = 1)
    {
        ItemObject pool = GetPool();
        pool.Push(GetItem(itemKey, count));
        return pool;
    }
}
