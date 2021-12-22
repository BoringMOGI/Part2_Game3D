using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : Singleton<InventoryUI>
{
    [SerializeField] Transform slotParent;

    ItemSlot[] slots;

    private void Start()
    {
        // SlotParent���Լ� �ڽ� ������Ʈ �˻�.
        slots = new ItemSlot[slotParent.childCount];    // �迭 ��ü ����.
        for(int i = 0; i< slots.Length; i++)
            slots[i] = slotParent.GetChild(i).GetComponent<ItemSlot>();
    }

    public void UpdateInventory(Item[] inventory)
    {
        for(int i = 0; i<inventory.Length; i++)
        {
            if (inventory[i] != null)               // �����Ϸ��� �������� ���� ���.
                slots[i].Setup(inventory[i]);       // ������ ���Կ� �����Ѵ�.
            else
                slots[i].Clear();                   // �������� ������ Clear �Ѵ�.
        }
    }

}
