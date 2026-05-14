using Auto_Parts_Store.Helpers;
using Auto_Parts_Store.Models;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Repositories
{
    public class QuickPayRepository : IQuickPayRepository
    {
        public async Task<bool> ExecuteQuickPaymentAsync(SafeTransaction transaction, int personId, PersonType type)
        {
            using (var con = DbHelper.GetConnection())
            {
                await con.OpenAsync();
                using (var trans = con.BeginTransaction())
                {
                    try { 
                    string updateQuery = "";
                    if (type == PersonType.Customer)
                    {
                        updateQuery = (transaction.TransactionType == "إيداع")
                            ? "UPDATE customers SET Balance = Balance - @amt WHERE ID = @id"
                            : "UPDATE customers SET Balance = Balance + @amt WHERE ID = @id";
                    }
                    else
                    {
                        updateQuery = (transaction.TransactionType == "سحب")
                            ? "UPDATE supplieres SET Balance = Balance - @amt WHERE ID = @id"
                            : "UPDATE supplieres SET Balance = Balance + @amt WHERE ID = @id";
                    }

                    await DbHelper.ExecuteNonQueryWithTransactionAsync(updateQuery, con, trans,
                        new SqlParameter("@amt", transaction.Amount),
                        new SqlParameter("@id", personId));

                    string safeQuery = @"
                        INSERT INTO SafeTransactions
                            (Amount, TransactionType, Description, TransactionDate, UserID, PersonID)
                        VALUES
                            (@amt, @type, @desc, @date, @uid, @pid)";
                    await DbHelper.ExecuteNonQueryWithTransactionAsync(safeQuery, con, trans,
                        new SqlParameter("@amt",  transaction.Amount),
                        new SqlParameter("@type", transaction.TransactionType),
                        new SqlParameter("@desc", transaction.Description),
                        new SqlParameter("@date", transaction.TransactionDate),
                        new SqlParameter("@uid",  transaction.UserID),
                        new SqlParameter("@pid",  (object)personId ?? DBNull.Value));

                    trans.Commit();
                    return true;
                    }
                    catch { trans.Rollback(); throw; }
                }
            }
        }
    }
    
}