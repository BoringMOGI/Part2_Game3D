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
    [SerializeField] LayerMask groundMask;      // 지면 레이어.
    [SerializeField] LayerMask searchMask;      // 탐색 레이어.
    [SerializeField] float patrolRange;
    [SerializeField] float searchRange;

    [Header("Speed")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;


    Vector3 originPosition;                 // 최초 위치.
    Transform targetPivot;                  // 탐지한 타겟의 위치.

    float nextPatrolTime = 0.0f;            // 다음 순찰 시간.

    private bool isPatrol = false;          // 순찰 중인가.
    private bool isChaseTarget = false;     // 타겟을 추적 중인가.


    private void Start()
    {
        originPosition = transform.position;
    }

    // 순찰.
    private void SetPartolDestination()
    {
        if (!isPatrol && !isChaseTarget && nextPatrolTime <= Time.time)
        {
            // 레이의 기준점 위치를 계산.
            Vector3 patrolPivot = originPosition + (Vector3.up * 2f);
            patrolPivot.x += Random.Range(-patrolRange, +patrolRange);
            patrolPivot.z += Random.Range(-patrolRange, +patrolRange);

            // 레이를 쏴서 바닥에 맞는 지점을 목적지로 설정한다.
            RaycastHit hit;
            if (Physics.Raycast(patrolPivot, Vector3.down, out hit, 10f, groundMask))
            {
                agent.SetDestination(hit.point);        // NavMesh에게 목적지 전달.
                isPatrol = true;                        // 정찰 시작.
            }
        }
    }
    private void Patrol()
    {
        if (!isPatrol)
            return;

        // 남은 거리가 0.0f 이하일때.
        if (agent.remainingDistance <= 0.001f)
        {
            // 목적지까지 도착했는가?
            isPatrol = false;
            nextPatrolTime = Time.time + 2f;
        }
        // 걷고 잇는 중.
        else
        {
            anim.SetBool("isWalk", true);
            agent.speed = walkSpeed;
        }
    }

    // 탐색
    private void SearchTarget()
    {
        // 감지 범위 크기의 구를 이용해 플레이어 감지.
        Collider[] hits = Physics.OverlapSphere(transform.position, searchRange, searchMask);
        if(hits.Length > 0)
        {
            isChaseTarget = true;                   // 추적 중이라는 것을 갱신.
            targetPivot = hits[0].transform;        // targetPivot에 대입.
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

        // 타겟을 추적중이지 않을때 순찰, 탐색
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

        // 게임이 실행 중일때.
        // 순찰 범위.
        if (Application.isPlaying)
            UnityEditor.Handles.DrawWireDisc(originPosition, Vector3.up, patrolRange);
        else
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, patrolRange);

        // 탐색 범위.
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, searchRange);
    }
}
