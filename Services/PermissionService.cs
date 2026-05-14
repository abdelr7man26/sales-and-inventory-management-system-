using Auto_Parts_Store.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Services
{
    /// <summary>
    /// Loads the current user's granted permissions into a fast in-process cache.
    /// Call LoadPermissionsAsync once after login; then HasPermission is O(1).
    /// Admin role bypasses all checks (full access guaranteed).
    /// </summary>
    public interface IPermissionService
    {
        Task LoadPermissionsAsync(string roleName);
        bool HasPermission(string permissionKey);
        void ClearPermissions();
    }

    public class PermissionService : IPermissionService
    {
        // Static cache shared across the process — one user session at a time
        private static readonly HashSet<string> _granted =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private static readonly object _lock = new object();

        public async Task LoadPermissionsAsync(string roleName)
        {
            const string sql = @"
                SELECT sp.PermissionKey
                FROM   SystemPermissions sp
                JOIN   RolePermissions   rp ON sp.PermissionID = rp.PermissionID
                JOIN   SystemRoles       sr ON rp.RoleID        = sr.RoleID
                WHERE  sr.RoleName = @role AND rp.IsGranted = 1";

            DataTable dt = await DbHelper.ExecuteQueryAsync(sql, new SqlParameter("@role", roleName));

            lock (_lock)
            {
                _granted.Clear();
                foreach (DataRow row in dt.Rows)
                    _granted.Add(row["PermissionKey"].ToString());
            }
        }

        public bool HasPermission(string permissionKey)
        {
            // Admin always has every permission regardless of the mapping table
            if (AuthService.CurrentSession?.CurrentUserRole
                    ?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true)
                return true;

            lock (_lock)
                return _granted.Contains(permissionKey);
        }

        public void ClearPermissions()
        {
            lock (_lock)
                _granted.Clear();
        }
    }
}
