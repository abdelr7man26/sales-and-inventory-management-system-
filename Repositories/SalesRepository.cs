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
            string updateStockQuery =
                "UPDATE Parts SET Quantity = Quantity - @Qty WHERE PartID = @PartID";

            await DbHelper.ExecuteNonQueryWithTransactionAsync(
                updateStockQuery,
                con,
                trans,

                new SqlParameter("@Qty", SqlDbType.Int)
                { Value = item.Quantity },

                new SqlParameter("@PartID", SqlDbType.Int)
                { Value = item.PartID }
            );
        }

        // =========================================
        // Load Customers
        // =========================================

        public async Task<DataTable> LoadCustomersAsync()
        {
            string query = @"SELECT c.ID, p.PersonName 
                             FROM customers c 
                             JOIN person p ON c.ID = p.ID where P.isdeleted = 0 and c.ID <> 8";

            return await DbHelper.ExecuteQueryAsync(query);
        }

        // =========================================
        // Get Stock Balance
        // =========================================

        public async Task<decimal> GetStockBalanceAsync(int partId)
        {
            string query = @"SELECT ISNULL(SUM(CASE 
                                WHEN TransactionsType = 'توريد' THEN Quantity 
                                WHEN TransactionsType = 'صرف مبيعات' THEN -Quantity 
                                ELSE 0 END), 0) 
                             FROM inventoryTransactions 
                             WHERE PartId = @pId";

            object result = await DbHelper.ExecuteQueryAsync(
                query,
                new SqlParameter("@pId", SqlDbType.Int) { Value = partId });

            DataTable dt = (DataTable)result;

            if (dt.Rows.Count == 0)
                return 0;

            return Convert.ToDecimal(dt.Rows[0][0]);
        }


        // =========================================
        // get statement
        // =========================================


        public async Task<DataTable> GetAccountStatementAsync(int personId, DateTime from, DateTime to)
        {
            string query = @"
        SELECT 
            Time AS [التاريخ], 
            ID AS [رقم الفاتورة], 
            invoiceType AS [النوع], 
            TotalAmount AS [الإجمالي], 
            paidamount AS [المدفوع], 
            (TotalAmount - paidamount) AS [المتبقي]
        FROM Invoices 
        WHERE (customerID = @id OR supplierID = @id) 
          AND CAST(Time AS DATE) BETWEEN @from AND @to
        ORDER BY Time DESC";

            SqlParameter[] parameters = {
        new SqlParameter("@id", SqlDbType.Int) { Value = personId },
        new SqlParameter("@from", SqlDbType.Date) { Value = from.Date },
        new SqlParameter("@to", SqlDbType.Date) { Value = to.Date }
    };

            return await DbHelper.ExecuteQueryAsync(query, parameters);
        }
        
        public async Task<DataTable> GetInvoiceDetailsAsync(int invId)
        {
            string query = @"SELECT p.PartName AS [اسم القطعة], d.Quantity AS [الكمية], 
                            d.PartPrice AS [السعر], d.Total AS [الإجمالي]
                     FROM InvoiceDetails d
                     INNER JOIN Parts p ON d.PartID = p.PartID
                     WHERE d.InvoiceID = @invID";

            return await DbHelper.ExecuteQueryAsync(query, new SqlParameter("@invID", invId));
        }
    }

}