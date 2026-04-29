using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.Entity
{
    [Table("T_RolePagePermission")]
    public class RolePagePermission
    {
        [Key]
        public Int64 Id { get; set; }
        public Int64 PermissionId { get; set; }
        public Int64 UserId { get; set; }
        public Int32 RoleId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32 CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Int32? UpdateBy { get; set; }
        public bool IsActive { get; set; }
    }
}
