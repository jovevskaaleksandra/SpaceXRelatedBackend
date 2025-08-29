using Microsoft.Extensions.Configuration;
using SpaceXBackend.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SpaceXBackend.Services.Implementations
{
    public class EncryptionService : IEncryptionService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public EncryptionService(IConfiguration config)
        {
            _key = Encoding.UTF8.GetBytes(config["Encryption:Key"]);
            _iv = Encoding.UTF8.GetBytes(config["Encryption:IV"]);
        }
        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using var sw = new StreamWriter(cs);

            sw.Write(plainText);
            sw.Close(); 
            cs.Close(); 

            var encryptedBytes = ms.ToArray();

            return Convert.ToBase64String(ms.ToArray());

        }

        public string Decrypt(string cipherText)
        {
            var buffer = Convert.FromBase64String(cipherText);
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(buffer);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            var decrypted = sr.ReadToEnd();

            sr.Close(); 
            cs.Close(); 

            return decrypted;
        }
    }
}
