using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� ���� ��� �� ����/���̵� ó���� �����ϴ� �Ŵ��� Ŭ����
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
    /// ������ ���� Ʈ���� ���̵� ��/�ƿ� ȿ���� �Բ� ����ϴ� �Լ�
    /// </summary>
    /// <param name="musicTrack">����� ���� Ʈ�� ScriptableObject</param>
    /// <param name="fadeOutTime">���� Ʈ���� ���̵� �ƿ� �ð�</param>
    /// <param name="fadeInTime">�� Ʈ���� ���̵� �� �ð�</param>
    public void PlayMusic(MusicTrackSO musicTrack, float fadeOutTime = Settings.musicFadeOutTime, float fadeInTime = Settings.musicFadeInTime)
    {
        StartCoroutine(PlayMusicRoutines(musicTrack, fadeInTime, fadeOutTime));
    }

    /// <summary>
    /// ���� ��ü �� ���̵� �ƿ�/�� ������ Ʈ���� ��ȯ�ϴ� �ڷ�ƾ �Լ�
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
    /// ���� ��� ���� ������ ������ �ð���ŭ ���̵� �ƿ��ϴ� �Լ�
    /// </summary>
    /// <param name="fadeOutTime">���̵� �ƿ��� �ɸ��� �ð� (��)</param>
    private IEnumerator FadeOutMusic(float fadeOutTime)
    {
        GameResources.Instance.musicLowSnapshot.TransitionTo(fadeOutTime);

        yield return new WaitForSeconds(fadeOutTime);
    }

    /// <summary>
    /// ���ο� ���� Ʈ���� ������ �ð���ŭ ���̵� ���Ͽ� ����ϴ� �Լ�
    /// </summary>
    /// <param name="musicTrack">����� ���� Ʈ��</param>
    /// <param name="fadeInTime">���̵� �ο� �ɸ��� �ð� (��)</param>
    private IEnumerator FadeInMusic(MusicTrackSO musicTrack, float fadeInTime)
    {
        musicAudioSouce.clip = musicTrack.musicClip;
        musicAudioSouce.volume = musicTrack.musicVolume;
        musicAudioSouce.Play();

        GameResources.Instance.musicOnFullSnapshot.TransitionTo(fadeInTime);

        yield return new WaitForSeconds(fadeInTime);
    }

    /// <summary>
    /// ���� ������ 1 ������Ű�� �����ϴ� �Լ�
    /// </summary>
    public void IncreaseMusicVolume()
    {
        int maxMusicVolume = 20;

        if (musicVolume >= maxMusicVolume) return;

        musicVolume += 1;

        SetMusicVolume(musicVolume);
    }

    /// <summary>
    /// ���� ������ 1 ���ҽ�Ű�� �����ϴ� �Լ�
    /// </summary>
    public void DecreaseMusicVolume()
    {
        if (musicVolume == 0) return;

        musicVolume -= 1;

        SetMusicVolume(musicVolume);
    }

    /// <summary>
    /// AudioMixer�� ���� ������ dB ������ �����ϴ� �Լ�
    /// </summary>
    /// <param name="musicVolume">������ ���� ���� (0~20)</param>
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