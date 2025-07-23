using System;
using System.Security.Cryptography;
using System.Text;

public static class AESCryptoUtil
{
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