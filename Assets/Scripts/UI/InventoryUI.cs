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
        // SlotParent���Լ� �ڽ� ������Ʈ �˻�.
        slots = new ItemSlot[slotParent.childCount];    // �迭 ��ü ����.
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotParent.GetChild(i).GetComponent<ItemSlot>();
            slots[i].Setup(i);

            // ������ ���Կ� �̺�Ʈ ����.
            slots[i].onBeginDrag += OnBeginDrag;
            slots[i].onDragging += OnDragging;
            slots[i].onEndDrag += OnEndDrag;
        }

        previewSlot.gameObject.SetActive(false);        // �̸����� ���� ����.
        panel.SetActive(false);
    }

    public bool SwitchInventory()
    {
        // activeSelf : ���� ������Ʈ�� Ȱ��ȭ �Ǿ��ִ���.
        panel.SetActive(!panel.activeSelf);
        return panel.activeSelf;
    }
    public void UpdateInventory(Item[] inventory)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null)               // �����Ϸ��� �������� ���� ���.
                slots[i].SetItem(inventory[i]);     // ������ ���Կ� �����Ѵ�.
            else
                slots[i].Clear();                   // �������� ������ Clear �Ѵ�.
        }
    }

    private void OnBeginDrag(ItemSlot slot)
    {
        // �巡�װ� ���۵� �� �̸����� ������ �Ҵ�.
        previewSlot.gameObject.SetActive(true);
        previewSlot.SetItem(slot);
    }
    private void OnDragging(ItemSlot slot)
    {
        // �巡�� ���� �� �̸����� ������ ��ġ�� �����Ѵ�.
        previewSlot.transform.position = Input.mousePosition;
    }
    private void OnEndDrag(ItemSlot slot)
    {
        // �巡�װ� ������ �� �̸����� ������ ����.
        previewSlot.gameObject.SetActive(false);

        // �̺�Ʈ ����.
        onChangedInven?.Invoke(slot.SlotIndex, ItemSlot.CurrentSlotIndex);
    }

}
