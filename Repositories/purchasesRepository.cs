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
    public class PurchasesRepository : IPurchasesRepository
    {
        public async Task<bool> SavePurchaseInvoiceAsync(InvoiceHeader header, List<InvoiceDetail> details)
        {
            using (var con = DbHelper.GetConnection())
            {
                await con.OpenAsync();
                using (var trans = con.BeginTransaction())
                {
                    try
                    {
                        string invQuery = @"INSERT INTO Invoices (Time, PaymentMethod, TotalAmount, UserID, supplierID, invoiceType, paidamount) 
                                   VALUES (GETDATE(), @payMethod, @total, @user, @sup, 'توريد', @paid);
                                   SELECT SCOPE_IDENTITY();";

                        var invId = Convert.ToInt32(await DbHelper.ExecuteScalarWithTransactionAsync(invQuery, con, trans,
                            new SqlParameter("@payMethod", header.PaymentMethod),
                            new SqlParameter("@total", header.TotalAmount),
                            new SqlParameter("@user", header.UserID),
                            new SqlParameter("@sup", header.SupplierID),
                            new SqlParameter("@paid", header.PaidAmount)));

                        decimal debt = header.TotalAmount - header.PaidAmount;
                        if (debt > 0 && header.SupplierID != 9)
                        {
                            string supQuery = "UPDATE supplieres SET Balance = ISNULL(balance, 0) + @debt WHERE ID = @supID";
                            await DbHelper.ExecuteNonQueryWithTransactionAsync(supQuery, con, trans,
                                new SqlParameter("@debt", debt),
                                new SqlParameter("@supID", header.SupplierID));
                        }

                        foreach (var item in details)
                        {
                            int pID = item.PartID;

                            if (pID == 0)
                            {
                                string partQuery = @"INSERT INTO Parts (PartName, PartNumber, PurchasePrice, SellingPrice, Quantity, MinimumStock, CategoryID) 
                                           VALUES (@n, @num, @pp, @sp, 0, 5, @cat);
                                           SELECT SCOPE_IDENTITY();";
                                pID = Convert.ToInt32(await DbHelper.ExecuteScalarWithTransactionAsync(partQuery, con, trans,
                                    new SqlParameter("@n", item.PartName),
                                    new SqlParameter("@num", item.PartNumber),
                                    new SqlParameter("@pp", item.PurchasePrice),
                                    new SqlParameter("@sp", item.SellingPrice),
                                    new SqlParameter("@cat", item.CategoryID)));
                            }
                            else
                            {
                                string updatePartPricesQuery = @"UPDATE Parts 
                                    SET PurchasePrice = @pp, 
                                        SellingPrice = @sp 
                                    WHERE PartID = @pid";

                                await DbHelper.ExecuteNonQueryWithTransactionAsync(updatePartPricesQuery, con, trans,
                                    new SqlParameter("@pp", item.PurchasePrice),
                                    new SqlParameter("@sp", item.SellingPrice),
                                    new SqlParameter("@pid", pID));

                            }

                            string detQuery = "INSERT INTO InvoiceDetails (Quantity, PartPrice, Total, PartID, InvoiceID) VALUES (@qty, @prc, @ttl, @pid, @invid)";
                            await DbHelper.ExecuteNonQueryWithTransactionAsync(detQuery, con, trans,
                                new SqlParameter("@qty", item.Quantity),
                                new SqlParameter("@prc", item.PurchasePrice),
                                new SqlParameter("@ttl", item.Total),
                                new SqlParameter("@pid", pID),
                                new SqlParameter("@invid", invId));

                            await DbHelper.ExecuteNonQueryWithTransactionAsync("UPDATE Parts SET Quantity = Quantity + @qty WHERE PartID = @pid", con, trans,
                                new SqlParameter("@qty", item.Quantity), new SqlParameter("@pid", pID));

                            string transQuery = "INSERT INTO inventoryTransactions (TransactionsType, Quantity, Date, notes, PartId, userId) VALUES ('توريد', @qty, GETDATE(), @note, @pid, @uid)";
                            await DbHelper.ExecuteNonQueryWithTransactionAsync(transQuery, con, trans,
                                new SqlParameter("@qty", item.Quantity),
                                new SqlParameter("@note", "فاتورة مشتريات رقم: " + invId),
                                new SqlParameter("@pid", pID),
                                new SqlParameter("@uid", header.UserID));
                        }

                        trans.Commit();
                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
