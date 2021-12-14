using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static DamageManager;

[RequireComponent(typeof(Animation))]
public class DamageText : MonoBehaviour, IPool<DamageText>
{
    static Camera mainCam;

    const string KEY_NORMAL = "DamageText_Appear";
    const string KEY_CRI = "DamageText_Appear_Cri";

    [SerializeField] Text damageText;
    
    Animation anim;


    // position:���� ��ġ, amount:������ ��ġ.
    public void Appear(Transform pivot, int amount, DAMAGE_TYPE type)
    {
        // amount ��ġ�� string���� ���.
        damageText.text = amount.ToString();

        switch(type)
        {
            case DAMAGE_TYPE.Normal:
                anim.Play(KEY_NORMAL);
                break;

            case DAMAGE_TYPE.Critical:
                anim.Play(KEY_CRI);
                break;
        }

        StartCoroutine(FixPosition(pivot));
    }

    IEnumerator FixPosition(Transform pivot)
    {
        while (anim.isPlaying)
        {
            // WorldToScreenPoint : ���� ��ǥ�� ��ũ�� ��ǥ�� ��ȯ.
            Vector2 damagePosition = mainCam.WorldToScreenPoint(pivot.position);
            transform.position = damagePosition;

            yield return null;
        }

        OnReturnPool(this);     // pool �Ŵ����� �ǵ��ư���.
    }

    OnReturnPoolEvent<DamageText> OnReturnPool;
    public void Setup(OnReturnPoolEvent<DamageText> OnReturnPool)
    {
        this.OnReturnPool = OnReturnPool;

        mainCam = Camera.main;
        anim = GetComponent<Animation>();
    }
}
