using Auto_Parts_Store.Helpers;
using Auto_Parts_Store.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Repositories
{
    public class UserAdminRepository : IUserAdminRepository
    {
        // ── Users ──────────────────────────────────────────────────

        public async Task<IList<UserAdminEntry>> GetAllUsersAsync()
        {
            const string sql = @"
                SELECT p.ID AS PersonID, u.UserName, p.PersonName AS FullName,
                       p.Phone, p.address AS Address, u.Role
                FROM   Users  u
                JOIN   person p ON u.ID = p.ID
                WHERE  p.isdeleted = 0
                ORDER  BY p.PersonName";

            DataTable dt = await DbHelper.ExecuteQueryAsync(sql);
            var list = new List<UserAdminEntry>(dt.Rows.Count);
            foreach (DataRow r in dt.Rows)
                list.Add(new UserAdminEntry
                {
                    PersonID = Convert.ToInt32(r["PersonID"]),
                    UserName = r["UserName"].ToString(),
                    FullName = r["FullName"].ToString(),
                    Phone    = r["Phone"]?.ToString(),
                    Address  = r["Address"]?.ToString(),
                    Role     = r["Role"].ToString()
                });
            return list;
        }

        public async Task AddUserAsync(UserAdminEntry user, string password)
        {
            using (SqlConnection con = DbHelper.GetConnection())
            {
                await con.OpenAsync();
                using (SqlTransaction tx = con.BeginTransaction())
                {
                    try
                    {
                        object personId = await DbHelper.ExecuteScalarWithTransactionAsync(
                            "INSERT INTO person (PersonName, Phone, address, isdeleted) OUTPUT INSERTED.ID VALUES (@name, @phone, @addr, 0)",
                            con, tx,
                            new SqlParameter("@name",  user.FullName),
                            new SqlParameter("@phone", (object)user.Phone   ?? DBNull.Value),
                            new SqlParameter("@addr",  (object)user.Address ?? DBNull.Value));

                        await DbHelper.ExecuteNonQueryWithTransactionAsync(
                            "INSERT INTO Users (UserName, password, Role, ID) VALUES (@uname, @pwd, @role, @id)",
                            con, tx,
                            new SqlParameter("@uname", user.UserName),
                            new SqlParameter("@pwd",   password),
                            new SqlParameter("@role",  user.Role),
                            new SqlParameter("@id",    Convert.ToInt32(personId)));

                        tx.Commit();
                    }
                    catch { tx.Rollback(); throw; }
                }
            }
        }

        public async Task UpdateUserAsync(UserAdminEntry user)
        {
            using (SqlConnection con = DbHelper.GetConnection())
            {
                await con.OpenAsync();
                using (SqlTransaction tx = con.BeginTransaction())
                {
                    try
                    {
                        await DbHelper.ExecuteNonQueryWithTransactionAsync(
                            "UPDATE person SET PersonName=@name, Phone=@phone, address=@addr WHERE ID=@id",
                            con, tx,
                            new SqlParameter("@name",  user.FullName),
                            new SqlParameter("@phone", (object)user.Phone   ?? DBNull.Value),
                            new SqlParameter("@addr",  (object)user.Address ?? DBNull.Value),
                            new SqlParameter("@id",    user.PersonID));

                        await DbHelper.ExecuteNonQueryWithTransactionAsync(
                            "UPDATE Users SET UserName=@uname, Role=@role WHERE ID=@id",
                            con, tx,
                            new SqlParameter("@uname", user.UserName),
                            new SqlParameter("@role",  user.Role),
                            new SqlParameter("@id",    user.PersonID));

                        tx.Commit();
                    }
                    catch { tx.Rollback(); throw; }
                }
            }
        }

        public async Task DeleteUserAsync(int personId)
        {
            await DbHelper.ExecuteNonQueryAsync(
                "UPDATE person SET isdeleted = 1 WHERE ID = @id",
                new SqlParameter("@id", personId));
        }

        public async Task ChangePasswordAsync(int personId, string newPassword)
        {
            await DbHelper.ExecuteNonQueryAsync(
                "UPDATE Users SET password = @pwd WHERE ID = @id",
                new SqlParameter("@pwd", newPassword),
                new SqlParameter("@id",  personId));
        }

        // ── Roles ──────────────────────────────────────────────────

        public async Task<IList<SystemRole>> GetAllRolesAsync()
        {
            DataTable dt = await DbHelper.ExecuteQueryAsync(
                "SELECT RoleID, RoleName, Description, IsBuiltIn FROM SystemRoles ORDER BY RoleID");

            var list = new List<SystemRole>(dt.Rows.Count);
            foreach (DataRow r in dt.Rows)
                list.Add(new SystemRole
                {
                    RoleID      = Convert.ToInt32(r["RoleID"]),
                    RoleName    = r["RoleName"].ToString(),
                    Description = r["Description"]?.ToString(),
                    IsBuiltIn   = Convert.ToBoolean(r["IsBuiltIn"])
                });
            return list;
        }

        // ── Permissions ────────────────────────────────────────────

        public async Task<IList<SystemPermissionEntry>> GetAllPermissionsAsync()
        {
            DataTable dt = await DbHelper.ExecuteQueryAsync(
                "SELECT PermissionID, PermissionKey, DisplayName, Module FROM SystemPermissions ORDER BY Module, DisplayName");

            var list = new List<SystemPermissionEntry>(dt.Rows.Count);
            foreach (DataRow r in dt.Rows)
                list.Add(new SystemPermissionEntry
                {
                    PermissionID  = Convert.ToInt32(r["PermissionID"]),
                    PermissionKey = r["PermissionKey"].ToString(),
                    DisplayName   = r["DisplayName"].ToString(),
                    Module        = r["Module"].ToString()
                });
            return list;
        }

        public async Task<IList<SystemPermissionEntry>> GetPermissionsForRoleAsync(int roleId)
        {
            const string sql = @"
                SELECT sp.PermissionID, sp.PermissionKey, sp.DisplayName, sp.Module,
                       ISNULL(rp.IsGranted, 0) AS IsGranted
                FROM   SystemPermissions sp
                LEFT JOIN RolePermissions rp
                       ON sp.PermissionID = rp.PermissionID AND rp.RoleID = @roleId
                ORDER  BY sp.Module, sp.DisplayName";

            DataTable dt = await DbHelper.ExecuteQueryAsync(sql, new SqlParameter("@roleId", roleId));
            var list = new List<SystemPermissionEntry>(dt.Rows.Count);
            foreach (DataRow r in dt.Rows)
                list.Add(new SystemPermissionEntry
                {
                    PermissionID  = Convert.ToInt32(r["PermissionID"]),
                    PermissionKey = r["PermissionKey"].ToString(),
                    DisplayName   = r["DisplayName"].ToString(),
                    Module        = r["Module"].ToString(),
                    IsGranted     = Convert.ToBoolean(r["IsGranted"])
                });
            return list;
        }

        public async Task SaveRolePermissionsAsync(int roleId, IEnumerable<int> grantedIds)
        {
            using (SqlConnection con = DbHelper.GetConnection())
            {
                await con.OpenAsync();
                using (SqlTransaction tx = con.BeginTransaction())
                {
                    try
                    {
                        await DbHelper.ExecuteNonQueryWithTransactionAsync(
                            "DELETE FROM RolePermissions WHERE RoleID = @rid",
                            con, tx, new SqlParameter("@rid", roleId));

                        foreach (int pid in grantedIds)
                            await DbHelper.ExecuteNonQueryWithTransactionAsync(
                                "INSERT INTO RolePermissions (RoleID, PermissionID, IsGranted) VALUES (@rid, @pid, 1)",
                                con, tx,
                                new SqlParameter("@rid", roleId),
                                new SqlParameter("@pid", pid));

                        tx.Commit();
                    }
                    catch { tx.Rollback(); throw; }
                }
            }
        }
    }
}
