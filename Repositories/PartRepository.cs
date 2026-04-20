using Auto_Parts_Store.Helpers;
using Auto_Parts_Store.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Repositories
{
    public class PartRepository : IPartRepository
    {
        // ================================
        // User Authentication
        // ================================

        public async Task<DataTable> GetUserByCredentialsAsync(string username, string password)
        {
            string query = @"SELECT u.ID, u.UserName, u.Role, p.PersonName 
                             FROM Users u INNER JOIN person p ON u.ID = p.ID 
                             WHERE u.Username = @user AND u.password = @pass AND P.IsDeleted = 0";

            return await DbHelper.ExecuteQueryAsync(query,
                new SqlParameter("@user", username),
                new SqlParameter("@pass", password));
        }

        // ================================
        // Dashboard
        // ================================

        public async Task<DataTable> GetDashboardStatsAsync()
        {
            string query = @"SELECT 
                                (SELECT COUNT(*) FROM Parts WHERE IsDeleted = 0) as Total,
                                (SELECT COUNT(*) FROM Parts WHERE Quantity <= MinimumStock AND Quantity > 0 AND IsDeleted = 0) as Low,
                                (SELECT COUNT(*) FROM Parts WHERE Quantity <= 0 AND IsDeleted = 0) as Out,
                               (
                                    (SELECT ISNULL(SUM(TotalAmount), 0) FROM Invoices 
                                     WHERE invoiceType = N'مبيعات' AND CAST(Time AS DATE) = CAST(GETDATE() AS DATE))
                                    - 
                                    (SELECT ISNULL(SUM(r.TotalRefundedAmount), 0) FROM Returns r
                                     INNER JOIN Invoices i ON r.InvoiceID = i.ID
                                     WHERE i.invoiceType = N'مبيعات' 
                                     AND CAST(r.ReturnDate AS DATE) = CAST(GETDATE() AS DATE))
                                ) as DailySales";

            return await DbHelper.ExecuteQueryAsync(query);
        }

        public async Task<DataTable> GetLowStockAlertsAsync()
        {
            string query = @"SELECT p.PartName as [اسم القطعة], p.Quantity as [الكمية المتاحة], 
                             p.MinimumStock as [حد الطلب], c.categoryName as [الفئة]
                             FROM Parts p INNER JOIN partscategories c ON p.CategoryID = c.categoryID
                             WHERE p.IsDeleted = 0 AND p.Quantity <= p.MinimumStock ORDER BY p.Quantity ASC";

            return await DbHelper.ExecuteQueryAsync(query);
        }

        // ================================
        // Parts
        // ================================

        public async Task<DataTable> GetAllPartsAsync()
        {
            string query = @"SELECT p.PartID as [كود], p.PartName as [الاسم], p.PartNumber as [رقم القطعة], 
                             p.PurchasePrice as [سعر الشراء], p.SellingPrice as [سعر البيع], 
                             p.Quantity as [الكمية], p.MinimumStock as [حد الطلب],
                             c.categoryName as [الفئة], p.Notes as [ملاحظات]
                             FROM Parts p INNER JOIN partscategories c ON p.CategoryID = c.categoryID
                             WHERE p.IsDeleted = 0";
            return await DbHelper.ExecuteQueryAsync(query);
        }

        public async Task AddPartAsync(AutoPart part)
        {
            string query = @"INSERT INTO Parts (PartName, PartNumber, PurchasePrice, SellingPrice, Quantity, MinimumStock, CategoryID, Notes, IsDeleted) 
                             VALUES (@name, @num, @pPrice, @sPrice, 0, @minStock, @catID, @notes, 0)";

            await DbHelper.ExecuteNonQueryAsync(query,
                new SqlParameter("@name", part.PartName),
                new SqlParameter("@num", part.PartNumber),
                new SqlParameter("@pPrice", part.PurchasePrice),
                new SqlParameter("@sPrice", part.SellingPrice),
                new SqlParameter("@minStock", part.MinimumStock),
                new SqlParameter("@catID", part.CategoryID),
                new SqlParameter("@notes", (object)part.Notes ?? DBNull.Value));
        }

        public async Task UpdatePartAsync(AutoPart part)
        {
            string query = @"UPDATE Parts SET PartName=@name, PartNumber=@num, PurchasePrice=@pPrice, 
                             SellingPrice=@sPrice, MinimumStock=@minStock, CategoryID=@catID, Notes=@notes 
                             WHERE PartID=@id ";

            await DbHelper.ExecuteNonQueryAsync(query,
             new SqlParameter("@name", part.PartName),
             new SqlParameter("@id", part.PartID),
             new SqlParameter("@num", part.PartNumber),
             new SqlParameter("@pPrice", part.PurchasePrice),
             new SqlParameter("@sPrice", part.SellingPrice),
             new SqlParameter("@minStock", part.MinimumStock),
             new SqlParameter("@catID", part.CategoryID),
             new SqlParameter("@notes", (object)part.Notes ?? DBNull.Value));
        }

        public async Task DeletePartAsync(int partID)
        {
            string query = "UPDATE Parts SET IsDeleted = 1 WHERE PartID = @id";
            await DbHelper.ExecuteNonQueryAsync(query, new SqlParameter("@id", partID));
        }

        // ================================
        // Categories
        // ================================

        public async Task<DataTable> GetAllCategoriesAsync()
        {
            return await DbHelper.ExecuteQueryAsync("SELECT categoryID, categoryName FROM partscategories WHERE IsDeleted = 0");
        }

        public async Task AddCategoryAsync(string categoryName)
        {
            await DbHelper.ExecuteNonQueryAsync("INSERT INTO partscategories (categoryName, IsDeleted) VALUES (@name, 0)",
                new SqlParameter("@name", categoryName));
        }

        public async Task UpdateCategoryAsync(int id, string newName)
        {
            await DbHelper.ExecuteNonQueryAsync("UPDATE partscategories SET categoryName = @name WHERE categoryID = @id",
                new SqlParameter("@name", newName), new SqlParameter("@id", id));
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await DbHelper.ExecuteNonQueryAsync("UPDATE partscategories SET IsDeleted = 1 WHERE categoryID = @id",
                new SqlParameter("@id", id));
        }

        // ================================
        // Parts By Category
        // ================================

        public async Task<DataTable> GetPartsByCategoryAsync(int catId)
        {
            string query = (catId == 0)
                ? "SELECT PartID, PartName, SellingPrice FROM Parts WHERE IsDeleted = 0"
                : "SELECT PartID, PartName, SellingPrice FROM Parts WHERE CategoryID = @id AND IsDeleted = 0";
            return await DbHelper.ExecuteQueryAsync(query, new SqlParameter("@id", catId));
        }

        // ================================
        // Part History
        // ================================

        public async Task<DataTable> GetPartHistoryAsync(int partId)
        {
            string query = @"SELECT I.Time as [التاريخ], I.invoiceType as [النوع], ID.Quantity as [الكمية], ID.PartPrice as [السعر وقتها]
                             FROM InvoiceDetails ID INNER JOIN Invoices I ON ID.InvoiceID = I.ID
                             WHERE ID.PartID = @id ORDER BY I.Time DESC";
            return await DbHelper.ExecuteQueryAsync(query, new SqlParameter("@id", partId));
        }

        // ================================
        // Part Number
        // ================================

        public async Task<string> GetPartNumberAsync()
        {
            var result = await DbHelper.ExecuteScalarAsync("SELECT COALESCE(MAX(TRY_CAST(PartNumber AS INT)), 1000) + 1 FROM Parts");
            return result?.ToString() ?? "1001";
        }
        public async Task<DataTable> GetPartsForAutoCompleteAsync()
        {
            string sql = "SELECT PartName, PartNumber FROM Parts WHERE IsDeleted = 0";
            return await DbHelper.ExecuteQueryAsync(sql);
        }

        public async Task<int> GetPartIdByNameAsync(string partName)
        {
            string query = "SELECT PartID FROM Parts WHERE PartName = @name AND IsDeleted = 0";

            object result = await DbHelper.ExecuteScalarAsync(query, new SqlParameter("@name", partName.Trim()));
            return result != null ? Convert.ToInt32(result) : 0;
        }


        public async Task<AutoPart> GetPartByNumberAsync(string partNumber)
        {
            string query = "SELECT PartID, PartName, CategoryID, PurchasePrice FROM Parts WHERE PartNumber = @num AND IsDeleted = 0";

            DataTable dt = await DbHelper.ExecuteQueryAsync(query, new SqlParameter("@num", partNumber.Trim()));

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                return new AutoPart
                {
                    PartID = Convert.ToInt32(dr["PartID"]),
                    PartName = dr["PartName"].ToString(),
                    CategoryID = Convert.ToInt32(dr["CategoryID"]),
                    PurchasePrice = Convert.ToDecimal(dr["PurchasePrice"])
                };
            }
            return null;
        }

        public async Task<bool> IsPartNumberExistsAsync(string partNumber)
        {
            string query = "SELECT COUNT(1) FROM Parts WHERE PartNumber = @num AND IsDeleted = 0";
            object result = await DbHelper.ExecuteScalarAsync(query, new SqlParameter("@num", partNumber.Trim()));
            return Convert.ToInt32(result) > 0;
        }

        public async Task<AutoPart> GetPartByNameAsync(string partName)
        {
            string query = "SELECT PartID, PartNumber, CategoryID, PurchasePrice FROM Parts WHERE PartName = @name AND IsDeleted = 0";

            DataTable dt = await DbHelper.ExecuteQueryAsync(query, new SqlParameter("@name", partName.Trim()));

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                return new AutoPart
                {
                    PartID = Convert.ToInt32(dr["PartID"]),
                    PartName = partName, 
                    PartNumber = dr["PartNumber"].ToString(),
                    CategoryID = Convert.ToInt32(dr["CategoryID"]),
                    PurchasePrice = Convert.ToDecimal(dr["PurchasePrice"])
                };
            }
            return null;
        }
    }
}