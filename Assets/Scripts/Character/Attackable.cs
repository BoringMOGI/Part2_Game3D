using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stateable))]
public class Attackable : MonoBehaviour
{
    [Header("Attackable")]
    [SerializeField] Transform attackPivot;
    [SerializeField] float attackRadius;
    [SerializeField] LayerMask exceptMask;          // 공격 제외 대상.

    LayerMask attackMask = int.MaxValue;  // 1111 1111.
    Stateable stat;

    /*
               1111 1111        XOR  A   B   R
          XOR) 0001 1000             1   1   0
          --------------             1   0   1
               1110 0111             0   1   1

            AND : & (그리고)   A논리 & B논리  :  둘 다 맞아야 TRUE.
            OR  : | (또는)     A논리 | B논리  :  둘 중 하나만 맞아'도' TRUE.
            XOR : ^ (....)     A논리 ^ B논리  :  둘 중 하나만 맞아'야' TRUE.
            NOT : ! (반대)      !A논리         :  TRUE면 FALSE, FLASE면 TRUE.
    */

    private void Start()
    {
        stat = GetComponent<Stateable>();

        // 모든 대상에서 except만 제외한다.
        attackMask = attackMask ^ exceptMask;
    }

    public void Attack()
    {
        // 10진수 3을 이진수로 바꾸면 -> 0000 0011.
        // 하지만 플레이어의 비트는 -> 0000 1000(8)

        // << (LEFT SHIFT 연산자 : 비트를 왼쪽으로 n칸 옮겨라).
        // 1은 이진수로 0000 0001, 플레이어의 NameToLayer 값은 3.
        // 결과 값은 0000 1000(8)

        Collider[] hits = Physics.OverlapSphere(attackPivot.position, attackRadius, attackMask);
        foreach (Collider hit in hits)
        {
            Damageable target = hit.GetComponent<Damageable>();
            if (target != null)
            {
                target.OnDamaged(stat.Stat);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPivot != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(attackPivot.position, attackRadius);
        }
    }

}
