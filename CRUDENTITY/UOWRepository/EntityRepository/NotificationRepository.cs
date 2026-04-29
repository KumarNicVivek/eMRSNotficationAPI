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
    public class NotificationRepository : GenericRepository<WebLinkTempEntity>, INotificationRepository
    {
        public NotificationRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
        public async Task<NotificationDataEntity> GetNotificationData(string sqlQuery, object[] param)
        {
            try
            {
                var notificationData = await _appDbContext.Database
                                            .SqlQueryRaw<NotificationDataEntity>(sqlQuery, param)
                                            .ToListAsync();

                if (notificationData == null || notificationData.Count == 0) {
                    return null;
                }

                //if (notificationData != null) {
                //    notificationData.Details = notificationData.Details?.Replace("\n", "<br/>");
                //}
                return notificationData.First();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
