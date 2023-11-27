using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Crypto
{
    public static class Hill
    {
        public static string EncryptHill(string plaintext, int[,] key)
        {
            if (plaintext.Length % 2 != 0)
            {
                plaintext += 'X';
            }
            StringBuilder encryptedText = new StringBuilder();

            for (int i = 0; i < plaintext.Length; i += 2)
            {
                if (char.IsLetter(plaintext[i]) && char.IsLetter(plaintext[i + 1]))
                {
                    char encryptedChar1 = GetChar(EncryptChar(GetCharValue(plaintext[i]), GetCharValue(plaintext[i + 1]), key, 0));
                    char encryptedChar2 = GetChar(EncryptChar(GetCharValue(plaintext[i]), GetCharValue(plaintext[i + 1]), key, 1));

                    if (char.IsUpper(plaintext[i]))
                        encryptedChar1 = char.ToUpper(encryptedChar1);
                    if (char.IsUpper(plaintext[i + 1]))
                        encryptedChar2 = char.ToUpper(encryptedChar2);

                    encryptedText.Append(encryptedChar1);
                    encryptedText.Append(encryptedChar2);
                }
                else
                {
                    encryptedText.Append(plaintext[i]);
                    encryptedText.Append(plaintext[i + 1]);
                }
            }
            // Loại bỏ ký tự 'X' được thêm vào cuối chuỗi khi mã hóa
            if (encryptedText[encryptedText.Length - 1] == 'X')
            {
                encryptedText.Remove(encryptedText.Length - 1, 1);
            }
            return encryptedText.ToString();

        }
        public static string DecryptHill(string ciphertext, int[,] key)
        {
            if (ciphertext.Length % 2 != 0)
            {
                ciphertext += 'X';
            }
            StringBuilder decryptedText = new StringBuilder();

            int[,] inverseKey = GetInverseKey(key);

            for (int i = 0; i < ciphertext.Length; i += 2)
            {
                if (char.IsLetter(ciphertext[i]) && char.IsLetter(ciphertext[i + 1]))
                {
                    char decryptedChar1 = GetChar(DecryptChar(GetCharValue(ciphertext[i]), GetCharValue(ciphertext[i + 1]), inverseKey, 0));
                    char decryptedChar2 = GetChar(DecryptChar(GetCharValue(ciphertext[i]), GetCharValue(ciphertext[i + 1]), inverseKey, 1));

                    if (char.IsUpper(ciphertext[i]))
                        decryptedChar1 = char.ToUpper(decryptedChar1);
                    if (char.IsUpper(ciphertext[i + 1]))
                        decryptedChar2 = char.ToUpper(decryptedChar2);

                    decryptedText.Append(decryptedChar1);
                    decryptedText.Append(decryptedChar2);
                }
                else
                {
                    decryptedText.Append(ciphertext[i]);
                    decryptedText.Append(ciphertext[i + 1]);
                }
            }
            if (decryptedText[decryptedText.Length - 1] == 'X')
            {
                decryptedText.Remove(decryptedText.Length - 1, 1);
            }
            return decryptedText.ToString();
        }
        #region Bổ trợ
        static int EncryptChar(int char1, int char2, int[,] key, int column)
        {
            return key[column, 0] * char1 + key[column, 1] * char2;
        }

        static int DecryptChar(int char1, int char2, int[,] key, int row)
        {
            return key[row, 0] * char1 + key[row, 1] * char2;
        }

        static int GetCharValue(char c)
        {
            return char.IsUpper(c) ? c - 'A' : c - 'a';
        }

        static char GetChar(int value)
        {
            return (char)((value % 26 + 26) % 26 + 'a');
        }

        static int[,] GetInverseKey(int[,] key)
        {
            int determinant = key[0, 0] * key[1, 1] - key[0, 1] * key[1, 0];
            int determinantInverse = ModInverseHill(determinant, 26);

            int[,] inverseKey = {
            { key[1, 1], -key[0, 1] },
            { -key[1, 0], key[0, 0] }
        };

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    inverseKey[i, j] = Mod(inverseKey[i, j] * determinantInverse, 26);
                }
            }

            return inverseKey;
        }

        static int ModInverseHill(int a, int m)
        {
            a = Mod(a, m);
            for (int x = 1; x < m; x++)
            {
                if (Mod(a * x, m) == 1)
                    return x;
            }
            throw new Exception("Modular inverse does not exist.");
        }

        static int Mod(int a, int m)
        {
            return (a % m + m) % m;
        }
        #endregion
    }
}

