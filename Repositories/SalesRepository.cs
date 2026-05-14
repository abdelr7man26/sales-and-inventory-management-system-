using Auto_Parts_Store.Models;
using Auto_Parts_Store.Repositories;
using Auto_Parts_Store.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Repositories
{
    public class SalesRepository : ISalesRepository
    {
        public async Task<int> SaveInvoiceAsync(InvoiceHeader header, List<InvoiceDetail> details)
        {
            int invoiceID = 0;

            using (SqlConnection con = DbHelper.GetConnection())
            {
                await con.OpenAsync();
                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    // =============================
                    // Insert Invoice
                    // =============================

                    string invQuery = @"INSERT INTO Invoices 
                                    (Time, PaymentMethod, TotalAmount, UserID, customerID, invoiceType, paidamount) 
                                    VALUES 
                                    (@time, @payMethod, @total, @userID, @custID, @invType, @paid); 
                                    SELECT SCOPE_IDENTITY();";

                    invoiceID = Convert.ToInt32(
                        await DbHelper.ExecuteScalarWithTransactionAsync(
                            invQuery,
                            con,
                            trans,

                            new SqlParameter("@time", SqlDbType.DateTime) { Value = header.Time },

                            new SqlParameter("@payMethod", SqlDbType.NVarChar) { Value = header.PaymentMethod },

                            new SqlParameter("@total", SqlDbType.Decimal) { Value = header.TotalAmount },

                            new SqlParameter("@userID", SqlDbType.Int) { Value = header.UserID },

                            new SqlParameter("@custID", SqlDbType.Int)
                            {
                                Value = (header.CustomerID == null || header.CustomerID == 0)
                                         ? 8
                                         : header.CustomerID.Value
                            },

                            new SqlParameter("@invType", SqlDbType.NVarChar) { Value = header.InvoiceType },

                            new SqlParameter("@paid", SqlDbType.Decimal) { Value = header.PaidAmount }
                        )
                    );

                    // =============================
                    // Customer Debt
                    // =============================

                    decimal debt = header.TotalAmount - header.PaidAmount;

                    if (debt > 0 && header.CustomerID.HasValue && header.CustomerID.Value != 0)
                    {
                        string updateCustQuery =
                            "UPDATE customers SET balance = ISNULL(balance, 0) + @debt WHERE ID = @custID";

                        await DbHelper.ExecuteNonQueryWithTransactionAsync(
                            updateCustQuery,
                            con,
                            trans,

                            new SqlParameter("@debt", SqlDbType.Decimal) { Value = debt },

                            new SqlParameter("@custID", SqlDbType.Int) { Value = header.CustomerID.Value }
                        );
                    }

                    // =============================
                    // Invoice Details
                    // =============================

                    foreach (var item in details)
                    {
                        await InsertInvoiceDetails(con, trans, item, invoiceID);

                        await InsertInventoryTransaction(con, trans, item, header.UserID, invoiceID);

                        await UpdateStock(con, trans, item);
                    }

                    trans.Commit();

                    return invoiceID;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("حدث خطأ أثناء الحفظ: " + ex.Message, ex);
                }
            }
        }

        // =========================================
        // Insert Invoice Details
        // =========================================

        private async Task InsertInvoiceDetails(
            SqlConnection con,
            SqlTransaction trans,
            InvoiceDetail item,
            int invoiceID)
        {
            string detailsQuery = @"INSERT INTO InvoiceDetails 
                                   (Quantity, PartPrice, Total, PartID, InvoiceID) 
                                   VALUES (@qty, @price, @total, @pID, @invID)";

            await DbHelper.ExecuteNonQueryWithTransactionAsync(
                detailsQuery,
                con,
                trans,

                new SqlParameter("@qty", SqlDbType.Int) { Value = item.Quantity },

                new SqlParameter("@price", SqlDbType.Decimal) { Value = item.SellingPrice },

                new SqlParameter("@total", SqlDbType.Decimal) { Value = item.Total },

                new SqlParameter("@pID", SqlDbType.Int) { Value = item.PartID },

                new SqlParameter("@invID", SqlDbType.Int) { Value = invoiceID }
            );
        }

        // =========================================
        // Inventory Transactions
        // =========================================

        private async Task InsertInventoryTransaction(
            SqlConnection con,
            SqlTransaction trans,
            InvoiceDetail item,
            int userId,
            int invoiceID)
        {
            string transQuery = @"INSERT INTO inventoryTransactions 
                                  (TransactionsType, Quantity, Date, notes, PartId, userId) 
                                  VALUES (@tType, @tQty, @tDate, @notes, @pId, @uId)";

            await DbHelper.ExecuteNonQueryWithTransactionAsync(
                transQuery,
                con,
                trans,

                new SqlParameter("@tType", SqlDbType.NVarChar)
                { Value = "صرف مبيعات" },

                new SqlParameter("@tQty", SqlDbType.Int)
                { Value = item.Quantity },

                new SqlParameter("@tDate", SqlDbType.DateTime)
                { Value = DateTime.Now },

                new SqlParameter("@notes", SqlDbType.NVarChar)
                { Value = "فاتورة مبيعات رقم " + invoiceID },

                new SqlParameter("@pId", SqlDbType.Int)
                { Value = item.PartID },

                new SqlParameter("@uId", SqlDbType.Int)
                { Value = userId }
            );
        }

        // =========================================
        // Update Stock
        // =========================================

        private async Task UpdateStock(
            SqlConnection con,
            SqlTransaction trans,
            InvoiceDetail item)
        {
            // Guard against negative stock: the UPDATE targets only rows where
            // Quantity >= requested amount. If 0 rows are affected the part
            // is out of stock and we throw before the transaction commits.
            const string updateStockQuery =
                "UPDATE Parts SET Quantity = Quantity - @Qty WHERE PartID = @PartID AND Quantity >= @Qty";

            int affected = await DbHelper.ExecuteNonQueryWithTransactionAsync(
                updateStockQuery,
                con,
                trans,
                new SqlParameter("@Qty",    SqlDbType.Int) { Value = item.Quantity },
                new SqlParameter("@PartID", SqlDbType.Int) { Value = item.PartID }
            );

            if (affected == 0)
                throw new InvalidOperationException(
                    $"الكمية المطلوبة ({item.Quantity}) غير متوفرة للقطعة: {item.PartName}");
        }

        // =========================================
        // Load Customers
        // =========================================

        public async Task<DataTable> LoadCustomersAsync()
        {
            string query = @"SELECT c.ID, p.PersonName 
                             FROM customers c 
                             JOIN person p ON c.ID = p.ID where P.isdeleted = 0";

            return await DbHelper.ExecuteQueryAsync(query);
        }

        // =========================================
        // Get Stock Balance
        // =========================================

        public async Task<decimal> GetStockBalanceAsync(int partId)
        {
            const string query = @"
                SELECT ISNULL(SUM(CASE
                    WHEN TransactionsType IN (N'توريد', N'مرتجع بيع')         THEN Quantity
                    WHEN TransactionsType IN (N'صرف مبيعات', N'مرتجع مشتريات') THEN -Quantity
                    ELSE 0 END), 0)
                FROM inventoryTransactions
                WHERE PartId = @pId";

            object result = await DbHelper.ExecuteScalarAsync(
                query,
                new SqlParameter("@pId", SqlDbType.Int) { Value = partId });

            return result == null || result == DBNull.Value ? 0m : Convert.ToDecimal(result);
        }


        // =========================================
        // get statement
        // =========================================


        public async Task<DataTable> GetAccountStatementAsync(int personId, DateTime from, DateTime to, string personName)
        {
            // SafeTransactions now joins on PersonID (FK added in SDatabase.sql migration).
            // The personName parameter is kept in the signature for UI display only;
            // it is no longer used in the WHERE clause to prevent name-overlap leakage.
            const string query = @"
                SELECT
                    CAST(i.ID       AS INT)            AS [رقم],
                    CAST(i.Time     AS DATE)           AS [التاريخ],
                    N'فاتورة ' + i.invoiceType         AS [المصدر],
                    CAST(i.TotalAmount   AS DECIMAL(18,2)) AS [قيمة الفاتورة],
                    CAST(i.paidamount    AS DECIMAL(18,2)) AS [المدفوع كاش],
                    CAST(0 AS DECIMAL(18,2))           AS [قيمة المرتجع],
                    CAST(0 AS DECIMAL(18,2))           AS [مدفوع من مرتجع]
                FROM Invoices i
                WHERE (i.customerID = @id OR i.supplierID = @id)
                  AND CAST(i.Time AS DATE) BETWEEN @from AND @to

                UNION ALL

                SELECT
                    CAST(r.ReturnID AS INT)                AS [رقم],
                    CAST(r.ReturnDate AS DATE)             AS [التاريخ],
                    N'مرتجع ' + i.invoiceType             AS [المصدر],
                    CAST(0 AS DECIMAL(18,2))               AS [قيمة الفاتورة],
                    CAST(0 AS DECIMAL(18,2))               AS [المدفوع كاش],
                    CAST(r.TotalRefundedAmount AS DECIMAL(18,2)) AS [قيمة المرتجع],
                    CAST(r.CashReturned        AS DECIMAL(18,2)) AS [مدفوع من مرتجع]
                FROM Returns r
                INNER JOIN Invoices i ON r.InvoiceID = i.ID
                WHERE (i.customerID = @id OR i.supplierID = @id)
                  AND CAST(r.ReturnDate AS DATE) BETWEEN @from AND @to

                UNION ALL

                SELECT
                    CAST(s.ID AS INT)                      AS [رقم],
                    CAST(s.TransactionDate AS DATE)        AS [التاريخ],
                    N'سداد سريع'                           AS [المصدر],
                    CAST(0 AS DECIMAL(18,2))               AS [قيمة الفاتورة],
                    CAST(CASE WHEN s.Description LIKE N'%تسوية%' THEN 0 ELSE s.Amount END AS DECIMAL(18,2)) AS [المدفوع كاش],
                    CAST(0 AS DECIMAL(18,2))               AS [قيمة المرتجع],
                    CAST(CASE WHEN s.Description LIKE N'%تسوية%' THEN s.Amount ELSE 0 END AS DECIMAL(18,2)) AS [مدفوع من مرتجع]
                FROM SafeTransactions s
                WHERE s.PersonID = @id
                  AND CAST(s.TransactionDate AS DATE) BETWEEN @from AND @to

                ORDER BY [التاريخ] ASC";

            SqlParameter[] parameters =
            {
                new SqlParameter("@id",   personId),
                new SqlParameter("@from", from.Date),
                new SqlParameter("@to",   to.Date)
            };

            return await DbHelper.ExecuteQueryAsync(query, parameters);
        }

        public async Task<DataTable> GetInvoiceDetailsAsync(int invId, string type)
        {
            string query = "";

            if (type.Contains("فاتورة"))
            {
                query = @"SELECT p.PartName AS [اسم الصنف], p.PartNumber AS [رقم الصنف], 
                         d.Quantity AS [الكمية], d.PartPrice AS [السعر], d.Total AS [الإجمالي]
                  FROM InvoiceDetails d 
                  INNER JOIN Parts p ON d.PartID = p.PartID 
                  WHERE d.InvoiceID = @id";
            }
            else 
            {
                query = @"SELECT p.PartName AS [اسم الصنف], p.PartNumber AS [رقم الصنف], 
                         rd.Quantity AS [الكمية المرتجعة], (rd.RefundAmount / rd.Quantity) AS [السعر] , rd.RefundAmount AS [الإجمالي]
                  FROM ReturnDetails rd 
                  INNER JOIN Parts p ON rd.PartID = p.PartID 
                  WHERE rd.ReturnID = @id";
            }

            return await DbHelper.ExecuteQueryAsync(query, new SqlParameter("@ID", invId));
        }
    }
    

}