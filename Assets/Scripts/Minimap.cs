using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] Camera cam;

    [SerializeField] int firstSize;   // 최초 크기.
    [SerializeField] int minSize;     // 최소 크기.
    [SerializeField] int maxSize;     // 최대 크기.

    [SerializeField] int increase;    // 증감 값.

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
