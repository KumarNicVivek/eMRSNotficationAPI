using CRUDENTITY.UOWRepository.GenericeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.UOWRepository
{
    public interface IUnitOfWork :IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;
        T GetRepository<T>() where T : class;
        void save();
        Task SaveAsync(); // For Async Operation
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
