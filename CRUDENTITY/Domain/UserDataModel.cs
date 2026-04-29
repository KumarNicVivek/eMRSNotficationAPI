using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.Domain
{
    public class UserDataModel
    {
        public Int64 Id { get; set; }
        public string First_Name { get; set; }
        public string? Last_Name { get; set; }
        public string? RoleName { get; set; }
        public Int32 RoleId { get; set; }
        public string? EncryptCode { get; set; }
        public string Email { get; set; }
        public Int64? Phone { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int64 CreatedBy { get; set; }
        public Int32? UserLevelId { get; set; }
        public string? RoleDescription { get; set; }
    }
}
