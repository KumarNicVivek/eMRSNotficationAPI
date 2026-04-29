using CRUDENTITY.DataContext;
using CRUDENTITY.Domain;
using CRUDENTITY.Entity;
using CRUDENTITY.UOWRepository.GenericeRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.UOWRepository.EntityRepository
{
    public class StudentAppointmentRepository : GenericRepository<StudentAppointment>, IStudentAppointmentRepository
    {
        public StudentAppointmentRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }

        public async Task<HashSet<string>> GetExistingUniqueIdsAsync(List<string> ids, int Year)
        {
            if (ids == null || !ids.Any())
                return new HashSet<string>();

            var existingIds = await _appDbContext.StudentAppointment
                .Where(x => ids.Contains(x.UniqueId) && x.AppointmentYear == Year)
                .Select(x => x.UniqueId)
                .ToListAsync();

            return existingIds.ToHashSet();
        }
        public async Task BulkInsertAsync(DataTable table)
        {
            var connection = (SqlConnection)_appDbContext.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            using var bulk = new SqlBulkCopy(connection)
            {
                DestinationTableName = "T_StudentAppointment", // ✅ FIXED
                BatchSize = 5000
            };

            // ✅ Column Mapping (must match DB column names)
            bulk.ColumnMappings.Add("UniqueId", "UniqueId");
            bulk.ColumnMappings.Add("RollNo", "RollNo");
            bulk.ColumnMappings.Add("CandidateName", "CandidateName");
            bulk.ColumnMappings.Add("Gender", "Gender");
            bulk.ColumnMappings.Add("Category", "Category");
            bulk.ColumnMappings.Add("PwBD", "PwBD");
            bulk.ColumnMappings.Add("Designation", "Designation");

            bulk.ColumnMappings.Add("CandidateState", "CandidateState");
            bulk.ColumnMappings.Add("CandidateDistrict", "CandidateDistrict");

            bulk.ColumnMappings.Add("SchoolState", "SchoolState");
            bulk.ColumnMappings.Add("HomeState", "HomeState");

            bulk.ColumnMappings.Add("SchoolName", "SchoolName");
            bulk.ColumnMappings.Add("SchoolBillUnitNo", "SchoolBillUnitNo");
            bulk.ColumnMappings.Add("SchoolDistrict", "SchoolDistrict");

            bulk.ColumnMappings.Add("Email", "Email");
            bulk.ColumnMappings.Add("DOB", "DOB");

            bulk.ColumnMappings.Add("PayLevel", "PayLevel");
            bulk.ColumnMappings.Add("BasicPay", "BasicPay");
            bulk.ColumnMappings.Add("AppointmentYear", "AppointmentYear");

            bulk.ColumnMappings.Add("CreatedDate", "CreatedDate");
            bulk.ColumnMappings.Add("CreatedBy", "CreatedBy");
            bulk.ColumnMappings.Add("IsActive", "IsActive");

            await bulk.WriteToServerAsync(table);
        }

        public async Task AddUploadLogAsync(StudentAppointmentUploadLog log)
        {
            try
            {
                if (log == null)
                    throw new ArgumentNullException(nameof(log));
                await _appDbContext.StudentAppointmentUploadLog.AddAsync(log);
               
            }
            catch (Exception ex)
            {
                // Log the exception (using your logging framework)
                //Console.Error.WriteLine($"Error adding upload log: {ex.Message}");
                throw; // Re-throw or handle as needed
            } 
        }

        public async Task<List<StudentAppointmentUploadLog>> GetUploadLogsAsync()
        {
            return await _appDbContext.StudentAppointmentUploadLog
           .OrderByDescending(x => x.UploadDate)
           .ToListAsync();
        }

        public async Task<List<StudentAppointment>> GetStudentsByYearAsync(int year)
        {
            return await _appDbContext.StudentAppointment
                .Where(x => x.AppointmentYear == year && x.IsActive && x.ISPDFGenerate == 0)
                .OrderBy(x => x.CandidateName)
                .ToListAsync();
        }

        public async Task<List<StudentAppointment>> GetStudentsdfGenByYearAsync(int year)
        {
            return await _appDbContext.StudentAppointment
                .Where(x => x.AppointmentYear == year && x.IsActive && x.ISPDFGenerate == 1 && x.ISPDFSigned == 0)
                .OrderBy(x => x.CandidateName)
                .ToListAsync();
        }

        public async Task<List<StudentAppointment>> GetStudentsByIdsAsync(List<long> ids)
        {
            return await _appDbContext.StudentAppointment
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }

        // ✅ Single Update
        public async Task MarkPdfGeneratedAsync(long studentId)
        {
            var entity = await _appDbContext.StudentAppointment
                                      .FirstOrDefaultAsync(x => x.Id == studentId);

            if (entity != null)
            {
                entity.ISPDFGenerate = 1; // or true
                //entity.UpdatedDate = DateTime.Now;
                                
            }
        }

        // ✅ Bulk Update (Recommended 🚀)
        public async Task MarkPdfGeneratedBulkAsync(List<long> studentIds)
        {
            var entities = await _appDbContext.StudentAppointment
                                         .Where(x => studentIds.Contains(x.Id))
                                         .ToListAsync();

            foreach (var item in entities)
            {
                item.ISPDFGenerate = 1;
                
            }

            
        }
        public async Task<List<StudentAppointment>> GetGeneratedPdfCandidates(int year)
        {
            return await _appDbContext.StudentAppointment
                .Where(x => x.AppointmentYear == year && x.ISPDFGenerate == 1)
                .ToListAsync();
        }

        public async Task MarkPdfSignedBulkAsync(List<long> ids)
        {
            await _appDbContext.StudentAppointment
                .Where(x => ids.Contains(x.Id))
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.ISPDFSigned, 1)
                );
        }

        public async Task MarkPdfSignedBulk(List<string> uniqueIds)
        {
            await _appDbContext.StudentAppointment
                .Where(x => uniqueIds.Contains(x.UniqueId))
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.ISPDFSigned, 1)
                );
        }

        public async Task<Dictionary<int, int>> GetStudentCountGroupedByYearAsync()
        {
            return await _appDbContext.StudentAppointment
                .Where(x => x.ISPDFGenerate == 1)
                .GroupBy(x => x.AppointmentYear)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Key, x => x.Count);
        }

      

    }
}
