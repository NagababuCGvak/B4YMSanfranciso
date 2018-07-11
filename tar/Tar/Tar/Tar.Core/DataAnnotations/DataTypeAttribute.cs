using System.ComponentModel.DataAnnotations;

namespace Tar.Core.DataAnnotations
{
    public class DataTypeAttribute : System.ComponentModel.DataAnnotations.DataTypeAttribute
    {
        public DataTypeAttribute(DataType dataType) : base(dataType)
        {
        }

        public DataTypeAttribute(string customDataType) : base(customDataType)
        {
        }
    }
}