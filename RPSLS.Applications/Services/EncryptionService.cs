using Microsoft.Extensions.Configuration;
using RPSLS.Applications.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace RPSLS.Applications.Services
{
    public class EncryptionService : IEncryptionService, IDisposable
    {
        private readonly byte[] _key;

        private readonly Aes _provider;

        public EncryptionService(IConfiguration config)
            : this(config["Security:SymmetricEncryptionKey"])
        {
        }

        public EncryptionService(string key)
            : this(Encoding.UTF8.GetBytes(key))
        {
        }

        public EncryptionService(byte[] key)
        {
            _provider = Aes.Create();
            int num = _provider.KeySize / 8;
            if (key.Length < num)
            {
                throw new ArgumentOutOfRangeException("key", $"Key's length cannot be less than {num} characters.");
            }

            _key = key.Take(num).ToArray();
        }

        public void Dispose()
        {
            _provider.Dispose();
        }

        public async Task<string> Encrypt(string cleartext)
        {
            using MemoryStream stream = new MemoryStream();
            _provider.GenerateIV();
            await stream.WriteAsync(_provider.IV, 0, _provider.IV.Length);
            using (ICryptoTransform encryptor = _provider.CreateEncryptor(_key, _provider.IV))
            {
                using CryptoStream encryptedStream = new CryptoStream(stream, encryptor, CryptoStreamMode.Write);
                byte[] bytes = Encoding.UTF8.GetBytes(cleartext);
                await encryptedStream.WriteAsync(bytes, 0, bytes.Length);
                encryptedStream.FlushFinalBlock();
            }

            return Convert.ToBase64String(stream.ToArray());
        }

        public async Task<string> Decrypt(string encrypted)
        {
            byte[] buffer2 = Convert.FromBase64String(encrypted);
            using MemoryStream stream = new MemoryStream(buffer2);
            _provider.Key = _key.Take(_provider.KeySize / 8).ToArray();
            byte[] ivBuffer = new byte[_provider.IV.Length];
            await stream.ReadAsync(ivBuffer, 0, _provider.IV.Length);
            using ICryptoTransform decryptor = _provider.CreateDecryptor(_key, ivBuffer);
            using MemoryStream dataOut = new MemoryStream();
            using CryptoStream decryptedStream = new CryptoStream(stream, decryptor, CryptoStreamMode.Read);
            using BinaryReader decryptedData = new BinaryReader(decryptedStream);
            byte[] buffer = new byte[4096];
            int count;
            while ((count = decryptedData.Read(buffer, 0, buffer.Length)) != 0)
            {
                await dataOut.WriteAsync(buffer, 0, count);
            }

            byte[] bytes = dataOut.ToArray();
            string @string = Encoding.UTF8.GetString(bytes);
            return @string;
        }
    }
}
