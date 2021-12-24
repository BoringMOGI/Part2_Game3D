using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandBag : MonoBehaviour, IInteraction
{
    public string GetName()
    {
        return "샌드백";
    }

    public void OnInteraction()
    {
        Debug.Log("샌드백의 상호작용");
    }

    public void OnDamageEffect()
    {
        StartCoroutine(DamageEffect());
    }
        
    IEnumerator DamageEffect()
    {
        Material mainMat = GetComponent<MeshRenderer>().material;       // MeshRenderer의 메인 머티리얼.

        Color originColor = mainMat.color;                              // 원래 가지고있던 색을 담는다.
        mainMat.color = Color.red;                                      // 색상을 빨강으로 변경.

        yield return new WaitForSeconds(0.05f);                          // 0.2초 기다림.

        mainMat.color = originColor;                                    // 원래의 색으로 변경.
    }
}
