using System.Security.Cryptography;
using System.Text;

public static class AESKeyGenerator
{
    public static string GetSeed()
    {
        char[] pool = new char[] { '#', '2', 'V', '2', '5', 'l', 'a', 'u', 'd', 'S', 'e', '0' };
        int[] order = new int[] { 9, 10, 11, 6, 5, 8, 1, 2, 4, 7, 0, 3 };

        StringBuilder sb = new StringBuilder();
        foreach (var i in order)
            sb.Append(pool[i]);

        return sb.ToString();
    }

    public static byte[] GenerateKey()
    {
        using var sha = SHA256.Create();
        return sha.ComputeHash(Encoding.UTF8.GetBytes(GetSeed()));
    }
}