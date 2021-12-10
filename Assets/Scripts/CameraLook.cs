using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] Transform playerBody;      // �÷��̾��� ��ü.

    [Range(0.5f, 4.0f)]
    [SerializeField] float sensitivityX;        // ���� ����.

    [Range(0.5f, 4.0f)]
    [SerializeField] float sensitivityY;        // ���� ����.


    [SerializeField] float minX;
    [SerializeField] float maxX;



    float xRotation = 0f;                       // x�� ȸ�� ��.

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX;  // ������ ���� ���콺�� x�� ������.
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;  // ������ ���� ���콺�� y�� ������.

        OnMouseLook(new Vector2(mouseX, mouseY));
    }

    private void OnMouseLook(Vector2 axis)
    {
        // ���� ȸ��.
        playerBody.Rotate(Vector3.up * axis.x);

        // ���� ȸ��.
        xRotation = Mathf.Clamp(xRotation - axis.y, minX, maxX);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
