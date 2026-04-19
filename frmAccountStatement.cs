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
            decimal totalInvoices = 0;    
            decimal totalReturns = 0;    
            decimal totalPaidCash = 0;  
            decimal totalReceivedFromReturn = 0;


            foreach (DataRow row in _dtMain.Rows)
            {
                totalInvoices += Convert.ToDecimal(row["قيمة الفاتورة"] ?? 0);
                totalReturns += Convert.ToDecimal(row["قيمة المرتجع"] ?? 0);
                totalPaidCash += Convert.ToDecimal(row["المدفوع كاش"] ?? 0);
                totalReceivedFromReturn += Convert.ToDecimal(row["مدفوع من مرتجع"] ?? 0);
            }

            lblTotalAccount.Text = totalInvoices.ToString("N2");
            returns.Text = totalReturns.ToString("N2");
            lblTotalPaid.Text = totalPaidCash.ToString("N2");
            recieved.Text = totalReceivedFromReturn.ToString("N2");

            decimal netGoods = totalInvoices - totalPaidCash;
            lblFinalBalance.Text = netGoods.ToString("N2");

            decimal netreturn = totalReturns - totalReceivedFromReturn;
            returnsfinal.Text = netreturn.ToString("N2"); 

            decimal finalBalance = netGoods - netreturn;

            final.Text = finalBalance.ToString("N2");

            this.final.ForeColor = finalBalance <= 0 ? Color.Green : Color.Red;
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
            sb.AppendLine($"الفترة من: {From.Value:yyyy/MM/dd} إلى: {To.Value:yyyy/MM/dd}");
            sb.AppendLine("------------------------------------------------------------------");

            sb.AppendLine(string.Format("{0,-12} | {1,-15} | {2,-10} | {3,-10} | {4,-10} | {5,-10}",
                "التاريخ", "المصدر", "الفاتورة", "مدفوع", "مرتجع", "م.مرتجع"));
            sb.AppendLine("------------------------------------------------------------------");

            foreach (DataRow row in _dtMain.Rows)
            {
                string date = Convert.ToDateTime(row["التاريخ"]).ToString("dd/MM/yyyy");
                string source = row["المصدر"].ToString();
                string invAmount = Convert.ToDecimal(row["قيمة الفاتورة"]).ToString("N0");
                string invPaid = Convert.ToDecimal(row["المدفوع كاش"]).ToString("N0");
                string retAmount = Convert.ToDecimal(row["قيمة المرتجع"]).ToString("N0");
                string retPaid = Convert.ToDecimal(row["مدفوع من مرتجع"]).ToString("N0");

                sb.AppendLine(string.Format("{0,-12} | {1,-15} | {2,-10} | {3,-10} | {4,-10} | {5,-10}",
                    date, source, invAmount, invPaid, retAmount, retPaid));
            }

            sb.AppendLine("------------------------------------------------------------------");
            sb.AppendLine($"إجمالي الفواتير: {lblTotalAccount.Text}  |  المدفوع منها: {lblTotalPaid.Text}");
            sb.AppendLine($"إجمالي المرتجعات: {returns.Text}  |  المستلم منها: {recieved.Text}");
            sb.AppendLine("------------------------------------------------------------------");
            sb.AppendLine($"صافي الفواتير: {lblFinalBalance.Text}");
            sb.AppendLine($"صافي المرتجعات: {returnsfinal.Text}");
            sb.AppendLine($"الرصيد النهائي المطلوب: {final.Text}");

            MessageBox.Show(sb.ToString(), "معاينة كشف الحساب قبل الطباعة");
        }




        private async void dgvStatement_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _isShowingDetails) return;

            try
            {
                int invID = Convert.ToInt32(dgvStatement.Rows[e.RowIndex].Cells["رقم"].Value);
                string opSource = dgvStatement.CurrentRow.Cells["المصدر"].Value.ToString();

                DataTable dtDetails = await _repo.GetInvoiceDetailsAsync(invID, opSource);

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
