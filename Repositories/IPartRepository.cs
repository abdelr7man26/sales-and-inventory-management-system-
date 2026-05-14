using Auto_Parts_Store.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Repositories
{
    public interface IPartRepository
    {
        Task<DataTable> GetUserByCredentialsAsync(string username, string password);
        Task UpgradePasswordHashAsync(int userId, string newHash);
        Task<DataTable> GetDashboardStatsAsync();
        Task<DataTable> GetLowStockAlertsAsync();




        Task<DataTable> GetAllPartsAsync();
        Task AddPartAsync(AutoPart Part);
        Task UpdatePartAsync(AutoPart Part);
        Task DeletePartAsync(int PartID);


        Task<DataTable> GetAllCategoriesAsync();
        Task AddCategoryAsync(string categoryName);
        Task UpdateCategoryAsync(int id, string newName);
        Task DeleteCategoryAsync(int id);


        Task<DataTable> GetPartsByCategoryAsync(int catId);


        Task<DataTable> GetPartHistoryAsync(int partId);

        Task<string> GetPartNumberAsync();

        Task<DataTable> GetPartsForAutoCompleteAsync();
        Task<int> GetPartIdByNameAsync(string partName);
        Task<AutoPart> GetPartByNumberAsync(string partNumber);
        Task<bool> IsPartNumberExistsAsync(string partNumber);
        Task<AutoPart> GetPartByNameAsync(string partName);
    }
}