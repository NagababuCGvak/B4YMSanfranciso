using Better4You.Core.Repositories;

namespace Better4You.EntityModel
{
    public class LookupItem : IConfigEntity
    {
        public virtual long Id { get; set; }
        public virtual string FieldValue { get; set; }
        public virtual string FieldText { get; set; }
        public virtual int ItemOrder { get; set; }
        public virtual string Description { get; set; }
        public virtual LookupGroup LookupGroup { get; set; }
        public virtual bool IsActive { get; set; }
    }
}