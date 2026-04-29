using CRUDENTITY.Domain;
using CRUDENTITY.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICEAPP.ServiceLayer
{
    public interface INotificationService
    {
         Task<NotificationDataModel> GetNotificationData(string LinkName);
    }
}
