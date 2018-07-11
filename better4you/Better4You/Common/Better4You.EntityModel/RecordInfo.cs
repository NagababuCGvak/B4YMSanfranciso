using System;
using Tar.Model;

namespace Better4You.EntityModel
{
    public class RecordInfo:IEntity
    {        
        public virtual DateTime CreatedDate { get; set; }
        public virtual int CreatedBy { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual string ModifiedReason { get; set; }
        public virtual RecordStatus RecordStatus { get; set; }        
    }
}