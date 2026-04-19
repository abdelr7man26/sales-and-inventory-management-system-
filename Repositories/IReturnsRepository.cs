
using Auto_Parts_Store.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Auto_Parts_Store.Repositories
{
    public interface IReturnsRepository
    {
        Task<(InvoiceHeader Invoice, string PersonName)> GetInvoiceHeaderAsync(int invoiceId);
        Task<List<InvoiceDetail>> GetInvoiceDetailsAsync(int invoiceId);
        Task<bool> SaveReturnTransactionAsync(ReturnHeaderDTO header, List<ReturnDetailDTO> details);
    }
}