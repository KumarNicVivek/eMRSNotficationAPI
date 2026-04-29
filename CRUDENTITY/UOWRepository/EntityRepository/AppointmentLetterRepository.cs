using CRUDENTITY.DataContext;
using CRUDENTITY.Entity;
using CRUDENTITY.UOWRepository.GenericeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CRUDENTITY.UOWRepository.EntityRepository
{
    public class AppointmentLetterRepository : GenericRepository<StudentAppointment>, IAppointmentLetterRepository
    {
        public AppointmentLetterRepository(AppDbContext appDbContext): base(appDbContext)
        {
        }
        public async Task<List<StudentAppointment>> GetByEmailOrMobile(string emailOrMobile)
        {
            var lstCandidate = await _appDbContext.StudentAppointment
                                    .Where(x => x.Email == emailOrMobile || x.RollNo == emailOrMobile)
                                    .ToListAsync();

            return lstCandidate;
            
        }

        public async Task<StudentAppointment> GetByIdAsync(long id)
        {
            var candidate =await _appDbContext.StudentAppointment.FirstOrDefaultAsync(x => x.Id == id);
            
            if(candidate == null)
            {
                return null;
            }
            return candidate;
        }

        public async Task<OtpVerification> GetValidOtp(string userKey, string otp)
        {
            var OtpforVerification = await _appDbContext.OtpVerification
                                            .Where(x => x.UserKey == userKey && x.OTP == otp
                                                    && !x.IsUsed && x.ExpiryTime > DateTime.Now)
                                            .OrderByDescending(x => x.Id)
                                            .FirstOrDefaultAsync();

            return OtpforVerification;

            
        }

        public async Task MarkOtpUsed(OtpVerification otp)
        {
             _appDbContext.OtpVerification.Update(otp);
            
        }

        public async Task SaveOtp(string userKey, string otp)
        {
            var entity = new OtpVerification
            {
                UserKey = userKey,
                OTP = otp,
                ExpiryTime = DateTime.Now.AddMinutes(5),
                IsUsed = false
            };

            await _appDbContext.OtpVerification.AddAsync(entity);

        }
    }
}
