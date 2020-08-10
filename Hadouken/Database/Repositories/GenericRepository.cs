using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hadouken.Database.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly HadoukenContext Context;

        public GenericRepository(HadoukenContext context)
        {
            Context = context;
        }

        public virtual IQueryable<T> All()
        {
            try
            {
                return Context.Set<T>();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual IEnumerable<T> Where(Func<T, bool> query, Func<T, bool> orderBy = null)
        {
            var result = Context.Set<T>().Where(query);

            if (orderBy != null)
            {
                result = result.OrderBy(orderBy);
            }

            return result;
        }

        public virtual T GetById(long id)
        {
            return Context.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public virtual T Create(T entity)
        {
            Context.Set<T>().Add(entity);
            Context.SaveChanges();

            return entity;
        }

        public virtual bool Update(long id, T entity)
        {
            Context.Set<T>().Update(entity);

            return Context.SaveChanges() > 0;
        }

        public virtual bool Update(T entity, Expression<Func<T, object>>[] properties)
        {
            var entry = Context.Set<T>().Attach(entity);

            foreach (var property in properties)
            {
                entry.Property(property).IsModified = true;
            }

            return Context.SaveChanges() > 0;
        }

        public virtual bool Delete(long id)
        {
            var entity = GetById(id);

            if (entity == null)
            {
                return false;
            }

            return Delete(entity);
        }

        public virtual bool Delete(T entity)
        {
            Context.Set<T>().Remove(entity);

            return Context.SaveChanges() > 0;
        }
    }
}