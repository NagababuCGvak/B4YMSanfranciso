using System;

namespace Tar.Core.DataAnnotations
{
    public class GreaterThanIntAttribute : GreaterThanBaseAttribute
    {
        public GreaterThanIntAttribute(int value)
            : base(value)
        {
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            return Convert.ToInt32(value) > Convert.ToInt32(Value);
        }
    }
}
