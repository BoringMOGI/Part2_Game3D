using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class PlayerController : Singleton<PlayerController>
{
    [Header("Interaction")]
    [SerializeField] KeyCode interactHotKey;            // 상호작용 키.
    [SerializeField] float searchRadius;                // 탐지 범위.

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


    // 상호작용할 물체를 찾는다.
    void Interaction()
    {
        // Vector3.방향 : 절대 좌표 상의 방향.
        // Transform.방향 : (내 기준)로컬 좌표 상의 방향.

        // 전체 검색을 해 가장 최초로 잡힌 대상과 상호작용.
        interactionTarget = null;

        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);
        foreach(Collider collider in colliders)
        {
            interactionTarget = collider.GetComponent<IInteraction>();
            if (interactionTarget != null)
            {
                // 인터페이스를 구현한 대상을 가져와 대상의 정보를 출력.
                InteractionUI.Instance.Open(interactHotKey.ToString(), interactionTarget.GetName());
                break;
            }
        }

        // 아무것도 검색되지 않았다면 UI를 닫는다.
        if (interactionTarget == null)
        {
            InteractionUI.Instance.Close();
        }
        // 상호작용 키를 눌렀다면.
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
        // 슬롯이 아닌 칸으로 움직였다.
        if(after == -1)
        {

        }
        else
        {
            Swap(inventory, before, after);                     // 이전, 이후 지점의 아이템을 교환.
            InventoryUI.Instance.UpdateInventory(inventory);    // UI에게 그려달라고 요청.
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
        // 추가하려는 아이템이 없는 경우.
        if (item == null)
            return false;

        // 같은 아이템이 있는지 찾는다.
        for (int i = 0; i < inventory.Length; i++)
        {
            if(inventory[i] != null && inventory[i].Equals(item))
            {
                inventory[i].Add(item);
                InventoryUI.Instance.UpdateInventory(inventory);
                return true;
            }
        }

        // 빈 공간 탐색.
        int emptyIndex = EmptyInven();

        // 빈 공간이 없을 경우.
        if (emptyIndex == -1)
            return false;

        // 빈 공간에 아이템 추가.
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
