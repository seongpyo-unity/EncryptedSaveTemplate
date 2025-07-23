using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaveManager : SingletonMonobehaviour<DataSaveManager>
{
    protected override void Awake()
    {
        base.Awake();       
    }

    public void Save(string reason = "���� ����")
    {
        var data = GameDataManager.Instance.CurrentData;
        GameSaveSystem.SaveGame(data);
        Debug.Log($"[TriggerSave] ���� �����: {reason}");
    }

    public bool TryLoad(out GameData result)
    {
        return GameSaveSystem.TryLoadGame(out result);
    }

    public void DeleteSaveFile(int slotIndex)
    {
        GameSaveSystem.DeleteSave(slotIndex);
    }


}
