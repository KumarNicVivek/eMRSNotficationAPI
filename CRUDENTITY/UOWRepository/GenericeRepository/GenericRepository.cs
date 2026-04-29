using CRUDENTITY.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.UOWRepository.GenericeRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _appDbContext;
        private DbSet<T> _entities;
        public GenericRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _entities = _appDbContext.Set<T>();
        }
        public T Add(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public int CreateWithSql(string sql, params object[] parameters)
        {
            var result = _appDbContext.Database.ExecuteSqlRaw(sql, parameters);
            return result;
        }

        public T? CreateWithSqlandWithReturn(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteWithSql(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FindByPredicate(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public T? FindEntityByPredicate(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<T?> FindEntityByPredicateAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAllQuerable()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAllSQL(string sql)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAllSQLParamObjArray(string sql, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAllUsingSP(string proc)
        {
            throw new NotImplementedException();
        }

        public T? GetById(long id)
        {
            throw new NotImplementedException();
        }

        public T? GetBySecureCode(string secureCode)
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetEntitySQLParamObjArray(string sql, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetListByPredicateAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public T GetSQLParamObject(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Query()
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public int UpdateWithSql(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
