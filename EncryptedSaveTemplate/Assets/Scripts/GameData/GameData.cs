using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class GameDataBase { }

[System.Serializable]
public class GameDataPreview
{
    public string lastSaveTime;
}

[System.Serializable]
public class GameData : GameDataBase{ 
    public CustomGameData customGameData;
    public string saveTest;
}

[System.Serializable]
public class CustomGameData : GameDataBase
{
    //public string nickname;
    //public int level;
    //public int exp;
}
