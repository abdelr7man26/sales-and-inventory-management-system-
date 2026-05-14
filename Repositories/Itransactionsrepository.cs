// File name kept for backwards-compatibility (was originally Itransactionsrepository.cs).
// The interface it contains is IQuickPayRepository — implemented by QuickPayRepository.
using Auto_Parts_Store.Models;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Repositories
{
    public interface IQuickPayRepository
    {
        Task<bool> ExecuteQuickPaymentAsync(SafeTransaction transaction, int personId, PersonType type);
    }
}