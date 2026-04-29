using CRUDENTITY.Domain;
using CRUDENTITY.Entity;
using CRUDENTITY.UOWRepository;
using CRUDENTITY.UOWRepository.EntityRepository;
using Microsoft.Data.SqlClient;
using SERVICEAPP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SERVICEAPP.ServiceLayer
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }   
        public string GetGeneratedSalt()
        {
            string passwordSalt = PasswordHashing.GenerateSalt();
            return passwordSalt;
        }

        public bool getSHA512CombinedashPassWordandSalt(string storedHashPass, string sessionSalt, string clientFinalHashHex)
        {
            if (string.IsNullOrEmpty(storedHashPass) || string.IsNullOrEmpty(sessionSalt))
            {
                return false;
            }

            byte[] sessionSaltUtf8 = Encoding.UTF8.GetBytes(sessionSalt);

            byte[] saltHashBytes = PasswordHashing.ComputeSHA512FromByte(sessionSaltUtf8);

            // 1️⃣ Stored password hash (HEX → BYTES)
            byte[] storedPasswordHashBytes = Convert.FromHexString(storedHashPass);

            // 3️⃣ BYTE-safe combine (THIS IS CRITICAL)
            byte[] combined = new byte[
                storedPasswordHashBytes.Length + saltHashBytes.Length
            ];

            Buffer.BlockCopy(
                storedPasswordHashBytes, 0,
                combined, 0,
                storedPasswordHashBytes.Length
            );

            Buffer.BlockCopy(
                saltHashBytes, 0,
                combined, storedPasswordHashBytes.Length,
                saltHashBytes.Length
            );

            // 4️⃣ Final SHA512
            byte[] finalHashBytes = SHA512.HashData(combined);

            //string saltHashHex = Convert.ToHexString(sha512Salt).ToLowerInvariant();

            //string combinedHash = storedHashPass + saltHashHex;

            //byte[] combinedUtf8 = Encoding.UTF8.GetBytes(combinedHash);
            //byte[] finalHashBytes = SHA512.HashData(combinedUtf8);

            string finalHashHex = Convert.ToHexString(finalHashBytes).ToLowerInvariant();

            bool isValid = CryptographicOperations.FixedTimeEquals(
                                        Convert.FromHexString(finalHashHex),
                                        Convert.FromHexString(clientFinalHashHex));

            return isValid;



        }


        public UserDataModel GetUserByEmailId(string emailId)
        {
            throw new NotImplementedException();
        }

        public UserDataModel GerUserWithRoleByEmailId(string emailid)
        {

            try
            {
                var repos = _unitOfWork.GetRepository<IUserRepository>();

                if (repos == null)
                {
                    return new UserDataModel();
                }

                var userData = repos.GerUserWithRoleByEmailId(emailid);
                if (userData == null)
                {
                    return null;
                }

                var userModel = new UserDataModel
                {
                    First_Name = userData.First_Name,
                    Last_Name = userData.Last_Name,
                    Email = userData.Email,
                    IsActive = userData.IsActive,
                    //IsDeleted = userData.IsDeleted,
                    Password = userData.Password,
                    Id = userData.Id,
                    EncryptCode = userData.EncryptCode,
                    RoleId = userData.RoleId,
                    UserLevelId = userData.Roles.UserLevelId,
                    RoleDescription = userData.Roles.RoleDescription
                };

                return userModel;
            }

            catch (Exception ex)
            {
                return null;
            }          
                        
        }

        public UserDataModel GetUserBySecretCode(string strSecret)
        {
            var repos = _unitOfWork.GetRepository<IUserRepository>();

            if (repos == null)
            {
                return new UserDataModel();
            }
            var userData = repos.GetUserBySecretCode(strSecret);

            if (userData != null)
            {
                var userDModel = new UserDataModel
                {
                    //PasswordSalt = userData.PasswordSalt,
                    Email = userData.Email,
                    First_Name = userData.First_Name,
                    Last_Name = userData.Last_Name,
                    Phone = userData.Phone,
                    EncryptCode = userData.EncryptCode,
                    CreatedBy = userData.CreatedBy,
                    //State = userData.State,
                    //District = userData.District,
                    IsActive = userData.IsActive,
                    //IsDeleted = userData.IsDeleted,
                    RoleId = (Int32)userData.RoleId,
                    UserLevelId = userData.Roles.UserLevelId,
                    //Password = userData.Password,
                    Id = userData.Id,
                };

                return userDModel;
            }
            return new UserDataModel();
        }

        public bool IsUserExist(string emailId)
        {
            var repos = _unitOfWork.GetRepository<IUserRepository>();

            if (repos == null)
            {
                return false;
            }
            var IsuserModel = repos.IsUserExist(emailId.ToLower());

            return IsuserModel;
        }

        public bool IsUserEmailadnMobileExist(string Email, long Mobile)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "logs");
            string logPath = Path.Combine(uploadsFolder, "myapp.log");
            try
            {
                var repos = _unitOfWork.GetRepository<IUserRepository>();

                if (repos == null)
                {
                    return false;
                }
                var IsUserdata = repos.IsUserEmailadnMobileExist(Email, Mobile);

                return IsUserdata;
            }
            catch (Exception ex)
            {
                string logMessage = $"[{DateTime.Now}] IN User Service (IsUserEmailadnMobileExist): {ex.Message}{Environment.NewLine}";

                System.IO.File.AppendAllText(logPath, logMessage);
                return false;
            }

        }
        public UserDataModel? ValidateUser(string ModelEmailId, string ModelPassword)
        {
            var repos = _unitOfWork.GetRepository<IUserRepository>();

            if (repos == null)
            {
                return new UserDataModel();
            }
            var userData = repos.ValidateUser(ModelEmailId, ModelPassword);

            if (userData == null)
            {
                return null;
            }

            var userModel = new UserDataModel
            {
                First_Name = userData.First_Name,
                Last_Name = userData.Last_Name,
                Email = userData.Email,
                IsActive = userData.IsActive,
                //IsDeleted = userData.IsDeleted,
                Password = userData.Password,
                Id = userData.Id,
                EncryptCode = userData.EncryptCode,
                RoleId = userData.RoleId,
                UserLevelId = userData.Roles.UserLevelId,
                RoleDescription = userData.Roles.RoleDescription
            };

            //bool isPAsswordmatch = VerifyPassword(ModelPassword,ModelPassword);

            return userModel;
        }

        public List<string> GetPermissionsForUser(string userId)
        {
            var roleWisePages = UserHasAccessToPagelst(userId);

            if (roleWisePages == null || !roleWisePages.Any())
                return new List<string>();

            var permissions = roleWisePages
                .Select(p => MapPermission(p))
                .Distinct()
                .ToList();

            return permissions;
        }

        public List<RoleWisePagePermission> UserHasAccessToPagelst(string userId)
        {
            bool isPageAccess = false;

            var user = GetUserBySecretCode(userId);
            if (user == null)
            {
                isPageAccess = false;
                return null;
            }

            var parameters = new[]
            {
                new SqlParameter("@RoleId", user.RoleId)
            };

            var repos = _unitOfWork.GetRepository<IRoleWisePagePermissionRepository>();

            var roleWisePages = repos.ExecuteStoredProc("SPS_GetRoleWisePagePermission", parameters);

            return roleWisePages;
        }

        private string MapPermission(RoleWisePagePermission p)
        {
            var controller = p.ControllerName.ToUpper();

            var action = p.ActionAccessibility switch
            {
                "C" => "CREATE",
                "E" => "EDIT",
                "D" => "DELETE",
                "L" => "VIEW",
                "V" => "VIEW",
                "AD" => "ACCESS_DENIED",
                "LOG" => "LOGOUT",
                _ => p.ActionName.ToUpper()
            };

            return $"{controller}_{action}";
        }
    }
}
