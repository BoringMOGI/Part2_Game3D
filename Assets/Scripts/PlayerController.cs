using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void OnDamaged(float amount, DAMAGE_TYPE type = DAMAGE_TYPE.Normal);
}
public enum DAMAGE_TYPE
{
    Normal,
    Critical,
}


public partial class PlayerController : Singleton<PlayerController>
{
    [SerializeField] Animator anim;
    [SerializeField] Attackable attackable;

    [Header("Etc")]
    [SerializeField] float comboDelay;

    Coroutine comboReset;

    bool isAttack;          // 공격 중인가?
    bool isDead;            // 죽었는가?
    int combo
    {
        get
        {
            return anim.GetInteger("combo");
        }
        set
        {
            anim.SetInteger("combo", value);
        }
    }

    void Start()
    {
        StartCoroutine(ComboReset());

        OnStart();
    }
    void Update()
    {
        // GetMouseButtonDown(0:왼쪽, 1:오른쪽, 2:휠)
        if (Input.GetMouseButtonDown(0) && !isDead && !isAttack && combo < 3)
        {
            Attack();
        }
    }

    // 공격.
    void Attack()
    {
        // 연속 공격 타이머.
        if (comboReset != null)
            StopCoroutine(comboReset);

        // 변수 지정.
        isAttack = true;
        combo += 1;
        anim.SetTrigger("onAttack");
    }
    public void OnHit()
    {
        comboReset = StartCoroutine(ComboReset());
        OnEndAttack();

        attackable.Attack();        // 공격!!
    }

    public void OnEndAttack()
    {
        isAttack = false;
    }

    IEnumerator ComboReset()
    {
        float comboTime = comboDelay;                       // 연속 공격 시간.

        while ((comboTime -= Time.deltaTime) > 0.0f)       // 시간이 다 되었다면.
            yield return null;

        combo = 0;                                          // 콤보를 초기화.
    }

    // 피격.
    public void OnDamaged()
    {

    }
    public void OnDead()
    {
        isDead = true;
        gameObject.layer = LayerMask.NameToLayer("Player_Dead");

        anim.SetTrigger("onDead");
    }
}

public partial class PlayerController
{
    Item[] inventory = new Item[20];

    void OnStart()
    {
        AddItem(ItemManager.Instance.GetItem("potion1"));
        AddItem(ItemManager.Instance.GetItem("potion2"));
        AddItem(ItemManager.Instance.GetItem("potion3"));

        InventoryUI.Instance.onChangedInven += OnChangedInven;
        InventoryUI.Instance.UpdateInventory(inventory);
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
    public bool AddItem(Item item)
    {
        // 추가하려는 아이템이 없는 경우.
        if (item == null)
            return false;

        for(int i = 0; i<inventory.Length; i++)
        {
            // 빈 공간을 발견했다.
            if(inventory[i] == null)
            {
                inventory[i] = item;
                return true;
            }
        }

        return false;
    }

    private void Swap<T>(T[] array, int a, int b)
    {
        T temp = array[a];
        array[a] = array[b];
        array[b] = temp;
    }
    
}
