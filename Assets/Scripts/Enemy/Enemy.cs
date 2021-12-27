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
    [SerializeField] Attackable attackable;     // ���� Ŭ����.
    [SerializeField] float attackCycle;         // ���� �ֱ�.

    [Header("DropItem")]
    [SerializeField] Item[] itemTable;


    float nextAttackCycle;              // ���� ���� �ֱ�.

    Transform player;                   // �÷��̾� Ʈ������.
    Vector3 originPosition;             // ���� ���� ��ġ.

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
    }             // Ž�� ������ ���Դ���?
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
    }             // ���� ������ ���Դ���?
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
    }                      // �׾��°�?

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

        // Ž�� ���� X, ���� ���� X
        if (!isInSearchRange && !isInAttackRange)
        {
            Patroling();
        }
        // Ž�� ���� O, ���� ���� X
        else if (isInSearchRange && !isInAttackRange)
        {
            ChaseToPlayer();
        }
        // Ž�� ���� O, ���� ���� O
        else if (isInSearchRange && isInAttackRange)
        {
            AttackToPlayer();
        }
    }

    Vector3 patrolPoint;    // ���� ����.
    bool isPatrolPoint;     // ���� ������ �����Ǿ��°�?
    bool isUnderAttack;     // ������ �޴� ���ΰ�?

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

        // ���� �̵� �ӵ��� �ȴ� �ӵ��� ����.
        agent.speed = walkSpeed;

        // ���� ���� ������ �Ÿ��� ����� �۾����ٸ� ������ �����Ѵ�.
        float distanceToPatrolPoint = Vector3.Distance(transform.position, patrolPoint);
        if (distanceToPatrolPoint <= 0.5f)
            isPatrolPoint = false;
    }
    void SetPatrolPoint()
    {
        float randomX = originPosition.x + Random.Range(-patrolRange, patrolRange);
        float randomZ = originPosition.z + Random.Range(-patrolRange, patrolRange);

        // Ư�� ��ġ�� �������� Ư�� �������� �̵��Ϸ��� ���̸� ����.
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
        agent.SetDestination(player.position);      // �������� �÷��̾��� ��ġ�� ����.
        agent.speed = runSpeed;                     // ���� �̵� �ӵ��� �޸��� �ӵ��� ����.
    }
    void AttackToPlayer()
    {
        agent.SetDestination(transform.position);   // �������� ���� ��ġ�� ��´�.
        transform.LookAt(player.position);          // LookAt : Ư�� ��ġ�� �ٶ󺻴�. (ȸ��)

        if (isUnderAttack)
            return;

        // attack..
        if(nextAttackCycle <= Time.time)
        {
            nextAttackCycle = Time.time + attackCycle;      // ���� ���� �ð� ����.
            anim.SetTrigger("onAttack");                    // �ִϸ����Ϳ� Ʈ���� �ߵ�. 
        }
    }

    public void OnDamaged()
    {
        anim.SetTrigger("onHit");
        isUnderAttack = true;

        CancelInvoke(nameof(ResetUnderAttack));     // ������ �ɾ�� Invoke ���.
        Invoke(nameof(ResetUnderAttack), 1.5f);     // 1.5�� �Ŀ� �Լ� ȣ��.
    }
    public void OnDead()
    {
        //collider.isTrigger = true;
        collider.enabled = false;
        isDead = true;

        // ������ ���.
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

        // ������ ���� ���϶�.
        // Ž�� ����.
        if (Application.isPlaying)
            UnityEditor.Handles.DrawWireDisc(originPosition, Vector3.up, patrolRange);
        else
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, patrolRange);

        // ���� ��ġ.
        if (isPatrolPoint)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(patrolPoint, 0.3f);
        }


        // Ž�� ����.
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, searchRange);

       
        // ���� ���� �׸���.
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, attackRange);
    }
}
