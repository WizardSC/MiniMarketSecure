using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Crypto
{
    public static class DES
    {
        static string ConvertToBinary(string input)
        {
            // Chuyển chuỗi thành mảng byte
            byte[] keyBytes = Encoding.UTF8.GetBytes(input);

            // Chuyển mảng byte thành dạng bit nhị phân
            StringBuilder binaryKey = new StringBuilder();
            foreach (byte b in keyBytes)
            {
                binaryKey.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            return binaryKey.ToString();
        }

        public static string EncryptDES(string trangThai, string secretKey)
        {
            // Chuyển chuỗi thành mảng byte
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(trangThai);

            // Chuyển khóa bí mật thành dạng bit nhị phân
            string binaryKey = ConvertToBinary(secretKey);

            // Chuyển dạng bit nhị phân thành mảng byte
            byte[] keyBytes = new byte[binaryKey.Length / 8];
            for (int i = 0; i < keyBytes.Length; i++)
            {
                keyBytes[i] = Convert.ToByte(binaryKey.Substring(i * 8, 8), 2);
            }

            using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
            {
                desAlg.Key = keyBytes;
                desAlg.Mode = CipherMode.ECB;
                desAlg.Padding = PaddingMode.PKCS7;

                // Tạo một bộ mã hóa từ khóa
                ICryptoTransform encryptor = desAlg.CreateEncryptor(desAlg.Key, desAlg.IV);

                // Mã hóa dữ liệu
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);

                // Chuyển mảng byte mã hóa thành chuỗi HEX
                return BitConverter.ToString(encryptedBytes).Replace("-", "");
            }
        }

        public static string DecryptDES(string encryptedValue, string secretKey)
        {
            // Chuyển chuỗi HEX thành mảng byte
            byte[] encryptedBytes = new byte[encryptedValue.Length / 2];
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                encryptedBytes[i] = byte.Parse(encryptedValue.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }

            // Chuyển khóa bí mật thành dạng bit nhị phân
            string binaryKey = ConvertToBinary(secretKey);

            // Chuyển dạng bit nhị phân thành mảng byte
            byte[] keyBytes = new byte[binaryKey.Length / 8];
            for (int i = 0; i < keyBytes.Length; i++)
            {
                keyBytes[i] = Convert.ToByte(binaryKey.Substring(i * 8, 8), 2);
            }

            using (DESCryptoServiceProvider desAlg = new DESCryptoServiceProvider())
            {
                desAlg.Key = keyBytes;
                desAlg.Mode = CipherMode.ECB;
                desAlg.Padding = PaddingMode.PKCS7;

                // Tạo một bộ giải mã từ khóa
                ICryptoTransform decryptor = desAlg.CreateDecryptor(desAlg.Key, desAlg.IV);

                // Giải mã dữ liệu
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                // Chuyển mảng byte giải mã thành chuỗi UTF-8
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}
