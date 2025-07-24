using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// AES 기반 암호화된 세이브 파일을 관리하는 게임 저장 시스템 클래스
/// </summary>
public static class GameSaveSystem
{
    private const string SaveFileFormat = "save_slot{0}.wrapped";
    private const string BackupFileFormat = "save_slot{0}_backup.wrapped";

    # region 저장 관련 함수

    /// <summary>
    /// 게임 데이터를 AES로 암호화하여 로컬 저장소에 저장하는 함수
    /// </summary>
    /// <typeparam name="T">GameDataBase를 상속한 저장 대상 데이터 타입</typeparam>
    /// <param name="data">저장할 게임 데이터 인스턴스</param>
    /// <param name="slotIndex">세이브 슬롯 인덱스 (기본값 0)</param>
    public static void SaveGame<T>(T data, int slotIndex = 0) where T : GameDataBase
    {
        string fullPath = GetSavePath(slotIndex);
        string backupPath = GetBackupPath(slotIndex);

        if (File.Exists(fullPath))
        {
            File.Copy(fullPath, backupPath, overwrite: true);
            Debug.Log($"[Save] 슬롯 {slotIndex} 백업 완료");
        }

        byte[] aesKey = AESKeyGenerator.GenerateKey();
        byte[] iv = GenerateRandomIV();

        string json = JsonUtility.ToJson(data);
        string encryptedData = AESCryptoUtil.EncryptWithKey(json, aesKey, iv);

        var blob = new WrappedSaveBlob
        {
            iv = Convert.ToBase64String(iv),
            encryptedData = encryptedData
        };

        File.WriteAllText(fullPath, JsonUtility.ToJson(blob));
        Debug.Log($"[Save] 슬롯 {slotIndex} 저장 완료: {fullPath}");
    }

    /// <summary>
    /// 슬롯에 대응하는 프리뷰 데이터를 암호화하여 저장하는 함수
    /// </summary>
    /// <param name="preview">저장할 프리뷰 데이터</param>
    /// <param name="slotIndex">슬롯 인덱스</param>
    public static void SavePreview(int slotIndex)
    {
        string path = GetPreviewPath(slotIndex);

        byte[] aesKey = AESKeyGenerator.GenerateKey();
        byte[] iv = GenerateRandomIV();

        GameDataPreview preview = new GameDataPreview()
        {
            lastSaveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
        };

        string json = JsonUtility.ToJson(preview);
        string encrypted = AESCryptoUtil.EncryptWithKey(json, aesKey, iv);

        var blob = new WrappedSaveBlob
        {
            iv = Convert.ToBase64String(iv),
            encryptedData = encrypted
        };

        File.WriteAllText(path, JsonUtility.ToJson(blob));
    }

    /// <summary>
    /// 지정된 슬롯의 세이브 및 백업 파일을 삭제하는 함수
    /// </summary>
    /// <param name="slotIndex">삭제할 슬롯 인덱스</param>
    public static void DeleteSave(int slotIndex)
    {
        string path = GetSavePath(slotIndex);
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        string backup = GetBackupPath(slotIndex);
        if (File.Exists(backup))
        {
            File.Delete(backup);
        }
    }

    #endregion

    #region 로드 관련 함수

    /// <summary>
    /// 저장 파일 또는 백업 파일에서 데이터를 복호화하여 불러오는 함수
    /// </summary>
    /// <typeparam name="T">GameDataBase를 상속한 로드 대상 데이터 타입</typeparam>
    /// <param name="result">로드된 결과 데이터를 담는 out 파라미터</param>
    /// <param name="slotIndex">로드할 슬롯 인덱스 (기본값 0)</param>
    /// <returns>성공 시 true, 실패 시 false</returns>
    public static bool TryLoadGame<T>(out T result, int slotIndex = 0) where T : GameDataBase, new()
    {
        result = null;

        string fullPath = GetSavePath(slotIndex);
        string backupPath = GetBackupPath(slotIndex);

        bool mainExists = File.Exists(fullPath);
        bool backupExists = File.Exists(backupPath);

        if (mainExists)
        {
            try
            {
                result = LoadFromPath<T>(fullPath);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Load] 슬롯 {slotIndex} 메인 세이브 손상됨: {e.Message}");

                if (backupExists)
                {
                    try
                    {
                        result = LoadFromPath<T>(backupPath);
                        Debug.LogWarning($"[Load] 슬롯 {slotIndex} 백업 세이브로 복원됨");
                        return true;
                    }
                    catch (Exception e2)
                    {
                        Debug.LogError($"[Load] 슬롯 {slotIndex} 백업 세이브도 손상됨: {e2.Message}");
                        return false; 
                    }
                }

                return false;
            }
        }

        Debug.Log($"[Load] 슬롯 {slotIndex} 세이브 없음 → 새로 생성");
        result = new T();
        SaveGame(result, slotIndex);
        return true;
    }

    /// <summary>
    /// 슬롯의 프리뷰 데이터를 복호화하여 불러오는 함수
    /// </summary>
    /// <param name="result">불러온 프리뷰 데이터 결과</param>
    /// <param name="slotIndex">슬롯 인덱스</param>
    /// <returns>성공 여부</returns>
    public static bool TryLoadPreview(out GameDataPreview result, int slotIndex)
    {
        result = null;

        string path = GetPreviewPath(slotIndex);
        if (!File.Exists(path))
            return false;

        try
        {
            string json = File.ReadAllText(path);
            var blob = JsonUtility.FromJson<WrappedSaveBlob>(json);

            byte[] aesKey = AESKeyGenerator.GenerateKey();
            byte[] iv = Convert.FromBase64String(blob.iv);

            string decryptedJson = AESCryptoUtil.DecryptWithKey(blob.encryptedData, aesKey, iv);
            result = JsonUtility.FromJson<GameDataPreview>(decryptedJson);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"[Preview Load] 슬롯 {slotIndex} 프리뷰 로드 실패: {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// 지정된 경로의 저장 파일을 AES 복호화하여 데이터로 변환하는 함수
    /// </summary>
    /// <typeparam name="T">GameDataBase를 상속한 데이터 타입</typeparam>
    /// <param name="path">복호화할 저장 파일 경로</param>
    /// <returns>복호화된 게임 데이터 인스턴스</returns>
    private static T LoadFromPath<T>(string path) where T : GameDataBase
    {
        string json = File.ReadAllText(path);
        var blob = JsonUtility.FromJson<WrappedSaveBlob>(json);

        byte[] aesKey = AESKeyGenerator.GenerateKey();
        byte[] iv = Convert.FromBase64String(blob.iv);

        string decryptedJson = AESCryptoUtil.DecryptWithKey(blob.encryptedData, aesKey, iv);
        return JsonUtility.FromJson<T>(decryptedJson);
    }

    #endregion

    #region 경로 및 내부 유틸함수

    /// <summary>
    /// 지정된 슬롯 인덱스의 저장 파일 전체 경로를 반환하는 함수
    /// </summary>
    /// <param name="slotIndex">슬롯 인덱스</param>
    /// <returns>저장 파일의 전체 경로 문자열</returns>
    private static string GetSavePath(int slotIndex)
    {
        return Path.Combine(Application.persistentDataPath, string.Format(SaveFileFormat, slotIndex));
    }

    /// <summary>
    /// 지정된 슬롯 인덱스의 백업 파일 전체 경로를 반환하는 함수
    /// </summary>
    /// <param name="slotIndex">슬롯 인덱스</param>
    /// <returns>백업 파일의 전체 경로 문자열</returns>
    private static string GetBackupPath(int slotIndex)
    {
        return Path.Combine(Application.persistentDataPath, string.Format(BackupFileFormat, slotIndex));
    }

    /// <summary>
    /// 프리뷰 파일 경로를 반환하는 함수
    /// </summary>
    /// <param name="slotIndex">슬롯 인덱스</param>
    /// <returns>프리뷰 저장 파일 경로</returns>
    private static string GetPreviewPath(int slotIndex)
    {
        string fileName = $"save_slot{slotIndex}_preview.wrapped";
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    /// <summary>
    /// AES 암호화를 위한 16바이트 랜덤 IV(초기화 벡터)를 생성하는 함수
    /// </summary>
    /// <returns>랜덤 IV 바이트 배열</returns>
    private static byte[] GenerateRandomIV()
    {
        byte[] iv = new byte[16];
        RandomNumberGenerator.Fill(iv);
        return iv;
    }

    #endregion
}