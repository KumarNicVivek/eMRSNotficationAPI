using CRUDENTITY.Domain;
using CRUDENTITY.Entity;
using CRUDENTITY.UOWRepository.GenericeRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.UOWRepository.EntityRepository
{
    public interface IStudentAppointmentRepository : IGenericRepository<StudentAppointment>
    {
        Task BulkInsertAsync(DataTable table);
        Task<HashSet<string>> GetExistingUniqueIdsAsync(List<string> ids, int Year);
        Task AddUploadLogAsync(StudentAppointmentUploadLog log);
        Task<List<StudentAppointmentUploadLog>> GetUploadLogsAsync();
        Task<List<StudentAppointment>> GetStudentsByYearAsync(int year);
        Task<List<StudentAppointment>> GetStudentsByIdsAsync(List<long> ids);
        Task MarkPdfGeneratedAsync(long studentId);
        Task MarkPdfGeneratedBulkAsync(List<long> studentIds);
        Task<List<StudentAppointment>> GetGeneratedPdfCandidates(int year);
        Task MarkPdfSignedBulkAsync(List<long> ids);
        Task<List<StudentAppointment>> GetStudentsdfGenByYearAsync(int year);
        Task<Dictionary<int, int>> GetStudentCountGroupedByYearAsync();
        Task MarkPdfSignedBulk(List<string> uniqueIds);
        
    }
}
