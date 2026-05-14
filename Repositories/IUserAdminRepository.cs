using Auto_Parts_Store.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Repositories
{
    public interface IUserAdminRepository
    {
        Task<IList<UserAdminEntry>>        GetAllUsersAsync();
        Task                               AddUserAsync(UserAdminEntry user, string password);
        Task                               UpdateUserAsync(UserAdminEntry user);
        Task                               DeleteUserAsync(int personId);
        Task                               ChangePasswordAsync(int personId, string newPassword);

        Task<IList<SystemRole>>            GetAllRolesAsync();
        Task<IList<SystemPermissionEntry>> GetAllPermissionsAsync();
        Task<IList<SystemPermissionEntry>> GetPermissionsForRoleAsync(int roleId);
        Task                               SaveRolePermissionsAsync(int roleId, IEnumerable<int> grantedPermissionIds);
    }
}
