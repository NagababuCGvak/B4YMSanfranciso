using System;

namespace Tar.Core.DataAnnotations
{
    public class GreaterThanLongAttribute : GreaterThanBaseAttribute
    {
        public GreaterThanLongAttribute(long value)
            : base(value)
        {
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            return Convert.ToInt64(value) > Convert.ToInt64(Value);
        }
    }
}