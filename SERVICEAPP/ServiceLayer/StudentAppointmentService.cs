using CRUDENTITY.Domain;
using CRUDENTITY.Entity;
using CRUDENTITY.UOWRepository;
using CRUDENTITY.UOWRepository.EntityRepository;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
//using iText.Layout.Element;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SERVICEAPP.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SERVICEAPP.ServiceLayer
{
    public class StudentAppointmentService : IStudentAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;       
        //private readonly IConfiguration _config;
        //private readonly IHttpContextAccessor _httpContext;

        private int _processedCount = 0;
        private int _totalCount = 0;
        private bool _isFinalizing = false;

        public StudentAppointmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<(int successCount, int failedCount)> BulkSignPdfAsync(List<long> ids,int year,string contentRootPath,string certPath,
                                            string password,string loginName,string role)
        {
            string baseFolder = Path.Combine(contentRootPath, "Uploads");

            _totalCount = ids.Count;
            _processedCount = 0;
            _isFinalizing = false;

            var successIds = new ConcurrentBag<long>();
            var failedIds = new ConcurrentBag<long>();

            int maxParallel = 5; // 🔥 control like your PDF generation

            using (SemaphoreSlim semaphore = new SemaphoreSlim(maxParallel))
            {
                var tasks = ids.Select(async id =>
                {
                    await semaphore.WaitAsync();

                    try
                    {
                        // 📂 Input file
                        string inputFile = $"ApLtrNoSignPDF_{year}_{id}.pdf";
                        string inputPath = Path.Combine(baseFolder, inputFile);

                        if (!File.Exists(inputPath))
                            throw new Exception("Input PDF not found");

                        // 📄 Output file
                        string signedFile = $"Signed_{year}_{id}.pdf";
                        string outputPath = Path.Combine(baseFolder, signedFile);

                        // 🔐 SIGN PDF (YOUR LOGIC)
                        var cert = new Cert(certPath, password);

                        var meta = new MetaData
                        {
                            Author = "Land Record Revenue",
                            Title = "Revenue Record",
                            Subject = "This file is digitally signed. Any change will invalidate it.",
                            Keywords = "files, digital files, revenue file",
                            Creator = loginName + " (" + role + ")",
                            Producer = "Haryana Land Records"
                        };

                        var signer = new PDfSignerWithMetaData(inputPath, outputPath, cert, meta);

                        signer.Sign(
                            "Digitally Signed by " + loginName,
                            "",
                            "Haryana",
                            true
                        );

                        // ✅ Verify output
                        if (!File.Exists(outputPath))
                            throw new Exception("Signing failed");

                        successIds.Add(id);
                    }
                    catch
                    {
                        failedIds.Add(id);
                    }
                    finally
                    {
                        Interlocked.Increment(ref _processedCount); // ✅ progress tracking
                        semaphore.Release();
                    }
                });

                await Task.WhenAll(tasks);
            }

            // 🔥 FINAL STAGE (DB UPDATE)
            _isFinalizing = true;

            var successList = successIds.ToList();

            if (successList.Count > 0)
            {
                var repo = _unitOfWork.GetRepository<IStudentAppointmentRepository>();

                if (repo == null)
                    throw new Exception("Repository not found");

                await repo.MarkPdfSignedBulkAsync(successList);              
            }

            await _unitOfWork.SaveAsync();

            return (successIds.Count, failedIds.Count);
        }

        public void SignSinglePdf(long id,int year,string baseFolder,string certPath,string password,string loginName,string role)
        {
            string inputFile = $"AppLtrNoSign_{year}_{id}.pdf";
            string inputPath = Path.Combine(baseFolder, "Unsigned", inputFile);

            if (!File.Exists(inputPath))
                throw new Exception("Input PDF not found");

            string signedFile = $"AppLtrSigned_{year}_{id}.pdf";
            string outputPath = Path.Combine(baseFolder, "Signed", signedFile);
            

            var cert = new Cert(certPath, password);

            var meta = new MetaData
            {
                Author = "Land Record Revenue",
                Title = "Revenue Record",
                Subject = "Digitally signed",
                Creator = loginName + " (" + role + ")"
            };

            var signer = new PDfSignerWithMetaData(inputPath, outputPath, cert, meta);

            signer.Sign("Digitally Signed", "", "Haryana", true);

            if (!File.Exists(outputPath))
                throw new Exception("Signing failed");

        }
        //public async Task BulkSignPdfAsync(List<long> ids, int year, string contentRootPath, string certPath, string password, string loginName, string role)
        //{
        //    try
        //    {
        //        if (ids == null || !ids.Any())
        //            return;

        //        var repo = _unitOfWork.GetRepository<IStudentAppointmentRepository>();

        //        if (repo == null)
        //            throw new Exception("Repository not found");

        //        var baseFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppLettersPdf");
        //        _totalCount = ids.Count;
        //        _processedCount = 0;
        //        _isFinalizing = false;

        //        var successIds = new ConcurrentBag<long>();
        //        var failedIds = new ConcurrentBag<long>();

        //        int maxParallel = 5; // 🔥 control like your PDF generation

        //        foreach (var id in ids)
        //        {
        //            string fileName = $"ApLtrNoSignPDF_{year}_{id}.pdf";
        //            string inputPath = Path.Combine(baseFolder, fileName);

        //            if (!File.Exists(inputPath))
        //                continue;

        //            // 📄 Output file
        //            string signedFileName = $"Signed_{year}_{id}.pdf";
        //            string outputPath = Path.Combine(baseFolder, signedFileName);

        //            // 🔐 ===== YOUR SIGN CALL START =====
        //            var cert = new Cert(certPath, password);

        //            var meta = new MetaData
        //            {
        //                Author = "Land Record Revenue",
        //                Title = "Revenue Record",
        //                Subject = "This file is digitally signed. Any change will invalidate it.",
        //                Keywords = "files, digital files, revenue file",
        //                Creator = loginName + " (" + role + ")",
        //                Producer = "Haryana Land Records"
        //            };

        //            var signer = new PDfSignerWithMetaData(inputPath, outputPath, cert, meta);

        //            signer.Sign(
        //                "Digitally Signed by " + loginName,
        //                "",
        //                "Haryana",
        //                true
        //            );

        //            //// ✅ Update DB
        //            //await repo.UpdateSignedPdfAsync(
        //            //    id,
        //            //    "/Uploads/" + signedFileName
        //            //);
        //        }





        //        await _unitOfWork.SaveAsync(); //  important
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception (using your logging framework)
        //        //Console.Error.WriteLine($"Error marking PDFs as generated: {ex.Message}");
        //        throw; // Re-throw or handle as needed
        //    }

        //}



        public async Task<(string message, byte[] errorFile)> UploadExcelAsync(IFormFile file, long userId, int AppointmentYear)
        {
           

            var repo = _unitOfWork.GetRepository<IStudentAppointmentRepository>();

            if (repo == null)
                return ("Repository not found", null);

            var dataTable = CreateTable();
            var errorList = new List<string[]>();

            var uniqueIdsInFile = new HashSet<string>();
            var excelIds = new List<string>();

            using var stream = file.OpenReadStream();
            using var doc = SpreadsheetDocument.Open(stream, false);

            var workbookPart = doc.WorkbookPart;
            var sheet = workbookPart.Workbook.Sheets.GetFirstChild<Sheet>();
            var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);

            var rows = worksheetPart.Worksheet.Descendants<Row>().ToList();

           

            // 🔥 Collect all IDs first
            for (int i = 1; i < rows.Count; i++) // skip header
            {
                var cells = rows[i].Elements<Cell>().ToList();
                var id = GetCellValue(doc, cells, 0);

                if (!string.IsNullOrEmpty(id))
                    excelIds.Add(id);
            }

            // 🔥 DB duplicate check
            var existingIds = await repo.GetExistingUniqueIdsAsync(excelIds, AppointmentYear);

            // 🔥 Process rows
            for (int i = 1; i < rows.Count; i++)
            {
                var cells = rows[i].Elements<Cell>().ToList();

                try
                {
                    string uniqueId = GetCellValue(doc, cells, 0);

                    if (string.IsNullOrEmpty(uniqueId) ||
                        uniqueIdsInFile.Contains(uniqueId) ||
                        existingIds.Contains(uniqueId))
                    {
                        errorList.Add(GetRow(doc, cells, "Duplicate or empty UniqueId"));
                        continue;
                    }

                    uniqueIdsInFile.Add(uniqueId);

                    var dr = dataTable.NewRow();

                    dr["UniqueId"] = uniqueId;
                    dr["RollNo"] = GetCellValue(doc, cells, 1);
                    dr["CandidateName"] = GetCellValue(doc, cells, 2);
                    dr["Gender"] = GetCellValue(doc, cells, 3);
                    dr["Category"] = GetCellValue(doc, cells, 4);
                    dr["PwBD"] = GetCellValue(doc, cells, 5);
                    dr["Designation"] = GetCellValue(doc, cells, 6);

                    dr["CandidateState"] = GetCellValue(doc, cells, 7);
                    dr["CandidateDistrict"] = GetCellValue(doc, cells, 8);
                    dr["SchoolState"] = GetCellValue(doc, cells, 9);
                    dr["HomeState"] = GetCellValue(doc, cells, 10);

                    dr["SchoolName"] = GetCellValue(doc, cells, 11);
                    dr["SchoolBillUnitNo"] = GetCellValue(doc, cells, 12);
                    dr["SchoolDistrict"] = GetCellValue(doc, cells, 13);

                    dr["Email"] = GetCellValue(doc, cells, 14);

                    // 🔥 Handle Date (Excel serial + normal)
                    var dobStr = GetCellValue(doc, cells, 15);
                    if (double.TryParse(dobStr, out double oaDate))
                        dr["DOB"] = DateTime.FromOADate(oaDate);
                    else if (DateTime.TryParse(dobStr, out DateTime dob))
                        dr["DOB"] = dob;
                    else
                        dr["DOB"] = DBNull.Value;

                    dr["PayLevel"] = GetCellValue(doc, cells, 16);

                    var payStr = GetCellValue(doc, cells, 17);
                    dr["BasicPay"] = decimal.TryParse(payStr, out decimal pay)
                        ? pay : DBNull.Value;
                    dr["AppointmentYear"] = AppointmentYear; //GetCellValue(doc, cells, 18);

                    dr["CreatedDate"] = DateTime.Now;
                    dr["CreatedBy"] = userId;
                    dr["IsActive"] = true;

                    dataTable.Rows.Add(dr);
                }
                catch
                {
                    errorList.Add(GetRow(doc, cells, "Parsing error"));
                }
            }

            // 🚀 BULK INSERT
            if (dataTable.Rows.Count > 0)
            {
                await repo.BulkInsertAsync(dataTable);
            }

            // 📄 Generate error file (CSV - no extra library)
            byte[] errorFile = errorList.Any() ? GenerateErrorCsv(errorList) : Array.Empty<byte>();

            var message = $"Upload Completed. Inserted: {dataTable.Rows.Count}, Errors: {errorList.Count}";

            return (message, errorFile);
        }

        public async Task<Int64> SaveUploadLogAsync(StudentAppointmentUploadLogModel log)
        {
            Int64 logId = 0;
            var repo = _unitOfWork.GetRepository<IStudentAppointmentRepository>();

            if (repo == null)
                throw new Exception("Repository not found");

            var studentUploadLog = new StudentAppointmentUploadLog
            {
                FileName = log.FileName,
                FilePath = log.FilePath,
                UploadDate = log.UploadDate,
                AppointmentYear = log.AppointmentYear,
                CreatedBy = log.CreatedBy
            };

            await repo.AddUploadLogAsync(studentUploadLog);

            await _unitOfWork.SaveAsync(); // 🔥 important
            logId = studentUploadLog.Id; // Get the generated ID

            return logId;
        }

        public async Task<List<StudentAppointmentUploadLogModel>> GetUploadLogsAsync()
        {
            var repo = _unitOfWork.GetRepository<IStudentAppointmentRepository>();

            if (repo == null)
                throw new Exception("Repository not found");

            var studnetupdateLoglst =  await repo.GetUploadLogsAsync();

            return studnetupdateLoglst.Select(log => new StudentAppointmentUploadLogModel
            {
                FileName = log.FileName,
                FilePath = log.FilePath,
                UploadDate = log.UploadDate,
                AppointmentYear = log.AppointmentYear,
                CreatedBy = (int) log.CreatedBy
            }).ToList();
        }

        public  async Task<int> GetStudentsCountByYearPdfGenrated(int year)
        {
            var repo = _unitOfWork.GetRepository<IStudentAppointmentRepository>();

            if (repo == null)
                throw new Exception("Repository not found");

            var studentappointData = await repo.GetStudentsdfGenByYearAsync(year);
            return studentappointData.Count;
        }
        public async Task<Dictionary<int, int>> GetStudentsCountByYearPdfGeneratedBulkAsync()
        {
            var repo = _unitOfWork.GetRepository<IStudentAppointmentRepository>();

            if (repo == null)
                throw new Exception("Repository not found");

            var studentappointData = await repo.GetStudentCountGroupedByYearAsync();
            return studentappointData;
        }



        public async Task<(List<StudentAppointmentModel>, int totalRecord)> GetStudentsAppLtrPdfGenByYearAsync(int year, int page, int pageSize)
        {
            var repo = _unitOfWork.GetRepository<IStudentAppointmentRepository>();

            if (repo == null)
                throw new Exception("Repository not found");

            var studentappointData = await repo.GetStudentsdfGenByYearAsync(year);

            int totalRecords = studentappointData.Count();

            var data = studentappointData
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();


            var studentappointDataModel = data.Select(student => new StudentAppointmentModel
            {
                Id = student.Id,
                UniqueId = student.UniqueId,
                RollNo = student.RollNo,
                CandidateName = student.CandidateName,
                Gender = student.Gender,
                BasicPay = student.BasicPay,
                Category = student.Category,
                PwBD = student.PwBD,
                Designation = student.Designation,
                CandidateState = student.CandidateState,
                CandidateDistrict = student.CandidateDistrict,
                SchoolState = student.SchoolState,
                HomeState = student.HomeState,
                SchoolName = student.SchoolName,
                SchoolBillUnitNo = student.SchoolBillUnitNo,
                SchoolDistrict = student.SchoolDistrict,
                Email = student.Email,
                DOB = student.DOB,
                PayLevel = student.PayLevel,
                AppointmentYear = student.AppointmentYear,
                ISPDFGenerate = student.ISPDFGenerate,
                ISPDFSigned = student.ISPDFSigned

            }).ToList();

            return (studentappointDataModel,totalRecords);
        }
        public async Task<(List<StudentAppointmentModel>, int TotalRecord)> GetStudentsByYearAsync(int year, int page, int pageSize)
        {
            var repo = _unitOfWork.GetRepository<IStudentAppointmentRepository>();

            if (repo == null)
                throw new Exception("Repository not found");

            var studentappointData = await repo.GetStudentsByYearAsync(year);

            int totalRecords = studentappointData.Count();

            var data = studentappointData
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();


            var studentappointDataModel = data.Select(student => new StudentAppointmentModel
            {
                Id = student.Id,
                UniqueId = student.UniqueId,
                RollNo = student.RollNo,
                CandidateName = student.CandidateName,
                Gender = student.Gender,
                BasicPay = student.BasicPay,
                Category = student.Category,
                PwBD = student.PwBD,
                Designation = student.Designation,
                CandidateState = student.CandidateState,
                CandidateDistrict = student.CandidateDistrict,
                SchoolState = student.SchoolState,
                HomeState = student.HomeState,
                SchoolName = student.SchoolName,
                SchoolBillUnitNo = student.SchoolBillUnitNo,
                SchoolDistrict = student.SchoolDistrict,
                Email = student.Email,
                DOB = student.DOB,
                PayLevel = student.PayLevel,
                AppointmentYear = student.AppointmentYear,
                ISPDFGenerate = student.ISPDFGenerate,
                ISPDFSigned = student.ISPDFSigned

            }).ToList();

            return (studentappointDataModel, totalRecords);
        }

        public async Task<List<StudentAppointmentModel>> GetStudentsByIdsAsync(List<long> ids)
        {
            var repo = _unitOfWork.GetRepository<IStudentAppointmentRepository>();

            if (repo == null)
                throw new Exception("Repository not found");

            var studentlstData = await repo.GetStudentsByIdsAsync(ids);

            var studentappointDataModel = studentlstData.Select(student => new StudentAppointmentModel
            {
                Id = student.Id,
                UniqueId = student.UniqueId,
                RollNo = student.RollNo,
                CandidateName = student.CandidateName,
                Gender = student.Gender,
                BasicPay = student.BasicPay,
                Category = student.Category,
                PwBD = student.PwBD,
                Designation = student.Designation,
                CandidateState = student.CandidateState,
                CandidateDistrict = student.CandidateDistrict,
                SchoolState = student.SchoolState,
                HomeState = student.HomeState,
                SchoolName = student.SchoolName,
                SchoolBillUnitNo = student.SchoolBillUnitNo,
                SchoolDistrict = student.SchoolDistrict,
                Email = student.Email,
                DOB = student.DOB,
                PayLevel = student.PayLevel,
                AppointmentYear = student.AppointmentYear,
                ISPDFGenerate = student.ISPDFGenerate,
                ISPDFSigned = student.ISPDFSigned

            }).ToList();

            return studentappointDataModel;
        }

        public async Task MarkPdfGeneratedAsync(long studentId)
        {
            var repo = _unitOfWork.GetRepository<IStudentAppointmentRepository>();

            if (repo == null)
                throw new Exception("Repository not found");

            await repo.MarkPdfGeneratedAsync(studentId);
            await _unitOfWork.SaveAsync(); // 🔥 important
        }

        public async Task MarkPdfGeneratedBulkAsync(List<long> studentIds)
        {
            try
            {
                if (studentIds == null || !studentIds.Any())
                    return;

                var repo = _unitOfWork.GetRepository<IStudentAppointmentRepository>();

                if (repo == null)
                    throw new Exception("Repository not found");

                await repo.MarkPdfGeneratedBulkAsync(studentIds);
                
                await _unitOfWork.SaveAsync(); // 🔥 important
            }
            catch(Exception ex)
            {
                // Log the exception (using your logging framework)
                //Console.Error.WriteLine($"Error marking PDFs as generated: {ex.Message}");
                throw; // Re-throw or handle as needed
            }
            
        }

        public async Task MarkPdfSignedBulkAsync(List<string> studentIds)
        {
            try
            {
                if (studentIds == null || !studentIds.Any())
                    return;

                var repo = _unitOfWork.GetRepository<IStudentAppointmentRepository>();

                if (repo == null)
                    throw new Exception("Repository not found");

                //await repo.MarkPdfSignedBulkAsync(studentIds);

                await repo.MarkPdfSignedBulk(studentIds);
                await _unitOfWork.SaveAsync(); // 🔥 important
            }
            catch (Exception ex)
            {
                // Log the exception (using your logging framework)
                //Console.Error.WriteLine($"Error marking PDFs as generated: {ex.Message}");
                throw; // Re-throw or handle as needed
            }

        }



        #region Private Methods


        // ================= HELPERS =================

        private DataTable CreateTable()
        {
            var dt = new DataTable();

            dt.Columns.Add("UniqueId");
            dt.Columns.Add("RollNo");
            dt.Columns.Add("CandidateName");
            dt.Columns.Add("Gender");
            dt.Columns.Add("Category");
            dt.Columns.Add("PwBD");
            dt.Columns.Add("Designation");

            dt.Columns.Add("CandidateState");
            dt.Columns.Add("CandidateDistrict");
            dt.Columns.Add("SchoolState");
            dt.Columns.Add("HomeState");

            dt.Columns.Add("SchoolName");
            dt.Columns.Add("SchoolBillUnitNo");
            dt.Columns.Add("SchoolDistrict");

            dt.Columns.Add("Email");
            dt.Columns.Add("DOB", typeof(DateTime));
            dt.Columns.Add("PayLevel");
            dt.Columns.Add("BasicPay", typeof(decimal));
            dt.Columns.Add("AppointmentYear");            
            dt.Columns.Add("CreatedDate", typeof(DateTime));
            dt.Columns.Add("CreatedBy", typeof(long));
            dt.Columns.Add("IsActive", typeof(bool));

            return dt;
        }

        private string GetCellValue(SpreadsheetDocument doc, List<Cell> cells, int index)
        {
            if (cells.Count <= index)
                return "";

            var cell = cells[index];
            var value = cell.InnerText;

            if (cell.DataType != null && cell.DataType == CellValues.SharedString)
            {
                var stringTable = doc.WorkbookPart.SharedStringTablePart.SharedStringTable;
                return stringTable.ElementAt(int.Parse(value)).InnerText;
            }

            return value;
        }

        private string[] GetRow(SpreadsheetDocument doc, List<Cell> cells, string error)
        {
            var arr = new string[19];

            for (int i = 0; i < 18; i++)
                arr[i] = GetCellValue(doc, cells, i);

            arr[18] = error;

            return arr;
        }

        private byte[] GenerateErrorCsv(List<string[]> errors)
        {
            var sb = new StringBuilder();

            foreach (var row in errors)
            {
                sb.AppendLine(string.Join(",", row.Select(x => $"\"{x}\"")));
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        public Task<List<StudentAppointmentModel>> GetGeneratedPdfCandidates(int year)
        {
            throw new NotImplementedException();
        }

       

        #endregion

    }
}
