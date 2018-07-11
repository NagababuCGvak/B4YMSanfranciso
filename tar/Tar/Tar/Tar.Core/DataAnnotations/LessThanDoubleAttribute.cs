using System;

namespace Tar.Core.DataAnnotations
{
    public class LessThanDoubleAttribute : LessThanBaseAttribute
    {

        public LessThanDoubleAttribute(double value)
            : base(value)
        {
        }
        public override bool IsValid(object value)
        {
            if (value == null) return false;

            return Convert.ToDouble(value) < Convert.ToDouble(Value);
        }
    }
}