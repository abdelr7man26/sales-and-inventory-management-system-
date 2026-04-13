using Auto_Parts_Store.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Repositories
{
    public interface ISalesRepository
    {
        Task<int> SaveInvoiceAsync(InvoiceHeader header, List<InvoiceDetail> details);

        Task<DataTable> LoadCustomersAsync();

        Task<decimal> GetStockBalanceAsync(int partId);
        Task<DataTable> GetAccountStatementAsync(int personId, DateTime from, DateTime to);
        Task<DataTable> GetInvoiceDetailsAsync(int invId);
    }
}