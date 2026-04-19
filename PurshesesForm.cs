using Auto_Parts_Store.Helpers;
using Auto_Parts_Store.Models;
using Auto_Parts_Store.Repositories;
using Auto_Parts_Store.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;


namespace Auto_Parts_Store

{

    public partial class PurshesesForm : Form

    {

        private readonly IPartRepository _partRepo;
        private readonly IPersonRepository _personRepo;
        private readonly IPurchasesRepository _purchasesRepository;
        private readonly ISafeRepository _safeRepo;


        decimal calculatedSellingPrice = 0;
        int currentPartID = 0;

        public PurshesesForm(IPartRepository partRepo, IPersonRepository personRepo, IPurchasesRepository purchasesRepository ,ISafeRepository saferepo)

        {

            InitializeComponent();
            _partRepo = partRepo ?? throw new ArgumentNullException(nameof(partRepo));
            _personRepo = personRepo ?? throw new ArgumentNullException(nameof(personRepo));
            _purchasesRepository = purchasesRepository ?? throw new ArgumentNullException(nameof(purchasesRepository));
            _safeRepo = saferepo ?? throw new ArgumentNullException(nameof(saferepo));

            this.KeyPreview = true;

            this.DoubleBuffered = true;



        }
        private async void PurshesesForm_Load(object sender, EventArgs e)
        {

            await Task.WhenAll(
            LoadSuppliersAsync(),
            LoadCategoriesAsync(),
            EnableAutoCompleteAsync()
            );
          
            SetupGrid();
            ClearInputFields();
            
            txtPartName.Focus();
            if (paymentmethod.Items.Count > 0)
            {
                paymentmethod.SelectedIndex = 0; 
            }
        }

        private void PostAddCleanup()
        {
            dgvPurshes.ClearSelection();

            if (dgvPurshes.Rows.Count > 0)
            {
                int lastIndex = dgvPurshes.Rows.Count - 1;
                dgvPurshes.Rows[lastIndex].Selected = true;
                dgvPurshes.FirstDisplayedScrollingRowIndex = lastIndex;
            }

            Add.Text = "إضافة للفاتورة";
            Add.BackColor = Color.ForestGreen;

            UpdateFinalTotals();
            ClearInputFields();
            RenumberRows();
            txtPartName.Focus();
        }
        void UpdateFinalTotals()
        {
            decimal finalTotal = dgvPurshes.Rows.Cast<DataGridViewRow>()
            .Where(r => r.Cells["TotalCol"].Value != null)
            .Sum(r => Convert.ToDecimal(r.Cells["TotalCol"].Value));

            totallbl.Text = finalTotal.ToString("N2");

            decimal.TryParse(Paid.Text, out decimal paid);
            decimal remainder = finalTotal - paid;

            Remaining.Text = remainder.ToString("N2");

            Remaining.ForeColor = remainder > 0 ? Color.Red : Color.ForestGreen;
            Remaining.Font = new Font(Remaining.Font, remainder > 0 ? FontStyle.Bold : FontStyle.Regular);

        }
        void ClearInputFields()
        {
            txtPartName.Clear();
            txtPartNumber.Clear();
            txtPurchasePrice.Text = "0.00";

            calculatedSellingPrice = 0;
            currentPartID = 0;
            Quantity.Value = 1;

            if (cmbCategories.Items.Count > 0)
                cmbCategories.SelectedIndex = 0;

            notestxt.Clear();
            txtPartName.Focus();
        }
        private void ResetFormAfterSave()
        {
            dgvPurshes.Rows.Clear();

            Paid.Text = "0.00";
            totallbl.Text = "0.00";
            Remaining.Text = "0.00";
            Remaining.ForeColor = Color.Green;

            if (supplierscmb.Items.Count > 0)
                supplierscmb.SelectedIndex = 0;

            if (paymentmethod.Items.Count > 0)
                paymentmethod.SelectedIndex = 0;

            ClearInputFields();

            txtPartName.Focus();
        }



        #region Loading Data
        private async Task LoadSuppliersAsync()
        {      
            try
            {
                DataTable dt = await _personRepo.GetAllPersonsAsync(PersonType.Supplier);

                DataRow[] generalSupplier = dt.Select("ID = 9");
                foreach (DataRow row in generalSupplier)
                {
                    dt.Rows.Remove(row);
                }

                DataRow dr = dt.NewRow();
                dr["ID"] = 0;
                dr["الأسم"] = "مورد نقدي / عام"; 
                dt.Rows.InsertAt(dr, 0);

                supplierscmb.DataSource = dt;
                supplierscmb.DisplayMember = "الأسم"; 
                supplierscmb.ValueMember = "ID";
                supplierscmb.SelectedValue = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل الموردين: " + ex.Message);
            }

        }
        private async Task LoadCategoriesAsync()

        {

            try
            {
                // سحب الفئات من الـ Repo
                DataTable dt = await _partRepo.GetAllCategoriesAsync();

                DataRow dr = dt.NewRow();
                dr["categoryID"] = 0;
                dr["categoryName"] = "--- اختر فئة القطعة ---";
                dt.Rows.InsertAt(dr, 0);

                cmbCategories.DataSource = dt;
                cmbCategories.DisplayMember = "categoryName";
                cmbCategories.ValueMember = "categoryID";
                cmbCategories.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل الفئات: " + ex.Message);
            }

        }
        private async Task EnableAutoCompleteAsync()
        {
            try
            {
                DataTable dt = await _partRepo.GetPartsForAutoCompleteAsync();

                AutoCompleteStringCollection namesData = new AutoCompleteStringCollection();
                AutoCompleteStringCollection numbersData = new AutoCompleteStringCollection();

                foreach (DataRow row in dt.Rows)
                {
                    namesData.Add(row["PartName"].ToString());
                    if (row["PartNumber"] != DBNull.Value)
                        numbersData.Add(row["PartNumber"].ToString());
                }

                txtPartName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtPartName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtPartName.AutoCompleteCustomSource = namesData;

                txtPartNumber.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtPartNumber.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtPartNumber.AutoCompleteCustomSource = numbersData;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل الإكمال التلقائي: " + ex.Message);
            }
        }
        private async Task UpdateAutoCompleteByCategoryAsync(int catID)
        {

            try
            {
                DataTable dt = await _partRepo.GetPartsByCategoryAsync(catID);

                AutoCompleteStringCollection filteredData = new AutoCompleteStringCollection();

                foreach (DataRow row in dt.Rows)
                {
                    filteredData.Add(row["PartName"].ToString());
                }

                txtPartName.AutoCompleteCustomSource = filteredData;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل الإكمال التلقائي: " + ex.Message);

            }
        }

        #endregion



        #region User Actions

        private async void Add_Click(object sender, EventArgs e)

        {

            if (!ValidationHelper.ValidatePurchaseInput(
                    txtPartName.Text,
                    txtPartNumber.Text,
                    txtPurchasePrice.Text,
                    Quantity.Value,
                    cmbCategories.SelectedIndex,
                    supplierscmb.SelectedIndex,
                    out string error))
            {
                MessageBox.Show(error, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string partName = txtPartName.Text.Trim();
            string partNum = txtPartNumber.Text.Trim();
            decimal buyPrice = decimal.Parse(txtPurchasePrice.Text);
            int newQty = (int)Quantity.Value;


            if (currentPartID == 0 && !string.IsNullOrWhiteSpace(txtPartName.Text))
            {
                currentPartID = await _partRepo.GetPartIdByNameAsync(partName);
            }
          
            if (currentPartID == 0)
            {
                bool exists = await _partRepo.IsPartNumberExistsAsync(partNum);
                if (exists)
                {
                    MessageBox.Show("رقم القطعة ده مسجل لصنف آخر! ابحث عنه بالرقم أو غير الرقم لو ده صنف جديد.",
                                    "تنبيه: تكرار رقم قطعة", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtPartNumber.Focus();
                    txtPartNumber.SelectAll();
                    return;
                }
            }

            bool isExist = false;
            foreach (DataGridViewRow row in dgvPurshes.Rows)

            {

                if (row.Cells["PartNumberCol"].Value?.ToString() == partNum &&
                    row.Cells["PartNameCol"].Value?.ToString() == partName)
                {
                    int currentQty = Convert.ToInt32(row.Cells["QuantityCol"].Value);
                    int updatedQty = currentQty + newQty;

                    row.Cells["QuantityCol"].Value = updatedQty;
                    row.Cells["PurchasePriceCol"].Value = buyPrice;
                    row.Cells["SellingPriceCol"].Value = calculatedSellingPrice;
                    row.Cells["TotalCol"].Value = buyPrice * updatedQty;

                    isExist = true;
                    break;
                }   

            }


            if (!isExist)
            {
                dgvPurshes.Rows.Add(
                    "",
                    partNum,
                    currentPartID,
                    partName,
                    buyPrice,
                    calculatedSellingPrice,
                    newQty,
                    buyPrice * newQty,
                    notestxt.Text.Trim(),
                    cmbCategories.SelectedValue
                );
            }
            PostAddCleanup();
        }
        private async void savebtn_Click(object sender, EventArgs e)

        {

            if (dgvPurshes.Rows.Count == 0)
            {
                MessageBox.Show("الفاتورة فارغة! برجاء إضافة أصناف أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var header = new InvoiceHeader
                {
                    PaymentMethod = paymentmethod.Text,
                    TotalAmount = Convert.ToDecimal(totallbl.Text),
                    UserID = AuthService.CurrentSession.UserID, 
                    PaidAmount = string.IsNullOrEmpty(Paid.Text) ? 0 : Convert.ToDecimal(Paid.Text),
                    SupplierID = (supplierscmb.SelectedValue == null || Convert.ToInt32(supplierscmb.SelectedValue) == 0) ? 9 : Convert.ToInt32(supplierscmb.SelectedValue)
                };

                var details = new List<InvoiceDetail>();
                foreach (DataGridViewRow row in dgvPurshes.Rows)
                {
                    if (row.IsNewRow) continue;
                    details.Add(new InvoiceDetail
                    {
                        PartID = Convert.ToInt32(row.Cells["PartIDCol"].Value),
                        PartName = row.Cells["PartNameCol"].Value.ToString(), 
                        PartNumber = row.Cells["PartNumberCol"].Value?.ToString() ?? "",
                        PurchasePrice = Convert.ToDecimal(row.Cells["PurchasePriceCol"].Value),
                        SellingPrice = Convert.ToDecimal(row.Cells["SellingPriceCol"].Value),
                        Quantity = Convert.ToDecimal(row.Cells["QuantityCol"].Value),
                        Total = Convert.ToDecimal(row.Cells["TotalCol"].Value),
                        CategoryID = Convert.ToInt32(row.Cells["categoryCol"].Value)
                    });
                }

                bool isSaved = await _purchasesRepository.SavePurchaseInvoiceAsync(header, details);

                if (isSaved)
                {
                    if (header.PaidAmount > 0)
                    {
                        await _safeRepo.AddTransactionAsync(new SafeTransaction
                        {
                            Amount = header.PaidAmount,
                            TransactionType = "سحب",
                            Description = $"دفع لمورد - فاتورة مشتريات",
                            UserID = header.UserID,
                            TransactionDate = DateTime.Now
                        });
                    }

                    MessageBox.Show("تم حفظ الفاتورة وتحديث المخازن بنجاح!", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetFormAfterSave();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في عملية الحفظ: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
  

        #endregion



        #region Search & Filtering (UX Focus)

        private async void txtPartNumber_KeyDown(object sender, KeyEventArgs e)

        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                if (string.IsNullOrWhiteSpace(txtPartNumber.Text)) return;

                var part = await _partRepo.GetPartByNumberAsync(txtPartNumber.Text.Trim());

                if (part != null)
                {
                    txtPartName.Text = part.PartName;
                    cmbCategories.SelectedValue = part.CategoryID;
                    currentPartID = part.PartID;

                    txtPurchasePrice.Focus();
                    txtPurchasePrice.SelectAll();
                }
                else
                {
                    currentPartID = 0;
                    txtPurchasePrice.Focus();
                }
            }

        }
        private async void txtPartName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                if (string.IsNullOrWhiteSpace(txtPartName.Text)) return;

                var part = await _partRepo.GetPartByNameAsync(txtPartName.Text.Trim());

                if (part != null)
                {
                    currentPartID = part.PartID;
                    txtPartNumber.Text = part.PartNumber;
                    cmbCategories.SelectedValue = part.CategoryID;

                    txtPurchasePrice.Focus();
                    txtPurchasePrice.SelectAll();
                }
                else
                {
                    currentPartID = 0;

                    SetNextPartNumber();

                    txtPurchasePrice.Text = "0";

                    txtPartNumber.Focus();
                    txtPartNumber.SelectAll();
                }
            }
        }

        private async void cmbCategories_SelectedIndexChanged(object sender, EventArgs e)

        {

            if (cmbCategories.SelectedValue != null && cmbCategories.ValueMember != "")

            {

                if (int.TryParse(cmbCategories.SelectedValue.ToString(), out int catId))
                {
                    await UpdateAutoCompleteByCategoryAsync(catId);
                }

            }

        }

        #endregion




        private void txtPurchasePrice_KeyDown(object sender, KeyEventArgs e)

        {

            if (e.KeyCode == Keys.Enter)

            {

                e.SuppressKeyPress = true; 



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


        private async void SetNextPartNumber()
        {

            string maxDBStr = await _partRepo.GetPartNumberAsync();
            int maxDB = int.Parse(maxDBStr) - 1;

            int maxGrid = dgvPurshes.Rows.Cast<DataGridViewRow>()
                .Where(r => r.Cells["PartNumberCol"].Value != null)
                .Select(r => {
                    int.TryParse(r.Cells["PartNumberCol"].Value.ToString(), out int num);
                    return num;
                }).DefaultIfEmpty(0).Max();

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
                    cmbCategories.Focus();
                    cmbCategories.DroppedDown = true;

                }

            }

        }
        void SetupGrid()
        {
            dgvPurshes.ApplyCustomStyle();
            dgvPurshes.AddIdColumn();

            if (!dgvPurshes.Columns.Contains("btnDelete"))
            {
                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                btnDelete.Name = "btnDelete";
                btnDelete.HeaderText = "حذف";
                btnDelete.Text = "❌";
                btnDelete.UseColumnTextForButtonValue = true;
                btnDelete.Width = 50;
                btnDelete.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                dgvPurshes.Columns.Add(btnDelete);
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
                        UpdateFinalTotals(); 
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

                txtPartNumber.Text = row.Cells["PartNumberCol"].Value.ToString();
                txtPartName.Text = row.Cells["PartNameCol"].Value.ToString();
                txtPurchasePrice.Text = row.Cells["PurchasePriceCol"].Value.ToString();
                calculatedSellingPrice = Convert.ToDecimal(row.Cells["SellingPriceCol"].Value);
                Quantity.Value = Convert.ToDecimal(row.Cells["QuantityCol"].Value);
                notestxt.Text = row.Cells["notesCol"].Value.ToString();
                cmbCategories.SelectedValue = row.Cells["categoryCol"].Value;

                dgvPurshes.Rows.RemoveAt(e.RowIndex);
                UpdateFinalTotals();
                RenumberRows();

                txtPartName.Focus();

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


