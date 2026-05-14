using Auto_Parts_Store.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Repositories
{
    public interface IReportsRepository
    {
        // Inventory
        Task<IList<StockAuditItem>>  GetStockAuditAsync(int? categoryId = null);
        Task<IList<StagnantItem>>    GetStagnantItemsAsync();
        Task<IList<LowStockItem>>    GetLowStockAlertsAsync();

        // Sales & profit
        Task<IList<ProfitReportItem>> GetProfitReportAsync(DateTime from, DateTime to);
        Task<IList<TopSellingItem>>   GetTopSellingItemsAsync(DateTime from, DateTime to, int topN = 20);

        // Financial
        Task<IList<SafeLedgerEntry>> GetSafeLedgerAsync(DateTime from, DateTime to);
        Task<IList<PersonBalance>>   GetAccountsReceivableAsync();
        Task<IList<PersonBalance>>   GetAccountsPayableAsync();
    }
}
