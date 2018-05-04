using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace StudentsManagement.Persistence.EF
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            Context = context;
        }

        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public T GetEntity(int id)
        {
            return Context.Set<T>().Find(id); 
        }

        public virtual IEnumerable<T> ListAll()
        {
            return Context.Set<T>().ToList();
        }
    }
}
