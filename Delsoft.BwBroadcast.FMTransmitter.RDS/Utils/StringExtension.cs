using System.Globalization;
using System.Linq;
using System.Text;

namespace Delsoft.BwBroadcast.FMTransmitter.RDS.Utils
{
    public static class StringExtension
    {
        public static string CleanAccent(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in 
                from c in normalizedString 
                let unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c) 
                where unicodeCategory != UnicodeCategory.NonSpacingMark 
                select c)
            {
                stringBuilder.Append(c);
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}