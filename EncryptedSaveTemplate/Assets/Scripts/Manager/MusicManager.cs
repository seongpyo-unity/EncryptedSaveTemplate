using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 배경 음악 재생 및 볼륨/페이드 처리를 관리하는 매니저 클래스
/// </summary>
public class MusicManager : SingletonMonobehaviour<MusicManager>
{
    private AudioSource musicAudioSouce = null;
    private AudioClip currentAudioClip = null;
    private Coroutine fadeOutMusicCoroutine;
    private Coroutine fadeInMusicCoroutine;
    public int musicVolume = 10;

    protected override void Awake()
    {
        base.Awake();

        musicAudioSouce = GetComponent<AudioSource>();

        GameResources.Instance.musicOffSnapshot.TransitionTo(0f);
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(Settings.musicVolumeHashKey))
        {
            musicVolume = PlayerPrefs.GetInt(Settings.musicVolumeHashKey);
        }

        SetMusicVolume(musicVolume);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt(Settings.musicVolumeHashKey, musicVolume);
    }

    /// <summary>
    /// 지정된 음악 트랙을 페이드 인/아웃 효과와 함께 재생하는 함수
    /// </summary>
    /// <param name="musicTrack">재생할 음악 트랙 ScriptableObject</param>
    /// <param name="fadeOutTime">이전 트랙의 페이드 아웃 시간</param>
    /// <param name="fadeInTime">새 트랙의 페이드 인 시간</param>
    public void PlayMusic(MusicTrackSO musicTrack, float fadeOutTime = Settings.musicFadeOutTime, float fadeInTime = Settings.musicFadeInTime)
    {
        StartCoroutine(PlayMusicRoutines(musicTrack, fadeInTime, fadeOutTime));
    }

    /// <summary>
    /// 음악 교체 시 페이드 아웃/인 순서로 트랙을 전환하는 코루틴 함수
    /// </summary>
    private IEnumerator PlayMusicRoutines(MusicTrackSO musicTrack, float fadeOutTime, float fadeInTime)
    {
        if (fadeOutMusicCoroutine != null)
        {
            StopCoroutine(fadeOutMusicCoroutine);
        }

        if (fadeInMusicCoroutine != null)
        {
            StopCoroutine(fadeInMusicCoroutine);
        }

        if (musicTrack.musicClip != currentAudioClip)
        {
            currentAudioClip = musicTrack.musicClip;

            yield return fadeOutMusicCoroutine = StartCoroutine(FadeOutMusic(fadeOutTime));

            yield return fadeInMusicCoroutine = StartCoroutine(FadeInMusic(musicTrack, fadeInTime));
        }

        yield return null;
    }

    /// <summary>
    /// 현재 재생 중인 음악을 지정된 시간만큼 페이드 아웃하는 함수
    /// </summary>
    /// <param name="fadeOutTime">페이드 아웃에 걸리는 시간 (초)</param>
    private IEnumerator FadeOutMusic(float fadeOutTime)
    {
        GameResources.Instance.musicLowSnapshot.TransitionTo(fadeOutTime);

        yield return new WaitForSeconds(fadeOutTime);
    }

    /// <summary>
    /// 새로운 음악 트랙을 지정된 시간만큼 페이드 인하여 재생하는 함수
    /// </summary>
    /// <param name="musicTrack">재생할 음악 트랙</param>
    /// <param name="fadeInTime">페이드 인에 걸리는 시간 (초)</param>
    private IEnumerator FadeInMusic(MusicTrackSO musicTrack, float fadeInTime)
    {
        musicAudioSouce.clip = musicTrack.musicClip;
        musicAudioSouce.volume = musicTrack.musicVolume;
        musicAudioSouce.Play();

        GameResources.Instance.musicOnFullSnapshot.TransitionTo(fadeInTime);

        yield return new WaitForSeconds(fadeInTime);
    }

    /// <summary>
    /// 음악 볼륨을 1 증가시키고 적용하는 함수
    /// </summary>
    public void IncreaseMusicVolume()
    {
        int maxMusicVolume = 20;

        if (musicVolume >= maxMusicVolume) return;

        musicVolume += 1;

        SetMusicVolume(musicVolume);
    }

    /// <summary>
    /// 음악 볼륨을 1 감소시키고 적용하는 함수
    /// </summary>
    public void DecreaseMusicVolume()
    {
        if (musicVolume == 0) return;

        musicVolume -= 1;

        SetMusicVolume(musicVolume);
    }

    /// <summary>
    /// AudioMixer에 음악 볼륨을 dB 단위로 적용하는 함수
    /// </summary>
    /// <param name="musicVolume">적용할 음악 볼륨 (0~20)</param>
    public void SetMusicVolume(int musicVolume)
    {
        float muteDecibels = -80f;

        if (musicVolume == 0)
        {
            GameResources.Instance.musicMasterMixerGroup.audioMixer.SetFloat(Settings.musicVolumeHashKey, muteDecibels);
        }
        else
        {
            GameResources.Instance.musicMasterMixerGroup.audioMixer.SetFloat(Settings.musicVolumeHashKey, HelperUtilities.LinearToDecibels(musicVolume));
        }
    }

}