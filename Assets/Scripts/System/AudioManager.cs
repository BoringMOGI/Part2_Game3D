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

    // 상속하고 있는 부모 Awake 함수와 중복이기 때문에
    // new키워드를 붙여서 다른 함수라는 것을 알림.
    protected new void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        // 데이터 로드(Load)
        masterVolumn = PlayerPrefs.HasKey(KEY_MASTER) ? PlayerPrefs.GetFloat(KEY_MASTER) : 1f;
        bgmVolumn = PlayerPrefs.HasKey(KEY_BGM)       ? PlayerPrefs.GetFloat(KEY_BGM) : 0.7f;
        effectVolumn = PlayerPrefs.HasKey(KEY_EFFECT) ? PlayerPrefs.GetFloat(KEY_EFFECT) : 0.4f;

        bgmSource.volume = bgmVolumn * masterVolumn;        // 최초 배경음 크기 설정.               
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
        AudioClip[] clips = (type == AUDIO.Bgm) ? bgms : effects;       // 클립 배열 선택.
        foreach(AudioClip clip in clips)
        {
            if (clip.name.Equals(str))                                  // 클립의 이름이 요청 이름과 같다면
                return clip;                                            // 해당 클립 반환.
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
