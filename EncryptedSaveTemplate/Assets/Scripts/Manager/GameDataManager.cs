using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���� ������ �����͸� �����ϰ�, ����/�ҷ������ �����͸� ĳ���ϴ� �Ŵ��� Ŭ����
/// </summary>
public class GameDataManager : SingletonMonobehaviour<GameDataManager>
{
    public GameData CurrentData { get; private set; } // ���� ��� ���� ���� ������ �ν��Ͻ��� �����ϴ� ������Ƽ

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// ����� �����Ͱ� ������ �ҷ�����, ������ �� �����͸� �����ϴ� �Լ�
    /// </summary>
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

    /// <summary>
    /// ���� �����͸� �ܺο��� ���޵� ��ü GameData �ν��Ͻ��� ����� �Լ�
    /// </summary>
    /// <param name="newData">���Ӱ� ������ GameData ��ü</param>
    public void Overwrite(GameData newData)
    {
        CurrentData = newData;
    }

    /// <summary>
    /// ���� ������ �� Ŀ���� ������(CustomGameData)�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="customData">���ο� CustomGameData ��</param>
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
