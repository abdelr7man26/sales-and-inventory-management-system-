using Auto_Parts_Store.Helpers;
using Auto_Parts_Store.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {
        // ── Single-key read/write ──────────────────────────────────

        public async Task<string> GetSettingAsync(string key)
        {
            object result = await DbHelper.ExecuteScalarAsync(
                "SELECT SettingValue FROM StoreSettings WHERE SettingKey = @key",
                new SqlParameter("@key", key));
            return result?.ToString();
        }

        public async Task SaveSettingAsync(string key, string value)
        {
            // MERGE avoids a race between the EXISTS check and the write
            const string sql = @"
                MERGE StoreSettings AS target
                USING (SELECT @key AS K) AS src ON target.SettingKey = src.K
                WHEN MATCHED THEN
                    UPDATE SET SettingValue = @val
                WHEN NOT MATCHED THEN
                    INSERT (SettingKey, SettingValue, SettingGroup)
                    VALUES (@key, @val, N'General');";

            await DbHelper.ExecuteNonQueryAsync(sql,
                new SqlParameter("@key", key),
                new SqlParameter("@val", (object)value ?? DBNull.Value));
        }

        // ── Bulk read helpers (1 round-trip per group) ─────────────

        private async Task<Dictionary<string, string>> GetSettingsBulkAsync(string[] keys)
        {
            // Build a parameterised IN list without string concatenation
            var parameters = new SqlParameter[keys.Length];
            var paramNames = new string[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                paramNames[i] = "@k" + i;
                parameters[i] = new SqlParameter(paramNames[i], keys[i]);
            }
            string sql = $"SELECT SettingKey, SettingValue FROM StoreSettings WHERE SettingKey IN ({string.Join(",", paramNames)})";

            DataTable dt = await DbHelper.ExecuteQueryAsync(sql, parameters);
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (DataRow row in dt.Rows)
                result[row["SettingKey"].ToString()] = row["SettingValue"]?.ToString();
            return result;
        }

        // ── Store profile (1 query instead of 4) ──────────────────

        public async Task<StoreProfile> GetStoreProfileAsync()
        {
            var d = await GetSettingsBulkAsync(new[] { "Store.Name", "Store.Phone", "Store.Address", "Store.TaxNumber" });
            return new StoreProfile
            {
                Name      = Get(d, "Store.Name",      string.Empty),
                Phone     = Get(d, "Store.Phone",     string.Empty),
                Address   = Get(d, "Store.Address",   string.Empty),
                TaxNumber = Get(d, "Store.TaxNumber", string.Empty)
            };
        }

        public async Task SaveStoreProfileAsync(StoreProfile p)
        {
            await SaveSettingAsync("Store.Name",      p.Name);
            await SaveSettingAsync("Store.Phone",     p.Phone);
            await SaveSettingAsync("Store.Address",   p.Address);
            await SaveSettingAsync("Store.TaxNumber", p.TaxNumber);
        }

        // ── UI preferences (1 query instead of 2) ─────────────────

        public async Task<UIPreferences> GetUIPreferencesAsync()
        {
            var d = await GetSettingsBulkAsync(new[] { "UI.Theme", "UI.PrintFormat" });
            return new UIPreferences
            {
                Theme       = Get(d, "UI.Theme",       "Light"),
                PrintFormat = Get(d, "UI.PrintFormat", "A4")
            };
        }

        private static string Get(Dictionary<string, string> d, string key, string fallback = "")
        {
            string v;
            return d.TryGetValue(key, out v) && v != null ? v : fallback;
        }

        public async Task SaveUIPreferencesAsync(UIPreferences prefs)
        {
            await SaveSettingAsync("UI.Theme",       prefs.Theme);
            await SaveSettingAsync("UI.PrintFormat", prefs.PrintFormat);
        }
    }
}
