using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public static class GameSaveSystem
{
    private const string SaveFileName = "save.wrapped";
    private const string BackupFileName = "save_backup.wrapped";

    // 저장
    public static void SaveGame<T>(T data) where T : GameDataBase
    {
        string fullPath = Path.Combine(Application.persistentDataPath, SaveFileName);
        string backupPath = Path.Combine(Application.persistentDataPath, BackupFileName);

        if (File.Exists(fullPath))
        {
            File.Copy(fullPath, backupPath, overwrite: true);
            Debug.Log("[Save] 기존 세이브 백업 완료");
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
        Debug.Log($"[Save] 저장 완료: {fullPath}");
    }

    public static bool TryLoadGame<T>(out T result) where T : GameDataBase
    {
        result = null;

        string fullPath = Path.Combine(Application.persistentDataPath, SaveFileName);
        string backupPath = Path.Combine(Application.persistentDataPath, BackupFileName);

        try
        {
            result = LoadFromPath<T>(fullPath);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[Load] 메인 세이브 실패: {e.Message}");

            try
            {
                result = LoadFromPath<T>(backupPath);
                Debug.LogWarning("[Load] 백업 세이브로 복원 성공");
                return true;
            }
            catch (Exception e2)
            {
                Debug.LogError($"[Load] 백업 세이브 복구 실패: {e2.Message}");
                return false;
            }
        }
    }

    private static T LoadFromPath<T>(string path) where T : GameDataBase
    {
        string json = File.ReadAllText(path);
        var blob = JsonUtility.FromJson<WrappedSaveBlob>(json);

        byte[] aesKey = AESKeyGenerator.GenerateKey();
        byte[] iv = Convert.FromBase64String(blob.iv);

        string decryptedJson = AESCryptoUtil.DecryptWithKey(blob.encryptedData, aesKey, iv);
        return JsonUtility.FromJson<T>(decryptedJson);
    }

    public static void DeleteSave()
    {
        string path = Path.Combine(Application.persistentDataPath, SaveFileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    private static byte[] GenerateRandomIV()
    {
        byte[] iv = new byte[16];
        RandomNumberGenerator.Fill(iv);
        return iv;
    }

}