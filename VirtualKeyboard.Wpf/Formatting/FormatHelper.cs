using System.Text.RegularExpressions;

namespace VirtualKeyboard.Wpf
{
    internal static class FormatHelper
    {
        private static readonly Regex _regexDecimal = new Regex("^(-|\\+)?[0-9]*\\.?[0-9]*$");
        private static readonly Regex _regexInteger = new Regex("^(-|\\+)?[0-9]*$");

        public static bool Match(this Format format, string text)
        {
            switch (format)
            {
                case Format.Decimal:
                    return _regexDecimal.IsMatch(text);
                case Format.Integer:
                    return _regexInteger.IsMatch(text);
                case Format.Alphabet:
                    return true;
                default:
                    return false;
            }
        }
    }
}