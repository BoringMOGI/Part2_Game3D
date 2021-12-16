using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : ObjectPool<DamageManager, DamageText>
{
    public void AppearDamage(Vector3 position, int amount, DAMAGE_TYPE type = DAMAGE_TYPE.Normal)
    {
        DamageText pool = GetPool();            // Ǯ���� ������Ʈ �ϳ��� �����´�.
        pool.Appear(position, amount, type);    // ������ �ؽ�Ʈ�� ����Ѵ�.
    }
}
