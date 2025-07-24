using System.Security.Cryptography;
using System.Text;

/// <summary>
/// AES 암호화에서 사용할 고정 시드 기반 키를 생성하는 유틸리티 클래스
/// </summary>
public static class AESKeyGenerator
{
    /// <summary>
    /// AES 키 생성을 위한 고정 시드 문자열을 생성하는 함수  
    /// (문자 배열과 인덱스 순서를 기반으로 시드를 조합 -> 리버싱 방지)
    /// </summary>
    /// <returns>SHA256 해싱에 사용할 시드 문자열</returns>
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
    /// 시드 문자열을 SHA-256 해싱하여 32바이트 AES 대칭 키를 생성하는 함수
    /// </summary>
    /// <returns>SHA256 해시로 생성된 256비트 AES 키</returns>
    public static byte[] GenerateKey()
    {
        using var sha = SHA256.Create();
        return sha.ComputeHash(Encoding.UTF8.GetBytes(GetSeed()));
    }
}