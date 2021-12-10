using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    const float GRAVITY = -9.81f;

    [Header("Ground")]
    [SerializeField] Transform groundChecker;       // 지면 체크를 위한 기준점.
    [SerializeField] float groundRadius;            // 지면 체크 반지름.
    [SerializeField] LayerMask groundMask;          // 자면 레이어 마스크.

    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpHeight;

    [Range(1.0f, 4.0f)]
    [SerializeField] float gravityScale;

    [Header("Physics")]
    [SerializeField] PhysicMaterial physicMaterial;


    CharacterController controller;         // 캐릭터 제어 컴포넌트.
    Vector3 velocity;                       // 중력 속도.    
    bool isGrounded;                        // 지면을 밟고있는지에 대한 여부.

    float gravity => GRAVITY * gravityScale; // 중력 가속도 * 중력 비율.

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        controller.material = physicMaterial;
    }

    private void Update()
    {
        Debug.Log(controller.isGrounded);

        CheckGround();          // ground 체크.

        Movement();             // 이동.
        Jump();                 // 점프.

        Gravity();              // 중력 값.
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundRadius, groundMask);
    }

    private void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");       // 키보드 좌,우 (좌측,우측)
        float z = Input.GetAxisRaw("Vertical");         // 키보드 상,하 (정면,후면)

        // transform.방향 => 내 기준 방향 (로컬 좌표)
        Vector3 direction = (transform.right * x) + (transform.forward * z);
        controller.Move(direction * moveSpeed * Time.deltaTime);
        
    }

    private void Gravity()
    {
        if (isGrounded && velocity.y < 0f)       // 땅을 밟았고 하강 속력이 있다면.
        {
            velocity.y = -2f;                   // 최소한의 값으로 속력 대입.
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
