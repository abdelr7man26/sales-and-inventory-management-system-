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
    public partial class SellingForm : Form
    {
        #region Fields & Initialization 

        private readonly ISalesRepository _salesRepo;
        private readonly IPartRepository _partRepo;
        private bool isLoaded = false;

        public SellingForm(ISalesRepository salesRepo, IPartRepository partRepo)
        {
            InitializeComponent();
            _salesRepo = salesRepo ?? throw new ArgumentNullException(nameof(salesRepo));
            _partRepo = partRepo ?? throw new ArgumentNullException(nameof(partRepo));

            this.DoubleBuffered = true;
            dgvSales.ApplyCustomStyle(); 
        }

        #endregion

        #region Form Load 
        private async void SellingForm_Load_1(object sender, EventArgs e)
        {
            try
            {
                isLoaded = false;
                await Task.WhenAll(
                                    LoadCategoriesAsync(),
                                    LoadCustomersAsync(),
                                    LoadPartsByCategoryAsync(0)
                                );

                ClearForm();

                isLoaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل الشاشة: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion

        #region Loading Data Methods

        private async Task LoadCategoriesAsync()
        {

            try
            {
                var dt = await _partRepo.GetAllCategoriesAsync();
                DataRow dr = dt.NewRow();
                dr["categoryID"] = 0;
                dr["categoryName"] = "الكل";
                dt.Rows.InsertAt(dr, 0);

                categorysearchcmbbox.DataSource = dt;
                categorysearchcmbbox.DisplayMember = "categoryName";
                categorysearchcmbbox.ValueMember = "categoryID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل فئات البحث: " + ex.Message);
            }
        }

        private async Task LoadPartsByCategoryAsync(int catId)
        {
            try {
                var dt = await _partRepo.GetPartsByCategoryAsync(catId);
                cmbParts.DataSource = dt;
                cmbParts.DisplayMember = "PartName";
                cmbParts.ValueMember = "PartID";
                cmbParts.SelectedIndex = -1;
                lblUnitPrice.Text = "0.00";
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل البضاعه: " + ex.Message);
            }
        }

        private async Task LoadCustomersAsync()
        {

            try {
                var dt = await _salesRepo.LoadCustomersAsync();
                DataRow dr = dt.NewRow();
                dr["ID"] =-1;
                dr["PersonName"] = "عميل نقدي";
                dt.Rows.InsertAt(dr, 0);

                custoomername.DataSource = dt;
                custoomername.DisplayMember = "PersonName";
                custoomername.ValueMember = "ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل العملاء: " + ex.Message);
            }
        }
        #endregion


        private decimal GetQtyInGrid(int partId)
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.Cells["CID"].Value != null && (int)row.Cells["CID"].Value == partId)
                    total += Convert.ToDecimal(row.Cells["CQty"].Value);
            }
            return total;
        }



        #region Core Sales Logic (Add, Calculate, Save)

        private async void add_Click(object sender, EventArgs e)    
        {
            if (cmbParts.SelectedIndex == -1 || quantity.Value <= 0) return;

            int partId = Convert.ToInt32(cmbParts.SelectedValue);
            decimal addedQty = quantity.Value;
            decimal price = decimal.Parse(lblUnitPrice.Text);

            decimal currentStock = await _salesRepo.GetStockBalanceAsync(partId);
            decimal inGridQty = GetQtyInGrid(partId);
            decimal available = currentStock - inGridQty;

            if (addedQty > available)
            {
                MessageBox.Show($"عذراً! الرصيد المتاح حالياً هو ({available}) فقط", "نقص مخزون", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UpdateOrAddRow(partId, cmbParts.Text, price, addedQty);
            UpdateInvoiceTotals();
            ResetInputArea();
        }

        private void UpdateOrAddRow(int partId, string partName, decimal price, decimal qty)
        {
            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.Cells["CID"].Value != null && (int)row.Cells["CID"].Value == partId)
                {
                    decimal newQty = Convert.ToDecimal(row.Cells["CQty"].Value) + qty;
                    row.Cells["CQty"].Value = newQty;
                    row.Cells["CTotal"].Value = newQty * price;
                    return;
                }
            }
            dgvSales.Rows.Add(partId, partName, price, qty, (price * qty));
        }

        private void UpdateInvoiceTotals()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dgvSales.Rows)
                total += Convert.ToDecimal(row.Cells["CTotal"].Value);

            totallbl.Text = total.ToString("N2") + " ج.م";
            CalculateRemainder();
        }
        void CalculateRemainder()
        {
            try
            {
                decimal total = decimal.TryParse(totallbl.Text.Replace(" ج.م", ""), out decimal t) ? t : 0;
                decimal paid = decimal.TryParse(textBox1.Text, out decimal p) ? p : 0;
                decimal remainder = paid - total;

                label5.Text = Math.Abs(remainder).ToString("N2");
                label5.ForeColor = remainder >= 0 ? Color.Blue : Color.Red;
            }
            catch
            {
                label5.Text = "0.00";
            }
        }
        private void ClearForm()
        {
            dgvSales.Rows.Clear();
            totallbl.Text = "0.00 ج.م";
            textBox1.Text = "0.00";
            label5.Text = "0.00";
            ResetInputArea();
        }

        private void ResetInputArea()
        {
            cmbParts.SelectedIndex = -1;
            paymentmethod.SelectedIndex = 0;
            lblUnitPrice.Text = "0.00";
            quantity.Value = 1;
            cmbParts.Focus();
        }

        private async void savebtn_Click(object sender, EventArgs e)
        {
            if (dgvSales.Rows.Count == 0)
            {
                MessageBox.Show("برجاء إضافة أصناف للفاتورة أولاً", "تنبيه");
                return;
            }



            try
            {
                decimal total = decimal.Parse(totallbl.Text.Replace(" ج.م", ""));
                decimal paid = decimal.TryParse(textBox1.Text, out decimal p) ? p : 0;

                int? selectedCustomerId = (int)custoomername.SelectedValue;
                int finalCustomerId = (selectedCustomerId == -1 || selectedCustomerId == null) ? 8 : selectedCustomerId.Value;

                var header = new InvoiceHeader
                {
                    Time = DateTime.Now,
                    PaymentMethod = paymentmethod.Text,
                    TotalAmount = total,
                    PaidAmount = paid,
                    CustomerID = (int)custoomername.SelectedValue == -1 ? 8 : (int?)custoomername.SelectedValue,
                    InvoiceType = "مبيعات",
                    UserID = AuthService.CurrentSession.UserID
                };

                var details = dgvSales.Rows.Cast<DataGridViewRow>()
             .Select(row => new InvoiceDetail
             {
                 PartID = (int)row.Cells["CID"].Value,
                 Quantity = Convert.ToDecimal(row.Cells["CQty"].Value),
                 PartPrice = Convert.ToDecimal(row.Cells["CPrice"].Value),
                 Total = Convert.ToDecimal(row.Cells["CTotal"].Value)
             }).ToList();

                int invoiceID = await _salesRepo.SaveInvoiceAsync(header, details);


                MessageBox.Show($"تم حفظ الفاتورة بنجاح! رقم: {invoiceID}", "نجاح");

                if (paid > 0)
                {
                    await RecordInSafe(invoiceID, paid);
                }
                ClearForm();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "خطأ في الحفظ"); }
        }
        private async Task RecordInSafe(int invoiceID, decimal amount)
        {
            var safeTrans = new SafeTransaction
            {
                Amount = amount,
                TransactionType = "إيداع",
                Description = $"دفعة من فاتورة مبيعات رقم {invoiceID}",
                UserID = AuthService.CurrentSession.UserID,
                TransactionDate = DateTime.Now
            };

            ISafeRepository _safeRepo = new SafeRepository();
            await _safeRepo.AddTransactionAsync(safeTrans);
        }

        #endregion

        #region UX Enhancements & Hotkeys
        private void cmbParts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoaded) return;

            if (cmbParts.SelectedIndex == -1 || cmbParts.SelectedValue == null)
            {
                lblUnitPrice.Text = "0.00";
                return;
            }

            try
            {
                if (cmbParts.SelectedItem is DataRowView row)
                {
                    decimal price = Convert.ToDecimal(row["SellingPrice"]);
                    lblUnitPrice.Text = price.ToString("N2");
                }
            }
            catch
            {
                lblUnitPrice.Text = "0.00";
            }
        }


        private async void categorysearchcmbbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoaded && categorysearchcmbbox.SelectedValue is int catId)
                await LoadPartsByCategoryAsync(catId);
        }

        private void dgvSales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSales.Columns[e.ColumnIndex].Name == "CDelete" && e.RowIndex >= 0)
            {
                dgvSales.Rows.RemoveAt(e.RowIndex);
                UpdateInvoiceTotals();
            }
        }

        private void cmbParts_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbParts.Text)) lblUnitPrice.Text = "0.00";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            CalculateRemainder();
        }  

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void categorysearchcmbbox_Enter_1(object sender, EventArgs e)
        {
            Control ctrl = (Control)sender;
            ctrl.BackColor = Color.LightGoldenrodYellow;
        }

        private void categorysearchcmbbox_Leave_1(object sender, EventArgs e)
        {
            Control ctrl = (Control)sender;
            ctrl.BackColor = Color.White;
        }

        private void SellingForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Add)
            {
                quantity.Value = Math.Min(quantity.Maximum, quantity.Value + 1);
                e.Handled = true;
            }

            if (e.KeyCode == Keys.Subtract)
            {
                quantity.Value = Math.Max(quantity.Minimum, quantity.Value - 1);
                e.Handled = true;
            }

            if (e.KeyCode == Keys.F5)
            {
                savebtn.PerformClick();
            }
            if (e.KeyCode == Keys.Escape)
            {
                cmbParts.SelectedIndex = -1;
                e.Handled = true;
            }
        }

        private void cmbParts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && cmbParts.SelectedIndex != -1)
            {
               quantity.Focus();
                quantity.Select(0, quantity.Text.Length);
                e.SuppressKeyPress = true; 
            }
        }

        private void headerpanel_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }


        #endregion

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                savebtn.PerformClick();
            }
        }

        private void quantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; 

         

                if (custoomername.SelectedValue == null || (int)custoomername.SelectedValue == -1)
                {
                    custoomername.Focus();
                    custoomername.DroppedDown = true; 
                }
                else
                {
                    add.PerformClick();

                    cmbParts.Focus();
                }
            }
        }

        private void custoomername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                add.PerformClick();
            }

        }
    }
}