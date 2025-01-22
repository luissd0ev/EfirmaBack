using APIEfirma.Models;
using Microsoft.EntityFrameworkCore;

namespace APIEfirma.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected EfirmaDbContext context;
        protected DbSet<T> _dbSet;
        private object _objectResult;
        public Repository(EfirmaDbContext context)
        {
            this.context = context;
            _dbSet = context.Set<T>();
        }

        /* Accesors */
        public object GetObjectResult()
        {
            return this._objectResult;
        }

        public IEnumerable<T>? GetAll()
        {
            IEnumerable<T>? lstOrg;

            //Obtener todos los registros
            var allRecords = _dbSet.ToList();
            try
            {
                lstOrg = allRecords;
            }
            catch (Exception ex)
            {
                lstOrg = null;
            }
            return lstOrg;
        }

        public int Insert(T entity)
        {
            int result = 0;
            try
            {
                _dbSet.Add(entity);
                context.SaveChanges();
                result = 1;
            }
            catch (Exception ex)
            {
                result = 1;
            }
            result = 1;

            return result;
        }

        public T? Get(params object[] keyValues)
        {
            T? entity = null;
            try
            {
                entity = _dbSet.Find(keyValues);
            }
            catch (Exception)
            {
              
            }

            return entity;
        }
    }
}
