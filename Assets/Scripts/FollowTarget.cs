using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform target;      // ���� ���.
    [SerializeField] Vector3 offset;        // ������ �Ÿ�.

    [SerializeField] bool isLockX;
    [SerializeField] bool isLockY;
    [SerializeField] bool isLockZ;

    private void Start()
    {
        if (target == null)
        {
            enabled = false;
            Debug.LogWarning("[FollowTarget] is not set target.");
            return;
        }

        transform.position = target.position + offset;
    }

    private void LateUpdate()
    {

        Vector3 myPosition = transform.position;

        if(!isLockX)
            myPosition.x = target.position.x + offset.x;
        if(!isLockY)
            myPosition.y = target.position.y + offset.y;
        if(!isLockZ)
            myPosition.z = target.position.z + offset.z;

        transform.position = myPosition;
    }

}
