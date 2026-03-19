using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips (Inspector 연결)")]
    public AudioClip interactionSFX;

    public AudioClip mainBGMClip;    // 배경 음악
    public AudioClip failSFXClip;    // 실패 효과음
    public AudioClip successSFXClip; // 성공 효과음

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 배경음악 재생
    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource != null && clip != null)
        {
            bgmSource.clip = clip;
            bgmSource.loop = true; // BGM은 반복 재생
            bgmSource.Play();
        }
    }

    // 효과음 1회 재생
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // BGM 정지
    public void StopBGM()
    {
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }
    }
}