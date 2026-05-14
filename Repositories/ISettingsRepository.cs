using Auto_Parts_Store.Models;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Repositories
{
    public interface ISettingsRepository
    {
        Task<string>        GetSettingAsync(string key);
        Task                SaveSettingAsync(string key, string value);
        Task<StoreProfile>  GetStoreProfileAsync();
        Task                SaveStoreProfileAsync(StoreProfile profile);
        Task<UIPreferences> GetUIPreferencesAsync();
        Task                SaveUIPreferencesAsync(UIPreferences prefs);
    }
}
