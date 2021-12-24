using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] protected Image iconImage;
    [SerializeField] protected Text countText;

    protected Item item;

    // 현재 내가 선택중인 슬롯이 몇번째인가?
    public static int CurrentSlotIndex
    {
        get;
        private set;
    }
    public int SlotIndex => index;

    // 드래그 이벤트.
    int index;
    public delegate void OnSlotDragEvent(ItemSlot slot);
    public event OnSlotDragEvent onBeginDrag;
    public event OnSlotDragEvent onDragging;
    public event OnSlotDragEvent onEndDrag;

    public void Setup(int index)
    {
        this.index = index;
    }
    public void SetItem(Item item)
    {
        iconImage.enabled = true;
        countText.enabled = true;

        // 세팅.
        iconImage.sprite = item.itemSprite;
        countText.text = string.Format("x{0}", item.itemCount);

        this.item = item;
    }
    public void SetItem(ItemSlot slot)
    {
        iconImage.sprite = slot.iconImage.sprite;
        countText.text = slot.countText.text;
    }
    public void Clear()
    {
        iconImage.enabled = false;
        countText.enabled = false;

        item = null;
    }



    // 이벤트 함수.
    public void OnEnterSlot()
    {
        if (item != null)
            Tooltip.Instance.Show(item.GetTooltip());

        // 현재 슬롯 번호를 설정.
        CurrentSlotIndex = index;
    }
    public void OnExitSlot()
    {
        Tooltip.Instance.Close();

        // 현재 슬롯 번호를 -1(외부)로 설정
        CurrentSlotIndex = -1;
    }

    public void OnBeginDrag()
    {
        if (item == null)
            return;

        onBeginDrag?.Invoke(this);
    }
    public void OnDragging()
    {
        if (item == null)
            return;

        onDragging?.Invoke(this);
    }
    public void OnEndDrag()
    {
        if (item == null)
            return;

        onEndDrag?.Invoke(this);
    }

}
