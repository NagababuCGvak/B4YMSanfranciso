using System;
using System.Linq;

namespace Tar.Core.Extensions
{
    public static class EnumExtensions
    {
        public static T ToEnum<T>(this string val)
        {
            var values = Enum.GetValues(typeof(T)).Cast<T>().Where(
                c => c.ToString().ToLowerInvariant() == val.ToLowerInvariant()).ToList();

            if (values.Count == 0)
                throw new Exception(string.Format("{0} saptanamadı! Value:{1}", typeof (T).Name, val));

            return values[0];
        }

        public static T ToEnum<T>(this int val)
        {
            var values = Enum.GetValues(typeof (T)).Cast<T>().Where(
                c => Convert.ToInt32(c) == Convert.ToInt32(val)).ToList();

            if (values.Count == 0)
                throw new Exception(string.Format("{0} saptanamadı! Value:{1}", typeof(T).Name, val));
            return values[0];
        }

        public static T ToEnum<T>(this object val)
        {
            var values = Enum.GetValues(typeof (T)).Cast<T>().Where(
                c => Convert.ToInt32(c) == Convert.ToInt32(val)).ToList();

            if (values.Count == 0)
                throw new Exception(string.Format("{0} saptanamadı! Value:{1}", typeof(T).Name, val));
            return values[0];
        }
    }
}
