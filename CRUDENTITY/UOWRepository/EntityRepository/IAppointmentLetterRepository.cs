using CRUDENTITY.Domain;
using CRUDENTITY.Entity;
using CRUDENTITY.UOWRepository.GenericeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.UOWRepository.EntityRepository
{
    public interface IAppointmentLetterRepository : IGenericRepository<StudentAppointment>
    {
        Task<List<StudentAppointment>> GetByEmailOrMobile(string input);
        Task<StudentAppointment> GetByIdAsync(long id);
        Task SaveOtp(string userKey, string otp);
        Task<OtpVerification> GetValidOtp(string userKey, string otp);
        Task MarkOtpUsed(OtpVerification OptEntity);

    }
}
