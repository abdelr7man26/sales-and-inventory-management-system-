using Auto_Parts_Store.Helpers;
using Auto_Parts_Store.Models;
using Auto_Parts_Store.Repositories;
using Auto_Parts_Store.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auto_Parts_Store
{
    public partial class frmReturns : Form
    {
        private readonly IReturnsRepository _returnsRepo;
        private string currentInvoiceType = "";

        public frmReturns()
        {
            InitializeComponent();
            _returnsRepo = new ReturnsRepository();
            dgvInvoiceItems.ApplyCustomStyle();
        }

        private void frmReturns_Load(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                txtInvoiceID.Focus();
            }));

            paymentmethod.Items.Clear();
            paymentmethod.Items.Add("كاش");
            paymentmethod.Items.Add("آجل");
            paymentmethod.SelectedIndex = 0;
            paymentmethod.DropDownStyle = ComboBoxStyle.DropDownList;

        }


        private void txtInvoiceID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInvoiceID.Text) || !int.TryParse(txtInvoiceID.Text, out int invoiceId))
            {
                MessageBox.Show("من فضلك أدخل رقم فاتورة صحيح.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                lblStatus.Text = "جاري البحث...";
                lblStatus.ForeColor = Color.Gray;
                btnSearch.Enabled = false;

                var result = await _returnsRepo.GetInvoiceHeaderAsync(invoiceId);

                if (result.Invoice != null)
                {
                    var inv = result.Invoice;
                    currentInvoiceType = result.Invoice.InvoiceType;
                    bool isSales = currentInvoiceType != "توريد";

                    string typeName = isSales ? "مبيعات" : "مشتريات";
                    Color themeColor = isSales ? Color.Green : Color.DodgerBlue;

                    lblStatus.Text = $"فاتورة {typeName} | {result.PersonName} | بتاريخ: {inv.Time:yyyy-MM-dd}";
                    lblStatus.ForeColor = themeColor;

                    await LoadInvoiceItems(invoiceId);
                }
                else
                {
                    lblStatus.Text = "عفواً.. رقم الفاتورة غير موجود!";
                    lblStatus.ForeColor = Color.Red;
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء البحث: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSearch.Enabled = true;
                txtInvoiceID.Focus();
            }
        }
        private async Task LoadInvoiceItems(int invId)
        {
            var items = await _returnsRepo.GetInvoiceDetailsAsync(invId);

            dgvInvoiceItems.Rows.Clear();

            foreach (var item in items)
            {
                int rowIndex = dgvInvoiceItems.Rows.Add();
                var row = dgvInvoiceItems.Rows[rowIndex];

                row.Cells["colPartID"].Value = item.PartID;
                row.Cells["colPartName"].Value = item.PartName;
                row.Cells["colPartNumber"].Value = item.PartNumber;
                row.Cells["colQty"].Value = item.Quantity;
                row.Cells["colPrice"].Value = item.PurchasePrice;
                row.Cells["colReturnQty"].Value = 0;
                row.Cells["colTotal"].Value = 0;
            }
            if (dgvInvoiceItems.Rows.Count > 0)
            {
                dgvInvoiceItems.CurrentCell = dgvInvoiceItems.Rows[0].Cells["colReturnQty"];
                dgvInvoiceItems.BeginEdit(true);
            }
        }

        private void dgvInvoiceItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= NumericCheck_KeyPress;
            if (dgvInvoiceItems.CurrentCell.ColumnIndex == dgvInvoiceItems.Columns["colReturnQty"].Index)
            {
                e.Control.KeyPress += NumericCheck_KeyPress;
            }
        }
        private void NumericCheck_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void dgvInvoiceItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvInvoiceItems.Columns[e.ColumnIndex].Name == "colReturnQty")
            {
                var row = dgvInvoiceItems.Rows[e.RowIndex];

                if (row.Cells["colPrice"].Value == null || row.Cells["colQty"].Value == null) return;

                try
                {
                    decimal price = Convert.ToDecimal(row.Cells["colPrice"].Value);
                    decimal originalQty = Convert.ToDecimal(row.Cells["colQty"].Value);

                    decimal returnQty = 0;
                    if (row.Cells["colReturnQty"].Value != null && !string.IsNullOrEmpty(row.Cells["colReturnQty"].Value.ToString()))
                    {
                        returnQty = Convert.ToDecimal(row.Cells["colReturnQty"].Value);
                    }

                    if (returnQty > originalQty)
                    {
                        MessageBox.Show($"عفواً! الكمية المتاحة للإرجاع هي {originalQty} فقط.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        dgvInvoiceItems.CellValueChanged -= dgvInvoiceItems_CellValueChanged;
                        row.Cells["colReturnQty"].Value = 0;
                        row.Cells["colTotal"].Value = 0;
                        dgvInvoiceItems.CellValueChanged += dgvInvoiceItems_CellValueChanged;
                        return;
                    }

                    row.Cells["colTotal"].Value = price * returnQty;

                    UpdateFinalTotal();
                }
                catch { }
            }
        }

        private void UpdateFinalTotal()
        {
            decimal finalTotal = 0;
            foreach (DataGridViewRow row in dgvInvoiceItems.Rows)
            {
                finalTotal += Convert.ToDecimal(row.Cells["colTotal"].Value ?? 0);
            }

            lblTotalRefund.Text = finalTotal.ToString("N2");
        }

        private async void btnSaveReturn_Click(object sender, EventArgs e)
        {
            var details = new List<ReturnDetailDTO>();
            foreach (DataGridViewRow row in dgvInvoiceItems.Rows)
            {
                if (row.Cells["colReturnQty"].Value != null)
                {
                    decimal retQty = Convert.ToDecimal(row.Cells["colReturnQty"].Value);
                    if (retQty > 0)
                    {
                        details.Add(new ReturnDetailDTO
                        {
                            PartID = (int)row.Cells["colPartID"].Value,
                            Quantity = (int)retQty,
                            RefundAmount = Convert.ToDecimal(row.Cells["colTotal"].Value)
                        });
                    }
                }
            }

            if (details.Count == 0)
            {
                MessageBox.Show("من فضلك حدد الكميات المراد إرجاعها.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var header = new ReturnHeaderDTO
            {
                InvoiceID = int.Parse(txtInvoiceID.Text),
                ReturnType = currentInvoiceType == "توريد" ? "مشتريات" : "بيع",
                PaymentMethod = paymentmethod.SelectedItem.ToString(),
                TotalAmount = Convert.ToDecimal(lblTotalRefund.Text),
                UserID = AuthService.CurrentSession.UserID,
                Notes = txtReturnReason.Text.Trim()
            };

            try
            {
                btnSaveReturn.Enabled = false;

                bool success = await _returnsRepo.SaveReturnTransactionAsync(header, details);

                if (success)
                {
                    MessageBox.Show("تم حفظ المرتجع بنجاح، وتحديث المخازن والحسابات.", "نجاح العملية", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetForm();
                    paymentmethod.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء الحفظ: " + ex.Message, "خطأ سحب بيانات", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSaveReturn.Enabled = true;
            }
        }
        private void ResetForm()
        {
            txtInvoiceID.Clear();
            txtReturnReason.Clear();
            dgvInvoiceItems.Rows.Clear();
            lblStatus.Text = "في انتظار رقم الفاتورة...";
            lblStatus.ForeColor = Color.Black;
            lblTotalRefund.Text = "0.00";
            currentInvoiceType = "";
            paymentmethod.SelectedIndex = 0;
            txtInvoiceID.Focus();
        }

        private void dgvInvoiceItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int currentRowIndex = dgvInvoiceItems.CurrentCell.RowIndex;
                int totalRows = dgvInvoiceItems.Rows.Count;

                if (currentRowIndex == totalRows - 1)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;

                    txtReturnReason.Focus();
                }
            }
        }

        private void txtReturnReason_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                paymentmethod.Focus();
                paymentmethod.DroppedDown =true;
            }
        }

        private void paymentmethod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSaveReturn.PerformClick();
                txtInvoiceID.Focus();
            }
        }
    }
}
