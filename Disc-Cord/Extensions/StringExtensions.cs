namespace Disc_Cord.Extensions
{
    public static class StringExtensions
    {
        public static string LimitLength(this string source, int maxLength)
        {
            if (source != null)
            {
                if (source.Length <= maxLength)
                {
                    return source;
                }

                return source.Substring(0, maxLength) + "...";
            }
            return source;
        }
    }
}
