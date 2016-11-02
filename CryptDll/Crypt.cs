using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptDll
{
    public class Crypt
    {

        private Crypt()
        {

            SaltP = new byte[] { 91, 30, 61, 29, 96, 47, 84, 64 };

            SaltIV = new byte[] { 13, 46, 40, 49, 27, 71, 83, 24 };
        }

        public Crypt(string password) : this()
        {
            key = generateKey(password);
            IV = generateIV(password);
        }


        private readonly byte[] SaltP;
        private readonly byte[] SaltIV;
        private byte[] key;
        private byte[] IV;


        public string Encrypt(string text)
        {
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = key;
                aesAlg.IV = IV;

                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                {
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(text);
                            }
                            return ToBase64(Encoding.Default.GetString(msEncrypt.ToArray()));
                        }
                    }
                }
            }
        }






        public string Decrypt(string text)
        {
            try
            {
                string result;
                using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = IV;

                    using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                    {
                        using (MemoryStream msDecrypt = new MemoryStream(Encoding.Default.GetBytes(FromBase64(text))))
                        {
                            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                            {
                                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                                {
                                    result = srDecrypt.ReadToEnd();
                                }
                            }
                        }
                    }

                }
                return result;
            }
            catch (CryptographicException ex)
            {
                throw ex;
            }
        }
        private string ToBase64(string text) { return Convert.ToBase64String(Encoding.UTF8.GetBytes(text)); }

        private string FromBase64(string text) { return Encoding.UTF8.GetString(Convert.FromBase64String(text)); }

        private byte[] generateKey(string password, int keyBytes = 32)
        {
            const int Iterations = 248;
            var keyGenerator = new Rfc2898DeriveBytes(password, SaltP, Iterations);
            return keyGenerator.GetBytes(keyBytes);
        }


        private byte[] generateIV(string password, int keyBytes = 16)
        {
            const int Iterations = 271;
            var keyGenerator = new Rfc2898DeriveBytes(password, SaltIV, Iterations);
            return keyGenerator.GetBytes(keyBytes);
        }

    }
}
