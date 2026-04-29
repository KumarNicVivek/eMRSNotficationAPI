using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.Entity
{
    public class RoleWisePagePermission
    {
        public Int64 PageId { get; set; }
        public Int64 PermissionId { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public Int32 RoleId { get; set; }
        public string ActionAccessibility { get; set; }
    }
}
