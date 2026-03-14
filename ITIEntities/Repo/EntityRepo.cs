using ITIEntities.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ITIEntities.Repo
{
    public interface IRepo<T>
    {
         void Add(T entity);
         void Update(T entity);
         void Delete(int d);
          T GetById(int id);
        IQueryable<T> GetQueryable();
        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
    }
    public class EntityRepo<T> : IRepo<T> where T : class
    {
        ITIContext _context;
        DbSet<T> _table;

        public EntityRepo(ITIContext context)
        {
            _context = context;
            _table = context.Set<T>();
        }



        public void Add(T entity)
        {
            _table.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if(entity != null)
            {
                _table.Remove(entity);
                _context.SaveChanges();
            }
        }


        public T GetById(int id)
        {
            return _table.Find(id);
        }

        public void Update(T entity)
        {
            _table.Update(entity);
            _context.SaveChanges();
        }

        IQueryable<T> IRepo<T>.GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _table;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query; 
        }
        public IQueryable<T> GetQueryable()
        {
            return _table.AsQueryable();
        }
    }
}
