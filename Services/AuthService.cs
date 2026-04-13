using Auto_Parts_Store.Repositories;
using Auto_Parts_Store.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string username, string password);
        bool IsAdmin();
        void Logout();
        bool VerifyAdminPin(string pin);
    }

    public class AuthService : IAuthService
    {

        private readonly IPartRepository _repo;

        public static Users CurrentSession { get; private set; }

        public AuthService(IPartRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            try
            {
                DataTable dt = await _repo.GetUserByCredentialsAsync(username, password);

                if (dt != null && dt.Rows.Count > 0)
                {
                    CreateSession(dt.Rows[0]);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                
                throw; 
            }
        }
        private void CreateSession(DataRow row)
        {
            CurrentSession = new Users
            {
                UserID = Convert.ToInt32(row["ID"]),
                CurrentUserName = row["UserName"]?.ToString(),
                FullName = row["PersonName"]?.ToString(),
                CurrentUserRole = row["Role"]?.ToString(),
                IsLoggedIn = true,
                LoginTime = DateTime.Now
            };
        }

        public bool IsAdmin()
        {
            return CurrentSession != null && CurrentSession.CurrentUserRole.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }

        public void Logout()
        {
            CurrentSession = null;
        }
        public bool VerifyAdminPin(string pin)
        {
            if (string.IsNullOrWhiteSpace(pin)) return false;

            const string ADMIN_PIN = "1212";

            return pin == ADMIN_PIN;
        }
    }
}
