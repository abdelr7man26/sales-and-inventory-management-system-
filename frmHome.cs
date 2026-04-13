using Auto_Parts_Store;
using Auto_Parts_Store.Repositories;
using Auto_Parts_Store.Helpers;
using Auto_Parts_Store.Services;
using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auto_Parts_Store
{
    public partial class frmHome : Form
    {

        private readonly IPartRepository _repo;
        private readonly ISafeRepository _safe = new SafeRepository();
        private readonly ISalesRepository _salesRepo = new SalesRepository();

        private readonly AuthService _authService;
        private Form activeForm = null;

        private BindingSource alertsBindingSource;
        public frmHome(IPartRepository repo , AuthService Service)
        {
            InitializeComponent();
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _authService = Service ?? throw new ArgumentNullException(nameof(Service));
            alertsBindingSource = new BindingSource();
            this.DoubleBuffered = true;
        }

        private async void frmHome_Load(object sender, EventArgs e)
        {
            dgvHomeAlerts.ApplyCustomStyle();

            string userName = AuthService.CurrentSession?.FullName ?? "غير مسجل";
            string role = AuthService.CurrentSession?.CurrentUserRole ?? "بدون";
            lblCurrentUser.Text = $"المستخدم الحالي: {userName} | الصلاحية: {role}";
            ApplyPermissions();
            await LoadDashboardDataAsync();
        }
        private void ApplyPermissions()
        {
            if (!_authService.IsAdmin())
            {
            }
        }


        private async Task LoadDashboardDataAsync()
        {
            try
            {
                await UpdateDashboardStatsAsync();
                await LoadHomeAlertsAsync();
            }
            catch (Exception ex)
            {
                    MessageBox.Show("حدث خطأ أثناء الاتصال بقاعدة البيانات. الرجاء المحاولة لاحقاً.", "خطأ تقني", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task UpdateDashboardStatsAsync()
        {
            DataTable dtStats = await _repo.GetDashboardStatsAsync();
            if (dtStats != null && dtStats.Rows.Count > 0)
            {
                var row = dtStats.Rows[0];
                lblTotalPartsCount.Text = row["Total"].ToString();
                lblLowStockCount.Text = row["Low"].ToString();
                lblOutOfStockCount.Text = row["Out"].ToString();
                lblTodaySalesCount.Tag = row["DailySales"].ToString();

                if (_authService.IsAdmin())
                {
                    lblTodaySalesCount.Text = row["DailySales"].ToString(); 
                    lblTodaySalesCount.ForeColor = Color.White; 
                }
                else
                {
                    lblTodaySalesCount.Text = "****"; 
                    lblTodaySalesCount.ForeColor = Color.DarkGreen; 
                }
            }
        }

        private async Task LoadHomeAlertsAsync()
        {
            DataTable dt = await _repo.GetLowStockAlertsAsync();

            alertsBindingSource.DataSource = dt;
            dgvHomeAlerts.DataSource = dt;
            ToggleAlertsVisibility(dt.Rows.Count > 0);
        }
        private void ToggleAlertsVisibility(bool hasAlerts)
        {
            dgvHomeAlerts.Visible = hasAlerts;
            txtSearchAlerts.Visible = hasAlerts;
            search.Visible = hasAlerts;
            lblNoAlerts.Visible = !hasAlerts;

            panel3.BackColor = !hasAlerts ? Color.SeaGreen : Color.FromArgb(30, 30, 30);
        }



        private void openChildForm(Form childForm)
        {
            if (childForm == null) return;

            SetHomeControlsVisibility(false);

            activeForm?.Close();
            activeForm?.Dispose();

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            pnlContainer.Controls.Add(childForm);
            pnlContainer.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        private void SetHomeControlsVisibility(bool isVisible)
        {
            dgvHomeAlerts.Visible = isVisible;
            tableLayoutPanel2.Visible = isVisible;
            headerpanel.Visible = isVisible;
            if (isVisible)
                pnlContainer.SendToBack();
            else
                pnlContainer.BringToFront();
        }




        private void inventorybtn_Click(object sender, EventArgs e)
        {
           
            openChildForm(new Form2(_repo));
        }

        private async void homebutton_Click(object sender, EventArgs e)
        {
            activeForm?.Close();
            activeForm = null;
            SetHomeControlsVisibility(true);
            await LoadDashboardDataAsync();
        }

        private void sellsbtn_Click(object sender, EventArgs e)
        {

            openChildForm(new SellingForm(_salesRepo, _repo));
        }

        private void Purshesbtn_Click(object sender, EventArgs e)
        {
            openChildForm(new PurshesesForm());
        }

        private void clientsbtn_Click(object sender, EventArgs e)
        {
            IPersonRepository _repo = new PersonRepository();

            openChildForm(new People(_repo));
        }

        private void Safebtn_Click(object sender, EventArgs e)
        {
            
            FrmSafe safeForm = new FrmSafe(_safe);

            safeForm.ShowDialog();

        }



        private void txtSearchAlerts_TextChanged(object sender, EventArgs e)
        {
            if (dgvHomeAlerts.DataSource is DataTable dt)
            {
                string safeSearchText = txtSearchAlerts.Text.Replace("'", "''").Replace("[", "[[]").Replace("]", "[]]");
                dt.DefaultView.RowFilter = $"[اسم القطعة] LIKE '%{safeSearchText}%'";
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder exportText = new StringBuilder();

            if (dgvHomeAlerts.Rows.Count == 0)
            {
                MessageBox.Show("لا توجد بيانات للتصدير", "تنبيه");
                exportText.AppendLine($" ");
                Clipboard.SetText(exportText.ToString());
                return;
            }

            exportText.AppendLine($"قائمة نواقص قطع الغيار - {DateTime.Now:dd/MM/yyyy}");
            exportText.AppendLine("---------------------------------");

            foreach (DataGridViewRow row in dgvHomeAlerts.Rows)
            {
                if (row.Cells["اسم القطعة"].Value != null)
                {
                    exportText.AppendLine($"- {row.Cells["اسم القطعة"].Value} (الموجود: {row.Cells["الكمية المتاحة"].Value})");
                }
            }

            Clipboard.SetText(exportText.ToString());
            MessageBox.Show("تم نسخ القائمة للـ Clipboard", "تم التصدير");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy  |  hh:mm:ss tt");

        }
        private void lblTodaySalesCount_Click(object sender, EventArgs e)
        {

            if (_authService.IsAdmin()) return;

            if (lblTodaySalesCount.Text != "****")
            {
                HideSales();
                return;
            }

            if (lblTodaySalesCount.Tag == null) return;

            string password = Microsoft.VisualBasic.Interaction.InputBox("أدخل كلمة المرور (PIN) لعرض المبيعات:", "تحقق من الهوية", "");

            if (_authService.VerifyAdminPin(password))
            {
                lblTodaySalesCount.Text = lblTodaySalesCount.Tag.ToString();
                lblTodaySalesCount.ForeColor = Color.White; 
                hideTimer.Start(); 
            }
            else if (!string.IsNullOrEmpty(password))
            {
                MessageBox.Show("كلمة المرور غير صحيحة!", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void hideTimer_Tick(object sender, EventArgs e)
        {
            HideSales();
        }
        private void HideSales()
        {
            if (!_authService.IsAdmin())
            {
                lblTodaySalesCount.Text = "****";
                lblTodaySalesCount.ForeColor = Color.DarkGreen; 
            }
            hideTimer.Stop();
        }

        private void dgvHomeAlerts_DataBindingComplete_1(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvHomeAlerts.Rows)
            {
                if (row.Cells["الكمية المتاحة"].Value != DBNull.Value && row.Cells["الكمية المتاحة"].Value != null)
                {
                    int qty = Convert.ToInt32(row.Cells["الكمية المتاحة"].Value);

                    if (qty == 0)
                    {
                        row.DefaultCellStyle.BackColor = Color.MistyRose;
                        row.DefaultCellStyle.ForeColor = Color.Red;
                        row.DefaultCellStyle.Font = new Font(dgvHomeAlerts.Font, FontStyle.Bold);
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.Yellow;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }
        }

     
    }
}