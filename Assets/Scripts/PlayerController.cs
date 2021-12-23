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

    bool isAttack;          // ���� ���ΰ�?
    bool isDead;            // �׾��°�?
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
        // GetMouseButtonDown(0:����, 1:������, 2:��)
        if (Input.GetMouseButtonDown(0) && !isDead && !isAttack && combo < 3)
        {
            Attack();
        }
    }

    // ����.
    void Attack()
    {
        // ���� ���� Ÿ�̸�.
        if (comboReset != null)
            StopCoroutine(comboReset);

        // ���� ����.
        isAttack = true;
        combo += 1;
        anim.SetTrigger("onAttack");
    }
    public void OnHit()
    {
        comboReset = StartCoroutine(ComboReset());
        OnEndAttack();

        attackable.Attack();        // ����!!
    }

    public void OnEndAttack()
    {
        isAttack = false;
    }

    IEnumerator ComboReset()
    {
        float comboTime = comboDelay;                       // ���� ���� �ð�.

        while ((comboTime -= Time.deltaTime) > 0.0f)       // �ð��� �� �Ǿ��ٸ�.
            yield return null;

        combo = 0;                                          // �޺��� �ʱ�ȭ.
    }

    // �ǰ�.
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
    public bool AddItem(Item item)
    {
        // �߰��Ϸ��� �������� ���� ���.
        if (item == null)
            return false;

        for(int i = 0; i<inventory.Length; i++)
        {
            // �� ������ �߰��ߴ�.
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
