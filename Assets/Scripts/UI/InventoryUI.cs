using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : Singleton<InventoryUI>
{
    [SerializeField] GameObject panel;
    [SerializeField] Transform slotParent;
    [SerializeField] PreviewSlot previewSlot;

    ItemSlot[] slots;

    public delegate void OnChangedInvenEvent(int before, int after);
    public event OnChangedInvenEvent onChangedInven;

    private void Start()
    {
        // SlotParent에게서 자식 오브젝트 검색.
        slots = new ItemSlot[slotParent.childCount];    // 배열 객체 생성.
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotParent.GetChild(i).GetComponent<ItemSlot>();
            slots[i].Setup(i);

            // 아이템 슬롯에 이벤트 연결.
            slots[i].onBeginDrag += OnBeginDrag;
            slots[i].onDragging += OnDragging;
            slots[i].onEndDrag += OnEndDrag;
        }

        previewSlot.gameObject.SetActive(false);        // 미리보기 슬롯 끄기.
        panel.SetActive(false);
    }

    public bool SwitchInventory()
    {
        // activeSelf : 게임 오브젝트가 활성화 되어있는지.
        panel.SetActive(!panel.activeSelf);
        return panel.activeSelf;
    }
    public void UpdateInventory(Item[] inventory)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null)               // 전달하려는 아이템이 있을 경우.
                slots[i].SetItem(inventory[i]);     // 아이템 슬롯에 세팅한다.
            else
                slots[i].Clear();                   // 아이템이 없으면 Clear 한다.
        }
    }

    private void OnBeginDrag(ItemSlot slot)
    {
        // 드래그가 시작될 때 미리보기 슬롯을 켠다.
        previewSlot.gameObject.SetActive(true);
        previewSlot.SetItem(slot);
    }
    private void OnDragging(ItemSlot slot)
    {
        // 드래그 중일 때 미리보기 슬롯의 위치를 변경한다.
        previewSlot.transform.position = Input.mousePosition;
    }
    private void OnEndDrag(ItemSlot slot)
    {
        // 드래그가 끝났을 때 미리보기 슬롯을 끈다.
        previewSlot.gameObject.SetActive(false);

        // 이벤트 전달.
        onChangedInven?.Invoke(slot.SlotIndex, ItemSlot.CurrentSlotIndex);
    }

}
