using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [SerializeField] GameObject selected;
    [SerializeField] bool isFirst;

    // ������Ʈ Ȱ��ȭ �� �Ź� ȣ��.
    private void OnEnable()
    {
        selected.SetActive(isFirst);
    }


    public void OnPointEnter()
    {
        AudioManager.Instance.OnChangedVolumn(AudioManager.AUDIO.Effect, 0.2f);
        AudioManager.Instance.PlayEffect("Tick");
    }

}
