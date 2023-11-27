using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Crypto
{
    public static class Affine
    {
        public static string EncryptAffine(string plainText, int a, int b)
        {
            StringBuilder encryptedText = new StringBuilder();

            foreach (char plainChar in plainText)
            {
                if (char.IsLetter(plainChar))
                {
                    char baseChar = char.IsUpper(plainChar) ? 'A' : 'a';
                    int encryptedChar = (a * (plainChar - baseChar) + b) % 26 + baseChar;
                    encryptedText.Append((char)encryptedChar);
                }
                else
                {
                    encryptedText.Append(plainChar);
                }
            }

            return encryptedText.ToString();
        }

        public static string DecryptAffine(string cipherText, int a, int b)
        {
            StringBuilder decryptedText = new StringBuilder();

            int modInverse = ModInverseAffine(a, 26);

            foreach (char cipherChar in cipherText)
            {
                if (char.IsLetter(cipherChar))
                {
                    char baseChar = char.IsUpper(cipherChar) ? 'A' : 'a';
                    int decryptedChar = (modInverse * (cipherChar - baseChar - b + 26)) % 26 + baseChar;
                    decryptedText.Append((char)decryptedChar);
                }
                else
                {
                    decryptedText.Append(cipherChar);
                }
            }

            return decryptedText.ToString();
        }

        private static int ModInverseAffine(int a, int m)
        {
            for (int x = 1; x < m; x++)
            {
                if ((a * x) % m == 1)
                {
                    return x;
                }
            }

            throw new Exception("Modular inverse does not exist.");
        }
    }
}
