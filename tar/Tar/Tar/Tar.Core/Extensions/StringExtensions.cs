using System.Text.RegularExpressions;

namespace Tar.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidMail(this string source)
        {
            var regex = new Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
            return regex.IsMatch(source);
        }
    }
}
