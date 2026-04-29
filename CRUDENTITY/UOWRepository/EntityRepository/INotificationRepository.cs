using CRUDENTITY.Entity;
using CRUDENTITY.UOWRepository.GenericeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.UOWRepository.EntityRepository
{
    public interface INotificationRepository : IGenericRepository<WebLinkTempEntity>
    {
        Task<NotificationDataEntity> GetNotificationData(string sqlQuery, object[] param);
    }
}
