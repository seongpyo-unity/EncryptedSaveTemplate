using System;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// AES 대칭키 기반 암호화/복호화 유틸리티 클래스 (CBC 모드 + PKCS7 패딩 사용)
/// </summary>
public static class AESCryptoUtil
{
    /// <summary>
    /// 지정된 AES 키와 IV를 사용하여 평문을 암호화하는 함수
    /// </summary>
    /// <param name="plainText">암호화할 문자열 (UTF-8 기준)</param>
    /// <param name="aesKey">AES 대칭 키 (16/24/32 바이트)</param>
    /// <param name="iv">초기화 벡터 (16바이트)</param>
    /// <returns>Base64로 인코딩된 암호문 문자열</returns>
    public static string EncryptWithKey(string plainText, byte[] aesKey, byte[] iv)
    {
        using Aes aes = Aes.Create();
        aes.Key = aesKey;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] encrypted = aes.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        return Convert.ToBase64String(encrypted);
    }

    /// <summary>
    /// 지정된 AES 키와 IV를 사용하여 암호문(Base64)을 복호화하는 함수
    /// </summary>
    /// <param name="encryptedBase64">Base64로 인코딩된 암호문</param>
    /// <param name="aesKey">AES 대칭 키 (16/24/32 바이트)</param>
    /// <param name="iv">초기화 벡터 (16바이트)</param>
    /// <returns>복호화된 평문 문자열 (UTF-8 기준)</returns>
    public static string DecryptWithKey(string encryptedBase64, byte[] aesKey, byte[] iv)
    {
        using Aes aes = Aes.Create();
        aes.Key = aesKey;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        byte[] encryptedBytes = Convert.FromBase64String(encryptedBase64);
        byte[] decrypted = aes.CreateDecryptor().TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
        return Encoding.UTF8.GetString(decrypted);
    }
}