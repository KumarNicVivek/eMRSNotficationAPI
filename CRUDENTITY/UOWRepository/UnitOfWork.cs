using CRUDENTITY.DataContext;
using CRUDENTITY.UOWRepository.GenericeRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.UOWRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, object> _repositories = new();
        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(AppDbContext appDbContext, IServiceProvider serviceProvider)
        {
            _appDbContext = appDbContext;
            _serviceProvider = serviceProvider;
        }
        public void save()
        {
            try
            {
                _appDbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_appDbContext != null)
                {
                    _appDbContext.Dispose();
                }
            }
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);

            if (_repositories.ContainsKey(type))
                return (IGenericRepository<T>)_repositories[type];

            // Resolve repository from DI (uses open generic)
            var repository = _serviceProvider.GetRequiredService<IGenericRepository<T>>();

            _repositories[type] = repository;
            return repository;
        }

        public T GetRepository<T>() where T : class
        {
            var type = typeof(T);

            if (_repositories.ContainsKey(type))
                return (T)_repositories[type];

            var repo = _serviceProvider.GetRequiredService<T>();
            _repositories[type] = repo;
            return repo;
        }

        public async Task SaveAsync()
        {
            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }

        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction == null)
                _currentTransaction = await _appDbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.CommitAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }
}
