using CRUDENTITY.DataContext;
using CRUDENTITY.Entity;
using CRUDENTITY.UOWRepository.GenericeRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.UOWRepository.EntityRepository
{
    public class VisitorRepository : GenericRepository<WebsiteVisitorLog>, IVisitorRepository
    {
        public VisitorRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }
        public Task AddVisitorAsync(WebsiteVisitorLog visitor)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalVisitorCountAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<int> TrackAndGetTotalVisitorsAsync(string sql, object[] parameters)
        {
            try
            {
                var result = await _appDbContext.Database.SqlQueryRaw<int>(sql, parameters).ToListAsync();


                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
