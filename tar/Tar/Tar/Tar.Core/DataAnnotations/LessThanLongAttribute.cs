using System;

namespace Tar.Core.DataAnnotations
{
    public class LessThanLongAttribute : LessThanBaseAttribute
    {
        public LessThanLongAttribute(long value)
            : base(value)
        {
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            return Convert.ToInt64(value) < Convert.ToInt64(Value);
        }
    }
}