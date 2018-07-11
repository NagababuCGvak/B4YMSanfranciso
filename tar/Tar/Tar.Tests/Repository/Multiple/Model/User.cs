using Tar.Tests.Repository.Multiple.Repository;

namespace Tar.Tests.Repository.Multiple.Model
{
    public class User : IConfigEntity
    {
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
    }
}