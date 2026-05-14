using Auto_Parts_Store.Helpers;
using Auto_Parts_Store.Models;
using Auto_Parts_Store.Repositories;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Services
{
    public interface ISettingsService
    {
        Task<StoreProfile>  GetStoreProfileAsync();
        Task                SaveStoreProfileAsync(StoreProfile profile);
        Task<UIPreferences> GetUIPreferencesAsync();
        Task                SaveUIPreferencesAsync(UIPreferences prefs);
        Task<string>        GetSettingAsync(string key);
        Task                SaveSettingAsync(string key, string value);
        Task<string>        BackupDatabaseAsync(string backupFolder);
    }

    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository _repo;

        // In-process UI pref cache — invalidated on save
        private static UIPreferences _cachedPrefs;

        public SettingsService(ISettingsRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public Task<StoreProfile> GetStoreProfileAsync()  => _repo.GetStoreProfileAsync();
        public Task SaveStoreProfileAsync(StoreProfile p)  => _repo.SaveStoreProfileAsync(p);

        public async Task<UIPreferences> GetUIPreferencesAsync()
        {
            if (_cachedPrefs != null) return _cachedPrefs;
            _cachedPrefs = await _repo.GetUIPreferencesAsync();
            return _cachedPrefs;
        }

        public async Task SaveUIPreferencesAsync(UIPreferences prefs)
        {
            await _repo.SaveUIPreferencesAsync(prefs);
            _cachedPrefs = prefs;
        }

        public Task<string> GetSettingAsync(string key)          => _repo.GetSettingAsync(key);
        public Task         SaveSettingAsync(string key, string v) => _repo.SaveSettingAsync(key, v);

        public async Task<string> BackupDatabaseAsync(string backupFolder)
        {
            if (!Directory.Exists(backupFolder))
                Directory.CreateDirectory(backupFolder);

            string fileName = $"AutoPartsStoreDB_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
            string fullPath = Path.Combine(backupFolder, fileName);

            // BACKUP DATABASE uses the SQL Server service account's filesystem access
            const string sql = "BACKUP DATABASE AutoPartsStoreDB TO DISK = @path WITH INIT, COMPRESSION, STATS = 5";
            await DbHelper.ExecuteNonQueryAsync(sql, new SqlParameter("@path", fullPath));
            return fullPath;
        }
    }
}
