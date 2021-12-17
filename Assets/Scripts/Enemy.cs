using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Damageable damageable;
    [SerializeField] new Collider collider;

    [Header("Variable")]
    [SerializeField] LayerMask groundMask;      // ���� ���̾�.
    [SerializeField] LayerMask searchMask;      // Ž�� ���̾�.
    [SerializeField] float patrolRange;
    [SerializeField] float searchRange;

    [Header("Speed")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;


    Vector3 originPosition;                 // ���� ��ġ.
    Transform targetPivot;                  // Ž���� Ÿ���� ��ġ.

    float nextPatrolTime = 0.0f;            // ���� ���� �ð�.

    private bool isPatrol = false;          // ���� ���ΰ�.
    private bool isChaseTarget = false;     // Ÿ���� ���� ���ΰ�.


    private void Start()
    {
        originPosition = transform.position;
    }

    // ����.
    private void SetPartolDestination()
    {
        if (!isPatrol && !isChaseTarget && nextPatrolTime <= Time.time)
        {
            // ������ ������ ��ġ�� ���.
            Vector3 patrolPivot = originPosition + (Vector3.up * 2f);
            patrolPivot.x += Random.Range(-patrolRange, +patrolRange);
            patrolPivot.z += Random.Range(-patrolRange, +patrolRange);

            // ���̸� ���� �ٴڿ� �´� ������ �������� �����Ѵ�.
            RaycastHit hit;
            if (Physics.Raycast(patrolPivot, Vector3.down, out hit, 10f, groundMask))
            {
                agent.SetDestination(hit.point);        // NavMesh���� ������ ����.
                isPatrol = true;                        // ���� ����.
            }
        }
    }
    private void Patrol()
    {
        if (!isPatrol)
            return;

        // ���� �Ÿ��� 0.0f �����϶�.
        if (agent.remainingDistance <= 0.001f)
        {
            // ���������� �����ߴ°�?
            isPatrol = false;
            nextPatrolTime = Time.time + 2f;
        }
        // �Ȱ� �մ� ��.
        else
        {
            anim.SetBool("isWalk", true);
            agent.speed = walkSpeed;
        }
    }

    // Ž��
    private void SearchTarget()
    {
        // ���� ���� ũ���� ���� �̿��� �÷��̾� ����.
        Collider[] hits = Physics.OverlapSphere(transform.position, searchRange, searchMask);
        if(hits.Length > 0)
        {
            isChaseTarget = true;                   // ���� ���̶�� ���� ����.
            targetPivot = hits[0].transform;        // targetPivot�� ����.
        }
    }
    private void ChasingTarget()
    {
        agent.SetDestination(targetPivot.position);
    }

    private void Update()
    {
        anim.SetBool("isRun", false);
        anim.SetBool("isWalk", false);

        // Ÿ���� ���������� ������ ����, Ž��
        if (!isChaseTarget)
        {
            SearchTarget();
            SetPartolDestination();            
            Patrol();
        }
        else
        {
            ChasingTarget();
            anim.SetBool("isRun", true);
            agent.speed = runSpeed;
        }

        anim.SetBool("isAlive", damageable.hp > 0.0f);
    }

    public void OnDamaged()
    {
        anim.SetTrigger("onHit");
    }
    public void OnDead()
    {
        //collider.isTrigger = true;
        collider.enabled = false;

        Invoke("DestroyEnemy", 3.0f);
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = Color.green;

        // ������ ���� ���϶�.
        // ���� ����.
        if (Application.isPlaying)
            UnityEditor.Handles.DrawWireDisc(originPosition, Vector3.up, patrolRange);
        else
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, patrolRange);

        // Ž�� ����.
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, searchRange);
    }
}
