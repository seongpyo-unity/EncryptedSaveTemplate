using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : SingletonMonobehaviour<GameDataManager>
{
    public GameData CurrentData { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }
    public void TryLoadOrInitialize()
    {
        if (DataSaveManager.Instance.TryLoad(out GameData loaded))
        {
            CurrentData = loaded;
        }
        else
        {
            CurrentData  = new GameData();
        }
    }

    public void Save()
    {
        if (CurrentData == null)
        {
            Debug.LogWarning("[GameDataManager] Current �����Ͱ� null�Դϴ�. ���� ����.");
            return;
        }

        DataSaveManager.Instance.Save(CurrentData);
    }

    public void Overwrite(GameData newData)
    {
        CurrentData = newData;
    }

    public void OverwriteCustom(CustomGameData customData)
    {
        if (CurrentData == null)
        {
            Debug.LogWarning("[GameDataManager] Current �����Ͱ� null�Դϴ�. �κ� ������Ʈ ����.");
            return;
        }

        CurrentData.customGameData = customData;
    }
}
