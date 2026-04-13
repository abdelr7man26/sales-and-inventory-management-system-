using Auto_Parts_Store.Models;
using Auto_Parts_Store.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        public async Task<int> SavePurchaseInvoiceAsync(InvoiceHeader header, List<InvoiceDetail> details)
        {
            int invoiceID = 0;

            using (SqlConnection con = DbHelper.GetConnection())
            {
                await con.OpenAsync();
                SqlTransaction trans = con.BeginTransaction();

                try
                {

                    string invQuery = @"INSERT INTO Invoices 
                                    (Time, PaymentMethod, TotalAmount, UserID, supplierID, invoiceType, paidamount) 
                                    VALUES 
                                    (@time, @payMethod, @total, @userID, @suppID, @invType, @paid); 
                                    SELECT SCOPE_IDENTITY();";

                    invoiceID = Convert.ToInt32(
                        await DbHelper.ExecuteScalarWithTransactionAsync(
                            invQuery, con, trans,
                            new SqlParameter("@time", header.Time),
                            new SqlParameter("@payMethod", header.PaymentMethod),
                            new SqlParameter("@total", header.TotalAmount),
                            new SqlParameter("@userID", header.UserID),
                            new SqlParameter("@suppID", (object)header.CustomerID ?? DBNull.Value),
                            new SqlParameter("@invType", "توريد"), 
                            new SqlParameter("@paid", header.PaidAmount)
                        ));

                    foreach (var item in details)
                    {
                        string detailQuery = @"INSERT INTO InvoiceDetails 
                                             (InvoiceID, PartID, Quantity, PartPrice, Total) 
                                             VALUES (@invID, @partID, @qty, @price, @total)";

                        await DbHelper.ExecuteNonQueryWithTransactionAsync(
                            detailQuery, con, trans,
                            new SqlParameter("@invID", invoiceID),
                            new SqlParameter("@partID", item.PartID),
                            new SqlParameter("@qty", item.Quantity),
                            new SqlParameter("@price", item.PartPrice),
                            new SqlParameter("@total", item.Total)
                        );

                        string updateStockQuery = "UPDATE Parts SET Quantity = Quantity + @qty WHERE PartID = @partID";

                        await DbHelper.ExecuteNonQueryWithTransactionAsync(
                            updateStockQuery, con, trans,
                            new SqlParameter("@qty", item.Quantity),
                            new SqlParameter("@partID", item.PartID)
                        );
                    }

                    trans.Commit();
                    return invoiceID;
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw; 
                }
            }
        }
    }
}