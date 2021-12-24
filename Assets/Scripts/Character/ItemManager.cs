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
                Item newItem = new Item(items[i]);      // i��° �������� �����ؼ� ����.
                newItem.itemCount = count;              // ������ ī��Ʈ�� count�� ����.

                return newItem;
            }
        }

        return null;
    }

    // ���� �������ִ� Item�� ��� �ִ� ������ ������Ʈ�� ��ȯ�ش޶�.
    public ItemObject GetItemObject(Item item)
    {
        ItemObject pool = GetPool();        // Ǯ���� ������Ʈ�� �ϳ� ������.
        pool.Push(item);
        return pool;
    }

    // ���ο� ������ �����͸� ��� �ִ� ������ ������Ʈ�� ��ȯ�ش޶�.
    public ItemObject GetItemObject(string itemKey, int count = 1)
    {
        ItemObject pool = GetPool();
        pool.Push(GetItem(itemKey, count));
        return pool;
    }
}
