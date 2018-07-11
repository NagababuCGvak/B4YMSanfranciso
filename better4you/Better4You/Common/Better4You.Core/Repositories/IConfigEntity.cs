using Tar.Model;

namespace Better4You.Core.Repositories
{
    public interface IConfigEntity : IMultipleRepositoryEntity
    {
        long Id { get; set; }
    }
}