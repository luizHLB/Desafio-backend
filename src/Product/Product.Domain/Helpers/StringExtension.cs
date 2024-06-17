using System.Text.RegularExpressions;

namespace Product.Domain.Helpers
{
    public static class StringExtension
    {
        public static string OnlyAlphaNumeric(this string text) => string.IsNullOrEmpty(text)
            ? string.Empty
            : Regex.Replace(text, @"(\W+)?", string.Empty);
        public static string OnlyDigits(this string text) => string.IsNullOrEmpty(text)
            ? string.Empty
            : Regex.Replace(text, @"(\D+)?", string.Empty);
    }
}
