using CRUDENTITY.Entity;
using CRUDENTITY.UOWRepository.GenericeRepository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.UOWRepository.EntityRepository
{
    public interface IRoleWisePagePermissionRepository : IGenericRepository<RoleWisePagePermission>
    {
        List<RoleWisePagePermission> ExecuteStoredProc(string storedProc, params SqlParameter[] parameters);
    }
}
