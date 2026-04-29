using CRUDENTITY.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICEAPP.ServiceLayer
{
    public interface ICandidateService
    {
        Task<List<CandidateDownloadLtrDModel>> GetCandidateLetters(string emailOrMobile);
        Task<bool> SendOtpAsync(string input);
        Task<string> GetFilePathById(long id, string baseFolder);
        Task<bool> VerifyOtpAsync(string input, string otp);
    }

}
