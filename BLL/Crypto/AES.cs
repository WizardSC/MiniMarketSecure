using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Crypto
{
    public static class AES
    {
        public static string EncryptAES(string plainText, string secretKey)
        {
            // Chuyển chuỗi thành mảng byte
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plainText);

            // Chuyển khóa bí mật từ chuỗi hex thành mảng byte
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                // Tạo một bộ mã hóa từ khóa
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Mã hóa dữ liệu
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);

                // Chuyển mảng byte mã hóa thành chuỗi Base64
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public static string DecryptAES(string encryptedValue, string secretKey)
        {
            // Chuyển chuỗi Base64 thành mảng byte
            byte[] encryptedBytes = Convert.FromBase64String(encryptedValue);

            // Chuyển khóa bí mật từ chuỗi hex thành mảng byte
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                // Tạo một bộ giải mã từ khóa
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Giải mã dữ liệu
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                // Chuyển mảng byte giải mã thành chuỗi UTF-8
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}
