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
    public partial class frmLogin : Form
    {
        private readonly AuthService _authService;

        private bool drag;
        private Point start_point = new Point(0, 0);

        public frmLogin(AuthService authService)
        {
             InitializeComponent();
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));


        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {

            if (!ValidateInput()) return;

            string user = txtUsername.Text.Trim();
            string pass = txtPassword.Text;

            SetLoadingState(true);


            try
            {
                bool success = await _authService.LoginAsync(user, pass);

                if (success)
                {
                    this.Hide();
                    IPartRepository repo = new PartRepository();
                    frmHome mainForm = new frmHome(repo, _authService);

                    MessageBox.Show($"أهلاً بك يا {AuthService.CurrentSession.FullName}");
                    mainForm.FormClosed += (s, args) => this.Close();
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("بيانات الدخول غير صحيحة، يرجى مراجعة اسم المستخدم وكلمة المرور.",
                                  "فشل تسجيل الدخول", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ تقني: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("من فضلك أدخل اسم المستخدم", "حقل مطلوب", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUsername.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("من فضلك أدخل كلمة المرور", "حقل مطلوب", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPassword.Focus();
                return false;
            }
            return true;
        }

        private void SetLoadingState(bool isLoading)
        {
            btnLogin.Enabled = !isLoading;
            btnLogin.Text = isLoading ? "جاري التحقق..." : "تسجيل الدخول";
            this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            ApplyTheme();
        }
        private void ApplyTheme()
        {
            this.BackColor = Color.FromArgb(30, 30, 45);
            txtUsername.BorderStyle = BorderStyle.None;
            txtPassword.BorderStyle = BorderStyle.None;
        }

        private void btnLogin_MouseEnter(object sender, EventArgs e)
        {

            btnLogin.BackColor = Color.FromArgb(0, 150, 255); 
        }
        private void btnLogin_MouseLeave(object sender, EventArgs e)
        {
            btnLogin.BackColor = Color.LightSkyBlue;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void label3_MouseEnter(object sender, EventArgs e)
        {
            label3.ForeColor = Color.White;
            label3.BackColor = Color.Red;
        }
        private void label3_MouseLeave(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Red;
            label3.BackColor = Color.Transparent;
        }

        private void frmLogin_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }
        private void frmLogin_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true;
            start_point = new Point(e.X, e.Y);
        }

        private void frmLogin_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; 
                txtPassword.Focus();       
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnLogin.PerformClick();   
            }
        }
    }
}
