using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    const float GRAVITY = -9.81f;

    [Header("Ground")]
    [SerializeField] Transform groundChecker;       // ���� üũ�� ���� ������.
    [SerializeField] float groundRadius;            // ���� üũ ������.
    [SerializeField] LayerMask groundMask;          // �ڸ� ���̾� ����ũ.

    [Header("Movement")]
    [SerializeField] Animator anim;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpHeight;

    [Range(1.0f, 4.0f)]
    [SerializeField] float gravityScale;

    [Header("Physics")]
    [SerializeField] PhysicMaterial physicMaterial;


    CharacterController controller;         // ĳ���� ���� ������Ʈ.


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
        controller.material = physicMaterial;
    }

    private void Update()
    {
        CheckGround();          // ground üũ.

        Movement();             // �̵�.
        Jump();                 // ����.

        Gravity();              // �߷� ��.
    }

    private void CheckGround()
    {
        // �׶��尡 ���̿� �浹 & ���� �ϰ� �ӵ��� 0���� �۰ų� ���� ���.
        bool isCheckGround = Physics.CheckSphere(groundChecker.position, groundRadius, groundMask);
        isGround = isCheckGround && velocityY <= 0f;
    }

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








    private void OnDrawGizmos()
    {
        if (groundChecker != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(groundChecker.position, groundRadius);
        }
    }
}
