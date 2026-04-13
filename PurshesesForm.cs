using Auto_Parts_Store.Models;
using Auto_Parts_Store.Repositories;
using Auto_Parts_Store.Services;
using System;

using System.Data;

using System.Data.SqlClient;

using System.Drawing;

using System.Windows.Forms;



namespace Auto_Parts_Store

{

    public partial class PurshesesForm : Form

    {

        string connString = @"Server=DESKTOP-21OI6J7; Database=AutoPartsStoreDB; Integrated Security=True;";

        decimal calculatedSellingPrice = 0;
        int currentPartID = 0;

        public PurshesesForm()

        {

            InitializeComponent();

            this.KeyPreview = true;

            this.DoubleBuffered = true;



        }



        private void PurshesesForm_Load(object sender, EventArgs e)

        {

            LoadSuppliers();

            LoadCategories();

            EnableAutoComplete();
            SetupGrid();

            ClearInputFields();
            txtPartName.Focus();
            if (paymentmethod.Items.Count > 0)
            {
                paymentmethod.SelectedIndex = 0; 
            }

        }



        #region Loading Data

        void LoadSuppliers()

        {

            try

            {

                using (SqlConnection con = new SqlConnection(connString))

                {

                    string sql = "SELECT s.ID, p.PersonName FROM supplieres s JOIN person p ON s.ID = p.ID where p.isdeleted = 0 AND s.ID <> 9 ";

                    SqlDataAdapter da = new SqlDataAdapter(sql, con);

                    DataTable dt = new DataTable();

                    da.Fill(dt);



                    DataRow dr = dt.NewRow();

                    dr["ID"] = 0;

                    dr["PersonName"] = "مورد نقدي / عام";

                    dt.Rows.InsertAt(dr, 0);



                    supplierscmb.DataSource = dt;

                    supplierscmb.DisplayMember = "PersonName";

                    supplierscmb.ValueMember = "ID";

                    supplierscmb.SelectedValue = -1;

                }

            }

            catch (Exception ex)

            {

                MessageBox.Show("خطأ في تحميل الموردين: " + ex.Message);

            }

        }



        void LoadCategories()

        {

            try

            {

                using (SqlConnection con = new SqlConnection(connString))

                {

                    string sql = "SELECT categoryID, categoryName FROM partscategories WHERE IsDeleted = 0";

                    SqlDataAdapter da = new SqlDataAdapter(sql, con);

                    DataTable dt = new DataTable();

                    da.Fill(dt);



                    DataRow dr = dt.NewRow();

                    dr["categoryID"] = 0;

                    dr["categoryName"] = "--- اختر فئة القطعة ---";

                    dt.Rows.InsertAt(dr, 0);



                    cmbCategories.DataSource = dt;

                    cmbCategories.DisplayMember = "categoryName";

                    cmbCategories.ValueMember = "categoryID";




                    cmbCategories.SelectedIndex = 0;

                }

            }

            catch (Exception ex)

            {

                MessageBox.Show("خطأ في تحميل الفئات: " + ex.Message);

            }

        }



        void EnableAutoComplete()

        {


            AutoCompleteStringCollection namesData = new AutoCompleteStringCollection();

            AutoCompleteStringCollection numbersData = new AutoCompleteStringCollection();



            using (SqlConnection con = new SqlConnection(connString))

            {

                SqlCommand cmd = new SqlCommand("SELECT PartID, PartName, PartNumber FROM Parts where isDeleted =0 ", con);

                con.Open();

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())

                {

                    namesData.Add(rd["PartName"].ToString());

                    if (rd["PartNumber"] != DBNull.Value)

                        numbersData.Add(rd["PartNumber"].ToString());

                }

            }




            txtPartName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtPartName.AutoCompleteSource = AutoCompleteSource.CustomSource;

            txtPartName.AutoCompleteCustomSource = namesData;




            txtPartNumber.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            txtPartNumber.AutoCompleteSource = AutoCompleteSource.CustomSource;

            txtPartNumber.AutoCompleteCustomSource = numbersData;





        }

        void UpdateAutoCompleteByCategory(int catID)


        {


            AutoCompleteStringCollection filteredData = new AutoCompleteStringCollection();

            using (SqlConnection con = new SqlConnection(connString))

            {
                string sql = "SELECT PartName FROM Parts WHERE CategoryID = @catID and IsDeleted = 0";

                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@catID", catID);

                con.Open();

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                
                { 
                    
                    filteredData.Add(rd["PartName"].ToString()); 

                }




            }


            txtPartName.AutoCompleteCustomSource = filteredData;





        }

        #endregion



        #region User Actions

        private void Add_Click(object sender, EventArgs e)

        {

            if (string.IsNullOrWhiteSpace(txtPartName.Text))

            {

                MessageBox.Show("من فضلك تأكد من كتابة اسم القطعة!", "بيانات ناقصة", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                txtPartName.Focus();

                return;

            }



            if (string.IsNullOrWhiteSpace(txtPartNumber.Text))

            {

                MessageBox.Show("من فضلك تأكد من كتابة رقم القطعة.", "بيانات ناقصة", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                txtPartNumber.Focus();

                return;

            }



            if (!decimal.TryParse(txtPurchasePrice.Text, out decimal buyPrice) || buyPrice <= 0)

            {

                MessageBox.Show("سعر الشراء غير صحيح! دخل رقم أكبر من صفر.", "غلط في السعر", MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtPurchasePrice.Focus();

                txtPurchasePrice.SelectAll();

                return;

            }



            if (Quantity.Value <= 0)

            {

                MessageBox.Show("الكمية لازم تكون 1 على الأقل.", "غلط في الكمية");

                Quantity.Focus();

                return;

            }



            if (cmbCategories.SelectedIndex <= 0)

            {

                MessageBox.Show("من فضلك اختار فئة القطعة ( مساعدين، فرامل.. إلخ).");

                cmbCategories.Focus();

                cmbCategories.DroppedDown = true;

                return;

            }

            if (supplierscmb.SelectedIndex == -1)

            {

                MessageBox.Show("من فضلك اختر المورد أولاً!", "تنبيه");

                supplierscmb.Focus();

                supplierscmb.DroppedDown = true;

                return;

            }

            if (currentPartID == 0 && !string.IsNullOrWhiteSpace(txtPartName.Text))
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT PartID FROM Parts WHERE PartName = @name AND IsDeleted = 0", con);
                    cmd.Parameters.AddWithValue("@name", txtPartName.Text.Trim());
                    con.Open();
                    object res = cmd.ExecuteScalar();
                    if (res != null) currentPartID = Convert.ToInt32(res);
                }
            }


            bool isExist = false;

            string partNum = txtPartNumber.Text.Trim();

            string partName = txtPartName.Text.Trim();


            int newQty = (int)Quantity.Value;



            foreach (DataGridViewRow row in dgvPurshes.Rows)

            {

                if (row.Cells["PartNumberCol"].Value?.ToString() == partNum && row.Cells["PartNameCol"].Value?.ToString() == partName)

                {

                    int currentQty = Convert.ToInt32(row.Cells[4].Value);

                    int updatedQty = currentQty + newQty;

                    row.Cells["QuantityCol"].Value = updatedQty;

                    row.Cells["PurchasePriceCol"].Value = txtPurchasePrice.Text;


                    row.Cells["SellingPriceCol"].Value = calculatedSellingPrice;



                    row.Cells["TotalCol"].Value = buyPrice * updatedQty;



                    isExist = true;

                    break;

                }

            }



            if (!isExist)

            {

                decimal total = buyPrice * newQty;

                dgvPurshes.Rows.Add(

                    "",


                    partNum,

                    currentPartID,

                    txtPartName.Text.Trim(),

                    buyPrice,

                    calculatedSellingPrice,

                    newQty,
                 

                    total,

                    notestxt.Text.Trim(), 
                    
                    cmbCategories.SelectedValue

                );
                currentPartID = 0;

            }







            dgvPurshes.ClearSelection();
            int lastRowIndex = dgvPurshes.Rows.Count - 1;
            if (lastRowIndex >= 0)
            {
                dgvPurshes.Rows[lastRowIndex].Selected = true;
                dgvPurshes.FirstDisplayedScrollingRowIndex = lastRowIndex;
            }


            Add.Text = "إضافة للفاتورة";
            Add.BackColor = Color.ForestGreen; // أو اللون الأصلي بتاعك


            UpdateFinalTotals();

            ClearInputFields();
            RenumberRows();

        }



        void UpdateFinalTotals()

        {

            decimal finalTotal = 0;
            foreach (DataGridViewRow row in dgvPurshes.Rows)
            {
                if (row.Cells["TotalCol"].Value != null)
                    finalTotal += Convert.ToDecimal(row.Cells["TotalCol"].Value);
            }

            // عرض الإجمالي الكلي
            totallbl.Text = finalTotal.ToString("N2");

            // حساب الباقي
            decimal paid = 0;
            decimal.TryParse(Paid.Text, out paid); // txtPaid هو تيكست بوكس "المدفوع"

            decimal remainder = finalTotal - paid;
            Remaining.Text = remainder.ToString("N2"); // lblRemainder هو لابل "الباقي"

            // حركة صايعة: لو الباقي > 0 خلي لونه أحمر عشان تنبه المستخدم
            Remaining.ForeColor = remainder > 0 ? Color.Red : Color.Green;

        }



        void ClearInputFields()

        {

            txtPartName.Clear();

            txtPartNumber.Clear();

            txtPurchasePrice.Text = "0";

            calculatedSellingPrice = 0;



            Quantity.Value = 1;



            cmbCategories.SelectedIndex = 0;



            notestxt.Clear();

            txtPartName.Focus();

        }



        private async void savebtn_Click(object sender, EventArgs e)

        {

            if (dgvPurshes.Rows.Count == 0)
            {
                MessageBox.Show("الفاتورة فارغة! برجاء إضافة أصناف أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    string invQuery = @"INSERT INTO Invoices (Time, PaymentMethod, TotalAmount, UserID, supplierID, invoiceType, paidamount) 
                                VALUES (GETDATE(), @payMethod, @total, @user, @sup, 'توريد', @paid); 
                                SELECT SCOPE_IDENTITY();";

                    SqlCommand cmdInv = new SqlCommand(invQuery, conn, transaction);
                    cmdInv.Parameters.AddWithValue("@payMethod", paymentmethod.Text);
                    cmdInv.Parameters.AddWithValue("@total", Convert.ToDecimal(totallbl.Text));
                    cmdInv.Parameters.AddWithValue("@user", 2);
                    int supplierId;
                    var selectedVal = supplierscmb.SelectedValue;
                    if (selectedVal == null || selectedVal == DBNull.Value || Convert.ToInt32(selectedVal) == 0)
                    {
                        supplierId = 9;
                    }
                    else
                    {
                        supplierId = Convert.ToInt32(supplierscmb.SelectedValue);
                    }

                    cmdInv.Parameters.AddWithValue("@sup", supplierId); cmdInv.Parameters.AddWithValue("@paid", string.IsNullOrEmpty(Paid.Text) ? 0 : Convert.ToDecimal(Paid.Text));

                    int lastInvoiceId = Convert.ToInt32(cmdInv.ExecuteScalar());
                    decimal totalInvoice = Convert.ToDecimal(totallbl.Text);
                    decimal paidAmount = string.IsNullOrEmpty(Paid.Text) ? 0 : Convert.ToDecimal(Paid.Text);
                    decimal debt = totalInvoice - paidAmount;

                    if (debt > 0 && supplierscmb.SelectedValue != null && Convert.ToInt32(supplierscmb.SelectedValue) != 0)
                    {
               
                        string updateSupplierQuery = "UPDATE supplieres SET Balance = ISNULL(balance, 0) + @debt WHERE ID = @supID";

                        SqlCommand cmdUpdateSup = new SqlCommand(updateSupplierQuery, conn, transaction);
                        cmdUpdateSup.Parameters.AddWithValue("@debt", debt);
                        cmdUpdateSup.Parameters.AddWithValue("@supID", supplierscmb.SelectedValue);

                        cmdUpdateSup.ExecuteNonQuery();
                    }

                    foreach (DataGridViewRow row in dgvPurshes.Rows)
                    {
                        if (row.IsNewRow) continue;

                        int pID = 0;
                        var cellVal = row.Cells["PartIDCol"].Value;

                        if (cellVal == null || cellVal.ToString() == "0" || string.IsNullOrEmpty(cellVal.ToString()))
                        {
                            string partQuery = @"INSERT INTO Parts (PartName, PartNumber, PurchasePrice, SellingPrice, Quantity, MinimumStock, CategoryID) 
                                        VALUES (@n, @num, @pp, @sp, 0,5, @cat); 
                                        SELECT SCOPE_IDENTITY();";
                            SqlCommand cmdP = new SqlCommand(partQuery, conn, transaction);
                            cmdP.Parameters.AddWithValue("@n", row.Cells["PartNameCol"].Value);
                            cmdP.Parameters.AddWithValue("@num", row.Cells["PartNumberCol"].Value ?? "");
                            cmdP.Parameters.AddWithValue("@pp", row.Cells["PurchasePriceCol"].Value);
                            cmdP.Parameters.AddWithValue("@sp", row.Cells["SellingPriceCol"].Value);
                            cmdP.Parameters.AddWithValue("@cat", row.Cells["categoryCol"].Value);
                            pID = Convert.ToInt32(cmdP.ExecuteScalar());
                        }
                        else
                        {
                            pID = Convert.ToInt32(cellVal);
                        }

                        // 3. حفظ تفاصيل الفاتورة - هنا بنستخدم الـ pID اللي لسه جايبينه
                        string detQuery = @"INSERT INTO InvoiceDetails (Quantity, PartPrice, Total, PartID, InvoiceID) 
                                   VALUES (@qty, @prc, @ttl, @pid, @invid)";
                        SqlCommand cmdD = new SqlCommand(detQuery, conn, transaction);
                        cmdD.Parameters.AddWithValue("@qty", row.Cells["QuantityCol"].Value);
                        cmdD.Parameters.AddWithValue("@prc", row.Cells["PurchasePriceCol"].Value);
                        cmdD.Parameters.AddWithValue("@ttl", row.Cells["TotalCol"].Value);
                        cmdD.Parameters.AddWithValue("@pid", pID);
                        cmdD.Parameters.AddWithValue("@invid", lastInvoiceId);
                        cmdD.ExecuteNonQuery();

                        // 4. تحديث المخزن
                        string stockQuery = "UPDATE Parts SET Quantity = Quantity + @qty WHERE PartID = @pid";
                        SqlCommand cmdS = new SqlCommand(stockQuery, conn, transaction);
                        cmdS.Parameters.AddWithValue("@qty", row.Cells["QuantityCol"].Value);
                        cmdS.Parameters.AddWithValue("@pid", pID);
                        cmdS.ExecuteNonQuery();

                        string transQuery = @"INSERT INTO inventoryTransactions (TransactionsType, Quantity, Date, notes, PartId, userId) 
                      VALUES (@type, @qty, @date, @note, @pid, @uid)";
                        SqlCommand cmdTrans = new SqlCommand(transQuery, conn, transaction);
                        cmdTrans.Parameters.AddWithValue("@type", "توريد"); 
                        cmdTrans.Parameters.AddWithValue("@qty", row.Cells["QuantityCol"].Value);
                        cmdTrans.Parameters.AddWithValue("@date", DateTime.Now);
                        cmdTrans.Parameters.AddWithValue("@note", "فاتورة مشتريات رقم: " + lastInvoiceId);
                        cmdTrans.Parameters.AddWithValue("@pid", pID);
                        cmdTrans.Parameters.AddWithValue("@uid", 2); 
                        cmdTrans.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("تم الحفظ بنجاح!");
                    dgvPurshes.Rows.Clear();
                    Paid.Text = "0";
                    totallbl.Text = "0.00";
                    Remaining.Text = "0.00";
                    Remaining.ForeColor = Color.Green;

                    // إعادة اختيار أول مورد وطريقة دفع
                    if (supplierscmb.Items.Count > 0) supplierscmb.SelectedIndex = 0;
                    if (paymentmethod.Items.Count > 0) paymentmethod.SelectedIndex = 0;

                    ClearInputFields();
                    txtPartName.Focus();


                    if (paidAmount > 0)
                    {
                        var safeTrans = new SafeTransaction
                        {
                            Amount = paidAmount,
                            TransactionType = "سحب",
                            Description = $"دفع لمورد - فاتورة مشتريات رقم {lastInvoiceId}",
                            UserID = AuthService.CurrentSession.UserID,
                            TransactionDate = DateTime.Now
                        };
                        ISafeRepository _safeRepo =new SafeRepository();
                        await _safeRepo.AddTransactionAsync(safeTrans);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("خطأ في الحفظ: " + ex.Message);
                }
            }
        }

        

        #endregion



        #region Search & Filtering (UX Focus)

        private void txtPartNumber_KeyDown(object sender, KeyEventArgs e)

        {



            if (e.KeyCode == Keys.Enter)

            {

                e.SuppressKeyPress = true;



                if (string.IsNullOrWhiteSpace(txtPartNumber.Text)) return;



                using (SqlConnection con = new SqlConnection(connString))

                {

                    string sql = "SELECT PartID, PartName, CategoryID FROM Parts WHERE PartNumber = @num and IsDeleted = 0";

                    SqlCommand cmd = new SqlCommand(sql, con);

                    cmd.Parameters.AddWithValue("@num", txtPartNumber.Text.Trim());



                    con.Open();

                    SqlDataReader dr = cmd.ExecuteReader();



                    if (dr.Read())

                    {
              

                        txtPartName.Text = dr["PartName"].ToString();

                        cmbCategories.SelectedValue = dr["CategoryID"];
                        currentPartID = Convert.ToInt32(dr["PartID"]);



                        txtPurchasePrice.Focus();

                        txtPurchasePrice.SelectAll();

                    }

                    else

                    {
                        currentPartID = 0;

                        txtPartName.Focus();

                    }

                }

                txtPurchasePrice.Focus();

                txtPurchasePrice.SelectAll();

            }

        }



        private void cmbCategories_SelectedIndexChanged(object sender, EventArgs e)

        {

            if (cmbCategories.SelectedValue != null && cmbCategories.ValueMember != "")

            {

                if (int.TryParse(cmbCategories.SelectedValue.ToString(), out int catId))
                {
                    UpdateAutoCompleteByCategory(catId);
                }

            }

        }



    

        #endregion



        private void txtPartName_KeyDown(object sender, KeyEventArgs e)

        {

            if (e.KeyCode == Keys.Enter)

            {

                e.SuppressKeyPress = true;



                if (string.IsNullOrWhiteSpace(txtPartName.Text)) return;



                using (SqlConnection con = new SqlConnection(connString))

                {

                    string sql = "SELECT PartID, PartNumber, CategoryID FROM Parts WHERE PartName = @name and IsDeleted = 0";

                    SqlCommand cmd = new SqlCommand(sql, con);

                    cmd.Parameters.AddWithValue("@name", txtPartName.Text.Trim());



                    con.Open();

                    SqlDataReader dr = cmd.ExecuteReader();



                    if (dr.Read())

                    {
                        currentPartID = Convert.ToInt32(dr["PartID"]);

                        txtPartNumber.Text = dr["PartNumber"].ToString();

                        cmbCategories.SelectedValue = dr["CategoryID"];



                        txtPurchasePrice.Focus();
                        txtPurchasePrice.SelectAll();
                    }

                    else

                    {

                        SetNextPartNumber();

                        txtPartNumber.SelectAll();



                        txtPurchasePrice.Text = "0";




                        txtPartNumber.Focus();

                    }

                }

            }

        }







        private void txtPurchasePrice_KeyDown(object sender, KeyEventArgs e)

        {

            if (e.KeyCode == Keys.Enter)

            {

                e.SuppressKeyPress = true; // كتم الصوت



                string input = txtPurchasePrice.Text.Trim();



                if (input.Contains("/"))

                {

                    try

                    {

                        string[] parts = input.Split('/');

                        if (parts.Length == 2)

                        {

                            decimal totalAmount = Convert.ToDecimal(parts[0]);

                            decimal count = Convert.ToDecimal(parts[1]);



                            if (count > 0)

                            {

                                decimal unitPrice = totalAmount / count;

                                txtPurchasePrice.Text = unitPrice.ToString("0.00");

                                input = unitPrice.ToString(); 

                            }

                        }

                    }

                    catch { /* لو دخل حروف غلط وسط العملية ميعملش حاجة */ }

                }



                if (decimal.TryParse(txtPurchasePrice.Text, out decimal buyPrice) && buyPrice > 0)

                {

                    calculatedSellingPrice = buyPrice * 1.20m;





                    Quantity.Focus();

                    Quantity.Select(0, Quantity.Text.Length);

                }

                else

                {

                    MessageBox.Show("أدخل سعر شراء صحيح!", "تنبيه");

                    txtPurchasePrice.Focus();

                    txtPurchasePrice.SelectAll();

                }

            }

        }



        private void Quantity_KeyDown(object sender, KeyEventArgs e)

        {

            if (e.KeyCode == Keys.Enter)

            {

                e.SuppressKeyPress = true;



                if (Quantity.Value > 0)

                {

                    notestxt.Focus();

                }

                else

                {

                    MessageBox.Show(".مينفعش تضيف صنف كميته صفر!");

                }

            }

        }





        private void supplierscmb_KeyDown(object sender, KeyEventArgs e)

        {

            if (e.KeyCode == Keys.Enter)

            {

                e.SuppressKeyPress = true;


                if (supplierscmb.SelectedIndex != -1)
                {
                    if (!string.IsNullOrWhiteSpace(txtPartName.Text) && decimal.TryParse(txtPurchasePrice.Text, out _) && !string.IsNullOrWhiteSpace(cmbCategories.Text) && 
                        !string.IsNullOrWhiteSpace(txtPartNumber.Text))
                    {
                        Add_Click(sender, e);
                    }
                    else
                    {
                        cmbCategories.Focus();

                        cmbCategories.DroppedDown = true;
                    }
                }

                else

                {

                    MessageBox.Show("يرجى اختيار مورد موجود في القائمة أو إضافة مورد جديد أولاً.");
                    supplierscmb.DroppedDown = true;

                }

            }

        }





        private void PurshesesForm_KeyDown(object sender, KeyEventArgs e)

        {

            if (e.KeyCode == Keys.Add)

            {

                Quantity.Value = Math.Min(Quantity.Maximum, Quantity.Value + 1);

                e.SuppressKeyPress = true;



                e.Handled = true;

            }



            if (e.KeyCode == Keys.Subtract)

            {

                Quantity.Value = Math.Max(Quantity.Minimum, Quantity.Value - 1);

                e.SuppressKeyPress = true;



                e.Handled = true;

            }

        }

        public int GetMaxPartNumberFromDB()

        {

            int maxId = 1000;
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    string query = "SELECT ISNULL(MAX(CAST(PartNumber AS INT)), 1000) FROM Parts WHERE ISNUMERIC(PartNumber) = 1 and IsDeleted = 0";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        maxId = Convert.ToInt32(result);
                    }
                }
            }
            catch { /* هيرجع الـ 1000 الافتراضية */ }
            return maxId;
        }

        

        void SetNextPartNumber()

        {

            // 1. نجيب أكبر رقم من الداتا بيز

            int maxDB = GetMaxPartNumberFromDB();



            // 2. نجيب أكبر رقم موجود في الجدول الحالي (DGV)

            int maxGrid = 0;

            foreach (DataGridViewRow row in dgvPurshes.Rows)

            {

                if (row.Cells["PartNumberCol"].Value != null)

                {

                    if (int.TryParse(row.Cells["PartNumberCol"].Value.ToString(), out int currentNum))

                    {

                        if (currentNum > maxGrid) maxGrid = currentNum;

                    }

                }

            }




            int finalMax = Math.Max(maxDB, maxGrid);

            txtPartNumber.Text = (finalMax + 1).ToString();

        }

        private void cmbCategories_Enter_1(object sender, EventArgs e)
        {
            Control ctrl = (Control)sender;

            ctrl.BackColor = Color.LightGoldenrodYellow;
        }

        private void cmbCategories_Leave_1(object sender, EventArgs e)
        {
            Control ctrl = (Control)sender;

            ctrl.BackColor = Color.White;
        }

        private void notestxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; 

                if (supplierscmb.SelectedIndex != -1 && !string.IsNullOrWhiteSpace(txtPartName.Text))
                {
                    Add_Click(sender, e);
                }
                else if (supplierscmb.SelectedIndex == -1)
                {
                    supplierscmb.Focus();
                    supplierscmb.DroppedDown = true;
                }
                else
                {
                    cmbCategories.Focus();

                    cmbCategories.DroppedDown = true;
                }
            }
        }

        private void cmbCategories_KeyDown(object sender, KeyEventArgs e)
        {
            
          



            if (e.KeyCode == Keys.Enter)

            {

                e.SuppressKeyPress = true;


                if (cmbCategories.SelectedIndex != -1)
                {
                    if (!string.IsNullOrWhiteSpace(txtPartName.Text) && decimal.TryParse(txtPurchasePrice.Text, out _) && !string.IsNullOrWhiteSpace(cmbCategories.Text) && !string.IsNullOrWhiteSpace(txtPartNumber.Text))
                    {
                        Add_Click(sender, e);
                    }
                    else
                    {
                        supplierscmb.Focus();

                        supplierscmb.DroppedDown = true;
                    }
                }

                else

                {
                    MessageBox.Show("يرجى اختيار فئة موجوده في القائمة أو إضافة فئة جديده أولاً.");
                    cmbCategories.DroppedDown = true;

                }

            }

        }
        void SetupGrid()
        {
            dgvPurshes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dgvPurshes.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            // تنسيق الهيدر (العناوين)
            dgvPurshes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dgvPurshes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPurshes.ColumnHeadersHeight = 35;
            dgvPurshes.EnableHeadersVisualStyles = false;

            // جعل الخط واضح وكبير شوية
            dgvPurshes.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvPurshes.RowTemplate.Height = 30;

            // منع المستخدم من إضافة سطور يدوياً (الإضافة من فوق بس)
            dgvPurshes.AllowUserToAddRows = false;
            dgvPurshes.AllowUserToDeleteRows = false;

            // جعل الأعمدة تاخد مساحة الـ Grid كلها بالتساوي
            dgvPurshes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
            btnDelete.Name = "btnDelete";
            btnDelete.HeaderText = "حذف";
            btnDelete.Text = "❌";
            btnDelete.UseColumnTextForButtonValue = true;
            btnDelete.Width = 50;
            btnDelete.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvPurshes.Columns.Add(btnDelete);
            if (!dgvPurshes.Columns.Contains("ID_Col"))
            {
                DataGridViewTextBoxColumn idCol = new DataGridViewTextBoxColumn();
                idCol.Name = "ID_Col";
                idCol.HeaderText = "م"; // اختصار مسلسل
                idCol.ReadOnly = true;
                idCol.Width = 40;
                dgvPurshes.Columns.Insert(0, idCol); // يضيفه أول عمود على اليمين (لو RTL مفعل)
            }
        }

        private void dgvPurshes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
                if (dgvPurshes.CurrentRow != null)
                {
                    var result = MessageBox.Show("هل تريد حذف هذا الصنف من الفاتورة؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        dgvPurshes.Rows.Remove(dgvPurshes.CurrentRow);
                        UpdateFinalTotals(); // تحديث الإجمالي الكلي للفاتورة بعد الحذف
                    }
                }
            }
            RenumberRows();
        }



        private void dgvPurshes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Add.Text == "تحديث (Update)")
            {
                MessageBox.Show("برجاء إنهاء تعديل الصنف الحالي أولاً (إضافة أو إلغاء) قبل اختيار صنف آخر!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPurshes.Rows[e.RowIndex];

                // سحب البيانات
                txtPartNumber.Text = row.Cells["PartNumberCol"].Value.ToString();
                txtPartName.Text = row.Cells["PartNameCol"].Value.ToString();
                txtPurchasePrice.Text = row.Cells["PurchasePriceCol"].Value.ToString();
                calculatedSellingPrice = Convert.ToDecimal(row.Cells["SellingPriceCol"].Value);
                Quantity.Value = Convert.ToDecimal(row.Cells["QuantityCol"].Value);
                notestxt.Text = row.Cells["notesCol"].Value.ToString();
                cmbCategories.SelectedValue = row.Cells["categoryCol"].Value;

                // حذف السطر
                dgvPurshes.Rows.RemoveAt(e.RowIndex);
                UpdateFinalTotals();
                RenumberRows();

                txtPartName.Focus();

                // 2. تفعيل وضع التعديل (القفل)
                Add.Text = "تحديث (Update)";
                Add.BackColor = Color.Orange;

            }
        }

        private void dgvPurshes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvPurshes.Columns[e.ColumnIndex].Name == "btnDelete")
            {
                if (MessageBox.Show("هل تريد حذف هذا الصنف؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    dgvPurshes.Rows.RemoveAt(e.RowIndex);
                    UpdateFinalTotals();
                }
            }
            RenumberRows();
        }

        void RenumberRows()
        {
            for (int i = 0; i < dgvPurshes.Rows.Count; i++)
            {
                dgvPurshes.Rows[i].Cells["ID_Col"].Value = (i + 1).ToString();
            }
        }

        private void Paid_TextChanged(object sender, EventArgs e)
        {
            UpdateFinalTotals();
        }

        private void Paid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                paymentmethod.Focus();
                paymentmethod.DroppedDown = true; 
            }
        }

        private void paymentmethod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                savebtn.Focus();
                savebtn_Click(sender, e);
            }
        }
    }
    }


