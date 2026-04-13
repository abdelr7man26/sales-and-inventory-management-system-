using Auto_Parts_Store.Models;
using System.Data;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Repositories
{
    public interface ISafeRepository
    {
        Task AddTransactionAsync(SafeTransaction transaction);

        Task<decimal> GetSafeBalanceAsync();

        Task<DataTable> GetSafeTransactionsHistoryAsync();
    }
}