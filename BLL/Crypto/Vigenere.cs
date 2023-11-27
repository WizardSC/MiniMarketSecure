using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Crypto
{
    public static class Vigenere
    {
        public static string EncryptVigenere(string plainText, string key)
        {
            StringBuilder encryptedText = new StringBuilder();
            int keyLength = key.Length;

            for (int i = 0, j = 0; i < plainText.Length; i++)
            {
                char plainChar = plainText[i];

                if (char.IsLetter(plainChar))
                {
                    char baseChar = char.IsUpper(plainChar) ? 'A' : 'a';
                    char keyChar = char.ToUpper(key[j % keyLength]);
                    int shift = keyChar - 'A';
                    int encryptedChar = (plainChar - baseChar + shift + 26) % 26 + baseChar;
                    encryptedText.Append((char)encryptedChar);
                    j = (j + 1) % keyLength;
                }
                else
                {
                    encryptedText.Append(plainChar);
                }
            }

            return encryptedText.ToString();
        }

        public static string DecryptVigenere(string cipherText, string key)
        {
            StringBuilder decryptedText = new StringBuilder();
            int keyLength = key.Length;

            for (int i = 0, j = 0; i < cipherText.Length; i++)
            {
                char cipherChar = cipherText[i];

                if (char.IsLetter(cipherChar))
                {
                    char baseChar = char.IsUpper(cipherChar) ? 'A' : 'a';
                    char keyChar = char.ToUpper(key[j % keyLength]);
                    int shift = keyChar - 'A';
                    int decryptedChar = (cipherChar - baseChar - shift + 26) % 26 + baseChar;
                    decryptedText.Append((char)decryptedChar);
                    j = (j + 1) % keyLength;
                }
                else
                {
                    decryptedText.Append(cipherChar);
                }
            }

            return decryptedText.ToString();
        }

    }
}
