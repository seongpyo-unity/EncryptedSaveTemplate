using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ����Ʈ ��� �� ���� ������ ����ϴ� �Ŵ��� Ŭ����
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
    /// ������ ���� ����Ʈ�� ������Ʈ Ǯ�� �̿��� ����ϴ� �Լ�
    /// </summary>
    /// <param name="soundEffect">����� ���� ����Ʈ SO</param>
    public void PlaySoundEffect(SoundEffectSO soundEffect)
    {
        SoundEffect sound = (SoundEffect)PoolManager.Instance.ReuseComponent(soundEffect.soundPrefab, Vector3.zero, Quaternion.identity);
        sound.SetSound(soundEffect);
        sound.gameObject.SetActive(true);
        StartCoroutine(DisableSound(sound, soundEffect.soundEffectClip.length));
    }

    /// <summary>
    /// ������ ���带 ���� �ð� �� ��Ȱ��ȭ�ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <param name="sound">��Ȱ��ȭ�� ���� ����Ʈ �ν��Ͻ�</param>
    /// <param name="soundDuration">���� ���� �ð� (��)</param>
    /// <returns>�ڷ�ƾ �ڵ�</returns>
    private IEnumerator DisableSound(SoundEffect sound, float soundDuration)
    {
        yield return new WaitForSeconds(soundDuration);
        sound.gameObject.SetActive(false);
    }

    /// <summary>
    /// ���� ������ 1 ������Ű�� �����ϴ� �Լ�
    /// </summary>
    public void IncreaseSoundsVolume()
    {
        int maxSoundsVolume = 20;

        if (maxSoundsVolume <= soundsVolume) return;

        soundsVolume += 1;

        SetSoundsVolume(soundsVolume);
    }

    /// <summary>
    /// ���� ������ 1 ���ҽ�Ű�� �����ϴ� �Լ�
    /// </summary>
    public void DecreaseSoundsVolume()
    {
        if (soundsVolume == 0) return;

        soundsVolume -= 1;

        SetSoundsVolume(soundsVolume);
    }
    
    /// <summary>
    /// AudioMixer�� ���� ���� ��ú��� �����ϴ� �Լ�
    /// </summary>
    /// <param name="soundsVolume">������ ���� �� (0~20)</param>
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