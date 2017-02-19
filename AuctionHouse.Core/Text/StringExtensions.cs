using System.Linq;

namespace AuctionHouse.Core.Text
{
    public static class StringExtensions
    {
        public static string LowerCaseFirstLetter(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            return text.First().ToString().ToLower() + text.Substring(1);
        }
    }
}
