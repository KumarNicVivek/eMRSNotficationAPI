using CRUDENTITY.Domain;
using CRUDENTITY.UOWRepository;
using CRUDENTITY.UOWRepository.EntityRepository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SERVICEAPP.ServiceLayer
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<NotificationDataModel> GetNotificationData(string LinkName = "")
        {
            var notificationRepository = _unitOfWork.GetRepository<INotificationRepository>();

            if(notificationRepository == null)
            {
                throw new Exception("Notification repository not found.");
            }
                       
            var sqlQuery = "EXECUTE dbo.SSP_GetLinkNameWiseData @LinkName";

            var param = new[]
            {
                    new SqlParameter("LinkName", LinkName),
                   
            };
            var notificationDataEntity = await notificationRepository.GetNotificationData(sqlQuery, param);

            // 👉 If no data, return null
            if (notificationDataEntity == null)
                return null;

            return new NotificationDataModel
            {
                LinkTempId = notificationDataEntity.LinkTempId,
                Title = notificationDataEntity.Title,
                Details = WebUtility.HtmlDecode(notificationDataEntity.Details), //?.Replace("\n", "<br/>"),
                LinkLevel = notificationDataEntity.LinkLevel,
                PublishDate = notificationDataEntity.PublishDate,
                PublishOn = notificationDataEntity.PublishOn
            };
        }
    }
}
