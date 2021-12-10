using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] Transform playerBody;      // 플레이어의 몸체.

    [Range(0.5f, 4.0f)]
    [SerializeField] float sensitivityX;        // 수평 감도.

    [Range(0.5f, 4.0f)]
    [SerializeField] float sensitivityY;        // 수직 감도.


    [SerializeField] float minX;
    [SerializeField] float maxX;



    float xRotation = 0f;                       // x축 회전 값.

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX;  // 프레임 단위 마우스의 x축 움직임.
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;  // 프레임 단위 마우스의 y축 움직임.

        OnMouseLook(new Vector2(mouseX, mouseY));
    }

    private void OnMouseLook(Vector2 axis)
    {
        // 수평 회전.
        playerBody.Rotate(Vector3.up * axis.x);

        // 수진 회전.
        xRotation = Mathf.Clamp(xRotation - axis.y, minX, maxX);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
