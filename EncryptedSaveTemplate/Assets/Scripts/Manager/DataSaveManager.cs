using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ������ ����/�ε�/���� ��û�� ó���ϴ� ���̺� �Ŵ��� Ŭ����
/// </summary>
public class DataSaveManager : SingletonMonobehaviour<DataSaveManager>
{
    protected override void Awake()
    {
        base.Awake();       
    }

    /// <summary>
    /// ���� ���� �����͸� ���� �ý��ۿ� ��û�Ͽ� �����ϴ� �Լ�
    /// </summary>
    /// <param name="reason">���� ��û ���� (������ �α׷� ���)</param>
    public void Save(string reason = "���� ����")
    {
        var data = GameDataManager.Instance.CurrentData;
        int saveSlotIndex = PlayerPrefs.GetInt(Settings.selectSlotHashKey);
        GameSaveSystem.SaveGame(data, saveSlotIndex);
        GameSaveSystem.SavePreview(saveSlotIndex);

        Debug.Log($"[TriggerSave] ���� �����: {reason}");
    }

    /// <summary>
    /// ���� ������ �����ϸ� �ε� �õ� �� ����� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="result">�ε�� GameData ��� (���� �� �ν��Ͻ� ��ȯ)</param>
    /// <returns>�ε� ���� ����</returns>
    public bool TryLoad(out GameData result)
    {
        int saveSlotIndex = PlayerPrefs.GetInt(Settings.selectSlotHashKey);

        return GameSaveSystem.TryLoadGame(out result);
    }

    /// <summary>
    /// ������ ���� �ε����� ���� �� ��� ������ �����ϴ� �Լ�
    /// </summary>
    /// <param name="slotIndex">������ ���� ���� �ε���</param>
    public void DeleteSaveFile(int slotIndex)
    {
        GameSaveSystem.DeleteSave(slotIndex);
    }


}
