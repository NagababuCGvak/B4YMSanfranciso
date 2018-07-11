using System.Globalization;
using System.Text.RegularExpressions;

namespace Tar.Core.Template
{
    public class NVelocityTemplateEngineFormatter
    {
        public string FormatInvariant(string format, params object[] parameters)
        {
            return string.Format(CultureInfo.InvariantCulture, format, parameters);
        }

        public string Format(string format, params object[] parameters)
        {
            return string.Format(CultureInfo.CurrentCulture, format, parameters);
        }
        public string FormatRegex(string pattern,string replacement, string parameter)
        {
            //Example Phone
            //replacement ="($1) $2-$3"
            //pattern = "(\\d{3})(\\d{3})(\\d{4})"
            return Regex.Replace(parameter, @pattern, replacement);
        }
    }
}