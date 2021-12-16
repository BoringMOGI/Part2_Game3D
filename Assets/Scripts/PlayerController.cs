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

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Attackable attackable;

    [Header("Etc")]
    [SerializeField] float comboDelay;

    Coroutine comboReset;

    bool isAttack;          // ���� ���ΰ�?
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

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;       // ���콺 ��ױ�.
        Cursor.visible = false;                         // ���콺 ������ ��Ȱ��ȭ.
    }

    void Update()
    {
        // GetMouseButtonDown(0:����, 1:������, 2:��)
        if(Input.GetMouseButtonDown(0) && !isAttack && combo < 3)
        {
            Attack();
        }
    }

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






}
