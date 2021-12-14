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

    [Header("Attackable")]
    [SerializeField] Transform attackPivot;
    [SerializeField] float attackRadius;
    [SerializeField] LayerMask attackMask;

    [Header("Etc")]
    [SerializeField] float comboDelay;

    Coroutine comboReset;

    bool isAttack;          // 공격 중인가?
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
        Cursor.lockState = CursorLockMode.Locked;       // 마우스 잠그기.
        Cursor.visible = false;                         // 마우스 포인터 비활성화.
    }

    void Update()
    {
        // GetMouseButtonDown(0:왼쪽, 1:오른쪽, 2:휠)
        if(Input.GetMouseButtonDown(0) && !isAttack && combo < 3)
        {
            Attack();
        }
    }

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

        Collider[] hits = Physics.OverlapSphere(attackPivot.position, attackRadius, attackMask);
        foreach (Collider hit in hits)
        {
            IDamageable target = hit.GetComponent<IDamageable>();
            if(target != null)
            {
                float damage = Random.Range(10, 30);            // 최소, 최대값 사이의 랜덤 값.
                bool isCri = (Random.value * 100f) < 20;        // 0~100사이 값이 20보다 작으면 (20%)

                if(isCri)
                {
                    damage *= 1.5f;                             // 데미지 1.5배 증가.
                }

                target.OnDamaged(damage, isCri ? DAMAGE_TYPE.Critical : DAMAGE_TYPE.Normal);
            }
        }
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





private void OnDrawGizmos()
    {
        if(attackPivot != null)
        {
            Gizmos.DrawWireSphere(attackPivot.position, attackRadius);
        }
    }
}
