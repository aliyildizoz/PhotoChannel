using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Core.Utilities.Security.Jwt;
using Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Core.DataAccess.EntityFramework
{
    public abstract class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
       where TEntity : class, IEntity, new()
       where TContext : DbContext, new()
    {
        private TContext Context { get; }

        public EfEntityRepositoryBase(TContext context)
        {
            Context = context;
        }
        public EfEntityRepositoryBase()
        {
        }
        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            return Context.Set<TEntity>().SingleOrDefault(filter);
        }
        public IList<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null ? Context.Set<TEntity>().ToList() : Context.Set<TEntity>().Where(filter).ToList();
        }

        public bool Contains(TEntity entity)
        {
            return Context.Set<TEntity>().Contains(entity);
        }

        public virtual void Add(TEntity entity)
        {
            var addedEntity = Context.Entry(entity);
            addedEntity.State = EntityState.Added;
            Context.SaveChanges();
        }
        public virtual void Update(TEntity entity)
        {
            var updatedEntity = Context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            Context.SaveChanges();
        }

        public virtual void Delete(TEntity entity)
        {
            var deletedEntity = Context.Entry(entity);
            deletedEntity.State = EntityState.Deleted;
            Context.SaveChanges();
        }
    }
}

