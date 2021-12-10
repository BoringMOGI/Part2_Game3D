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
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpHeight;

    [Range(1.0f, 4.0f)]
    [SerializeField] float gravityScale;

    [Header("Physics")]
    [SerializeField] PhysicMaterial physicMaterial;


    CharacterController controller;         // ĳ���� ���� ������Ʈ.
    Vector3 velocity;                       // �߷� �ӵ�.    
    bool isGrounded;                        // ������ ����ִ����� ���� ����.

    float gravity => GRAVITY * gravityScale; // �߷� ���ӵ� * �߷� ����.

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        controller.material = physicMaterial;
    }

    private void Update()
    {
        Debug.Log(controller.isGrounded);

        CheckGround();          // ground üũ.

        Movement();             // �̵�.
        Jump();                 // ����.

        Gravity();              // �߷� ��.
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundRadius, groundMask);
    }

    private void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");       // Ű���� ��,�� (����,����)
        float z = Input.GetAxisRaw("Vertical");         // Ű���� ��,�� (����,�ĸ�)

        // transform.���� => �� ���� ���� (���� ��ǥ)
        Vector3 direction = (transform.right * x) + (transform.forward * z);
        controller.Move(direction * moveSpeed * Time.deltaTime);
        
    }

    private void Gravity()
    {
        if (isGrounded && velocity.y < 0f)       // ���� ��Ұ� �ϰ� �ӷ��� �ִٸ�.
        {
            velocity.y = -2f;                   // �ּ����� ������ �ӷ� ����.
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
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
