using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 데이터 저장/로드/삭제 요청을 처리하는 세이브 매니저 클래스
/// </summary>
public class DataSaveManager : SingletonMonobehaviour<DataSaveManager>
{
    protected override void Awake()
    {
        base.Awake();       
    }

    /// <summary>
    /// 현재 게임 데이터를 저장 시스템에 요청하여 저장하는 함수
    /// </summary>
    /// <param name="reason">저장 요청 사유 (디버깅용 로그로 출력)</param>
    public void Save(string reason = "수동 저장")
    {
        var data = GameDataManager.Instance.CurrentData;
        int saveSlotIndex = PlayerPrefs.GetInt(Settings.selectSlotHashKey);
        GameSaveSystem.SaveGame(data, saveSlotIndex);
        GameSaveSystem.SavePreview(saveSlotIndex);

        Debug.Log($"[TriggerSave] 저장 실행됨: {reason}");
    }

    /// <summary>
    /// 저장 파일이 존재하면 로드 시도 후 결과를 반환하는 함수
    /// </summary>
    /// <param name="result">로드된 GameData 결과 (성공 시 인스턴스 반환)</param>
    /// <returns>로드 성공 여부</returns>
    public bool TryLoad(out GameData result)
    {
        int saveSlotIndex = PlayerPrefs.GetInt(Settings.selectSlotHashKey);

        return GameSaveSystem.TryLoadGame(out result);
    }

    /// <summary>
    /// 지정된 슬롯 인덱스의 저장 및 백업 파일을 삭제하는 함수
    /// </summary>
    /// <param name="slotIndex">삭제할 저장 슬롯 인덱스</param>
    public void DeleteSaveFile(int slotIndex)
    {
        GameSaveSystem.DeleteSave(slotIndex);
    }


}
