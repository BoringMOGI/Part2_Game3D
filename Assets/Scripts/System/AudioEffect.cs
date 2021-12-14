using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ش� Ŭ������ �����Ϸ��� AudioSource�� �־���Ѵ�.
[RequireComponent(typeof(AudioSource))]
public class AudioEffect : MonoBehaviour, IPool<AudioEffect>
{
    AudioSource source;
    OnReturnPoolEvent<AudioEffect> OnReturnPool;

    public void Setup(OnReturnPoolEvent<AudioEffect> OnReturnPool)
    {
        this.OnReturnPool = OnReturnPool;
        source = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip, float volumn)
    {
        source.clip = clip;
        source.volume = volumn;
        source.Play();

        StartCoroutine(Playing());
    }

    IEnumerator Playing()
    {
        while (source.isPlaying)
            yield return null;

        // �ٽ� ����� �Ŵ������� �ǵ�����.
        // delegate�̺�Ʈ�� Ÿ�� �� �ڽ��� �����Ѵ�.
        OnReturnPool?.Invoke(this);
    }
}
