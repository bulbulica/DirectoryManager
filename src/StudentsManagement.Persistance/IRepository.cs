using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentsManagement.Persistence
{
    public interface IRepository<TEntity> where TEntity : class
    {

        IEnumerable<TEntity> ListAll();
        TEntity GetEntity(int id);

        void Add(TEntity entity);

        void Delete(TEntity entity);
        
    }
}
