using Auto_Parts_Store.Models;
using Auto_Parts_Store.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Services
{
    public interface IReportsService
    {
        Task<IList<StockAuditItem>>   GetStockAuditAsync(int? categoryId = null);
        Task<StockSummary>            GetStockSummaryAsync();
        Task<IList<StagnantItem>>     GetStagnantItemsAsync();
        Task<IList<LowStockItem>>     GetLowStockAlertsAsync();
        Task<IList<ProfitReportItem>> GetProfitReportAsync(DateTime from, DateTime to);
        Task<ProfitSummary>           GetProfitSummaryAsync(DateTime from, DateTime to);
        Task<IList<TopSellingItem>>   GetTopSellingItemsAsync(DateTime from, DateTime to, int topN = 20);
        Task<IList<SafeLedgerEntry>>  GetSafeLedgerAsync(DateTime from, DateTime to);
        Task<IList<PersonBalance>>    GetAccountsReceivableAsync();
        Task<IList<PersonBalance>>    GetAccountsPayableAsync();
    }

    public class ReportsService : IReportsService
    {
        private readonly IReportsRepository _repo;

        public ReportsService(IReportsRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public Task<IList<StockAuditItem>>   GetStockAuditAsync(int? categoryId = null) => _repo.GetStockAuditAsync(categoryId);
        public Task<IList<StagnantItem>>     GetStagnantItemsAsync()                    => _repo.GetStagnantItemsAsync();
        public Task<IList<LowStockItem>>     GetLowStockAlertsAsync()                  => _repo.GetLowStockAlertsAsync();
        public Task<IList<ProfitReportItem>> GetProfitReportAsync(DateTime f, DateTime t) => _repo.GetProfitReportAsync(f, t);
        public Task<IList<TopSellingItem>>   GetTopSellingItemsAsync(DateTime f, DateTime t, int n = 20) => _repo.GetTopSellingItemsAsync(f, t, n);
        public Task<IList<SafeLedgerEntry>>  GetSafeLedgerAsync(DateTime f, DateTime t)  => _repo.GetSafeLedgerAsync(f, t);
        public Task<IList<PersonBalance>>    GetAccountsReceivableAsync()               => _repo.GetAccountsReceivableAsync();
        public Task<IList<PersonBalance>>    GetAccountsPayableAsync()                  => _repo.GetAccountsPayableAsync();

        public async Task<StockSummary> GetStockSummaryAsync()
        {
            var items = await _repo.GetStockAuditAsync();
            return new StockSummary
            {
                TotalItems           = items.Count,
                TotalCostValue       = items.Sum(i => i.TotalCostValue),
                TotalSellingValue    = items.Sum(i => i.TotalSellingValue),
                TotalPotentialProfit = items.Sum(i => i.PotentialProfit),
                LowStockCount        = items.Count(i => i.StockStatus == "تحت الحد الأدنى"),
                OutOfStockCount      = items.Count(i => i.StockStatus == "نفد المخزون")
            };
        }

        public async Task<ProfitSummary> GetProfitSummaryAsync(DateTime from, DateTime to)
        {
            var items = await _repo.GetProfitReportAsync(from, to);
            if (items.Count == 0) return new ProfitSummary();

            decimal revenue = items.Sum(i => i.Revenue);
            decimal profit  = items.Sum(i => i.GrossProfit);
            return new ProfitSummary
            {
                TotalRevenue      = revenue,
                TotalCost         = revenue - profit,
                TotalGrossProfit  = profit,
                AvgMarginPct      = revenue > 0 ? Math.Round(profit / revenue * 100, 2) : 0,
                TotalTransactions = items.Count
            };
        }
    }
}
