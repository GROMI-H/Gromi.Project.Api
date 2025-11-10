using System.Security.Cryptography;
using System.Text;

namespace Gromi.Infra.Utils.Helpers
{
    /// <summary>
    /// 加/解密帮助类
    /// </summary>
    public static class EncryptHelper
    {
        #region AES

        /// <summary>
        /// AES加密字符串
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="aesKey">32字节密钥</param>
        /// <returns></returns>
        public static string EncryptAes(string plainText, string aesKey)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(aesKey);
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// AES解密字符串
        /// </summary>
        /// <param name="clipherText">密文</param>
        /// <param name="aesKey">32字节密钥</param>
        /// <returns></returns>
        public static string DecryptAes(string clipherText, string aesKey)
        {
            var fullCipher = Convert.FromBase64String(clipherText);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(aesKey);
                var iv = new byte[aes.BlockSize / 8];
                Array.Copy(fullCipher, iv, iv.Length);
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        #endregion AES

        #region RSA

        /// <summary>
        /// RSA生成密钥对
        /// </summary>
        /// <returns>公钥和私钥</returns>
        public static (string publicKey, string privateKey) GenerateRsaKeys()
        {
            using (var rsa = RSA.Create())
            {
                var publicKey = Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo());
                var privateKey = Convert.ToBase64String(rsa.ExportPkcs8PrivateKey());
                return (publicKey, privateKey);
            }
        }

        /// <summary>
        /// RSA加密字符串
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public static string EncryptRsa(string plainText, string publicKey)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(publicKey), out _);
                var encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(plainText), RSAEncryptionPadding.OaepSHA256);
                return Convert.ToBase64String(encryptedData);
            }
        }

        /// <summary>
        /// RSA解密字符串
        /// </summary>
        /// <param name="cipherText">密文</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public static string DecryptRsa(string cipherText, string privateKey)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);
                var decryptedData = rsa.Decrypt(Convert.FromBase64String(cipherText), RSAEncryptionPadding.OaepSHA256);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        #endregion RSA

        #region MD5

        /// <summary>
        /// MD5 生成字符串的哈希值
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public static string Md5(string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);
                var stringBuilder = new StringBuilder();

                foreach (var b in hashBytes)
                {
                    stringBuilder.Append(b.ToString("x2")); // 将字节转换为十六进制字符串
                }

                return stringBuilder.ToString();
            }
        }

        #endregion MD5
    }
}