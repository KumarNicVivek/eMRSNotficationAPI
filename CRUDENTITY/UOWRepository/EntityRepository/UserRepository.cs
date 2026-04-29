using CRUDENTITY.DataContext;
using CRUDENTITY.Domain;
using CRUDENTITY.Entity;
using CRUDENTITY.UOWRepository.GenericeRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace CRUDENTITY.UOWRepository.EntityRepository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }
        public User GerUserWithRoleByEmailId(string emailid)
        {
            var userData = _appDbContext.User.Include("Roles").Where(x => x.Email == emailid && x.IsActive == true).FirstOrDefault();

            if (userData == null)
            {
                return null;
            }
            return userData;
        }

        public User GetUserBySecretCode(string strSecret)
        {
            var userData = _appDbContext.User.Include("Roles").Where(x => x.EncryptCode == strSecret).FirstOrDefault();
            //var userData = _appDbContext.User.Any(x => x.Email == email && x.IsActive == true);
            if (userData != null)
            {
                return userData;
            }
            return null;
        }

        public bool IsUserEmailadnMobileExist(string Email, long Mobile)
        {
            try
            {
                var IsUserData = _appDbContext.User.Any(u => u.Email.ToLower() == Email.ToLower()
                                           || u.Phone == Mobile);

                return IsUserData;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool IsUserExist(string emailId)
        {
            try
            {
                var IsUserData = _appDbContext.User.Any(x => x.Email == emailId && x.IsActive == true);

                return IsUserData;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public User? ValidateUser(string ModelEmailId, string ModelPassword)
        {
            throw new NotImplementedException();
        }
    }
}
