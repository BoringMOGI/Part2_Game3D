using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/CreateNew")]
public class ItemData : ScriptableObject
{
    public enum TYPE
    {
        Equipment,      // ���.
        Useable,        // �Һ�.
        Etc,            // ��Ÿ.
    }

    public string itemKey;    
    public string itemName;
    public Sprite itemSprite;
    public TYPE itemType;
}
