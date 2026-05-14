using Auto_Parts_Store.Helpers;
using Auto_Parts_Store.Models;
using Auto_Parts_Store.Repositories;
using Auto_Parts_Store.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Auto_Parts_Store
{
    public class frmSettings : Form
    {
        // ── Palette ────────────────────────────────────────────────
        private static readonly Color NavyDark  = Color.FromArgb(20,  25,  72);
        private static readonly Color NavyDeep  = Color.FromArgb(30,  34,  90);
        private static readonly Color NavyMid   = Color.FromArgb(45,  52, 120);
        private static readonly Color Accent    = Color.FromArgb(0,  149, 255);
        private static readonly Color PageBg    = Color.FromArgb(245, 246, 250);
        private static readonly Color CardWhite = Color.White;
        private static readonly Color DangerRed = Color.FromArgb(220, 53, 69);
        private static readonly Color SuccessGreen = Color.FromArgb(40, 167, 69);

        // ── Services ───────────────────────────────────────────────
        private readonly IUserAdminService _userSvc;
        private readonly ISettingsService  _settSvc;

        // ── Layout controls ────────────────────────────────────────
        private Panel pnlHeader;
        private Panel pnlSidebar;
        private Panel pnlContent;

        // Content panels (switched by sidebar)
        private Panel pnlStoreProfile;
        private Panel pnlUsers;
        private Panel pnlPermissions;
        private Panel pnlBackup;
        private Panel pnlUIPrefs;

        private Panel  _activePanel;
        private Button _activeBtn;

        // ── Store-profile controls ─────────────────────────────────
        private TextBox txtStoreName, txtStorePhone, txtStoreAddress, txtStoreTax;

        // ── Users controls ─────────────────────────────────────────
        private DataGridView dgvUsers;
        private ComboBox     cmbUserRole;
        private TextBox      txtUserName, txtFullName, txtPhone, txtAddress, txtPassword;
        private int          _selectedPersonId = -1;

        // ── Permissions controls ───────────────────────────────────
        private ComboBox         cmbRole;
        private CheckedListBox   clbPermissions;
        private IList<SystemPermissionEntry> _currentPermissions;

        // ── UI-pref controls ───────────────────────────────────────
        private ComboBox cmbTheme, cmbPrintFormat;

        // ──────────────────────────────────────────────────────────

        public frmSettings()
        {
            _userSvc = new UserAdminService(new UserAdminRepository());
            _settSvc = new SettingsService(new SettingsRepository());
            Build();
        }

        // ══════════════════════════════════════════════════════════
        //  UI CONSTRUCTION
        // ══════════════════════════════════════════════════════════

        private void Build()
        {
            SuspendLayout();

            Text          = "الإعدادات وإدارة النظام";
            Size          = new Size(1080, 720);
            MinimumSize   = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor     = PageBg;
            Font          = new Font("Segoe UI", 10);
            RightToLeft   = RightToLeft.Yes;

            BuildHeader();
            BuildSidebar();
            BuildContentArea();
            BuildStoreProfilePanel();
            BuildUsersPanel();
            BuildPermissionsPanel();
            BuildBackupPanel();
            BuildUIPrefsPanel();

            // Show store profile on open
            ShowPanel(pnlStoreProfile);

            ResumeLayout(true);
        }

        // ── Header ─────────────────────────────────────────────────

        private void BuildHeader()
        {
            pnlHeader = new Panel { Dock = DockStyle.Top, Height = 58, BackColor = NavyDark };

            var lbl = MakeLbl("⚙  الإعدادات وإدارة النظام", 15, FontStyle.Bold, Color.White);
            lbl.AutoSize = true;
            lbl.Location = new Point(20, 14);

            var btnClose = MakeBtn("✕", DangerRed, 38, 34);
            btnClose.Anchor   = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Location = new Point(pnlHeader.Width - 50, 12);
            btnClose.Click   += (s, e) => Close();

            pnlHeader.Controls.AddRange(new Control[] { lbl, btnClose });
            pnlHeader.SizeChanged += (s, e) => btnClose.Location = new Point(pnlHeader.Width - 50, 12);
            Controls.Add(pnlHeader);
        }

        // ── Sidebar ────────────────────────────────────────────────

        private void BuildSidebar()
        {
            pnlSidebar = new Panel
            {
                Dock      = DockStyle.Left,
                Width     = 200,
                BackColor = NavyDeep,
                Padding   = new Padding(0, 20, 0, 0)
            };

            var sections = new (string Label, string Key)[]
            {
                ("🏪  ملف المتجر",       "StoreProfile"),
                ("👥  إدارة المستخدمين", "Users"),
                ("🔒  الصلاحيات",        "Permissions"),
                ("💾  النسخ الاحتياطي",  "Backup"),
                ("🎨  المظهر والطباعة",  "UIPrefs"),
            };

            int y = 20;
            foreach (var (label, key) in sections)
            {
                var btn = new Button
                {
                    Text      = "  " + label,
                    Tag       = key,
                    Location  = new Point(0, y),
                    Size      = new Size(200, 42),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = NavyDeep,
                    ForeColor = Color.FromArgb(200, 210, 240),
                    Font      = new Font("Segoe UI", 10),
                    TextAlign = ContentAlignment.MiddleRight,
                    Cursor    = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize         = 0;
                btn.FlatAppearance.MouseOverBackColor = NavyMid;
                btn.Click += SidebarBtn_Click;

                pnlSidebar.Controls.Add(btn);
                y += 44;
            }

            Controls.Add(pnlSidebar);
        }

        private void SidebarBtn_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            SetActiveSideBtn(btn);

            switch (btn.Tag?.ToString())
            {
                case "StoreProfile":  ShowPanel(pnlStoreProfile); LoadStoreProfile();   break;
                case "Users":         ShowPanel(pnlUsers);         _ = LoadUsers();      break;
                case "Permissions":   ShowPanel(pnlPermissions);   _ = LoadRoles();      break;
                case "Backup":        ShowPanel(pnlBackup);                               break;
                case "UIPrefs":       ShowPanel(pnlUIPrefs);       _ = LoadUIPrefs();    break;
            }
        }

        private void SetActiveSideBtn(Button btn)
        {
            if (_activeBtn != null) { _activeBtn.BackColor = NavyDeep; _activeBtn.ForeColor = Color.FromArgb(200, 210, 240); }
            _activeBtn = btn;
            btn.BackColor = NavyMid;
            btn.ForeColor = Color.White;
        }

        // ── Content container ──────────────────────────────────────

        private void BuildContentArea()
        {
            pnlContent = new Panel { Dock = DockStyle.Fill, BackColor = PageBg, Padding = new Padding(20) };
            Controls.Add(pnlContent);
        }

        private void ShowPanel(Panel target)
        {
            _activePanel?.Hide();
            _activePanel = target;
            if (!pnlContent.Controls.Contains(target))
            {
                target.Dock = DockStyle.Fill;
                pnlContent.Controls.Add(target);
            }
            target.Show();
            target.BringToFront();
        }

        // ══════════════════════════════════════════════════════════
        //  PANEL 1 — STORE PROFILE
        // ══════════════════════════════════════════════════════════

        private void BuildStoreProfilePanel()
        {
            pnlStoreProfile = new Panel { BackColor = PageBg, Visible = false };

            var card = MakeCard(0, 0, 600, 320, "بيانات المتجر");
            card.Dock = DockStyle.Fill;

            txtStoreName    = MakeTextBox(card, "اسم المتجر",   50);
            txtStorePhone   = MakeTextBox(card, "رقم الهاتف",   100);
            txtStoreAddress = MakeTextBox(card, "العنوان",      150);
            txtStoreTax     = MakeTextBox(card, "الرقم الضريبي", 200);

            var btnSave = MakeBtn("💾  حفظ البيانات", SuccessGreen, 160, 38);
            btnSave.Location = new Point(card.Width - 180, 260);
            btnSave.Anchor   = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.Click   += BtnSaveProfile_Click;
            card.Controls.Add(btnSave);

            pnlStoreProfile.Controls.Add(card);
        }

        private async void LoadStoreProfile()
        {
            try
            {
                var p = await _settSvc.GetStoreProfileAsync();
                txtStoreName.Text    = p.Name;
                txtStorePhone.Text   = p.Phone;
                txtStoreAddress.Text = p.Address;
                txtStoreTax.Text     = p.TaxNumber;
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private async void BtnSaveProfile_Click(object sender, EventArgs e)
        {
            try
            {
                await _settSvc.SaveStoreProfileAsync(new StoreProfile
                {
                    Name      = txtStoreName.Text.Trim(),
                    Phone     = txtStorePhone.Text.Trim(),
                    Address   = txtStoreAddress.Text.Trim(),
                    TaxNumber = txtStoreTax.Text.Trim()
                });
                ShowSuccess("تم حفظ بيانات المتجر بنجاح.");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        // ══════════════════════════════════════════════════════════
        //  PANEL 2 — USER MANAGEMENT
        // ══════════════════════════════════════════════════════════

        private void BuildUsersPanel()
        {
            pnlUsers = new Panel { BackColor = PageBg, Visible = false };

            // Two-column layout: grid fills left, form is fixed-width on the other side
            var tbl = new System.Windows.Forms.TableLayoutPanel
            {
                Dock        = DockStyle.Fill,
                ColumnCount = 2,
                RowCount    = 1,
                Margin      = new Padding(0)
            };
            tbl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
            tbl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 340f));
            tbl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));

            // Grid card (fills remaining width)
            var gridCard = MakeCard(0, 0, 100, 100, "قائمة المستخدمين");
            gridCard.Dock = DockStyle.Fill;

            dgvUsers = new DataGridView { Dock = DockStyle.Fill };
            dgvUsers.ApplyCustomStyle();
            dgvUsers.SelectionChanged += DgvUsers_SelectionChanged;

            var btnRefresh = MakeBtn("🔄  تحديث", Accent, 100, 32);
            btnRefresh.Dock    = DockStyle.Bottom;
            btnRefresh.Click  += (s, e) => _ = LoadUsers();
            gridCard.Controls.Add(btnRefresh);
            gridCard.Controls.Add(dgvUsers);

            // Form card (fixed 340px column)
            var formCard = MakeCard(0, 0, 100, 100, "إضافة / تعديل مستخدم");
            formCard.Dock = DockStyle.Fill;

            txtUserName  = MakeTextBox(formCard, "اسم المستخدم",  50);
            txtFullName  = MakeTextBox(formCard, "الاسم الكامل",  100);
            txtPhone     = MakeTextBox(formCard, "الهاتف",        150);
            txtAddress   = MakeTextBox(formCard, "العنوان",       200);
            txtPassword  = MakeTextBox(formCard, "كلمة المرور",  250);
            txtPassword.PasswordChar = '●';

            var lblRole = MakeLbl("الدور:", 9, FontStyle.Regular, Color.Gray);
            lblRole.Location = new Point(10, 302);
            cmbUserRole = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Size          = new Size(200, 28),
                Location      = new Point(10, 320)
            };
            formCard.Controls.AddRange(new Control[] { lblRole, cmbUserRole });

            var btnAdd = MakeBtn("➕  إضافة", SuccessGreen, 140, 34);
            btnAdd.Location = new Point(10, 360);
            btnAdd.Click   += BtnAddUser_Click;

            var btnUpdate = MakeBtn("✏  تعديل", Color.FromArgb(255, 153, 0), 140, 34);
            btnUpdate.Location = new Point(160, 360);
            btnUpdate.Click   += BtnUpdateUser_Click;

            var btnDelete = MakeBtn("🗑  حذف", DangerRed, 290, 34);
            btnDelete.Location = new Point(10, 400);
            btnDelete.Click   += BtnDeleteUser_Click;

            formCard.Controls.AddRange(new Control[] { btnAdd, btnUpdate, btnDelete });

            tbl.Controls.Add(gridCard, 0, 0);
            tbl.Controls.Add(formCard, 1, 0);
            pnlUsers.Controls.Add(tbl);

            _ = LoadRolesIntoCombo();
        }

        private async System.Threading.Tasks.Task LoadUsers()
        {
            try
            {
                var users = await _userSvc.GetAllUsersAsync();
                dgvUsers.DataSource = users.Select(u => new
                {
                    PersonID  = u.PersonID,
                    اسم_المستخدم = u.UserName,
                    الاسم_الكامل = u.FullName,
                    الهاتف        = u.Phone,
                    الدور         = u.Role
                }).ToList();

                if (dgvUsers.Columns.Contains("PersonID"))
                    dgvUsers.Columns["PersonID"].Visible = false;
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private async System.Threading.Tasks.Task LoadRolesIntoCombo()
        {
            try
            {
                var roles = await _userSvc.GetAllRolesAsync();
                cmbUserRole.DataSource    = roles;
                cmbUserRole.DisplayMember = "RoleName";
                cmbUserRole.ValueMember   = "RoleName";
            }
            catch { /* table may not exist yet */ }
        }

        private void DgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null) return;
            var row = dgvUsers.CurrentRow;
            _selectedPersonId = Convert.ToInt32(row.Cells["PersonID"].Value);
            txtUserName.Text  = row.Cells["اسم_المستخدم"]?.Value?.ToString();
            txtFullName.Text  = row.Cells["الاسم_الكامل"]?.Value?.ToString();
            txtPhone.Text     = row.Cells["الهاتف"]?.Value?.ToString();
            txtPassword.Text  = string.Empty;

            string role = row.Cells["الدور"]?.Value?.ToString();
            if (role != null) cmbUserRole.Text = role;
        }

        private async void BtnAddUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            { MessageBox.Show("اسم المستخدم وكلمة المرور مطلوبان.", "تنبيه"); return; }

            try
            {
                await _userSvc.AddUserAsync(new UserAdminEntry
                {
                    UserName = txtUserName.Text.Trim(),
                    FullName = txtFullName.Text.Trim(),
                    Phone    = txtPhone.Text.Trim(),
                    Address  = txtAddress.Text.Trim(),
                    Role     = cmbUserRole.Text
                }, txtPassword.Text);

                ShowSuccess("تم إضافة المستخدم بنجاح.");
                await LoadUsers();
                ClearUserForm();
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private async void BtnUpdateUser_Click(object sender, EventArgs e)
        {
            if (_selectedPersonId < 0) { MessageBox.Show("اختر مستخدماً من القائمة.", "تنبيه"); return; }
            try
            {
                await _userSvc.UpdateUserAsync(new UserAdminEntry
                {
                    PersonID = _selectedPersonId,
                    UserName = txtUserName.Text.Trim(),
                    FullName = txtFullName.Text.Trim(),
                    Phone    = txtPhone.Text.Trim(),
                    Address  = txtAddress.Text.Trim(),
                    Role     = cmbUserRole.Text
                });

                if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                    await _userSvc.ChangePasswordAsync(_selectedPersonId, txtPassword.Text);

                ShowSuccess("تم تعديل المستخدم بنجاح.");
                await LoadUsers();
                ClearUserForm();
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private async void BtnDeleteUser_Click(object sender, EventArgs e)
        {
            if (_selectedPersonId < 0) { MessageBox.Show("اختر مستخدماً من القائمة.", "تنبيه"); return; }
            if (_selectedPersonId == AuthService.CurrentSession?.UserID)
            { MessageBox.Show("لا يمكن حذف المستخدم الحالي.", "تنبيه"); return; }

            if (MessageBox.Show("هل أنت متأكد من حذف هذا المستخدم؟", "تأكيد",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                await _userSvc.DeleteUserAsync(_selectedPersonId);
                ShowSuccess("تم حذف المستخدم.");
                await LoadUsers();
                ClearUserForm();
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void ClearUserForm()
        {
            txtUserName.Text = txtFullName.Text = txtPhone.Text =
            txtAddress.Text  = txtPassword.Text = string.Empty;
            _selectedPersonId = -1;
        }

        // ══════════════════════════════════════════════════════════
        //  PANEL 3 — PERMISSIONS MATRIX
        // ══════════════════════════════════════════════════════════

        private void BuildPermissionsPanel()
        {
            pnlPermissions = new Panel { BackColor = PageBg, Visible = false };

            var card = MakeCard(0, 0, 700, 560, "مصفوفة الصلاحيات");
            card.Dock = DockStyle.Fill;

            var lblRole = MakeLbl("اختر الدور:", 10, FontStyle.Regular, NavyDark);
            lblRole.Location = new Point(15, 45);
            cmbRole = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Size = new Size(220, 30), Location = new Point(15, 70) };
            cmbRole.SelectedIndexChanged += async (s, e) => await LoadPermissionsForRole();

            clbPermissions = new CheckedListBox
            {
                Location      = new Point(15, 110),
                Size          = new Size(660, 380),
                CheckOnClick  = true,
                Font          = new Font("Segoe UI", 10),
                BorderStyle   = BorderStyle.FixedSingle,
                RightToLeft   = RightToLeft.Yes,
                Anchor        = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };

            var btnSave = MakeBtn("💾  حفظ الصلاحيات", SuccessGreen, 180, 36);
            btnSave.Location = new Point(card.Width - 200, 505);
            btnSave.Anchor   = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.Click   += BtnSavePermissions_Click;

            card.Controls.AddRange(new Control[] { lblRole, cmbRole, clbPermissions, btnSave });
            pnlPermissions.Controls.Add(card);
        }

        private async System.Threading.Tasks.Task LoadRoles()
        {
            try
            {
                var roles = await _userSvc.GetAllRolesAsync();
                cmbRole.DataSource    = roles;
                cmbRole.DisplayMember = "RoleName";
                cmbRole.ValueMember   = "RoleID";
                if (roles.Count > 0) await LoadPermissionsForRole();
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private async System.Threading.Tasks.Task LoadPermissionsForRole()
        {
            if (cmbRole.SelectedItem == null) return;
            int roleId = ((SystemRole)cmbRole.SelectedItem).RoleID;

            try
            {
                _currentPermissions = await _userSvc.GetPermissionsForRoleAsync(roleId);
                clbPermissions.Items.Clear();

                string lastModule = string.Empty;
                foreach (var p in _currentPermissions)
                {
                    if (p.Module != lastModule)
                    {
                        clbPermissions.Items.Add("── " + p.Module + " ──");
                        lastModule = p.Module;
                    }

                    int idx = clbPermissions.Items.Add(p);
                    clbPermissions.SetItemChecked(idx, p.IsGranted);
                }
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private async void BtnSavePermissions_Click(object sender, EventArgs e)
        {
            if (cmbRole.SelectedItem == null) return;
            int roleId = ((SystemRole)cmbRole.SelectedItem).RoleID;

            if (((SystemRole)cmbRole.SelectedItem).IsBuiltIn &&
                ((SystemRole)cmbRole.SelectedItem).RoleName == "Admin")
            {
                MessageBox.Show("لا يمكن تعديل صلاحيات دور المدير العام.", "تنبيه"); return;
            }

            var grantedIds = new List<int>();
            for (int i = 0; i < clbPermissions.Items.Count; i++)
            {
                if (clbPermissions.Items[i] is SystemPermissionEntry pe && clbPermissions.GetItemChecked(i))
                    grantedIds.Add(pe.PermissionID);
            }

            try
            {
                await _userSvc.SaveRolePermissionsAsync(roleId, grantedIds);
                ShowSuccess("تم حفظ الصلاحيات بنجاح.");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        // ══════════════════════════════════════════════════════════
        //  PANEL 4 — DATABASE BACKUP
        // ══════════════════════════════════════════════════════════

        private void BuildBackupPanel()
        {
            pnlBackup = new Panel { BackColor = PageBg, Visible = false };

            var card = MakeCard(0, 0, 600, 340, "النسخ الاحتياطي واستعادة قاعدة البيانات");
            card.Dock = DockStyle.Fill;

            var lblPath = MakeLbl("مجلد النسخ الاحتياطية:", 10, FontStyle.Regular, NavyDark);
            lblPath.Location = new Point(15, 50);

            var txtPath = new TextBox
            {
                Location = new Point(15, 75),
                Size     = new Size(450, 28),
                Anchor   = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Text     = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                           + @"\AutoPartsBackup"
            };

            var btnBrowse = MakeBtn("📂", Color.Gray, 50, 28);
            btnBrowse.Location = new Point(470, 75);
            btnBrowse.Anchor   = AnchorStyles.Top | AnchorStyles.Right;
            btnBrowse.Click   += (s, e) =>
            {
                using (var dlg = new FolderBrowserDialog { SelectedPath = txtPath.Text })
                    if (dlg.ShowDialog() == DialogResult.OK) txtPath.Text = dlg.SelectedPath;
            };

            var btnBackup = MakeBtn("💾  نسخ احتياطي الآن", SuccessGreen, 220, 42);
            btnBackup.Location = new Point(15, 125);
            btnBackup.Click   += async (s, e) =>
            {
                btnBackup.Enabled = false;
                try
                {
                    string path = await _settSvc.BackupDatabaseAsync(txtPath.Text);
                    ShowSuccess("تم إنشاء النسخة الاحتياطية:\n" + path);
                }
                catch (Exception ex) { ShowError(ex); }
                finally { btnBackup.Enabled = true; }
            };

            var sep = new Label { Text = "─────────────────────────────────────────", Location = new Point(15, 180), AutoSize = true, ForeColor = Color.LightGray };

            var lblRestore = MakeLbl("استعادة من نسخة احتياطية:", 10, FontStyle.Bold, Color.FromArgb(180, 80, 0));
            lblRestore.Location = new Point(15, 195);

            var lblWarn = MakeLbl("⚠  سيتم إغلاق جميع الاتصالات. أعد تشغيل التطبيق بعد الاستعادة.", 8, FontStyle.Regular, Color.Gray);
            lblWarn.Location = new Point(15, 215);
            lblWarn.AutoSize = true;

            var btnRestore = MakeBtn("📂  اختر ملف واستعد", DangerRed, 220, 42);
            btnRestore.Location = new Point(15, 240);
            btnRestore.Click   += async (s, e) =>
            {
                using (var dlg = new OpenFileDialog { Filter = "Backup files (*.bak)|*.bak", Title = "اختر ملف النسخة الاحتياطية" })
                {
                    if (dlg.ShowDialog() != DialogResult.OK) return;

                    if (MessageBox.Show("هل أنت متأكد من الاستعادة؟ سيتم حذف جميع البيانات الحالية.",
                        "تأكيد الاستعادة", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                    btnRestore.Enabled = false;
                    try
                    {
                        string sql = $@"
                            USE master;
                            ALTER DATABASE AutoPartsStoreDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                            RESTORE DATABASE AutoPartsStoreDB FROM DISK = '{dlg.FileName.Replace("'", "''")}' WITH REPLACE;
                            ALTER DATABASE AutoPartsStoreDB SET MULTI_USER;";
                        await DbHelper.ExecuteNonQueryAsync(sql);
                        MessageBox.Show("تمت الاستعادة. يرجى إعادة تشغيل التطبيق.", "نجاح",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.Exit();
                    }
                    catch (Exception ex) { ShowError(ex); }
                    finally { btnRestore.Enabled = true; }
                }
            };

            card.Controls.AddRange(new Control[] { lblPath, txtPath, btnBrowse, btnBackup, sep, lblRestore, lblWarn, btnRestore });
            pnlBackup.Controls.Add(card);
        }

        // ══════════════════════════════════════════════════════════
        //  PANEL 5 — UI PREFERENCES
        // ══════════════════════════════════════════════════════════

        private void BuildUIPrefsPanel()
        {
            pnlUIPrefs = new Panel { BackColor = PageBg, Visible = false };

            var card = MakeCard(0, 0, 500, 280, "تفضيلات الواجهة");
            card.Dock = DockStyle.Fill;

            var lblTheme = MakeLbl("المظهر:", 10, FontStyle.Regular, NavyDark);
            lblTheme.Location = new Point(15, 50);

            cmbTheme = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Size = new Size(200, 30), Location = new Point(15, 74) };
            cmbTheme.Items.AddRange(new[] { "Light", "Dark" });

            var lblPrint = MakeLbl("تنسيق الطباعة:", 10, FontStyle.Regular, NavyDark);
            lblPrint.Location = new Point(15, 115);

            cmbPrintFormat = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Size = new Size(200, 30), Location = new Point(15, 139) };
            cmbPrintFormat.Items.AddRange(new[] { "A4", "80mm" });

            var btnSave = MakeBtn("💾  حفظ التفضيلات", SuccessGreen, 180, 36);
            btnSave.Location = new Point(card.Width - 200, 220);
            btnSave.Anchor   = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.Click   += BtnSaveUIPrefs_Click;

            card.Controls.AddRange(new Control[] { lblTheme, cmbTheme, lblPrint, cmbPrintFormat, btnSave });
            pnlUIPrefs.Controls.Add(card);
        }

        private async System.Threading.Tasks.Task LoadUIPrefs()
        {
            try
            {
                var prefs = await _settSvc.GetUIPreferencesAsync();
                cmbTheme.Text       = prefs.Theme;
                cmbPrintFormat.Text = prefs.PrintFormat;
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private async void BtnSaveUIPrefs_Click(object sender, EventArgs e)
        {
            try
            {
                await _settSvc.SaveUIPreferencesAsync(new UIPreferences
                {
                    Theme       = cmbTheme.Text,
                    PrintFormat = cmbPrintFormat.Text
                });
                ShowSuccess("تم حفظ تفضيلات الواجهة.");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        // ══════════════════════════════════════════════════════════
        //  FACTORY HELPERS
        // ══════════════════════════════════════════════════════════

        private Panel MakeCard(int x, int y, int w, int h, string title)
        {
            var card = new Panel
            {
                Location  = new Point(x, y),
                Size      = new Size(w, h),
                BackColor = CardWhite,
                Padding   = new Padding(10)
            };
            card.Paint += (s, e) => e.Graphics.DrawRectangle(new Pen(Color.FromArgb(220, 224, 235)), 0, 0, card.Width - 1, card.Height - 1);

            if (!string.IsNullOrEmpty(title))
            {
                var accent = new Panel { Size = new Size(w, 4), Location = new Point(0, 0), BackColor = Accent };
                var lbl    = MakeLbl(title, 12, FontStyle.Bold, NavyDark);
                lbl.Location = new Point(12, 10);
                lbl.AutoSize = true;
                card.Controls.AddRange(new Control[] { accent, lbl });
            }
            return card;
        }

        private static TextBox MakeTextBox(Panel parent, string label, int top)
        {
            var lbl = new Label
            {
                Text      = label + ":",
                Location  = new Point(10, top),
                AutoSize  = true,
                Font      = new Font("Segoe UI", 8.5f),
                ForeColor = Color.Gray
            };
            var txt = new TextBox
            {
                Location = new Point(10, top + 18),
                Size     = new Size(parent.Width - 30, 28),
                Font     = new Font("Segoe UI", 10),
                Anchor   = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            parent.Controls.AddRange(new Control[] { lbl, txt });
            return txt;
        }

        private static Label MakeLbl(string text, float size, FontStyle style, Color fore)
            => new Label { Text = text, Font = new Font("Segoe UI", size, style), ForeColor = fore, AutoSize = true };

        private static Button MakeBtn(string text, Color back, int w, int h)
        {
            var b = new Button
            {
                Text      = text,
                Size      = new Size(w, h),
                BackColor = back,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        private static void ShowSuccess(string msg)
            => MessageBox.Show(msg, "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);

        private static void ShowError(Exception ex)
            => MessageBox.Show("حدث خطأ:\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
