using System;

namespace Tar.Model
{
    public interface IDeletableObject
    {
        bool IsDeleted { get; set; }
        DateTime DeletedDate { get; set; }
    }
}
