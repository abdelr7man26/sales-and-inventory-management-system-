using Auto_Parts_Store.Helpers;
using Auto_Parts_Store.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Repositories
{
    public class SafeRepository : ISafeRepository
    {
        public async Task AddTransactionAsync(SafeTransaction trans)
        {
            string query = @"INSERT INTO SafeTransactions (Amount, TransactionType, Description, UserID, InvoiceID) 
                             VALUES (@amount, @type, @desc, @userId, @invId)";

            await DbHelper.ExecuteNonQueryAsync(query,
                new SqlParameter("@amount", trans.Amount),
                new SqlParameter("@type", trans.TransactionType),
                new SqlParameter("@desc", trans.Description),
                new SqlParameter("@userId", trans.UserID),
                new SqlParameter("@invId", (object)trans.InvoiceID ?? DBNull.Value));
        }

        public async Task<decimal> GetSafeBalanceAsync()
        {
            string query = @"SELECT 
                             ISNULL(SUM(CASE WHEN TransactionType = N'إيداع' THEN Amount ELSE 0 END), 0) - 
                             ISNULL(SUM(CASE WHEN TransactionType = N'سحب' THEN Amount ELSE 0 END), 0) 
                             FROM SafeTransactions";

            var result = await DbHelper.ExecuteScalarAsync(query);
            return result != DBNull.Value ? Convert.ToDecimal(result) : 0m;
        }

        public async Task<DataTable> GetSafeTransactionsHistoryAsync()
        {
            string query = @"SELECT s.ID as [رقم الحركة], s.Amount as [المبلغ], 
                                    s.TransactionType as [النوع], s.Description as [البيان], 
                                    s.TransactionDate as [التاريخ], u.UserName as [المستخدم]
                             FROM SafeTransactions s
                             LEFT JOIN Users u ON s.UserID = u.ID
                             ORDER BY s.TransactionDate DESC";

            return await DbHelper.ExecuteQueryAsync(query);
        }
    }
}