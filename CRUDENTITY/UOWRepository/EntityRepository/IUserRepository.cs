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
    public interface IUserRepository : IGenericRepository<User>
    {
        User GerUserWithRoleByEmailId(string emailid);
        User GetUserBySecretCode(string strSecret);
        bool IsUserExist(string emailId);
        bool IsUserEmailadnMobileExist(string Email, long Mobile);
        User? ValidateUser(string ModelEmailId, string ModelPassword);
    }
}
