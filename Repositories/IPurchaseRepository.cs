using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Auto_Parts_Store.Models;

namespace Auto_Parts_Store.Repositories
{
    public interface IPurchaseRepository
    {

        Task<int> SavePurchaseInvoiceAsync(InvoiceHeader header, List<InvoiceDetail> details);
    }
}