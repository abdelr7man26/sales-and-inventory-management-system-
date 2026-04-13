using Auto_Parts_Store.Helpers;
using Auto_Parts_Store.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auto_Parts_Store
{
    public partial class frmAccountStatement : Form
    {
        private readonly ISalesRepository _repo;
        private readonly int _targetID;
        private readonly string _targetName;
        private DataTable _dtMain;
        private bool _isShowingDetails = false;

        public frmAccountStatement(int id, string name, ISalesRepository repo)
        {
            InitializeComponent();
            _targetID = id;
            _targetName = name;
            _repo = repo; 

            NameLBL.Text = name;
            this.KeyPreview = true;
        }

        private async void frmAccountStatement_Load_1(object sender, EventArgs e)
        {
            dgvStatement.ApplyCustomStyle();
            await LoadStatementData();
        }

        private async Task LoadStatementData()
        {
            try
            {
                _dtMain = await _repo.GetAccountStatementAsync(_targetID, From.Value, To.Value);
                dgvStatement.DataSource = _dtMain;
                CalculateTotals();
            }
            catch (Exception ex) { MessageBox.Show("خطأ في التحميل: " + ex.Message); }
        }

        private void CalculateTotals()
        {
            decimal tInv = 0, tPaid = 0;
            foreach (DataRow row in _dtMain.Rows)
            {
                tInv += Convert.ToDecimal(row["الإجمالي"]);
                tPaid += Convert.ToDecimal(row["المدفوع"]);
            }
            total.Text = tInv.ToString("N2");
            Mdin.Text = tPaid.ToString("N2");
            yden.Text = (tInv - tPaid).ToString("N2");
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await LoadStatementData();
        }

        private void print_Click(object sender, EventArgs e)
        {
            if (_dtMain == null || _dtMain.Rows.Count == 0) return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"كشف حساب: {_targetName}");
            sb.AppendLine($"إجمالي: {total.Text} | مدفوع: {Mdin.Text} | متبقي: {yden.Text}");
            sb.AppendLine("--------------------------------------------------");

            foreach (DataRow row in _dtMain.Rows)
                sb.AppendLine($"{row["التاريخ"]:dd/MM} | فاتورة {row["رقم الفاتورة"]} | {row["المتبقي"]}");

            MessageBox.Show(sb.ToString(), "معاينة الطباعة");
        }
        



        private async void dgvStatement_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _isShowingDetails) return;

            try
            {
                int invID = Convert.ToInt32(dgvStatement.Rows[e.RowIndex].Cells["رقم الفاتورة"].Value);

                DataTable dtDetails = await _repo.GetInvoiceDetailsAsync(invID);

                if (dtDetails != null)
                {
                    dgvStatement.DataSource = dtDetails;
                    _isShowingDetails = true;

                    NameLBL.Visible = false;
                    label1.Visible = false;

                    DetailsLBL.Text = $"تفاصيل فاتورة رقم: {invID} (اضغط Backspace للعودة)";
                    DetailsLBL.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في عرض التفاصيل: " + ex.Message);
            }
        }
        

        private void frmAccountStatement_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back && _isShowingDetails)
            {
                dgvStatement.DataSource = _dtMain;
                _isShowingDetails = false;
                UpdateUIState(false);
                e.Handled = true;
            }

        }

        private void UpdateUIState(bool showDetails, int invId = 0)
        {
            DetailsLBL.Visible = showDetails;
            DetailsLBL.Text = showDetails ? $"تفاصيل فاتورة: {invId}" : "";
            NameLBL.Visible = !showDetails;
            label1.Visible = !showDetails;
        }
    }
}
