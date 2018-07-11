using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Objects;
using System.Linq;
using Tar.Core;
using Tar.Model;

namespace Tar.Repository.Ef
{
    public class EfDbContext : DbContext
    {
        public ObjectContext ObjectContext
        {
            get { return ((IObjectContextAdapter)this).ObjectContext; }
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            ServiceLocator.Current.GetAll<IModelBuilder>().ToList().ForEach(item => item.Build(modelBuilder));
            base.OnModelCreating(modelBuilder);
        }
        private readonly Dictionary<Type, object> _objectSets = new Dictionary<Type, object>();
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            if (!_objectSets.ContainsKey(typeof(TEntity)))
            {
                var objectSet = ObjectContext.CreateObjectSet<TEntity>();
                _objectSets.Add(typeof(TEntity), objectSet);
            }
            return base.Set<TEntity>();
        }

        public EfDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Database.SetInitializer<EfDbContext>(null);
        }

        public ObjectSet<T> CreateObjectSet<T>() where T : class ,IEntity
        {
            return ObjectContext.CreateObjectSet<T>();
        }
    }
}