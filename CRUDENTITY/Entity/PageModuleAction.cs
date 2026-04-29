using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.Entity
{
    [Table("M_PageModuleAction")]
    public class PageModuleAction
    {
        [Key]
        public Int64 Id { get; set; }
        public Int32 ModuleId { get; set; }
        public string PageModuleName { get; set; }
        public string PageUrl { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32 CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Int32? UpdateBy { get; set; }
        public bool IsActive { get; set; }
        public string ActionType { get; set; }
    }
}
