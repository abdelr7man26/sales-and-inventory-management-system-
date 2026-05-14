using Auto_Parts_Store.Helpers;
using Auto_Parts_Store.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Repositories
{
    public class ReportsRepository : IReportsRepository
    {
        // ── Inventory ──────────────────────────────────────────────

        public async Task<IList<StockAuditItem>> GetStockAuditAsync(int? categoryId = null)
        {
            string sql = categoryId.HasValue
                ? "SELECT * FROM vw_StockAudit WHERE Category = (SELECT categoryName FROM partscategories WHERE categoryID = @catId) ORDER BY PartName"
                : "SELECT * FROM vw_StockAudit ORDER BY Category, PartName";

            SqlParameter[] prms = categoryId.HasValue
                ? new[] { new SqlParameter("@catId", categoryId.Value) }
                : null;

            DataTable dt = await DbHelper.ExecuteQueryAsync(sql, prms);
            var list = new List<StockAuditItem>(dt.Rows.Count);
            foreach (DataRow r in dt.Rows)
                list.Add(new StockAuditItem
                {
                    PartID            = Convert.ToInt32(r["PartID"]),
                    PartName          = r["PartName"].ToString(),
                    PartNumber        = r["PartNumber"].ToString(),
                    Category          = r["Category"].ToString(),
                    Quantity          = Convert.ToInt32(r["Quantity"]),
                    PurchasePrice     = Convert.ToDecimal(r["PurchasePrice"]),
                    SellingPrice      = Convert.ToDecimal(r["SellingPrice"]),
                    TotalCostValue    = Convert.ToDecimal(r["TotalCostValue"]),
                    TotalSellingValue = Convert.ToDecimal(r["TotalSellingValue"]),
                    PotentialProfit   = Convert.ToDecimal(r["PotentialProfit"]),
                    MinimumStock      = Convert.ToInt32(r["MinimumStock"]),
                    StockStatus       = r["StockStatus"].ToString()
                });
            return list;
        }

        public async Task<IList<StagnantItem>> GetStagnantItemsAsync()
        {
            DataTable dt = await DbHelper.ExecuteQueryAsync(
                "SELECT * FROM vw_StagnantItems ORDER BY DaysSinceLastSale DESC");

            var list = new List<StagnantItem>(dt.Rows.Count);
            foreach (DataRow r in dt.Rows)
            {
                int days = Convert.ToInt32(r["DaysSinceLastSale"]);
                list.Add(new StagnantItem
                {
                    PartID            = Convert.ToInt32(r["PartID"]),
                    PartName          = r["PartName"].ToString(),
                    PartNumber        = r["PartNumber"].ToString(),
                    Category          = r["Category"].ToString(),
                    Quantity          = Convert.ToInt32(r["Quantity"]),
                    PurchasePrice     = Convert.ToDecimal(r["PurchasePrice"]),
                    TotalValue        = Convert.ToDecimal(r["TotalValue"]),
                    DaysSinceLastSale = days == 9999 ? -1 : days
                });
            }
            return list;
        }

        public async Task<IList<LowStockItem>> GetLowStockAlertsAsync()
        {
            DataTable dt = await DbHelper.ExecuteQueryAsync(
                "SELECT * FROM vw_LowStockAlert ORDER BY Shortage DESC");

            var list = new List<LowStockItem>(dt.Rows.Count);
            foreach (DataRow r in dt.Rows)
                list.Add(new LowStockItem
                {
                    PartID                     = Convert.ToInt32(r["PartID"]),
                    PartName                   = r["PartName"].ToString(),
                    PartNumber                 = r["PartNumber"].ToString(),
                    Category                   = r["Category"].ToString(),
                    CurrentStock               = Convert.ToInt32(r["CurrentStock"]),
                    MinimumStock               = Convert.ToInt32(r["MinimumStock"]),
                    Shortage                   = Convert.ToInt32(r["Shortage"]),
                    PurchasePrice              = Convert.ToDecimal(r["PurchasePrice"]),
                    EstimatedReplenishmentCost = Convert.ToDecimal(r["EstimatedReplenishmentCost"])
                });
            return list;
        }

        // ── Sales & profit ─────────────────────────────────────────

        public async Task<IList<ProfitReportItem>> GetProfitReportAsync(DateTime from, DateTime to)
        {
            string sql = @"SELECT * FROM vw_ProfitReport
                           WHERE SaleDate BETWEEN @from AND @to
                           ORDER BY SaleDate DESC, GrossProfit DESC";

            DataTable dt = await DbHelper.ExecuteQueryAsync(sql,
                new SqlParameter("@from", from.Date),
                new SqlParameter("@to",   to.Date));

            var list = new List<ProfitReportItem>(dt.Rows.Count);
            foreach (DataRow r in dt.Rows)
                list.Add(new ProfitReportItem
                {
                    InvoiceID       = Convert.ToInt32(r["InvoiceID"]),
                    SaleDate        = Convert.ToDateTime(r["SaleDate"]),
                    PartID          = Convert.ToInt32(r["PartID"]),
                    PartName        = r["PartName"].ToString(),
                    Category        = r["Category"].ToString(),
                    Quantity        = Convert.ToDecimal(r["Quantity"]),
                    SellingPrice    = Convert.ToDecimal(r["SellingPrice"]),
                    PurchasePrice   = Convert.ToDecimal(r["PurchasePrice"]),
                    Revenue         = Convert.ToDecimal(r["Revenue"]),
                    GrossProfit     = Convert.ToDecimal(r["GrossProfit"]),
                    ProfitMarginPct = Convert.ToDecimal(r["ProfitMarginPct"])
                });
            return list;
        }

        public async Task<IList<TopSellingItem>> GetTopSellingItemsAsync(DateTime from, DateTime to, int topN = 20)
        {
            string sql = @"
                SELECT TOP (@topN) ts.*
                FROM   vw_TopSellingItems ts
                WHERE  ts.PartID IN (
                    SELECT DISTINCT id.PartID
                    FROM   InvoiceDetails id
                    JOIN   Invoices i ON id.InvoiceID = i.ID
                    WHERE  i.invoiceType = N'مبيعات'
                      AND  CAST(i.Time AS DATE) BETWEEN @from AND @to)
                ORDER BY ts.TotalSold DESC";

            DataTable dt = await DbHelper.ExecuteQueryAsync(sql,
                new SqlParameter("@topN", topN),
                new SqlParameter("@from", from.Date),
                new SqlParameter("@to",   to.Date));

            var list = new List<TopSellingItem>(dt.Rows.Count);
            foreach (DataRow r in dt.Rows)
                list.Add(new TopSellingItem
                {
                    PartID       = Convert.ToInt32(r["PartID"]),
                    PartName     = r["PartName"].ToString(),
                    PartNumber   = r["PartNumber"].ToString(),
                    Category     = r["Category"].ToString(),
                    TotalSold    = Convert.ToInt32(r["TotalSold"]),
                    TotalRevenue = Convert.ToDecimal(r["TotalRevenue"]),
                    TotalProfit  = Convert.ToDecimal(r["TotalProfit"]),
                    AvgMarginPct = Convert.ToDecimal(r["AvgMarginPct"])
                });
            return list;
        }

        // ── Financial ──────────────────────────────────────────────

        public async Task<IList<SafeLedgerEntry>> GetSafeLedgerAsync(DateTime from, DateTime to)
        {
            string sql = @"SELECT * FROM vw_SafeLedger
                           WHERE CAST(TransactionDate AS DATE) BETWEEN @from AND @to
                           ORDER BY ID";

            DataTable dt = await DbHelper.ExecuteQueryAsync(sql,
                new SqlParameter("@from", from.Date),
                new SqlParameter("@to",   to.Date));

            var list = new List<SafeLedgerEntry>(dt.Rows.Count);
            foreach (DataRow r in dt.Rows)
                list.Add(new SafeLedgerEntry
                {
                    ID              = Convert.ToInt32(r["ID"]),
                    TransactionDate = Convert.ToDateTime(r["TransactionDate"]),
                    TransactionType = r["TransactionType"].ToString(),
                    Amount          = Convert.ToDecimal(r["Amount"]),
                    Description     = r["Description"]?.ToString(),
                    UserName        = r["UserName"].ToString(),
                    InvoiceType     = r["InvoiceType"].ToString(),
                    InvoiceID       = r["InvoiceID"] == DBNull.Value ? (int?)null : Convert.ToInt32(r["InvoiceID"]),
                    RunningBalance  = Convert.ToDecimal(r["RunningBalance"])
                });
            return list;
        }

        public async Task<IList<PersonBalance>> GetAccountsReceivableAsync()
        {
            return MapBalances(await DbHelper.ExecuteQueryAsync(
                "SELECT * FROM vw_AccountsReceivable WHERE OutstandingBalance > 0 ORDER BY OutstandingBalance DESC"));
        }

        public async Task<IList<PersonBalance>> GetAccountsPayableAsync()
        {
            return MapBalances(await DbHelper.ExecuteQueryAsync(
                "SELECT * FROM vw_AccountsPayable WHERE OutstandingBalance > 0 ORDER BY OutstandingBalance DESC"));
        }

        private static IList<PersonBalance> MapBalances(DataTable dt)
        {
            var list = new List<PersonBalance>(dt.Rows.Count);
            foreach (DataRow r in dt.Rows)
                list.Add(new PersonBalance
                {
                    PersonID           = Convert.ToInt32(r["PersonID"]),
                    PersonName         = r["PersonName"].ToString(),
                    Phone              = r["Phone"]?.ToString(),
                    Address            = r["Address"]?.ToString(),
                    OutstandingBalance = Convert.ToDecimal(r["OutstandingBalance"]),
                    InvoiceCount       = Convert.ToInt32(r["InvoiceCount"])
                });
            return list;
        }
    }
}
