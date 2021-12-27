using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/CreateNew")]
public class ItemData : ScriptableObject
{
    public enum TYPE
    {
        Equipment,      // 장비.
        Useable,        // 소비.
        Etc,            // 기타.
    }

    public string itemKey;    
    public string itemName;
    public Sprite itemSprite;
    public TYPE itemType;
}
