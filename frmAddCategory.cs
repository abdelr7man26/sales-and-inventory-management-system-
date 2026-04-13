using Auto_Parts_Store.Helpers;
using Auto_Parts_Store.Repositories;
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
    public partial class frmAddCategory : Form
    {
        private readonly IPartRepository _repo;
        private int selectedCatID = 0;
        private bool isResetting = false; 

        public frmAddCategory(IPartRepository repo)
        {
            InitializeComponent();
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));

            ConfigureGrid();
        }

        private void ConfigureGrid()
        {
            dgvcategories.ApplyCustomStyle();
            dgvcategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvcategories.MultiSelect = false;
            dgvcategories.ReadOnly = true;

            if (dgvcategories.Columns.Count == 0)
            {
                dgvcategories.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "categoryID", Name = "categoryID", HeaderText = "الكود", Width = 70 });
                dgvcategories.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "categoryName", Name = "categoryName", HeaderText = "اسم الفئة", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            }
            dgvcategories.CellFormatting += dgvcategories_CellFormatting;

        }
        private async void frmAddCategory_Load(object sender, EventArgs e)
        {
            await LoadGridAsync();
            textBox1.Focus();
        }

        private async Task LoadGridAsync()
        {
            try
            {
                dgvcategories.DataSource = await _repo.GetAllCategoriesAsync();
                ResetUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في تحميل البيانات: {ex.Message}");
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("من فضلك اكتب اسم الفئة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                await _repo.AddCategoryAsync(textBox1.Text.Trim());

                MessageBox.Show("تم إضافة الفئة بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                await LoadGridAsync(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء الحفظ: " + ex.Message);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (selectedCatID == 0) return;

            if (MessageBox.Show("هل أنت متأكد من حذف هذه الفئة؟\nتنبيه: قد يؤدي هذا لحذف أو تأثر الأصناف المرتبطة بها.",
                "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    await _repo.DeleteCategoryAsync(selectedCatID);
                    MessageBox.Show("تم حذف الفئة بنجاح");
                    this.DialogResult = DialogResult.OK;
                    await LoadGridAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("لا يمكن حذف الفئة لأنها مرتبطة بأصناف موجودة.");
                }
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (selectedCatID == 0) return;

            try
            {
                await _repo.UpdateCategoryAsync(selectedCatID, textBox1.Text.Trim());
                MessageBox.Show("تم تحديث اسم الفئة بنجاح");
                this.DialogResult = DialogResult.OK;
                await LoadGridAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في التعديل: " + ex.Message);
            }

        }

        private void dgvcategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvcategories.Rows[e.RowIndex];
                selectedCatID = Convert.ToInt32(row.Cells["categoryID"].Value);

                isResetting = true; 
                textBox1.Text = row.Cells["categoryName"].Value.ToString();
                isResetting = false;

                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }


        private void ResetUI()
        {
            selectedCatID = 0;
            textBox1.Clear();
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            textBox1.Focus();
            dgvcategories.ClearSelection();
        }

        private void dgvcategories_MouseDown(object sender, MouseEventArgs e)
        {
            var hit = dgvcategories.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.None)
            {
                ResetUI();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (isResetting) return; // لا تفعل شيئاً إذا كان التغيير بسبب ResetUI

            if (dgvcategories.DataSource is DataTable dt)
            {
                string filterText = textBox1.Text.Trim().Replace("'", "''");
                dt.DefaultView.RowFilter = string.IsNullOrEmpty(filterText) ? "" : $"categoryName LIKE '%{filterText}%'";

                // لو لقى تطابق تام
                bool matchFound = false;
                foreach (DataRowView rowView in dt.DefaultView)
                {
                    if (rowView["categoryName"].ToString().Equals(filterText, StringComparison.OrdinalIgnoreCase))
                    {
                        selectedCatID = Convert.ToInt32(rowView["categoryID"]);
                        button1.Enabled = false; // اخفاء الاضافة
                        button2.Enabled = true;  // تفعيل الحذف
                        button3.Enabled = true;  // تفعيل التعديل
                        matchFound = true;
                        break;
                    }
                }

                if (!matchFound)
                {
                    selectedCatID = 0;
                    button1.Enabled = !string.IsNullOrWhiteSpace(filterText);
                    button2.Enabled = false;
                    button3.Enabled = false;
                    dgvcategories.ClearSelection();
                }
            }
        }

        private void dgvcategories_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvcategories.Rows[e.RowIndex].Selected)
            {
                e.CellStyle.SelectionBackColor = Color.DodgerBlue;
                e.CellStyle.SelectionForeColor = Color.White;
                e.CellStyle.BackColor = Color.DodgerBlue; // إجبار اللون حتى لو فيه AlternatingStyle
                e.CellStyle.ForeColor = Color.White;
            }
        }
    }
}
    