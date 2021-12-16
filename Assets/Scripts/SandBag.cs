using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandBag : MonoBehaviour
{
    public void OnDamageEffect()
    {
        StartCoroutine(DamageEffect());
    }

    IEnumerator DamageEffect()
    {
        Material mainMat = GetComponent<MeshRenderer>().material;       // MeshRenderer�� ���� ��Ƽ����.

        Color originColor = mainMat.color;                              // ���� �������ִ� ���� ��´�.
        mainMat.color = Color.red;                                      // ������ �������� ����.

        yield return new WaitForSeconds(0.05f);                          // 0.2�� ��ٸ�.

        mainMat.color = originColor;                                    // ������ ������ ����.
    }
}
