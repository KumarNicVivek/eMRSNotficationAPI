using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SERVICEAPP.Utility
{
    public static class PasswordHashing
    {

        public static string ComputeShortUrlHashCode(string input)
        {
            using (var sha = SHA512.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha.ComputeHash(bytes);

                byte[] _iv = hash.Take(16).ToArray();
                byte[] _key = hash.Take(32).ToArray();

                var shortCode = GenerateShortEncryptedCode(_key, _iv);

                return shortCode.ToLowerInvariant(); // 128-char hex
            }

        }

        public static string ComputeSHA512(string input)
        {
            using (var sha = SHA512.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha.ComputeHash(bytes);
                //byte[] _iv = hash.Take(16).ToArray();

                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant(); // 128-char hex

            }
        }



        public static string GenerateSalt()
        {
            using var rng = RandomNumberGenerator.Create();
            var byteSalt = new byte[16];
            rng.GetBytes(byteSalt);
            var salt = Convert.ToBase64String(byteSalt);
            return salt;

        }

        // ✅ NEW byte[] version (THIS IS WHAT YOU NEED)
        public static byte[] ComputeSHA512FromByte(byte[] inputBytes)
        {
            using var sha = SHA512.Create();
            return sha.ComputeHash(inputBytes);
        }

        public static string AesEncrypt(string input, byte[] _key, byte[] _iv)
        {
            using var aes = System.Security.Cryptography.Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            var encryptor = aes.CreateEncryptor();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var encrypted = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

            return Convert.ToBase64String(encrypted)
                          .Replace('+', '-')
                          .Replace('/', '_')
                          .TrimEnd('=');
        }

        public static string GenerateShortEncryptedCode(byte[] _key, byte[] iv)
        {
            var guid = Guid.NewGuid().ToString("N").Substring(0, 8); // or use random string
            return AesEncrypt(guid, _key, iv);
        }

    }
}
