[System.Serializable]
public class WrappedSaveBlob
{
    public string iv;              // Base64 encoded IV
    public string encryptedData;  // Base64 encoded AES
}