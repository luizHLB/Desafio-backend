using System.Security.Cryptography;
using System.Text;

namespace Product.Domain.Secutiry
{
    public static class CryptoHelper
    {
        public static string Encrypt(string plaintext, string key, string iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key.Split(',').Select(s => Convert.ToByte(s)).ToArray();
                aesAlg.IV = iv.Split(',').Select(s => Convert.ToByte(s)).ToArray();
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                byte[] encryptedBytes;
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] plainBytes = Encoding.UTF8.GetBytes(plaintext);
                        csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                    }
                    encryptedBytes = msEncrypt.ToArray();
                }
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public static string Decrypt(string data, string key, string iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key.Split(',').Select(s => Convert.ToByte(s)).ToArray();
                aesAlg.IV = iv.Split(',').Select(s => Convert.ToByte(s)).ToArray(); 
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] decryptedBytes;
                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(data)))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var msPlain = new MemoryStream())
                        {
                            csDecrypt.CopyTo(msPlain);
                            decryptedBytes = msPlain.ToArray();
                        }
                    }
                }
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}
