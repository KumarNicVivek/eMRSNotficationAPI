using CRUDENTITY.UOWRepository;
using CRUDENTITY.UOWRepository.EntityRepository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICEAPP.ServiceLayer
{
    public class VisitorService : IVisitorService
    {
        private readonly IUnitOfWork _unitOfWork;
        public VisitorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> TrackVisitorAsync(HttpContext context)
        {
            int resultCount = 0;
            string IpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            string userAgent = context.Request.Headers["User-Agent"].ToString() ?? "Unknown";

            try
            {
                var repos = _unitOfWork.GetRepository<IVisitorRepository>();

                if (repos == null)
                {
                    resultCount = 0;
                    return resultCount;
                }

                var pageVistior = await repos.TrackAndGetTotalVisitorsAsync("EXEC SSP_TrackWebsiteVisitor @IpAddress, @UserAgent, @VisitDate",
                    new object[]
                    {
                        new Microsoft.Data.SqlClient.SqlParameter("@IpAddress", IpAddress),
                        new Microsoft.Data.SqlClient.SqlParameter("@UserAgent", userAgent),
                        new Microsoft.Data.SqlClient.SqlParameter("@VisitDate", DateTime.UtcNow.Date)
                    });
                resultCount = pageVistior;
                return resultCount;
            }
            catch (Exception ex)
            {
                resultCount = 0;
                throw ex;
            }


        }
    }
}
