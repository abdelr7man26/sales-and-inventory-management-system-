using Auto_Parts_Store.Helpers;
using Auto_Parts_Store.Models;
using Auto_Parts_Store.Repositories;
using Auto_Parts_Store.Services;
using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Auto_Parts_Store
{
    public partial class QuickPayForm : Form
    {
        private int _selectedPersonId = 0;
        private readonly IPersonRepository _personRepo;
        private readonly IQuickPayRepository _quickPayRepo;
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        public QuickPayForm()
        {
            InitializeComponent();
            _personRepo = new PersonRepository();
            _quickPayRepo = new QuickPayRepository();

            customerbtn.Checked = true;
        }
        private async Task SetupAutoComplete()
        {
            var type = customerbtn.Checked ? PersonType.Customer : PersonType.Supplier;
            DataTable dt = await _personRepo.GetAllPersonsAsync(type);

            AutoCompleteStringCollection data = new AutoCompleteStringCollection();

            foreach (DataRow row in dt.Rows)
            {
                data.Add(row["الأسم"].ToString());
            }

            txtSearch.AutoCompleteCustomSource = data;
            txtSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private async void btnConfirm_Click(object sender, EventArgs e)
        {
            if (_selectedPersonId == 0)
            {
                MessageBox.Show("برجاء اختيار عميل أو مورد أولاً");
                return;
            }

            if (!decimal.TryParse(Amount.Text, out decimal payAmount) )
            {
                MessageBox.Show("برجاء إدخال مبلغ صحيح");
                return;
            }

            try
            {
                decimal finalAmount = Math.Abs(payAmount);
                string finalTransactionType = customerbtn.Checked ? "إيداع" : "سحب";
                if (payAmount < 0)
                {
                    
                    finalTransactionType = (customerbtn.Checked) ? "سحب" : "إيداع";
                }

                var transaction = new SafeTransaction
                {
                    Amount = finalAmount,
                    TransactionType = finalTransactionType,
                    Description = $"{(payAmount < 0 ? "تسوية رصيد" : "سداد نقدي")} {lblName.Text} - {notes.Text}",
                    TransactionDate = DateTime.Now,
                    UserID = AuthService.CurrentSession.UserID
                };

                var type = customerbtn.Checked ? PersonType.Customer : PersonType.Supplier;
                bool success = await _quickPayRepo.ExecuteQuickPaymentAsync(transaction, _selectedPersonId, type);

                if (success)
                {
                    MessageBox.Show("تمت العملية بنجاح وتحديث الحسابات");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("فشلت العملية: " + ex.Message);
            }

        }


        private void ClearPersonDetails()
        {
            _selectedPersonId = 0;
            lblName.Text = "---";
            lblPhone.Text = "---";
            lblBalance.Text = "0.00";
        }

        private async void customerbtn_CheckedChanged_1(object sender, EventArgs e)
        {
            ClearPersonDetails();
            await SetupAutoComplete();

        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pnlHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private async void QuickPayForm_Load(object sender, EventArgs e)
        {
           await SetupAutoComplete();
            SetupPaymentMethods();

        }
        private void SetupPaymentMethods()
        {
            paymentmethod.Items.Clear();
            paymentmethod.Items.Add("نقدي");
            paymentmethod.Items.Add("تحويل بنكي");
            paymentmethod.Items.Add("فيزا");
            paymentmethod.SelectedIndex = 0; 
        }

        private async void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                await LoadSelectedPerson();
            }
        }
        private async Task LoadSelectedPerson()
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text)) return; 

            var type = customerbtn.Checked ? PersonType.Customer : PersonType.Supplier;
            DataTable dt = await _personRepo.SearchPersonsAsync(type, txtSearch.Text.Trim());

            DataRow selectedRow = null;
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["الأسم"].ToString().Trim() == txtSearch.Text.Trim())
                    {
                        selectedRow = row;
                        break;
                    }
                }
            }

            if (selectedRow != null)
            {
                _selectedPersonId = Convert.ToInt32(selectedRow["ID"]);
                lblName.Text = selectedRow["الأسم"].ToString();
                lblPhone.Text = selectedRow["التليفون"].ToString();
                lblBalance.Text = Convert.ToDecimal(selectedRow["الرصيد"]).ToString("N2");

                lblBalance.ForeColor = Convert.ToDecimal(selectedRow["الرصيد"]) < 0 ? Color.Red : Color.Black;
            }
            else
            {
                ClearPersonDetails();
            }
        }

        private void Amount_TextChanged(object sender, EventArgs e)
        {
            UpdateRemainingBalance();
        }
        private void UpdateRemainingBalance()
        {
            if (!decimal.TryParse(lblBalance.Text, out decimal currentBalance))
            {
                remaining.Text = "0.00";
                return;
            }

            if (decimal.TryParse(Amount.Text, out decimal paidAmount))
            {
                decimal Remaining = currentBalance - paidAmount;
                remaining.Text = Remaining.ToString("N2");

                remaining.ForeColor = (Remaining == 0) ? Color.LimeGreen : Color.OrangeRed;
            }
            else
            {
                remaining.Text = currentBalance.ToString("N2");
            }
        }
    }
}