using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Crypto
{
    public static class StringHelper
    {
        public static string RemoveDiacritics(string text)
        {

            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder result = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (category != UnicodeCategory.NonSpacingMark)
                {
                    if (c == 'đ' || c == 'Đ')
                    {
                        result.Append(c == 'đ' ? 'd' : 'D');
                    }
                    else
                    {
                        result.Append(c);
                    }
                }
            }

            return result.ToString();
        }
    }
}
