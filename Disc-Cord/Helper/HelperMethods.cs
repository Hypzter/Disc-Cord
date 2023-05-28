using System.Text.RegularExpressions;

namespace Disc_Cord.Helper
{
    public class HelperMethods
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public static string CensorText(string text)
        {
            string[] badWords = new string[] { "jävla", "jävel", "fan", "fanskap", "idiot", "satmara" };

            string censoredText = text;
            foreach (string word in badWords)
            {
                string pattern = $"({word})";
                censoredText = Regex.Replace(censoredText, pattern, "****", RegexOptions.IgnoreCase);
            }
            return censoredText;
        }

    }
}
