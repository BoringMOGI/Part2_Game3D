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
                Item newItem = new Item(items[i]);      // i��° �������� �����ؼ� ����.
                newItem.itemCount = count;              // ������ ī��Ʈ�� count�� ����.

                return newItem;
            }
        }

        return null;
    }

}
