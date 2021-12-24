using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteraction
{
    string GetName();
    void OnInteraction();
}

public class ItemObject : MonoBehaviour, IPool<ItemObject>, IInteraction
{
    OnReturnPoolEvent<ItemObject> OnReturnPool;
    
    [SerializeField] Item item;

    public void Push(Item item)
    {
        this.item = item;
    }
    public void Setup(OnReturnPoolEvent<ItemObject> OnReturnPool)
    {
        this.OnReturnPool = OnReturnPool;
    }

    public string GetName()
    {
        return item.itemName;
    }

    public void OnInteraction()
    {
        PlayerController.Instance.AddItem(item);        // 가지고 있는 아이템 데이터 전달.

        if (OnReturnPool != null)
            OnReturnPool(this);                         // 오브젝트를 풀로 되돌림.
        else
            Destroy(gameObject);
    }
}
