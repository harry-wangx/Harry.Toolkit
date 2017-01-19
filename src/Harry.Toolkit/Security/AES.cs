/**********************************************************************************************
 * Aes加解密类,NET20环境下使用Rijindael,NET35及以上版本,使用Aes.
 * 详见https://blogs.msdn.microsoft.com/shawnfa/2006/10/09/the-differences-between-rijndael-and-aes/
 **********************************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Harry.Security
{
    public static class AES
    {
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="plainData"></param>
        /// <param name="mode"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] key, byte[] iv, byte[] plainData, CipherMode mode = CipherMode.CBC, PaddingMode? padding = null)
        {
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv));
            if (plainData == null || plainData.Length <= 0)
                throw new ArgumentNullException(nameof(plainData));

            byte[] encrypted;
            using (var aesAlg =
#if NET20
                System.Security.Cryptography.Rijndael.Create()
#else
                System.Security.Cryptography.Aes.Create()
#endif

                )
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                aesAlg.Mode = mode;
                if (padding != null)
                {
                    aesAlg.Padding = padding.Value;
                }

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(plainData, 0, plainData.Length);
                        //csEncrypt.Flush();
                        csEncrypt.FlushFinalBlock();
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }


        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="cipherData"></param>
        /// <param name="bufferSize"></param>
        /// <param name="mode"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] key, byte[] iv, byte[] cipherData, int bufferSize = 1024, CipherMode mode = CipherMode.CBC, PaddingMode? padding = null)
        {
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv));
            if (cipherData == null || cipherData.Length <= 0)
                throw new ArgumentNullException(nameof(cipherData));
            if (bufferSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            int curPos = 0;
            byte[] buffer = null;
            byte[] decrypted = null;

            using (var aesAlg =
#if NET20
                System.Security.Cryptography.Rijndael.Create()
#else
                System.Security.Cryptography.Aes.Create()
#endif

)
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                aesAlg.Mode = mode;
                if (padding != null)
                {
                    aesAlg.Padding = padding.Value;
                }

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        int len = 0;
                        buffer = new byte[bufferSize];
                        do
                        {
                            if (curPos == buffer.Length)
                            {
                                byte[] newBuffer = new byte[buffer.Length * 2];
                                buffer.CopyTo(newBuffer, 0);
                                buffer = newBuffer;
                            }
                            len = csDecrypt.Read(buffer, curPos, buffer.Length - curPos);
                            curPos += len;

                        } while (len > 0);
                        decrypted = new byte[curPos];
                        Array.Copy(buffer, 0, decrypted, 0, curPos);
                    }
                }
            }
            return decrypted;
        }

    }
}
