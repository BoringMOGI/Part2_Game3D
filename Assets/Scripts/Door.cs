using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteraction
{
    [SerializeField] Animation anim;

    bool isOpened;

    public string GetName()
    {
        return isOpened ? "�� �ݱ�" : "�� ����";
    }

    public void OnInteraction()
    {
        if (anim.isPlaying)
            return;

        if (isOpened)
            anim.Play("Door_Close");
        else
            anim.Play("Door_Open");

        isOpened = !isOpened;
    }
    
}
