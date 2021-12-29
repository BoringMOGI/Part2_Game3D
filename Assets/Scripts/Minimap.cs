using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] Camera cam;

    [SerializeField] int firstSize;   // ���� ũ��.
    [SerializeField] int minSize;     // �ּ� ũ��.
    [SerializeField] int maxSize;     // �ִ� ũ��.

    [SerializeField] int increase;    // ���� ��.

    private void Start()
    {
        cam.orthographicSize = Mathf.Clamp(firstSize, minSize, maxSize);
    }

    public void OnFar()
    {
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + increase, minSize, maxSize);
    }
    public void OnNear()
    {
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - increase, minSize, maxSize);
    }

}
