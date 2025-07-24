using System.Security.Cryptography;
using System.Text;

/// <summary>
/// AES ��ȣȭ���� ����� ���� �õ� ��� Ű�� �����ϴ� ��ƿ��Ƽ Ŭ����
/// </summary>
public static class AESKeyGenerator
{
    /// <summary>
    /// AES Ű ������ ���� ���� �õ� ���ڿ��� �����ϴ� �Լ�  
    /// (���� �迭�� �ε��� ������ ������� �õ带 ���� -> ������ ����)
    /// </summary>
    /// <returns>SHA256 �ؽ̿� ����� �õ� ���ڿ�</returns>
    public static string GetSeed()
    {
        char[] pool = new char[] { '#', '2', 'V', '2', '5', 'l', 'a', 'u', 'd', 'S', 'e', '0' };
        int[] order = new int[] { 9, 10, 11, 6, 5, 8, 1, 2, 4, 7, 0, 3 };

        StringBuilder sb = new StringBuilder();
        foreach (var i in order)
            sb.Append(pool[i]);

        return sb.ToString();
    }

    /// <summary>
    /// �õ� ���ڿ��� SHA-256 �ؽ��Ͽ� 32����Ʈ AES ��Ī Ű�� �����ϴ� �Լ�
    /// </summary>
    /// <returns>SHA256 �ؽ÷� ������ 256��Ʈ AES Ű</returns>
    public static byte[] GenerateKey()
    {
        using var sha = SHA256.Create();
        return sha.ComputeHash(Encoding.UTF8.GetBytes(GetSeed()));
    }
}