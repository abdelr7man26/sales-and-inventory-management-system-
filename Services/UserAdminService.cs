using Auto_Parts_Store.Helpers;
using Auto_Parts_Store.Models;
using Auto_Parts_Store.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Services
{
    public interface IUserAdminService
    {
        Task<IList<UserAdminEntry>>        GetAllUsersAsync();
        Task                               AddUserAsync(UserAdminEntry user, string plainPassword);
        Task                               UpdateUserAsync(UserAdminEntry user);
        Task                               DeleteUserAsync(int personId);
        Task                               ChangePasswordAsync(int personId, string newPlainPassword);
        Task<IList<SystemRole>>            GetAllRolesAsync();
        Task<IList<SystemPermissionEntry>> GetAllPermissionsAsync();
        Task<IList<SystemPermissionEntry>> GetPermissionsForRoleAsync(int roleId);
        Task                               SaveRolePermissionsAsync(int roleId, IEnumerable<int> grantedIds);
    }

    public class UserAdminService : IUserAdminService
    {
        private readonly IUserAdminRepository _repo;

        public UserAdminService(IUserAdminRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public Task<IList<UserAdminEntry>>        GetAllUsersAsync()                     => _repo.GetAllUsersAsync();
        public Task                               UpdateUserAsync(UserAdminEntry u)       => _repo.UpdateUserAsync(u);
        public Task                               DeleteUserAsync(int id)                 => _repo.DeleteUserAsync(id);
        public Task<IList<SystemRole>>            GetAllRolesAsync()                     => _repo.GetAllRolesAsync();
        public Task<IList<SystemPermissionEntry>> GetAllPermissionsAsync()               => _repo.GetAllPermissionsAsync();
        public Task<IList<SystemPermissionEntry>> GetPermissionsForRoleAsync(int roleId) => _repo.GetPermissionsForRoleAsync(roleId);
        public Task                               SaveRolePermissionsAsync(int roleId, IEnumerable<int> ids) => _repo.SaveRolePermissionsAsync(roleId, ids);

        public Task AddUserAsync(UserAdminEntry user, string plainPassword)
            => _repo.AddUserAsync(user, Auto_Parts_Store.Helpers.PasswordHasher.Hash(plainPassword));

        public Task ChangePasswordAsync(int personId, string newPlainPassword)
            => _repo.ChangePasswordAsync(personId, Auto_Parts_Store.Helpers.PasswordHasher.Hash(newPlainPassword));
    }
}
