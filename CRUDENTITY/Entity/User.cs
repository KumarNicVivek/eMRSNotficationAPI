using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.Entity
{
    [Table("T_User")]
    public class User
    {
        [Key]
        public Int64 Id { get; set; }
        public required string Email { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }       
        public Int64? Phone { get; set; }
        public string EncryptCode { get; set; }
        public required string Password { get; set; }        
        public DateTime CreatedDate { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Int64? UpdatedBy { get; set; }
        public bool IsActive { get; set; }        

        [ForeignKey("Roles")]
        public Int32 RoleId { get; set; }
        public Roles? Roles { get; set; }
    }
}
