using Auto_Parts_Store.Repositories;
using Auto_Parts_Store.Models;
using Auto_Parts_Store.Helpers;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string username, string password);
        bool IsAdmin();
        void Logout();
        Task<bool> VerifyAdminPinAsync(string pin);
    }

    public class AuthService : IAuthService
    {
        private readonly IPartRepository      _repo;
        private readonly ISettingsRepository  _settings;

        public static Users CurrentSession { get; private set; }

        public AuthService(IPartRepository repo, ISettingsRepository settings)
        {
            _repo     = repo     ?? throw new ArgumentNullException(nameof(repo));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            // 1. Try hashed password (post-migration accounts)
            string hashedInput = PasswordHasher.Hash(password);
            DataTable dt = await _repo.GetUserByCredentialsAsync(username, hashedInput);

            if (dt != null && dt.Rows.Count > 0)
            {
                CreateSession(dt.Rows[0]);
                return true;
            }

            // 2. Migration fallback: check plain-text (pre-migration accounts).
            // If it matches, immediately re-hash and save so the account is upgraded.
            // This branch becomes unreachable once all users have logged in once.
            DataTable dtPlain = await _repo.GetUserByCredentialsAsync(username, password);
            if (dtPlain != null && dtPlain.Rows.Count > 0)
            {
                await _repo.UpgradePasswordHashAsync(
                    Convert.ToInt32(dtPlain.Rows[0]["ID"]), hashedInput);
                CreateSession(dtPlain.Rows[0]);
                return true;
            }

            return false;
        }

        private void CreateSession(DataRow row)
        {
            CurrentSession = new Users
            {
                UserID           = Convert.ToInt32(row["ID"]),
                CurrentUserName  = row["UserName"]?.ToString(),
                FullName         = row["PersonName"]?.ToString(),
                CurrentUserRole  = row["Role"]?.ToString(),
                IsLoggedIn       = true,
                LoginTime        = DateTime.Now
            };
        }

        public bool IsAdmin()
            => CurrentSession != null &&
               CurrentSession.CurrentUserRole.Equals("Admin", StringComparison.OrdinalIgnoreCase);

        public void Logout() => CurrentSession = null;

        /// <summary>
        /// Reads the admin PIN from StoreSettings (key "Security.AdminPin").
        /// Falls back to the SHA-256 hash of "1212" when the row does not yet exist,
        /// giving existing deployments a grace period to set a real PIN via Settings.
        /// </summary>
        public async Task<bool> VerifyAdminPinAsync(string pin)
        {
            if (string.IsNullOrWhiteSpace(pin)) return false;

            string storedHash = await _settings.GetSettingAsync("Security.AdminPin");

            // Fallback: first-time deployments that have not yet set a PIN via Settings UI
            if (string.IsNullOrEmpty(storedHash))
                storedHash = PasswordHasher.Hash("1212");

            return storedHash == PasswordHasher.Hash(pin);
        }
    }
}
