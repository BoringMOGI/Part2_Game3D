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
        itemCount = Mathf.Clamp(itemCount, 1, 9999);    // �ŰԺ��� ������ ������ �ּ�, �ִ밪���� ����.

        ItemData data = GetItemData(itemKey);           // itemKey�� ���� ������ ������ ã��.
        if(data != null)                                // �����͸� ã�Ҵٸ�.
        {
            Item item = new Item(data, itemCount);      // �����Ϳ� ������ �����ϴ� ������ Ŭ���� ����.
            return item;                                // �ش� ������ ����.
        }

        return null;                                    // ��ã�Ҵٸ� null�� ����.
    }

    // ���� �������ִ� Item�� ��� �ִ� ������ ������Ʈ�� ��ȯ�ش޶�.
    public ItemObject GetItemObject()
    {
        return GetPool();
    }
}
