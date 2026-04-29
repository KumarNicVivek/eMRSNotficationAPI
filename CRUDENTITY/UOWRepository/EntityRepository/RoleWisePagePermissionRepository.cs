using CRUDENTITY.DataContext;
using CRUDENTITY.Entity;
using CRUDENTITY.UOWRepository.GenericeRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.UOWRepository.EntityRepository
{
    public class RoleWisePagePermissionRepository : GenericRepository<RoleWisePagePermission>, IRoleWisePagePermissionRepository
    {
        public RoleWisePagePermissionRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public List<RoleWisePagePermission> ExecuteStoredProc(string storedProc, params SqlParameter[] parameters)
        {
            Int32 roleId = Convert.ToInt32(parameters[0].Value);
            var roleList = _appDbContext.RoleWisePagePermissions
                            .FromSqlInterpolated($"EXEC {storedProc} @RoleId = {roleId}")
                            .ToList();



            return roleList;


        }
    }
}
