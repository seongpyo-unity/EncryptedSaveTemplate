using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 현재 게임 세션의 데이터를 관리하고, 저장/불러오기된 데이터를 캐싱하는 매니저 클래스
/// </summary>
public class GameDataManager : SingletonMonobehaviour<GameDataManager>
{
    public GameData CurrentData { get; private set; } // 현재 사용 중인 게임 데이터 인스턴스를 보관하는 프로퍼티

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 저장된 데이터가 있으면 불러오고, 없으면 새 데이터를 생성하는 함수
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
    /// 현재 데이터를 외부에서 전달된 전체 GameData 인스턴스로 덮어쓰는 함수
    /// </summary>
    /// <param name="newData">새롭게 설정할 GameData 객체</param>
    public void Overwrite(GameData newData)
    {
        CurrentData = newData;
    }

    /// <summary>
    /// 기존 데이터 중 커스텀 데이터(CustomGameData)만 갱신하는 함수
    /// </summary>
    /// <param name="customData">새로운 CustomGameData 값</param>
    public void OverwriteCustom(CustomGameData customData)
    {
        if (CurrentData == null)
        {
            Debug.LogWarning("[GameDataManager] Current 데이터가 null입니다. 부분 업데이트 실패.");
            return;
        }

        CurrentData.customGameData = customData;
    }
}
