using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class PlayerController : Singleton<PlayerController>
{
    [Header("Interaction")]
    [SerializeField] KeyCode interactHotKey;            // ��ȣ�ۿ� Ű.
    [SerializeField] float searchRadius;                // Ž�� ����.

    IInteraction interactionTarget;

    void Start()
    {
        OnStart();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            InventoryUI.Instance.SwitchInventory();
        }

        Interaction();
    }


    // ��ȣ�ۿ��� ��ü�� ã�´�.
    void Interaction()
    {
        // Vector3.���� : ���� ��ǥ ���� ����.
        // Transform.���� : (�� ����)���� ��ǥ ���� ����.

        // ��ü �˻��� �� ���� ���ʷ� ���� ���� ��ȣ�ۿ�.
        interactionTarget = null;

        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);
        foreach(Collider collider in colliders)
        {
            interactionTarget = collider.GetComponent<IInteraction>();
            if (interactionTarget != null)
            {
                // �������̽��� ������ ����� ������ ����� ������ ���.
                InteractionUI.Instance.Open(interactHotKey.ToString(), interactionTarget.GetName());
                break;
            }
        }

        // �ƹ��͵� �˻����� �ʾҴٸ� UI�� �ݴ´�.
        if (interactionTarget == null)
        {
            InteractionUI.Instance.Close();
        }
        // ��ȣ�ۿ� Ű�� �����ٸ�.
        else if(Input.GetKeyDown(interactHotKey))
        {
            interactionTarget.OnInteraction();
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}

public partial class PlayerController
{
    [Header("Item")]
    [SerializeField] Item[] firstItems;

    Item[] inventory = new Item[20];

    void OnStart()
    {
        InventoryUI.Instance.onChangedInven += OnChangedInven;

        for (int i = 0; i < firstItems.Length; i++)
            AddItem(firstItems[i]);
    }

    private void OnChangedInven(int before, int after)
    {
        // ������ �ƴ� ĭ���� ��������.
        if(after == -1)
        {

        }
        else
        {
            Swap(inventory, before, after);                     // ����, ���� ������ �������� ��ȯ.
            InventoryUI.Instance.UpdateInventory(inventory);    // UI���� �׷��޶�� ��û.
        }
    }
    private int EmptyInven()
    {
        for(int i = 0; i<inventory.Length; i++)
        {
            if (inventory[i] == null)
                return i;
        }

        return -1;
    }

    public bool AddItem(Item item)
    {
        // �߰��Ϸ��� �������� ���� ���.
        if (item == null)
            return false;

        // ���� �������� �ִ��� ã�´�.
        for (int i = 0; i < inventory.Length; i++)
        {
            if(inventory[i] != null && inventory[i].Equals(item))
            {
                inventory[i].Add(item);
                InventoryUI.Instance.UpdateInventory(inventory);
                return true;
            }
        }

        // �� ���� Ž��.
        int emptyIndex = EmptyInven();

        // �� ������ ���� ���.
        if (emptyIndex == -1)
            return false;

        // �� ������ ������ �߰�.
        inventory[emptyIndex] = item;
        InventoryUI.Instance.UpdateInventory(inventory);
        return true;
    }

    private void Swap<T>(T[] array, int a, int b)
    {
        T temp = array[a];
        array[a] = array[b];
        array[b] = temp;
    }
    
}
