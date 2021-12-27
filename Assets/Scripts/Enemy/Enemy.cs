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

    [Header("Speed")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;

    [Header("Variable")]
    [SerializeField] LayerMask playerMask;
    [SerializeField] float patrolRange;
    [SerializeField] float searchRange;
    [SerializeField] float attackRange;

    [Header("Attack")]
    [SerializeField] Attackable attackable;     // 공격 클래스.
    [SerializeField] float attackCycle;         // 공격 주기.

    [Header("DropItem")]
    [SerializeField] Item[] itemTable;


    float nextAttackCycle;              // 다음 공격 주기.

    Transform player;                   // 플레이어 트랜스폼.
    Vector3 originPosition;             // 적의 원점 위치.

    bool isInSearchRange
    {
        get
        {
            return anim.GetBool("isInSearchRange");
        }
        set
        {
            anim.SetBool("isInSearchRange", value);
        }
    }             // 탐지 범위에 들어왔는지?
    bool isInAttackRange
    {
        get
        {
            return anim.GetBool("isInAttackRange");
        }
        set
        {
            anim.SetBool("isInAttackRange", value);
        }
    }             // 공격 범위에 들어왔는지?
    bool isDead
    {
        get
        {
            return anim.GetBool("isDead");
        }
        set
        {
            anim.SetBool("isDead", value);
        }
    }                      // 죽었는가?

    private void Start()
    {
        player = PlayerController.Instance.transform;
        originPosition = transform.position;
    }
    private void Update()
    {
        if (isDead)
            return;

        isInSearchRange = Physics.CheckSphere(transform.position, searchRange, playerMask);
        isInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        // 탐지 범위 X, 공격 범위 X
        if (!isInSearchRange && !isInAttackRange)
        {
            Patroling();
        }
        // 탐지 범위 O, 공격 범위 X
        else if (isInSearchRange && !isInAttackRange)
        {
            ChaseToPlayer();
        }
        // 탐지 범위 O, 공격 범위 O
        else if (isInSearchRange && isInAttackRange)
        {
            AttackToPlayer();
        }
    }

    Vector3 patrolPoint;    // 정찰 지점.
    bool isPatrolPoint;     // 정찰 지점이 설정되었는가?
    bool isUnderAttack;     // 공격을 받는 중인가?

    void Patroling()
    {
        if(!isPatrolPoint)
        {
            SetPatrolPoint();
        }
        else
        {
            agent.SetDestination(patrolPoint);
        }

        // 적의 이동 속도를 걷는 속도로 변경.
        agent.speed = walkSpeed;

        // 나와 정찰 지점의 거리가 충분히 작아졌다면 정찰을 종료한다.
        float distanceToPatrolPoint = Vector3.Distance(transform.position, patrolPoint);
        if (distanceToPatrolPoint <= 0.5f)
            isPatrolPoint = false;
    }
    void SetPatrolPoint()
    {
        float randomX = originPosition.x + Random.Range(-patrolRange, patrolRange);
        float randomZ = originPosition.z + Random.Range(-patrolRange, patrolRange);

        // 특정 위치를 시작으로 특정 방향으로 이동하려는 레이를 생성.
        Ray ray = new Ray(new Vector3(randomX, originPosition.y + 2f, randomZ), Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            patrolPoint = hit.point;
            isPatrolPoint = true;
        }
    }

    void ChaseToPlayer()
    {
        agent.SetDestination(player.position);      // 목적지를 플레이어의 위치로 설정.
        agent.speed = runSpeed;                     // 적의 이동 속도를 달리는 속도로 변경.
    }
    void AttackToPlayer()
    {
        agent.SetDestination(transform.position);   // 목적지를 나의 위치로 잡는다.
        transform.LookAt(player.position);          // LookAt : 특정 위치를 바라본다. (회전)

        if (isUnderAttack)
            return;

        // attack..
        if(nextAttackCycle <= Time.time)
        {
            nextAttackCycle = Time.time + attackCycle;      // 다음 공격 시간 갱신.
            anim.SetTrigger("onAttack");                    // 애니메이터에 트리거 발동. 
        }
    }

    public void OnDamaged()
    {
        anim.SetTrigger("onHit");
        isUnderAttack = true;

        CancelInvoke(nameof(ResetUnderAttack));     // 이전에 걸어둔 Invoke 취소.
        Invoke(nameof(ResetUnderAttack), 1.5f);     // 1.5초 후에 함수 호출.
    }
    public void OnDead()
    {
        //collider.isTrigger = true;
        collider.enabled = false;
        isDead = true;

        // 아이템 드랍.
        DropItem();

        Invoke("DestroyEnemy", 3.0f);
    }
    private void DropItem()
    {
        if (itemTable.Length <= 0)
            return;

        Item pickItem = itemTable[Random.Range(0, itemTable.Length)];
        Item dropItem = new Item(pickItem);

        ItemObject itemObject = ItemManager.Instance.GetPool();
        itemObject.Push(dropItem);

        itemObject.Show(transform.position);
    }


    void ResetUnderAttack()
    {
        isUnderAttack = false;
    }
    void DestroyEnemy()
    {
        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = Color.green;

        // 게임이 실행 중일때.
        // 탐색 범위.
        if (Application.isPlaying)
            UnityEditor.Handles.DrawWireDisc(originPosition, Vector3.up, patrolRange);
        else
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, patrolRange);

        // 정찰 위치.
        if (isPatrolPoint)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(patrolPoint, 0.3f);
        }


        // 탐색 범위.
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, searchRange);

       
        // 공격 범위 그리기.
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, attackRange);
    }
}
