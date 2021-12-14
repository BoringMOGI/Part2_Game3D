using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : ObjectPool<DamageManager, DamageText>
{
    public void AppearDamage(Transform pivot, int amount, DAMAGE_TYPE type = DAMAGE_TYPE.Normal)
    {
        DamageText pool = GetPool();            // Ǯ���� ������Ʈ �ϳ��� �����´�.
        pool.Appear(pivot, amount, type);       // ������ �ؽ�Ʈ�� ����Ѵ�.
    }
}
