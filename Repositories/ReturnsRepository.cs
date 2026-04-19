using Auto_Parts_Store.Helpers;
using Auto_Parts_Store.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Auto_Parts_Store.Repositories
{
    public class ReturnsRepository : IReturnsRepository
    {
        public async Task<(InvoiceHeader Invoice, string PersonName)> GetInvoiceHeaderAsync(int invoiceId)
        {
            string query = @"
              SELECT i.*, p.PersonName 
              FROM Invoices i
              LEFT JOIN customers c ON i.customerID = c.ID
              LEFT JOIN supplieres s ON i.supplierID = s.ID
              LEFT JOIN person p ON (c.ID = p.ID OR s.ID = p.ID)
              WHERE i.ID = @id";

            SqlParameter[] parameters = { new SqlParameter("@id", invoiceId) };
            DataTable dt = await DbHelper.ExecuteQueryAsync(query, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                var invoice = new InvoiceHeader
                {
                    Time = Convert.ToDateTime(dr["Time"]),

                    PaymentMethod = dr["PaymentMethod"]?.ToString(),

                    TotalAmount = Convert.ToDecimal(dr["TotalAmount"]),

                    UserID = Convert.ToInt32(dr["UserID"]),

                    CustomerID = dr["customerID"] != DBNull.Value ? (int?)dr["customerID"] : null,
                    SupplierID = dr["supplierID"] != DBNull.Value ? (int?)dr["supplierID"] : null,

                    InvoiceType = dr["invoiceType"].ToString(),

                    PaidAmount = Convert.ToDecimal(dr["paidamount"])
                };

                string personName = dr["PersonName"] != DBNull.Value ? dr["PersonName"].ToString() : "غير محدد";

                return (invoice, personName);
            }

            return (null, null);
        }
        public async Task<List<InvoiceDetail>> GetInvoiceDetailsAsync(int invoiceId)
        {
            string query = @"
            SELECT d.PartID, p.PartName, p.PartNumber, 
               (d.Quantity - ISNULL(RD_Sum.TotalReturned, 0)) AS RemainingQty, 
               d.PartPrice, d.Total 
            FROM InvoiceDetails d
            INNER JOIN Parts p ON d.PartID = p.PartID
            LEFT JOIN (
            SELECT rd.PartID, SUM(rd.Quantity) AS TotalReturned
            FROM ReturnDetails rd
            INNER JOIN Returns r ON rd.ReturnID = r.ReturnID
            WHERE r.InvoiceID = @id
            GROUP BY rd.PartID
                ) RD_Sum ON d.PartID = RD_Sum.PartID
            WHERE d.InvoiceID = @id AND (d.Quantity - ISNULL(RD_Sum.TotalReturned, 0)) > 0";

            DataTable dt = await DbHelper.ExecuteQueryAsync(query, new SqlParameter("@id", invoiceId));
            List<InvoiceDetail> details = new List<InvoiceDetail>();

            foreach (DataRow dr in dt.Rows)
            {
                details.Add(new InvoiceDetail
                {
                    PartID = Convert.ToInt32(dr["PartID"]),
                    PartName = dr["PartName"].ToString(),
                    PartNumber = dr["PartNumber"].ToString(),
                    Quantity = Convert.ToDecimal(dr["RemainingQty"]),
                    PurchasePrice = Convert.ToDecimal(dr["PartPrice"]),
                    Total = Convert.ToDecimal(dr["Total"])
                });
            }
            return details;
        }
        public async Task<bool> SaveReturnTransactionAsync(ReturnHeaderDTO header, List<ReturnDetailDTO> details)
        {
            using (SqlConnection connection = DbHelper.GetConnection())
            {
                await connection.OpenAsync();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string returnQuery = @"INSERT INTO Returns (InvoiceID, ReturnDate, PaymentMethod, TotalRefundedAmount, UserID, Notes) 
                                     VALUES (@invId, GETDATE(), @payMethod, @total, @userId, @notes);
                                     SELECT SCOPE_IDENTITY();";

                        SqlCommand cmd = new SqlCommand(returnQuery, connection, transaction);
                        cmd.Parameters.AddWithValue("@invId", header.InvoiceID);
                        cmd.Parameters.AddWithValue("@payMethod", header.PaymentMethod);
                        cmd.Parameters.AddWithValue("@total", header.TotalAmount);
                        cmd.Parameters.AddWithValue("@userId", header.UserID);
                        cmd.Parameters.AddWithValue("@notes", (object)header.Notes ?? DBNull.Value);

                        int returnId = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                        foreach (var item in details)
                        {
                            string detailQuery = @"INSERT INTO ReturnDetails (ReturnID, PartID, Quantity, RefundAmount) 
                                         VALUES (@retId, @partId, @qty, @amount)";
                            SqlCommand cmdDet = new SqlCommand(detailQuery, connection, transaction);
                            cmdDet.Parameters.AddWithValue("@retId", returnId);
                            cmdDet.Parameters.AddWithValue("@partId", item.PartID);
                            cmdDet.Parameters.AddWithValue("@qty", item.Quantity);
                            cmdDet.Parameters.AddWithValue("@amount", item.RefundAmount);
                            await cmdDet.ExecuteNonQueryAsync();

                            decimal qtyForLog = item.Quantity;

                            decimal qtyForUpdateParts = (header.ReturnType == "بيع") ? item.Quantity : -item.Quantity;

                            string invTransQuery = @"
                            INSERT INTO inventoryTransactions (PartId, Quantity, TransactionsType, Date, notes, userId) 
                            VALUES (@pId, @qtyLog, @type, GETDATE(), @note, @uId);

                            UPDATE Parts SET Quantity = Quantity + @qtyUpdate WHERE PartID = @pId";

                            SqlCommand cmdInv = new SqlCommand(invTransQuery, connection, transaction);
                            cmdInv.Parameters.AddWithValue("@pId", item.PartID);
                            cmdInv.Parameters.AddWithValue("@qtyLog", qtyForLog);          
                            cmdInv.Parameters.AddWithValue("@qtyUpdate", qtyForUpdateParts); 
                            cmdInv.Parameters.AddWithValue("@type", "مرتجع " + header.ReturnType);
                            cmdInv.Parameters.AddWithValue("@note", "مرتجع رقم: " + returnId);
                            cmdInv.Parameters.AddWithValue("@uId", header.UserID);

                            await cmdInv.ExecuteNonQueryAsync();
                        }

                        if (header.PaymentMethod == "كاش")
                        {
                            string safeQuery = @"INSERT INTO SafeTransactions (Amount, TransactionType, Description, TransactionDate, UserID, InvoiceID) 
                     VALUES (@amt, @type, @desc, GETDATE(), @uId, @invId)";

                         
                            string safeTransType = (header.ReturnType == "بيع") ? "سحب" : "إيداع";

                            decimal safeAmount = (header.ReturnType == "بيع") ? -header.TotalAmount : header.TotalAmount;

                            SqlCommand cmdSafe = new SqlCommand(safeQuery, connection, transaction);
                            cmdSafe.Parameters.AddWithValue("@amt", safeAmount);
                            cmdSafe.Parameters.AddWithValue("@type", safeTransType); 

                            string returnTypeText = (header.ReturnType == "بيع" ? "مبيعات" : "مشتريات");
                            cmdSafe.Parameters.AddWithValue("@desc", $"مرتجع {returnTypeText} رقم {returnId} - فاتورة أصلية {header.InvoiceID}");

                            cmdSafe.Parameters.AddWithValue("@uId", header.UserID);
                            cmdSafe.Parameters.AddWithValue("@invId", header.InvoiceID); 

                            await cmdSafe.ExecuteNonQueryAsync();
                        }

                        else 
                        {
                            string updateBalanceQuery = "";
                            if (header.ReturnType == "بيع")
                            {
                                updateBalanceQuery = "UPDATE customers SET Balance = Balance - @total WHERE ID = (SELECT customerID FROM Invoices WHERE ID = @invId)";
                            }
                            else
                            {
                                updateBalanceQuery = "UPDATE supplieres SET Balance = Balance - @total WHERE ID = (SELECT supplierID FROM Invoices WHERE ID = @invId)";
                            }

                            SqlCommand cmdBalance = new SqlCommand(updateBalanceQuery, connection, transaction);
                            cmdBalance.Parameters.AddWithValue("@total", header.TotalAmount);
                            cmdBalance.Parameters.AddWithValue("@invId", header.InvoiceID);
                            await cmdBalance.ExecuteNonQueryAsync();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}


