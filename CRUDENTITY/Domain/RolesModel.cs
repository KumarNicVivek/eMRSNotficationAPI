using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.Domain
{
    public class RolesModel
    {
        public Int32 Id { get; set; }
        public string RoleName { get; set; }
        public Int32 DisplayOrder { get; set; }
        public string? RoleDescription { get; set; }
        public Int32 UserLevelId { get; set; }
        public int AllowApprove { get; set; } = 0;
        public int AllowCreate { get; set; } = 0;
        public int AllowDelete { get; set; } = 0;
        public int Allowview { get; set; } = 0;
        public int AllowVerify { get; set; } = 0;
        public DateTime CreatedDate { get; set; }
        public Int32 CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Int32? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
