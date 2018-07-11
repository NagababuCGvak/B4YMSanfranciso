using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Tar.Repository.Ef;
using Tar.Tests.Model;

namespace Tar.Tests.Repository
{
    public class TestRepositoryModelBuilder:IModelBuilder
    {
        public void Build(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Product>().HasKey(p => p.Id)
                .Property(p => p.Id)
                .HasColumnName("ProductId");
            modelBuilder.Entity<Product>().Property(p => p.Name)
                .HasColumnName("ProductName");

//            modelBuilder.Configurations.Add(new TestMap());

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }

    //public class Test
    //{
    //    public int TestId { get; set; }
    //    public string TestName { get; set; }
    //}

    //public class TestMap : EntityTypeConfiguration<Test>
    //{
    //    public TestMap()
    //    {
    //        HasKey(t => t.TestId).Property(t => t.TestId).HasColumnName("TestNo");
    //        Property(t => t.TestName);
    //    }
    //}
}
