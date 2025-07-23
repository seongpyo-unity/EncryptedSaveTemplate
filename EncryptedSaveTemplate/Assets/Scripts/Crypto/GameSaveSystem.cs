using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public static class GameSaveSystem
{
    private const string SaveFileFormat = "save_slot{0}.wrapped";
    private const string BackupFileFormat = "save_slot{0}_backup.wrapped";

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

    private static T LoadFromPath<T>(string path) where T : GameDataBase
    {
        string json = File.ReadAllText(path);
        var blob = JsonUtility.FromJson<WrappedSaveBlob>(json);

        byte[] aesKey = AESKeyGenerator.GenerateKey();
        byte[] iv = Convert.FromBase64String(blob.iv);

        string decryptedJson = AESCryptoUtil.DecryptWithKey(blob.encryptedData, aesKey, iv);
        return JsonUtility.FromJson<T>(decryptedJson);
    }

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

    private static string GetSavePath(int slotIndex)
    {
        return Path.Combine(Application.persistentDataPath, string.Format(SaveFileFormat, slotIndex));
    }

    private static string GetBackupPath(int slotIndex)
    {
        return Path.Combine(Application.persistentDataPath, string.Format(BackupFileFormat, slotIndex));
    }

    private static byte[] GenerateRandomIV()
    {
        byte[] iv = new byte[16];
        RandomNumberGenerator.Fill(iv);
        return iv;
    }
}