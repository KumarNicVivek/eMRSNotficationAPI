using CRUDENTITY.Entity;
using CRUDENTITY.UOWRepository.GenericeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.UOWRepository.EntityRepository
{
    public interface IRolePagePermissionRepository : IGenericRepository<RolePagePermission>
    {
        Task<IEnumerable<RolePagePermission>> GetByRoleIdAsync(long roleId);
        bool DeleteByRoleIdAsync(long roleId);
        Task AddPermissionsAsync(IEnumerable<RolePagePermission> permissions);
    }
}
