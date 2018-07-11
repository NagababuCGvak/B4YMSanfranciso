using System;

namespace Tar.Core.DataAnnotations
{
    public class LessThanDecimalAttribute : LessThanBaseAttribute
    {
        public LessThanDecimalAttribute(decimal value)
            : base(value)
        {
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            return Convert.ToDecimal(value) < Convert.ToDecimal(Value);
        }
    }
}