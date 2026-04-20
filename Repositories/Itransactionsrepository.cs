using Auto_Parts_Store.Models;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Repositories
{
    public interface IQuickPayRepository
    {
        Task<bool> ExecuteQuickPaymentAsync(SafeTransaction transaction, int personId, PersonType type);
    }
}