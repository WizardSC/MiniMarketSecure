using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Crypto
{
    public static class Substitution
    {
        #region Substituion
        private static string originalAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string EncryptSubstitution(string input, string key)
        {
            StringBuilder result = new StringBuilder();

            foreach (char currentChar in input)
            {
                if (char.IsLetter(currentChar))
                {
                    int index = originalAlphabet.IndexOf(char.ToUpper(currentChar));
                    char substitutedChar = (index != -1) ? key[index] : currentChar;
                    result.Append(char.IsUpper(currentChar) ? substitutedChar : char.ToLower(substitutedChar));
                }
                else
                {
                    result.Append(currentChar);
                }
            }

            return result.ToString();
        }

        public static string DecryptSubstitution(string ciphertext, string key)
        {
            StringBuilder result = new StringBuilder();

            foreach (char currentChar in ciphertext)
            {
                if (char.IsLetter(currentChar))
                {
                    int index = key.IndexOf(char.ToUpper(currentChar));
                    char originalChar = (index != -1) ? originalAlphabet[index] : currentChar;
                    result.Append(char.IsUpper(currentChar) ? originalChar : char.ToLower(originalChar));
                }
                else
                {
                    result.Append(currentChar);
                }
            }

            return result.ToString();
        }

        #endregion
    }
}
