using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] Transform damagePivot;

    public void OnDamaged(float amount, DAMAGE_TYPE type = DAMAGE_TYPE.Normal)
    {
        // 데미지 공식..
        int finalDamage = Mathf.RoundToInt(amount);
        DamageManager.Instance.AppearDamage(damagePivot, finalDamage, type);
    }
}
