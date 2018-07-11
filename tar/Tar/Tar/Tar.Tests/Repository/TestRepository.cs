using System;
using Tar.Model;

namespace Tar.Tests.Repository
{
    public class TestRepository<T> : Tar.Repository.General.GeneralRepository<T>, ITestRepository<T> 
        where T : class, IEntity
    {
        public TestRepository(string innerRepositoryKey) : base(innerRepositoryKey)
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
    }
}