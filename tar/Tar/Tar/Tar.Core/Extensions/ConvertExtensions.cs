using System;
using System.Text;
using Tar.Core.Serialization;

namespace Tar.Core.Extensions
{
    public static class ConvertExtensions
    {
        public static Int32 ToInt32(this Object source)
        {
            return Convert.ToInt32(source);
        }

        public static Int64 ToInt64(this Object source)
        {
            return Convert.ToInt64(source);
        }

        public static Int16 ToInt16(this Object source)
        {
            return Convert.ToInt16(source);
        }

        public static Decimal ToDecimal(this object source)
        {
            return Convert.ToDecimal(source);
        }

        public static Double ToDouble(this object source)
        {
            return Convert.ToDouble(source);
        }

        public static T To<T>(object source)
        {
            return (T) source;
        }

        public static string ToJson(this object source)
        {
            return JsonSerializer.Instance.Serialize(source);
        }

        public static void ToJson(this object source, StringBuilder output)
        {
            output.Append(source.ToJson());
        }

        public static T FromJson<T>(this string source)
        {
            return JsonSerializer.Instance.Deserialize<T>(source);
        }
    }
}
