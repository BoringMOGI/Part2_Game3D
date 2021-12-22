using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : Singleton<InventoryUI>
{
    [SerializeField] Transform slotParent;

    ItemSlot[] slots;

    private void Start()
    {
        // SlotParent에게서 자식 오브젝트 검색.
        slots = new ItemSlot[slotParent.childCount];    // 배열 객체 생성.
        for(int i = 0; i< slots.Length; i++)
            slots[i] = slotParent.GetChild(i).GetComponent<ItemSlot>();
    }

    public void UpdateInventory(Item[] inventory)
    {
        for(int i = 0; i<inventory.Length; i++)
        {
            if (inventory[i] != null)               // 전달하려는 아이템이 있을 경우.
                slots[i].Setup(inventory[i]);       // 아이템 슬롯에 세팅한다.
            else
                slots[i].Clear();                   // 아이템이 없으면 Clear 한다.
        }
    }

}
