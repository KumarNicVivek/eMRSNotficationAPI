using CRUDENTITY.Domain;
using CRUDENTITY.Entity;
using CRUDENTITY.UOWRepository;
using CRUDENTITY.UOWRepository.EntityRepository;
using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICEAPP.ServiceLayer
{
    public class CandidateService : ICandidateService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CandidateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<CandidateDownloadLtrDModel>> GetCandidateLetters(string emailOrMobile)
        {
            var repos  = _unitOfWork.GetRepository<IAppointmentLetterRepository>();

            if(repos == null) {
                throw new Exception("Repository not found");
            }
            var data = await repos.GetByEmailOrMobile(emailOrMobile);            

            var result = data.Select(x => new CandidateDownloadLtrDModel
            {
                Id = x.Id,
                Year = x.AppointmentYear,
                FileName = $"/Signed/AppLtrSigned_{x.AppointmentYear}_{x.Id}.pdf",
                PostDescription = x.Designation
            }).ToList();

            return result;
            
        }

        public async Task<bool> SendOtpAsync(string input)
        {
            try
            {
                var otp = new Random().Next(100000, 999999).ToString();

                var repos = _unitOfWork.GetRepository<IAppointmentLetterRepository>();

                if (repos == null)
                {
                    throw new Exception("Repository not found");
                }
                // Save OTP (DB / Cache)

                await repos.SaveOtp(input, otp);
                await _unitOfWork.SaveAsync();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            

            // Send SMS/Email (implement API)
        }

        public async Task<bool> VerifyOtpAsync(string input, string otp)
        {
            try
            {
                var repos = _unitOfWork.GetRepository<IAppointmentLetterRepository>();

                if (repos == null)
                {
                    throw new Exception("Repository not found");
                }
                var savedOtp = await repos.GetValidOtp(input, otp);

                if(savedOtp ==  null)
                {
                    //_logger.LogWarning($"Invalid OTP attempt for {input}");
                    return false;
                }

                savedOtp.IsUsed = true;

                await _unitOfWork.SaveAsync();

                return savedOtp.OTP == otp;

               
            }
            catch(Exception ex)
            {
                //_logger.LogWarning(ex, $"Error while verifying OTP for {input}");
                return false;
            }

        }

        public async Task<string> GetFilePathById(long id,string baseFolder)
        {
            try
            {
                var repos = _unitOfWork.GetRepository<IAppointmentLetterRepository>();
                    
                if(repos == null)
                {
                    throw new Exception("Repository not found");
                }

                var data = await repos.GetByIdAsync(id);

                if (data == null)
                {
                    //_logger.LogWarning($"File not found for Id: {id}");
                    return null;
                }
                string FileName = $"Signed/AppLtrSigned_{data.AppointmentYear}_{data.UniqueId}.pdf";
                //string baseFolder = Path.Combine(baseFolder,);
                var path = Path.Combine(baseFolder, "AppointmentLetters", data.AppointmentYear.ToString(), FileName);

                //_logger.LogInformation($"File accessed: {data.FileName}");

                return path;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"Error fetching file for Id: {id}");
                return null;
            }
        }
    }
}
