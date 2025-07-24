using System;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// AES ��ĪŰ ��� ��ȣȭ/��ȣȭ ��ƿ��Ƽ Ŭ���� (CBC ��� + PKCS7 �е� ���)
/// </summary>
public static class AESCryptoUtil
{
    /// <summary>
    /// ������ AES Ű�� IV�� ����Ͽ� ���� ��ȣȭ�ϴ� �Լ�
    /// </summary>
    /// <param name="plainText">��ȣȭ�� ���ڿ� (UTF-8 ����)</param>
    /// <param name="aesKey">AES ��Ī Ű (16/24/32 ����Ʈ)</param>
    /// <param name="iv">�ʱ�ȭ ���� (16����Ʈ)</param>
    /// <returns>Base64�� ���ڵ��� ��ȣ�� ���ڿ�</returns>
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
    /// ������ AES Ű�� IV�� ����Ͽ� ��ȣ��(Base64)�� ��ȣȭ�ϴ� �Լ�
    /// </summary>
    /// <param name="encryptedBase64">Base64�� ���ڵ��� ��ȣ��</param>
    /// <param name="aesKey">AES ��Ī Ű (16/24/32 ����Ʈ)</param>
    /// <param name="iv">�ʱ�ȭ ���� (16����Ʈ)</param>
    /// <returns>��ȣȭ�� �� ���ڿ� (UTF-8 ����)</returns>
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