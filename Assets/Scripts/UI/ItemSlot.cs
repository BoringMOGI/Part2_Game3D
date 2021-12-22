using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Text countText;

    public void Setup(Sprite iconSprite, int count)
    {
        iconImage.enabled = true;
        countText.enabled = true;

        // ¼¼ÆÃ.
        iconImage.sprite = iconSprite;
        countText.text = string.Format("x{0}", count);
    }
    public void Setup(Item item)
    {
        Setup(item.itemSprite, item.itemCount);
    }

    public void Clear()
    {
        iconImage.enabled = false;
        countText.enabled = false;
    }

}
