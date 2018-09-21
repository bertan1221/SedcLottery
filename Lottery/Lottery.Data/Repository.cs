using Lottery.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery.Data
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        protected DbSet<T> dbSet;

        public Repository(DbContext dataContext)
        {
            dbSet = dataContext.Set<T>();
        }

        public void Insert(T entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return dbSet;
        }



    }
}
