using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings 
{
    #region SceneName Parameters
    public const string StartMenu = "StartMenu";
    public const string BootSceneName = "BootScene";
    public const string GameScene = "GameScene";
    #endregion

    #region AUDIO
    public const float musicFadeOutTime = 0.5f;
    public const float musicFadeInTime = 0.5f;
    #endregion AUDIO

    #region PlayerPrefs Hash Key
    public const string soundsVolumeHashKey = "soundsVolume";
    public const string musicVolumeHashKey = "musicVolume";
    public const string selectSlotHashKey = "SelectedSlot";
    #endregion
}
