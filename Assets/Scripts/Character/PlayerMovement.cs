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

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    const float GRAVITY = -9.81f;

    [Header("Attack")]
    [SerializeField] Animator anim;
    [SerializeField] Attackable attackable;
    [SerializeField] float comboDelay;

    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpHeight;

    [Range(1.0f, 4.0f)]
    [SerializeField] float gravityScale;


    CharacterController controller;         // ĳ���� ���� ������Ʈ.

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


    // �ִϸ����� �Ķ���͸� �̿��Ѵ�.
    float velocityY
    {
        get
        {
            return anim.GetFloat("velocityY");
        }
        set
        {
            anim.SetFloat("velocityY", value);
        }
    }
    bool isGround
    {
        get
        {
            return anim.GetBool("isGround");
        }
        set
        {
            anim.SetBool("isGround", value);
        }
    }
    float inputX
    {
        get
        {
            return anim.GetFloat("inputX");
        }
        set
        {
            anim.SetFloat("inputX", value);
        }
    }
    float inputY
    {
        get
        {
            return anim.GetFloat("inputY");
        }
        set
        {
            anim.SetFloat("inputY", value);
        }
    }

    float gravity => GRAVITY * gravityScale; // �߷� ���ӵ� * �߷� ����.

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        StartCoroutine(ComboReset());
    }
    private void Update()
    {
        //CheckGround();          // ground üũ.
        isGround = controller.isGrounded;

        if (!isDead && !Chatting.IsChatting)
        {
            Movement();             // �̵�.
            Jump();                 // ����.

            // GetMouseButtonDown(0:����, 1:������, 2:��)
            if (Input.GetMouseButtonDown(0) && !isAttack && combo < 3)
            {
                Attack();
            }
        }

        Gravity();              // �߷� ��.
    }

    // ����.
    void Attack()
    {
        if (InventoryUI.Instance.isOpenInven)
            return;

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

    // �̵�.
    private void Movement()
    {
        // Input.GetAxisRaw = -1, 0  1.
        // Input.GetAxis = -1.0f ~ 1.0f.
        inputX = Input.GetAxis("Horizontal");       // Ű���� ��,�� (����,����)
        inputY = Input.GetAxis("Vertical");         // Ű���� ��,�� (����,�ĸ�)

        // transform.���� => �� ���� ���� (���� ��ǥ)
        Vector3 direction = (transform.right * inputX) + (transform.forward * inputY);
        controller.Move(direction * moveSpeed * Time.deltaTime);
    }
    private void Gravity()
    {
        if (isGround && velocityY < 0f)          // ���� ��Ұ� �ϰ� �ӷ��� �ִٸ�.
        {
            velocityY = -2f;                       // �ּ����� ������ �ӷ� ����.
        }

        velocityY += gravity * Time.deltaTime;
        controller.Move(new Vector3(0f, velocityY, 0f) * Time.deltaTime);

        anim.SetFloat("velocityY", velocityY);     // �ִϸ������� �ĸ����͸� ����.
    }
    private void Jump()
    {
        if (isGround && Input.GetButtonDown("Jump"))
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
            anim.SetTrigger("onJump");
        }
    }
}
