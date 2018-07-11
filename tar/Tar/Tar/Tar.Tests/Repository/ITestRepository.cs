using System;
using Tar.Model;
using Tar.Repository;

namespace Tar.Tests.Repository
{
    public interface ITestRepository<T> : IRepository<T> where T : class ,IEntity
    {
        Guid Id { get; set; }
    }
}
