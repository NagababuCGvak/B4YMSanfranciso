using System;
using AutoMapper;
using Tar.Model;
using Tar.ViewModel;

namespace Tar.Business
{
    public static class MappingExtension
    {
        public static TEntity ToEntity<TEntity>(this IView view)
            where TEntity : IEntity
        {
            if (view == null) throw new ArgumentNullException("view");
            return (TEntity)Mapper.Map(view, view.GetType(), typeof(TEntity));
        }

        public static TView ToView<TView>(this IEntity entity)
            where TView : IView
        {
            if (entity == null) throw new ArgumentNullException("entity");
            return (TView)Mapper.Map(entity, entity.GetType(), typeof(TView));
        }

        public static TEntity ToEntity<TEntity>(this IEntity entity)
            where TEntity : IEntity
        {
            return (TEntity)Mapper.Map(entity, entity.GetType(), typeof(TEntity));
        }

        public static TView ToView<TView>(this IView view)
            where TView : IView
        {
            return (TView)Mapper.Map(view, view.GetType(), typeof(TView));
        }

        public static void SetTo(this IEntity entity, IView view)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            if (view == null) throw new ArgumentNullException("view");
            Mapper.Map(entity, view, entity.GetType(), view.GetType());
        }

        public static void SetTo(this IView view, IEntity entity)
        {
            if (view == null) throw new ArgumentNullException("view");
            if (entity == null) throw new ArgumentNullException("entity");
            Mapper.Map(view, entity, view.GetType(), entity.GetType());
        }

        public static void SetTo(this IEntity entity, IEntity entity2)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            if (entity2 == null) throw new ArgumentNullException("entity2");
            Mapper.Map(entity, entity2, entity.GetType(), entity2.GetType());
        }

        public static void SetTo(this IView view, IView view2)
        {
            if (view == null) throw new ArgumentNullException("view");
            if (view2 == null) throw new ArgumentNullException("view2");
            Mapper.Map(view, view2, view.GetType(), view2.GetType());
        }
    }
}
