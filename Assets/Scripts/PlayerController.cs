using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator anim;

    [Header("Attackable")]
    [SerializeField] Transform attackPivot;
    [SerializeField] float attackRadius;
    [SerializeField] LayerMask attackMask;

    bool isAttack;          // ���� ���ΰ�?

    void Update()
    {
        // GetMouseButtonDown(0:����, 1:������, 2:��)
        if(Input.GetMouseButtonDown(0) && !isAttack)
        {
            isAttack = true;
            anim.SetTrigger("onAttack");
        }
    }

    public void OnHit()
    {
        Collider[] hits = Physics.OverlapSphere(attackPivot.position, attackRadius, attackMask);
        foreach(Collider hit in hits)
        {
            Debug.Log(hit.gameObject.name);
        }

        OnEndAttack();
    }

    public void OnEndAttack()
    {
        isAttack = false;
    }

    private void OnDrawGizmos()
    {
        if(attackPivot != null)
        {
            Gizmos.DrawWireSphere(attackPivot.position, attackRadius);
        }
    }
}
