using CRUDENTITY.Domain;
using CRUDENTITY.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICEAPP.ServiceLayer
{
    public interface IStudentAppointmentService
    {
        Task<Int64> SaveUploadLogAsync(StudentAppointmentUploadLogModel log);
        Task MarkPdfGeneratedAsync(long studentId);
        Task MarkPdfGeneratedBulkAsync(List<long> studentIds);
        Task<List<StudentAppointmentUploadLogModel>> GetUploadLogsAsync();
        Task<(string message, byte[] errorFile)> UploadExcelAsync(IFormFile file, long userId, int AppointmentYear);
        Task<(List<StudentAppointmentModel>, int TotalRecord)> GetStudentsByYearAsync(int year, int page, int pageSize);
        Task<(List<StudentAppointmentModel>, int totalRecord)> GetStudentsAppLtrPdfGenByYearAsync(int year, int page, int pageSize);
        Task<List<StudentAppointmentModel>> GetStudentsByIdsAsync(List<long> ids);
        Task<List<StudentAppointmentModel>> GetGeneratedPdfCandidates(int year);
        Task<(int successCount, int failedCount)> BulkSignPdfAsync(List<long> ids, int year, string contentRootPath, string certPath, string password, string loginName, string role);
        Task<int> GetStudentsCountByYearPdfGenrated(int year);
        Task<Dictionary<int, int>> GetStudentsCountByYearPdfGeneratedBulkAsync();
        void SignSinglePdf(long id, int year, string baseFolder, string certPath, string password, string loginName, string role);
        Task MarkPdfSignedBulkAsync(List<string> studentIds);
    }
}
