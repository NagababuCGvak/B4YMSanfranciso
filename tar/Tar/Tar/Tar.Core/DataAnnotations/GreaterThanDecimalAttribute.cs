using System;

namespace Tar.Core.DataAnnotations
{
    public class GreaterThanDecimalAttribute : GreaterThanBaseAttribute
    {
        public GreaterThanDecimalAttribute(decimal value)
            : base(value)
        {
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            return Convert.ToDecimal(value) > Convert.ToDecimal(Value);
        }
    }
}