using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : ObjectPool<AudioManager, AudioEffect>
{
    public enum AUDIO
    {
        Master,
        Bgm,
        Effect,
    }

    public float masterVolumn { get; private set; }
    public float bgmVolumn    { get; private set; }
    public float effectVolumn { get; private set; }

    const string KEY_MASTER = "MasterVolumn";
    const string KEY_BGM    = "BgmVolumn";
    const string KEY_EFFECT = "EffectVolumn";

    [SerializeField] AudioClip[] bgms;
    [SerializeField] AudioClip[] effects;

    [Header("Source")]
    [SerializeField] AudioSource bgmSource;

    // ����ϰ� �ִ� �θ� Awake �Լ��� �ߺ��̱� ������
    // newŰ���带 �ٿ��� �ٸ� �Լ���� ���� �˸�.
    protected new void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        // ������ �ε�(Load)
        masterVolumn = PlayerPrefs.HasKey(KEY_MASTER) ? PlayerPrefs.GetFloat(KEY_MASTER) : 1f;
        bgmVolumn = PlayerPrefs.HasKey(KEY_BGM)       ? PlayerPrefs.GetFloat(KEY_BGM) : 0.7f;
        effectVolumn = PlayerPrefs.HasKey(KEY_EFFECT) ? PlayerPrefs.GetFloat(KEY_EFFECT) : 0.4f;

        bgmSource.volume = bgmVolumn * masterVolumn;        // ���� ����� ũ�� ����.               
    }

    public void OnChangedVolumn(AUDIO type, float value)
    {
        switch(type)
        {
            case AUDIO.Master:
                masterVolumn = value;                
                PlayerPrefs.SetFloat(KEY_MASTER, masterVolumn);
                break;
            case AUDIO.Bgm:
                bgmVolumn = value;
                PlayerPrefs.SetFloat(KEY_BGM, bgmVolumn);
                break;
            case AUDIO.Effect:
                effectVolumn = value;
                PlayerPrefs.SetFloat(KEY_EFFECT, effectVolumn);
                break;
        }

        bgmSource.volume    = bgmVolumn * masterVolumn;
    }

    private AudioClip GetClip(AUDIO type, string str)
    {
        AudioClip[] clips = (type == AUDIO.Bgm) ? bgms : effects;       // Ŭ�� �迭 ����.
        foreach(AudioClip clip in clips)
        {
            if (clip.name.Equals(str))                                  // Ŭ���� �̸��� ��û �̸��� ���ٸ�
                return clip;                                            // �ش� Ŭ�� ��ȯ.
        }

        return null;
    }
    public void PlayBgm(string str)
    {
        bgmSource.clip = GetClip(AUDIO.Bgm, str);
        bgmSource.Play();
    }
    public void PauseBGM()
    {
        bgmSource.Pause();
    }
    public void UnPuaseBGM()
    {
        bgmSource.UnPause();
    }

    public void PlayEffect(string str)
    {
        AudioEffect effect = GetPool();
        effect.Play(GetClip(AUDIO.Effect, str), effectVolumn * masterVolumn);
    }
}
