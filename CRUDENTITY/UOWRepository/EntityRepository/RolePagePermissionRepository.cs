using CRUDENTITY.DataContext;
using CRUDENTITY.Entity;
using CRUDENTITY.UOWRepository.GenericeRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.UOWRepository.EntityRepository
{
    public class RolePagePermissionRepository : GenericRepository<RolePagePermission>, IRolePagePermissionRepository
    {
        public RolePagePermissionRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
        public async Task AddPermissionsAsync(IEnumerable<RolePagePermission> permissions)
        {
            try
            {
                if (permissions == null || !permissions.Any())
                {
                    throw new ArgumentException("Permissions collection cannot be null or empty.", nameof(permissions));
                }
                await _appDbContext.Set<RolePagePermission>().AddRangeAsync(permissions);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding permissions: {ex.Message}", ex);
            }
        }

        public bool DeleteByRoleIdAsync(long roleId)
        {
            try
            {
                var permissionsToDelete = _appDbContext.RolePagePermission
                                        .Where(rp => rp.RoleId == roleId && rp.IsActive);
                if (!permissionsToDelete.Any())
                {
                    return false;
                }

                _appDbContext.RolePagePermission.RemoveRange(permissionsToDelete);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting permissions for role ID {roleId}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<RolePagePermission>> GetByRoleIdAsync(long roleId)
        {
            try
            {
                var permissions = await _appDbContext.RolePagePermission
                                    .Where(rp => rp.RoleId == roleId && rp.IsActive)
                                    .ToListAsync();

                return permissions;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving permissions for role ID {roleId}: {ex.Message}", ex);
            }
        }
    }
}
