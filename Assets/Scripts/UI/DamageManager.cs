using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : ObjectPool<DamageManager, DamageText>
{
    public void AppearDamage(Transform pivot, int amount, DAMAGE_TYPE type = DAMAGE_TYPE.Normal)
    {
        DamageText pool = GetPool();            // 풀링한 오브젝트 하나를 가져온다.
        pool.Appear(pivot, amount, type);       // 데미지 텍스트를 출력한다.
    }
}
