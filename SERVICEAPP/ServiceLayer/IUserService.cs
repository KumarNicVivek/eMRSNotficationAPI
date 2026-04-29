using CRUDENTITY.Domain;
using CRUDENTITY.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICEAPP.ServiceLayer
{
    public interface IUserService
    {
        UserDataModel GetUserByEmailId(string emailId);
        bool IsUserExist(string emailId);
        string GetGeneratedSalt();
        bool getSHA512CombinedashPassWordandSalt(string storedHashPass, string sessionSalt, string clientFinalHashHex);
        UserDataModel GerUserWithRoleByEmailId(string emailid);
        List<RoleWisePagePermission> UserHasAccessToPagelst(string userId);
        List<string> GetPermissionsForUser(string userId);
        UserDataModel GetUserBySecretCode(string strSecret);
    }
}
