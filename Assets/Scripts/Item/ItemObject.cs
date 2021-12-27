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

    Rigidbody body;

    public void Push(Item item)
    {
        this.item = item;
    }
    public void Show(Vector3 position)
    {
        transform.position = position;
        body.AddForce(Vector3.up * 2f, ForceMode.Impulse);
    }
    public void Setup(OnReturnPoolEvent<ItemObject> OnReturnPool)
    {
        this.OnReturnPool = OnReturnPool;
        body = GetComponent<Rigidbody>();
    }

    public string GetName()
    {
        return string.Format("{0}({1})", item.ItemName, item.Count);
    }

    public void OnInteraction()
    {
        PlayerController.Instance.AddItem(item);        // ������ �ִ� ������ ������ ����.

        if (OnReturnPool != null)
            OnReturnPool(this);                         // ������Ʈ�� Ǯ�� �ǵ���.
        else
            Destroy(gameObject);
    }
}
