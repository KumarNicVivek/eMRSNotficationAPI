using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.Domain
{
    public class RoleWisePagePermissionModel
    {
        public Int64 PageId { get; set; }
        public Int64 PermissionId { get; set; }
        public string ControllerName { get; set; } = string.Empty;
        public string ActionName { get; set; } = string.Empty;
        public Int32 RoleId { get; set; }
        public string ActionAccessibility { get; set; } = string.Empty;
    }
}
