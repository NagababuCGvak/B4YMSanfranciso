using System.Data.Entity;

namespace Tar.Repository.Ef
{
    public interface IModelBuilder
    {
        void Build(DbModelBuilder modelBuilder);
    }
}
