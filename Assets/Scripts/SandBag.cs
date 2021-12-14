using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandBag : MonoBehaviour, IDamageable
{
    void IDamageable.OnDamaged(float amount, DAMAGE_TYPE type)
    {
        // RoundToInt : �ݿø�.
        // CeilToInt : �ø�.
        // FloorToInt : ����.        
        StartCoroutine(DamageEffect());

        // ������ �Ŵ������� ��û.
        DamageManager.Instance.AppearDamage(transform, Mathf.RoundToInt(amount), type);
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
