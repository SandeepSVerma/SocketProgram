using System.Security.Cryptography;
using System.Text;

namespace ConsoleSocketProgram
{
    public static class CryptoHelper
    {
        public static readonly byte[] key = Encoding.UTF8.GetBytes("12345678123456781234567812345678"); //32 bytes AES-25
        public static readonly byte[] iv = Encoding.UTF8.GetBytes("1234567812345678"); //16 bytes AES

        public static byte[] Encrypt(string text)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, aes.CreateEncryptor(),CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(text);
                    sw.Flush();
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }

        public static string Decrypt(byte[] cipher)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                using (var ms = new MemoryStream(cipher))
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
