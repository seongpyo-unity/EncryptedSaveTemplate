using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사운드 이펙트 재생 및 볼륨 설정을 담당하는 매니저 클래스
/// </summary>
[DisallowMultipleComponent]
public class SoundEffectManager : SingletonMonobehaviour<SoundEffectManager>
{
    public int soundsVolume = 8;

    private void Start()
    {
        if (PlayerPrefs.HasKey(Settings.soundsVolumeHashKey))
        {
            soundsVolume = PlayerPrefs.GetInt(Settings.soundsVolumeHashKey);
        }

        SetSoundsVolume(soundsVolume);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt(Settings.soundsVolumeHashKey, soundsVolume);
    }

    /// <summary>
    /// 지정된 사운드 이펙트를 오브젝트 풀을 이용해 재생하는 함수
    /// </summary>
    /// <param name="soundEffect">재생할 사운드 이펙트 SO</param>
    public void PlaySoundEffect(SoundEffectSO soundEffect)
    {
        SoundEffect sound = (SoundEffect)PoolManager.Instance.ReuseComponent(soundEffect.soundPrefab, Vector3.zero, Quaternion.identity);
        sound.SetSound(soundEffect);
        sound.gameObject.SetActive(true);
        StartCoroutine(DisableSound(sound, soundEffect.soundEffectClip.length));
    }

    /// <summary>
    /// 지정된 사운드를 일정 시간 후 비활성화하는 코루틴 함수
    /// </summary>
    /// <param name="sound">비활성화할 사운드 이펙트 인스턴스</param>
    /// <param name="soundDuration">사운드 지속 시간 (초)</param>
    /// <returns>코루틴 핸들</returns>
    private IEnumerator DisableSound(SoundEffect sound, float soundDuration)
    {
        yield return new WaitForSeconds(soundDuration);
        sound.gameObject.SetActive(false);
    }

    /// <summary>
    /// 사운드 볼륨을 1 증가시키고 적용하는 함수
    /// </summary>
    public void IncreaseSoundsVolume()
    {
        int maxSoundsVolume = 20;

        if (maxSoundsVolume <= soundsVolume) return;

        soundsVolume += 1;

        SetSoundsVolume(soundsVolume);
    }

    /// <summary>
    /// 사운드 볼륨을 1 감소시키고 적용하는 함수
    /// </summary>
    public void DecreaseSoundsVolume()
    {
        if (soundsVolume == 0) return;

        soundsVolume -= 1;

        SetSoundsVolume(soundsVolume);
    }
    
    /// <summary>
    /// AudioMixer에 볼륨 값을 디시벨로 적용하는 함수
    /// </summary>
    /// <param name="soundsVolume">설정할 볼륨 값 (0~20)</param>
    private void SetSoundsVolume(int soundsVolume)
    {
        float muteDecibels = -80f;

        if (soundsVolume == 0)
        {
            GameResources.Instance.soundsMasterMixerGroup.audioMixer.SetFloat("soundsVolume", muteDecibels);
        }
        else
        {
            GameResources.Instance.soundsMasterMixerGroup.audioMixer.SetFloat("soundsVolume", HelperUtilities.LinearToDecibels(soundsVolume));
        }
    }
}