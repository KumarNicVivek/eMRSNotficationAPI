using CRUDENTITY.Entity;
using CRUDENTITY.UOWRepository.GenericeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.UOWRepository.EntityRepository
{
    public interface IVisitorRepository : IGenericRepository<WebsiteVisitorLog>
    {
        Task AddVisitorAsync(WebsiteVisitorLog visitor);
        Task<int> GetTotalVisitorCountAsync();

        Task<int> TrackAndGetTotalVisitorsAsync(string sql, object[] parameters);
    }
}
