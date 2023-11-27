using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Crypto
{
    public static class XOR
    {
        public static int EncryptXOR(int plaintext, int key)
        {
            // Chuyển plaintext và key sang dạng nhị phân
            string binaryPlaintext = Convert.ToString(plaintext, 2);
            string binaryKey = Convert.ToString(key, 2);

            // Đảm bảo độ dài của binaryPlaintext bằng độ dài của binaryKey
            if (binaryPlaintext.Length < binaryKey.Length)
            {
                binaryPlaintext = binaryPlaintext.PadLeft(binaryKey.Length, '0');
            }
            else if (binaryKey.Length < binaryPlaintext.Length)
            {
                // Đảm bảo độ dài của binaryKey bằng độ dài của binaryPlaintext
                binaryKey = binaryKey.PadLeft(binaryPlaintext.Length, '0');
            }
            Console.WriteLine(binaryPlaintext);
            Console.WriteLine(binaryKey);

            // Thực hiện phép toán XOR
            int ciphertext = Convert.ToInt32(binaryPlaintext, 2) ^ Convert.ToInt32(binaryKey, 2);

            return ciphertext;
        }

        public static int DecryptXOR(int ciphertext, int key)
        {
            // Chuyển ciphertext và key sang dạng nhị phân
            string binaryCiphertext = Convert.ToString(ciphertext, 2);
            string binaryKey = Convert.ToString(key, 2);

            // Đảm bảo độ dài của binaryKey bằng độ dài của binaryCiphertext
            if (binaryKey.Length < binaryCiphertext.Length)
            {
                binaryKey = binaryKey.PadLeft(binaryCiphertext.Length, '0');
            }
            else if (binaryCiphertext.Length < binaryKey.Length)
            {
                // Đảm bảo độ dài của binaryCiphertext bằng độ dài của binaryKey
                binaryCiphertext = binaryCiphertext.PadLeft(binaryKey.Length, '0');
            }
            Console.WriteLine(binaryCiphertext);
            Console.WriteLine(binaryKey);
            // Thực hiện phép toán XOR để giải mã
            int decryptedValue = Convert.ToInt32(binaryCiphertext, 2) ^ Convert.ToInt32(binaryKey, 2);

            return decryptedValue;
        }
    }
}
