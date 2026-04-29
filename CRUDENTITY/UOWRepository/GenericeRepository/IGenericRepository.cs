using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.UOWRepository.GenericeRepository
{
    public interface IGenericRepository<T> where T : class
    {
        T? GetById(Int64 id);
        T? GetBySecureCode(string secureCode);
        IEnumerable<T> FindByPredicate(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll();
        IQueryable<T> GetAllQuerable();
        T Add(T entity);
        Task<T> AddAsync(T entity);
        void Update(T entity);
        Task UpdateAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);
        void RemoveRange(IEnumerable<T> entities);
        bool Delete(int id);
        IQueryable<T> GetAllSQL(string sql);
        IEnumerable<T> GetAllUsingSP(string proc);
        IEnumerable<T> GetAllSQLParamObjArray(string sql, object[] parameters);
        Task<T?> GetEntitySQLParamObjArray(string sql, object[] parameters);
        T GetSQLParamObject(string sql, params object[] parameters);
        int CreateWithSql(string sql, params object[] parameters);
        T? CreateWithSqlandWithReturn(string sql, params object[] parameters);
        int UpdateWithSql(string sql, params object[] parameters);
        bool DeleteWithSql(string sql, params object[] parameters);
       
        T? FindEntityByPredicate(Expression<Func<T, bool>> predicate);
        Task<T?> FindEntityByPredicateAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetListByPredicateAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> Query();
       
    }
}
