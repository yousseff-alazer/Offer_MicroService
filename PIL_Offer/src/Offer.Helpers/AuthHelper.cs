using System;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Offer.Helpers
{
    public class AuthHelper
    {
        public const string ROUTINGAPPLICATION_KEY_CODE = "JAN_ENC_KEY_05022020";
        public const string APPLICATION_KEY_CODE = "JAN_ENC_KEY_05022020";

        public static string GetClaimValue(ClaimsPrincipal user, string type)
        {
            if (user.Identity is ClaimsIdentity identity)
            {
                var claim = identity.FindFirst(c => c.Type == type);
                if (claim != null)
                    return claim.Value;
            }

            return null;
        }

        public static T DecryptQueryValue<T>(string encryptedValue)
        {
            var value = StringCipher.Decrypt(encryptedValue, ROUTINGAPPLICATION_KEY_CODE);
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static string EncryptQueryValue<T>(T value)
        {
            return StringCipher.Encrypt(Convert.ToString(value), ROUTINGAPPLICATION_KEY_CODE);
        }

        public static T EncryptQueryValue<T>(string value)
        {
            var encryptedValue = StringCipher.Encrypt(value, ROUTINGAPPLICATION_KEY_CODE);
            return (T)Convert.ChangeType(encryptedValue, typeof(T));
        }

        public static class StringCipher
        {
            // This constant is used to determine the keysize of the encryption algorithm.
            private const int keysize = 256;

            // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
            // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
            // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
            private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");

            public static string EncryptedLoginInfo(string loginguid)
            {
                return Encrypt(loginguid, APPLICATION_KEY_CODE);
            }

            public static string DecryptLoginInfo(string loginguid)
            {
                return Decrypt(loginguid, APPLICATION_KEY_CODE);
            }

            public static string Encrypt(string plainText, string passPhrase)
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                using (var password = new PasswordDeriveBytes(passPhrase, null))
                {
                    var keyBytes = password.GetBytes(keysize / 8);
                    using (var symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.Mode = CipherMode.CBC;
                        using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                using (var cryptoStream =
                                    new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                                {
                                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                    cryptoStream.FlushFinalBlock();
                                    var cipherTextBytes = memoryStream.ToArray();
                                    return Convert.ToBase64String(cipherTextBytes);
                                }
                            }
                        }
                    }
                }
            }

            public static string Decrypt(string cipherText, string passPhrase)
            {
                if (cipherText == null)
                    cipherText = "";
                var cipherTextBytes = Convert.FromBase64String(cipherText);
                using (var password = new PasswordDeriveBytes(passPhrase, null))
                {
                    var keyBytes = password.GetBytes(keysize / 8);
                    using (var symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.Mode = CipherMode.CBC;
                        using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                        {
                            using (var memoryStream = new MemoryStream(cipherTextBytes))
                            {
                                using (var cryptoStream =
                                    new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                {
                                    var plainTextBytes = new byte[cipherTextBytes.Length];
                                    var decryptedByteCount =
                                        cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}