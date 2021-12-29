using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stateable))]
public class Attackable : MonoBehaviour
{
    [Header("Attackable")]
    [SerializeField] Transform attackPivot;
    [SerializeField] float attackRadius;
    [SerializeField] LayerMask exceptMask;          // ���� ���� ���.

    LayerMask attackMask = int.MaxValue;  // 1111 1111.
    Stateable stat;

    /*
               1111 1111        XOR  A   B   R
          XOR) 0001 1000             1   1   0
          --------------             1   0   1
               1110 0111             0   1   1

            AND : & (�׸���)   A�� & B��  :  �� �� �¾ƾ� TRUE.
            OR  : | (�Ǵ�)     A�� | B��  :  �� �� �ϳ��� �¾�'��' TRUE.
            XOR : ^ (....)     A�� ^ B��  :  �� �� �ϳ��� �¾�'��' TRUE.
            NOT : ! (�ݴ�)      !A��         :  TRUE�� FALSE, FLASE�� TRUE.
    */

    private void Start()
    {
        stat = GetComponent<Stateable>();

        // ��� ��󿡼� except�� �����Ѵ�.
        attackMask = attackMask ^ exceptMask;
    }

    public void Attack()
    {
        // 10���� 3�� �������� �ٲٸ� -> 0000 0011.
        // ������ �÷��̾��� ��Ʈ�� -> 0000 1000(8)

        // << (LEFT SHIFT ������ : ��Ʈ�� �������� nĭ �Űܶ�).
        // 1�� �������� 0000 0001, �÷��̾��� NameToLayer ���� 3.
        // ��� ���� 0000 1000(8)

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
