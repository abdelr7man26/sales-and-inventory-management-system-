using Auto_Parts_Store.Helpers;
using Auto_Parts_Store.Models;
using Auto_Parts_Store.Repositories;
using Auto_Parts_Store.Services;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Auto_Parts_Store
{
    public partial class FrmSafe : Form
    {
        private readonly ISafeRepository _safeRepo;

        static  private readonly IPartRepository _repo = new PartRepository();
        private readonly AuthService _authService = new AuthService(_repo);

        private DataTable _allTransactions;
        
        public FrmSafe(ISafeRepository safeRepo)
        {
            InitializeComponent();
            _safeRepo = safeRepo;
            dgvSafeHistory.ApplyCustomStyle();

            this.DoubleBuffered = true;
            this.ActiveControl = txtAmount;

        }

        private async void FrmSafe_Load(object sender, EventArgs e)
        {
            if (!ValidationHelper.ConfirmAdminAccess(_authService))
            {
                this.Close();
                return;
            }


            SetDefaultUIState();
            await LoadDataAndRefreshUI();
        }
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(out decimal amount, out string reason)) return;

            await ExecuteSaveTransaction(amount, reason);
        }
        private async void from_ValueChanged_1(object sender, EventArgs e)
        {
            ApplyFilter();
        }




        private void SetDefaultUIState()
        {
            from.Value = DateTime.Now.Date;
            to.Value = DateTime.Now.Date.AddDays(1).AddTicks(-1);
            rbWithdraw.Checked = true;
            txtAmount.Focus();
        }
        private async Task LoadDataAndRefreshUI()
        {
            try
            {
                var balanceTask = _safeRepo.GetSafeBalanceAsync();
                var historyTask = _safeRepo.GetSafeTransactionsHistoryAsync();

                await Task.WhenAll(balanceTask, historyTask);

                UpdateDailyBalanceLabel();
                _allTransactions = historyTask.Result; 

                ApplyFilter();
            }
            catch (Exception ex)
            {
                HandleError("فشل تحميل البيانات", ex);
            }
        }
        private void UpdateDailyBalanceLabel()
        {
            if (_allTransactions == null) return;

            string filter = string.Format("التاريخ >= #{0:MM/dd/yyyy}# AND التاريخ <= #{1:MM/dd/yyyy} 23:59:59#",
                                           DateTime.Now.Date, DateTime.Now.Date);

            var todayRows = _allTransactions.Select(filter);
            decimal dailyBalance = 0;

            foreach (var row in todayRows)
            {
                decimal amount = Convert.ToDecimal(row["المبلغ"]);
                string type = row["النوع"].ToString(); 
                dailyBalance += (type == "إيداع" ? amount : -amount);
            }

            lblBalance.Text = dailyBalance.ToString("C2"); 
        }
        private void ApplyFilter()
        {
            if (_allTransactions == null) return;

            DataView dv = _allTransactions.DefaultView;
            string filterString = string.Format("التاريخ >= #{0:MM/dd/yyyy} 00:00:00# AND التاريخ <= #{1:MM/dd/yyyy} 23:59:59#",
                                                from.Value, to.Value);
            dv.RowFilter = filterString;
            dgvSafeHistory.DataSource = dv;
            CalculateFilteredBalance(dv);
            FormatGridColumns();
        }
        private void CalculateFilteredBalance(DataView dv)
        {
            decimal totalIn = 0;
            decimal totalOut = 0;

            foreach (DataRowView rowView in dv)
            {
                DataRow row = rowView.Row;
                decimal amount = Convert.ToDecimal(row["المبلغ"]);
                string type = row["النوع"].ToString(); 

                if (type == "إيداع")
                    totalIn += amount;
                else if (type == "سحب")
                    totalOut += amount;
            }

            decimal netBalance = totalIn - totalOut;

            lblBalance.Text = netBalance.ToString("N2");

            lblBalance.ForeColor = netBalance >= 0 ? System.Drawing.Color.LimeGreen : System.Drawing.Color.Red;
        }
        private async Task ExecuteSaveTransaction(decimal amount, string reason)
        {
            btnSave.Enabled = false; // منع النقرات المتكررة (Double-click protection)
            try
            {
                var transaction = new SafeTransaction
                {
                    Amount = amount,
                    TransactionType = rbDeposit.Checked ? "إيداع" : "سحب",
                    Description = reason.Trim(),
                    UserID = AuthService.CurrentSession.UserID,
                    TransactionDate = DateTime.Now
                };

                await _safeRepo.AddTransactionAsync(transaction);

                txtAmount.Clear();
                txtReason.Clear();
                await LoadDataAndRefreshUI(); // تحديث تلقائي بعد الحفظ
                txtAmount.Focus();
            }
            catch (Exception ex)
            {
                HandleError("فشل في حفظ العملية", ex);
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }
    
        
        
        
        
        private void FormatGridColumns()
        {
            if (dgvSafeHistory.Columns.Count > 0 && dgvSafeHistory.Columns.Contains("المبلغ"))
            {
                dgvSafeHistory.Columns["المبلغ"].DefaultCellStyle.Format = "N2";
                dgvSafeHistory.Columns["المبلغ"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }
        private void HandleError(string message, Exception ex) =>
            MessageBox.Show($"{message}: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

        private void ShowWarning(string message) =>
            MessageBox.Show(message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private bool ValidateInputs(out decimal amount, out string reason)
        {
            amount = 0;
            reason = txtReason.Text;

            if (!decimal.TryParse(txtAmount.Text, out amount) || amount <= 0)
            {
                ShowWarning("برجاء إدخال مبلغ صحيح أكبر من الصفر");
                txtAmount.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                ShowWarning("برجاء إدخال بيان الحركة");
                txtReason.Focus();
                return false;
            }

            return true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }




        private void txtAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                txtReason.Focus();
            }
        }

        private void txtReason_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                btnSave.PerformClick();
                txtAmount.Focus();
            }

        }


    }
}
