using System;

namespace Tar.Repository
{
    public class NotFoundRecordException:ApplicationException
    {
        public string EntityName { get; set; }

        public NotFoundRecordException(string entityName)
        {
            EntityName = entityName;
        }
    }
}
