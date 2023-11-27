using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Crypto
{
    public static class Caesar
    {
        public static string EncryptCaesar(string plainText, int shift)
        {
            StringBuilder result = new StringBuilder();

            foreach (char c in plainText)
            {
                if (char.IsLetter(c))
                {
                    char baseChar = char.IsUpper(c) ? 'A' : 'a';
                    char shiftedChar = (char)(((c + shift - baseChar) % 26) + baseChar);
                    result.Append(shiftedChar);
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }


        public static string DecryptCaesar(string cipherText, int shift)
        {
            StringBuilder result = new StringBuilder();

            foreach (char c in cipherText)
            {
                if (char.IsLetter(c))
                {
                    char baseChar = char.IsUpper(c) ? 'A' : 'a';
                    char shiftedChar = (char)(((c - shift - baseChar + 26) % 26) + baseChar);
                    result.Append(shiftedChar);
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }
    }
}
