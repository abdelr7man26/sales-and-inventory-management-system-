using Auto_Parts_Store.Helpers;
using Auto_Parts_Store.Models;
using Auto_Parts_Store.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Auto_Parts_Store
{
    public partial class Form2 : Form
    {
        IPartRepository _repo = new PartRepository();
        int selectedID = 0;
        DataTable dtParts = new DataTable();

        public Form2(IPartRepository repo)  
        {
            InitializeComponent();
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            this.DoubleBuffered = true;

            dgvParts.ApplyCustomStyle();
            dgvHistory.ApplyCustomStyle();
          
        }


        private async void Form2_Load(object sender, EventArgs e)
        {
            await Task.WhenAll(
                FillCategoriesCombo(),
                FillFilterCombo(),
                LoadAllParts(),
                GenerateAutoPartNumber()
            );
            AddHistoryButtonToGrid();
            ClearFields();
      
        }
        private async Task LoadAllParts()
        {
          
            try
            {
                dtParts = await _repo.GetAllPartsAsync();
                dgvParts.DataSource = dtParts;
                UpdateAutoComplete();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل البيانات: " + ex.Message);
            }
        }
        void AddHistoryButtonToGrid()
        {
            if (dgvParts.Columns.Contains("btnHistory")) return;

            DataGridViewButtonColumn btnCol = new DataGridViewButtonColumn();
            btnCol.HeaderText = "حركة الصنف";
            btnCol.Name = "btnHistory";
            btnCol.Text = "عرض السجل";
            btnCol.UseColumnTextForButtonValue = true;

            btnCol.FlatStyle = FlatStyle.Flat;
            btnCol.DefaultCellStyle.BackColor = Color.ForestGreen;
            btnCol.DefaultCellStyle.ForeColor = Color.White;
            btnCol.Width = 100;

            dgvParts.Columns.Add(btnCol);
        }



        private async Task GenerateAutoPartNumber()
        {
            try
            {
                string nextNumber = await _repo.GetPartNumberAsync();
                txtPartNumber.Text = nextNumber;
            }
            catch { /* معالجة الخطأ */ }
        }
        private async Task FillCategoriesCombo()
        {
            try
            {
                DataTable dt = await _repo.GetAllCategoriesAsync();
                cmbCategories.DataSource = dt;
                cmbCategories.DisplayMember = "categoryName";
                cmbCategories.ValueMember = "categoryID";
                cmbCategories.SelectedIndex = -1;
            }
            catch (Exception ex) { MessageBox.Show($"خطأ في تحميل الفئات: {ex.Message}"); }
        }

        private async Task FillFilterCombo()
        {
            try
            {
                DataTable dtFilter = await _repo.GetAllCategoriesAsync();
                DataRow dr = dtFilter.NewRow();
                dr["categoryID"] = 0;
                dr["categoryName"] = "الكل";
                dtFilter.Rows.InsertAt(dr, 0);


            Filterationcombobox.DataSource = dtFilter;
            Filterationcombobox.DisplayMember = "categoryName";
            Filterationcombobox.ValueMember = "categoryID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل فئات البحث: " + ex.Message);
            }
        }

        void ApplyFilter()
        {
            if (dtParts == null || dtParts.Rows.Count == 0) return;

            string filter = "1=1";

            if (Filterationcombobox.SelectedIndex > 0 && Filterationcombobox.Text != "الكل")
            {
                filter += string.Format(" AND [الفئة] = '{0}'", Filterationcombobox.Text.Replace("'", "''"));
            }

            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                filter += string.Format(" AND ([الاسم] LIKE '%{0}%' OR [رقم القطعة] LIKE '%{0}%')", txtSearch.Text);
            }

            dtParts.DefaultView.RowFilter = filter;

            dgvParts.DataSource = dtParts.DefaultView;

        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void Filterationcombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }


        void UpdateAutoComplete()
        {
            if (dtParts == null || dtParts.Rows.Count == 0) return;

            AutoCompleteStringCollection source = new AutoCompleteStringCollection();

            string[] namesAndNumbers = dtParts.AsEnumerable()
                .Where(r => r["الاسم"] != DBNull.Value && r["رقم القطعة"] != DBNull.Value)
                .Select(r => r.Field<string>("الاسم"))
                .Union(dtParts.AsEnumerable().Where(r => r["الاسم"] != DBNull.Value && r["رقم القطعة"] != DBNull.Value).Select(r => r.Field<string>("رقم القطعة")))
                .ToArray();

            source.AddRange(namesAndNumbers);

            txtSearch.AutoCompleteCustomSource = source;
            txtSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        void ClearFields()
        {
            selectedID = 0;
            txtPartName.Clear();
            txtPurchasePrice.Clear();
            txtSellingPrice.Clear();
            txtMinStock.Text = "5";
            txtQty.Clear();
            txtNotes.Clear();
            cmbCategories.SelectedIndex = -1;
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            txtPartName.Focus();
            dgvHistory.Visible = false;
            dgvParts.Visible = true;

        }

        private async Task ResetFormAsync()
        {

            ClearFields();


            if (string.IsNullOrEmpty(cmbCategories.Text) || cmbCategories.SelectedIndex >= 0)
            {
                cmbCategories.SelectedIndex = -1;
            }
         
            await GenerateAutoPartNumber();

        }
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (selectedID != 0)
            {
                MessageBox.Show("عفواً، أنت الآن في وضع التعديل. استخدم زرار 'تعديل' بدلاً من 'حفظ'.", "تنبيه");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPartName.Text) || string.IsNullOrWhiteSpace(txtPartNumber.Text))
            {
                MessageBox.Show("برجاء إدخال اسم ورقم القطعة", "بيانات ناقصة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPartName.Focus();
                return;
            }

            if (cmbCategories.SelectedIndex == -1)
            {
                MessageBox.Show("برجاء اختيار فئة الصنف أولاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategories.Focus();

                return;
            }

            decimal pPrice, sPrice;
            int MinQty;

            if (!decimal.TryParse(txtPurchasePrice.Text, out pPrice))
            {
                MessageBox.Show("سعر الشراء يجب أن يكون رقماً");
                txtPurchasePrice.Focus();
                txtPurchasePrice.SelectAll();
                return;
            }



            if (!decimal.TryParse(txtSellingPrice.Text, out sPrice))
            {
                MessageBox.Show("سعر البيع يجب أن يكون رقماً");
                txtSellingPrice.Focus();
                txtSellingPrice.SelectAll();
                return;
            }

            if (pPrice > sPrice)
            {
                var result = MessageBox.Show("تنبيه: سعر الشراء أكبر من سعر البيع، هل تريد الاستمرار؟",
                                             "تأكيد منطقي",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Warning);
                if (result == DialogResult.No) { txtSellingPrice.Focus(); return; } 
            }

            if (!int.TryParse(txtMinStock.Text, out MinQty))
            {
                MessageBox.Show("الكمية يجب أن تكون رقماً صحيحاً");
                txtMinStock.Focus();
                txtMinStock.SelectAll();
                return;
            }


            try
            {
                var newPart = new AutoPart
                {
                    PartName = txtPartName.Text.Trim(),
                    PartNumber = txtPartNumber.Text.Trim(),
                    PurchasePrice = decimal.Parse(txtPurchasePrice.Text),
                    SellingPrice = decimal.Parse(txtSellingPrice.Text),
                    MinimumStock = int.Parse(txtMinStock.Text),
                    CategoryID = (int)cmbCategories.SelectedValue,
                    Notes = txtNotes.Text.Trim(),
                    Quantity = 0
                };
                ValidationHelper.ValidatePart(newPart);

                await _repo.AddPartAsync(newPart);
                MessageBox.Show("تم الحفظ بنجاح!");
                await ResetFormAsync();
                await LoadAllParts(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ: " + ex.Message);
            }
        }


        private async void btnUpdate_Click(object sender, EventArgs e)
        {

            if (selectedID == 0)
            {
                MessageBox.Show("برجاء اختيار قطعة من الجدول أولاً لتعديلها");
                return;
            }

            decimal pPrice, sPrice;
            int qty, MinQty;
            if (!decimal.TryParse(txtPurchasePrice.Text, out pPrice) ||
                !decimal.TryParse(txtSellingPrice.Text, out sPrice) ||
                !int.TryParse(txtQty.Text, out qty) ||
                !int.TryParse(txtMinStock.Text, out MinQty))
            {
                MessageBox.Show("تأكد من إدخال الأرقام بشكل صحيح");
                txtPurchasePrice.Focus();
                return;
            }
            if (cmbCategories.SelectedIndex == -1)
            {
                MessageBox.Show("برجاء اختيار فئة الصنف أولاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategories.Focus();
                return;
            }

            if (pPrice > sPrice)
            {
                var result = MessageBox.Show("تنبيه: سعر الشراء أكبر من سعر البيع، هل تريد الاستمرار؟",
                                             "تأكيد منطقي",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Warning);
                if (result == DialogResult.No) { txtSellingPrice.Focus(); return; }
            }

            try
            {
                AutoPart updatedPart = new AutoPart
                {
                    PartID = selectedID,
                    PartName = txtPartName.Text.Trim(),
                    PartNumber = txtPartNumber.Text.Trim(),
                    PurchasePrice = decimal.Parse(txtPurchasePrice.Text),
                    SellingPrice = decimal.Parse(txtSellingPrice.Text),
                    Quantity = int.Parse(txtQty.Text),
                    MinimumStock = int.Parse(txtMinStock.Text),
                    CategoryID = (int)cmbCategories.SelectedValue,
                    Notes = txtNotes.Text.Trim()
                };
                ValidationHelper.ValidatePart(updatedPart);
                await _repo.UpdatePartAsync(updatedPart);

                MessageBox.Show("تم تحديث بيانات القطعة بنجاح!", "تعديل");

                await ResetFormAsync();
                await LoadAllParts(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ: " + ex.Message);
            }

        }


        private async void btnDelete_Click(object sender, EventArgs e)
        {

            if (selectedID == 0) { MessageBox.Show("اختر قطعة أولاً"); return; }
            DataGridViewRow row = dgvParts.SelectedRows[0];
            int quantity = 0;

            if (row.Cells["الكمية"].Value != DBNull.Value)
            {
                quantity = Convert.ToInt32(row.Cells["الكمية"].Value);
            }

            if (quantity > 0)
            {
                MessageBox.Show($"عفواً، لا يمكن حذف هذا الصنف لأن الكمية المتوفرة بالمخزن لم تنتهِ.\n(الكمية الحالية: {quantity})",
                                "رفض الحذف",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return; 
            }

            var result = MessageBox.Show("هل أنت متأكد من حذف هذه القطعة نهائياً؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                await _repo.DeletePartAsync(selectedID);
                await ResetFormAsync();
                await LoadAllParts(); 
                MessageBox.Show("تم الحذف");

            }
        }


        private async void button2_Click(object sender, EventArgs e)
        {
            frmAddCategory frm = new frmAddCategory(_repo);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                await RefreshCategoriesAsync();
            }

        }


        private async void dgvParts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvParts.Columns[e.ColumnIndex].Name == "btnHistory")
            {
                int id = Convert.ToInt32(dgvParts.Rows[e.RowIndex].Cells["كود"].Value);

                var historyData = await _repo.GetPartHistoryAsync(id);

                if (historyData.Rows.Count > 0)
                {
                    dgvHistory.DataSource = historyData;

                    dgvHistory.ApplyCustomStyle();
                    dgvHistory.ReadOnly = true;
                    dgvHistory.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvHistory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    dgvParts.Visible = false;
                    dgvHistory.Visible = true;
                    dgvHistory.BringToFront();
                }
                else
                {
                    MessageBox.Show("لا توجد حركات مسجلة لهذا الصنف بعد.");
                }
            }
        }


        private async void dgvParts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
             if (e.RowIndex >= 0 && dgvParts.Rows[e.RowIndex].Cells["كود"].Value != DBNull.Value)
                {
                    try
                    {
                        btnSave.Enabled = false;
                        btnUpdate.Enabled = true;
                        btnDelete.Enabled = true;

                        var row = dgvParts.Rows[e.RowIndex];
                        selectedID = Convert.ToInt32(row.Cells["كود"].Value);

                        txtPartName.Text = row.Cells["الاسم"].Value?.ToString();
                        txtPartNumber.Text = row.Cells["رقم القطعة"].Value?.ToString();
                        txtPurchasePrice.Text = row.Cells["سعر الشراء"].Value?.ToString();
                        txtSellingPrice.Text = row.Cells["سعر البيع"].Value?.ToString();
                        txtMinStock.Text = row.Cells["حد الطلب"].Value?.ToString();
                        cmbCategories.Text = row.Cells["الفئة"].Value?.ToString();
                        txtNotes.Text = row.Cells["ملاحظات"].Value?.ToString();

                        txtQty.Text = row.Cells["الكمية"].Value?.ToString();
                        txtQty.ReadOnly = true;
                        txtQty.BackColor = Color.LightGray; ;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"خطأ في عرض بيانات الصنف: {ex.Message}");
                    }
                }
             else
                {
                   await ResetFormAsync();
                }

        }


        private void dgvParts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvParts.Rows.Count) return;

            if (dgvParts.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                return;
            }


            e.CellStyle.SelectionBackColor = Color.DodgerBlue;
            e.CellStyle.SelectionForeColor = Color.White;

            try
            {
                DataGridViewRow row = dgvParts.Rows[e.RowIndex];

                if (row.Cells["الكمية"].Value != DBNull.Value && row.Cells["حد الطلب"].Value != DBNull.Value)
                {
                    int qty = Convert.ToInt32(row.Cells["الكمية"].Value);
                    int minStock = Convert.ToInt32(row.Cells["حد الطلب"].Value);

                    if (qty <= 0)
                    {
                        e.CellStyle.BackColor = Color.MistyRose;
                        e.CellStyle.ForeColor = Color.DarkRed;
                    }
                    else if (qty <= minStock)
                    {
                        e.CellStyle.BackColor = Color.LightGoldenrodYellow;
                        e.CellStyle.ForeColor = Color.DarkGoldenrod;
                    }
                }
            }
            catch { }
        }
        

        private async void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                await ResetFormAsync();
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (!(ActiveControl is TextBox txt && txt.Multiline))
                {
                    e.SuppressKeyPress = true; 
                    this.SelectNextControl(ActiveControl, true, true, true, true);
                }
            }
            if (e.KeyCode == Keys.F5)
            {
                await LoadAllParts();
                await FillCategoriesCombo();
                await FillFilterCombo();
                MessageBox.Show("تم تحديث البيانات");
            }
            if (e.KeyCode == Keys.Back && dgvHistory.Visible == true)
            {
                dgvHistory.Visible = false;
                dgvParts.Visible = true;
                e.SuppressKeyPress = true; 
            }
        }

        private async void dgvParts_MouseDown(object sender, MouseEventArgs e)
        {
            var hit = dgvParts.HitTest(e.X, e.Y);

            if (hit.Type == DataGridViewHitTestType.None)
            {
                await ResetFormAsync();
                dgvParts.ClearSelection();
            }
        }


        private void txtPartName_Enter(object sender, EventArgs e)
        {
            (sender as TextBox).BackColor = Color.LightGoldenrodYellow;
            txtPartName.SelectAll();
        }

        private void txtPartName_Leave(object sender, EventArgs e)
        {
            (sender as TextBox).BackColor = Color.White;
        }

        private void txtPurchasePrice_Enter(object sender, EventArgs e)
        {
            (sender as TextBox).BackColor = Color.LightGoldenrodYellow;
            txtPurchasePrice.SelectAll();


        }

        private void txtPurchasePrice_Leave(object sender, EventArgs e)
        {
            (sender as TextBox).BackColor = Color.White;
        }

        private void txtSellingPrice_Enter(object sender, EventArgs e)
        {
            (sender as TextBox).BackColor = Color.LightGoldenrodYellow;
            txtSellingPrice.SelectAll();

        }

  
        private void txtSellingPrice_Leave(object sender, EventArgs e)
        {
            (sender as TextBox).BackColor = Color.White;

        }


        private void txtPartNumber_Enter(object sender, EventArgs e)
        {
            (sender as TextBox).BackColor = Color.LightGoldenrodYellow;
            txtPartNumber.SelectAll();

        }

        private void txtPartNumber_Leave(object sender, EventArgs e)
        {
            (sender as TextBox).BackColor = Color.White;

        }

        private void txtMinStock_Enter(object sender, EventArgs e)
        {
            (sender as TextBox).BackColor = Color.LightGoldenrodYellow;
            txtMinStock.SelectAll();

        }

        private void cmbCategories_Enter(object sender, EventArgs e)
        {
            (sender as ComboBox).BackColor = Color.LightGoldenrodYellow;
            if (sender is ComboBox cmb)
            {
                cmb.DroppedDown = true;
            }

        }

        private void txtNotes_Enter(object sender, EventArgs e)
        {
            (sender as RichTextBox).BackColor = Color.LightGoldenrodYellow;
            txtNotes.SelectAll();

        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            (sender as TextBox).BackColor = Color.LightGoldenrodYellow;

        }

        private void Filterationcombobox_Enter(object sender, EventArgs e)
        {
            (sender as ComboBox).BackColor = Color.LightGoldenrodYellow;

        }

        private void txtMinStock_Leave(object sender, EventArgs e)
        {
            (sender as TextBox).BackColor = Color.White;

        }

        private void cmbCategories_Leave(object sender, EventArgs e)
        {
            (sender as ComboBox).BackColor = Color.White;

        }

        private void txtNotes_Leave(object sender, EventArgs e)
        {
            (sender as RichTextBox).BackColor = Color.White;

        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            (sender as TextBox).BackColor = Color.White;

        }

        private void Filterationcombobox_Leave(object sender, EventArgs e)
        {
            (sender as ComboBox).BackColor = Color.White;

        }




   
        private async Task RefreshCategoriesAsync()
        {
            try
            {
                DataTable dt = await _repo.GetAllCategoriesAsync();

                cmbCategories.DataSource = dt;
                cmbCategories.DisplayMember = "categoryName";
                cmbCategories.ValueMember = "categoryID";

                await FillFilterCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في تحديث الفئات: {ex.Message}");
            }
        }

        private void dgvHistory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvHistory.Columns[e.ColumnIndex].Name != "النوع") return;

            try
            {
                string type = e.Value?.ToString();

                if (string.IsNullOrEmpty(type)) return;

         
                if (type == "مبيعات" || type == "Sales")
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new Font(dgvHistory.Font, FontStyle.Bold); 
                }
                else if (type == "مشتريات" || type == "Purchase")
                {
                    e.CellStyle.ForeColor = Color.Green;
                    e.CellStyle.Font = new Font(dgvHistory.Font, FontStyle.Bold);
                }
            }
            catch
            {
            }
        }

        private void dgvHistory_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvHistory.Rows)
            {
                string type = row.Cells["النوع"].Value?.ToString();
                if (type == "مبيعات")
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                }
                else if (type == "Purchase")
                {
                    row.DefaultCellStyle.ForeColor = Color.Green;
                }
            }
        }
    }
}

