namespace Auto_Parts_Store
{
    partial class People
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Suppcontainer = new System.Windows.Forms.TabPage();
            this.data = new System.Windows.Forms.DataGridView();
            this.supplierDetails = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.search = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStatement = new System.Windows.Forms.Button();
            this.btndelete = new System.Windows.Forms.Button();
            this.btnedit = new System.Windows.Forms.Button();
            this.btnadd = new System.Windows.Forms.Button();
            this.PhoneTXT = new System.Windows.Forms.TextBox();
            this.AddressTXT = new System.Windows.Forms.TextBox();
            this.NameTXT = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CustContainer = new System.Windows.Forms.TabPage();
            this.custData = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.CUSTSEARCHTXT = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.custStatement = new System.Windows.Forms.Button();
            this.deleteCust = new System.Windows.Forms.Button();
            this.editCust = new System.Windows.Forms.Button();
            this.ADDCUST = new System.Windows.Forms.Button();
            this.CustPhoneTXT = new System.Windows.Forms.TextBox();
            this.CUSTAddressTXT = new System.Windows.Forms.TextBox();
            this.custNameTXT = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.Suppcontainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.data)).BeginInit();
            this.supplierDetails.SuspendLayout();
            this.CustContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.custData)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Suppcontainer);
            this.tabControl1.Controls.Add(this.CustContainer);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabControl1.RightToLeftLayout = true;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 450);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // Suppcontainer
            // 
            this.Suppcontainer.BackColor = System.Drawing.Color.Transparent;
            this.Suppcontainer.Controls.Add(this.data);
            this.Suppcontainer.Controls.Add(this.supplierDetails);
            this.Suppcontainer.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Suppcontainer.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Suppcontainer.Location = new System.Drawing.Point(4, 30);
            this.Suppcontainer.Name = "Suppcontainer";
            this.Suppcontainer.Padding = new System.Windows.Forms.Padding(3);
            this.Suppcontainer.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Suppcontainer.Size = new System.Drawing.Size(792, 416);
            this.Suppcontainer.TabIndex = 0;
            this.Suppcontainer.Text = "الموردين";
            // 
            // data
            // 
            this.data.AllowUserToAddRows = false;
            this.data.AllowUserToDeleteRows = false;
            this.data.AllowUserToResizeColumns = false;
            this.data.AllowUserToResizeRows = false;
            this.data.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.data.Location = new System.Drawing.Point(3, 156);
            this.data.Name = "data";
            this.data.ReadOnly = true;
            this.data.Size = new System.Drawing.Size(786, 257);
            this.data.TabIndex = 1;
            this.data.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.data_CellClick);
            this.data.MouseDown += new System.Windows.Forms.MouseEventHandler(this.data_MouseDown);
            // 
            // supplierDetails
            // 
            this.supplierDetails.Controls.Add(this.label4);
            this.supplierDetails.Controls.Add(this.search);
            this.supplierDetails.Controls.Add(this.label2);
            this.supplierDetails.Controls.Add(this.btnStatement);
            this.supplierDetails.Controls.Add(this.btndelete);
            this.supplierDetails.Controls.Add(this.btnedit);
            this.supplierDetails.Controls.Add(this.btnadd);
            this.supplierDetails.Controls.Add(this.PhoneTXT);
            this.supplierDetails.Controls.Add(this.AddressTXT);
            this.supplierDetails.Controls.Add(this.NameTXT);
            this.supplierDetails.Controls.Add(this.label3);
            this.supplierDetails.Controls.Add(this.label1);
            this.supplierDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.supplierDetails.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.supplierDetails.ForeColor = System.Drawing.Color.Black;
            this.supplierDetails.Location = new System.Drawing.Point(3, 3);
            this.supplierDetails.Name = "supplierDetails";
            this.supplierDetails.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.supplierDetails.Size = new System.Drawing.Size(786, 153);
            this.supplierDetails.TabIndex = 0;
            this.supplierDetails.TabStop = false;
            this.supplierDetails.Text = "بيانات المورد";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(720, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 21);
            this.label4.TabIndex = 11;
            this.label4.Text = "بحث";
            // 
            // search
            // 
            this.search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.search.Location = new System.Drawing.Point(475, 103);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(219, 29);
            this.search.TabIndex = 5;
            this.search.TextChanged += new System.EventHandler(this.search_TextChanged);
            this.search.Enter += new System.EventHandler(this.NameTXT_Enter);
            this.search.Leave += new System.EventHandler(this.NameTXT_Leave);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(427, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "العنوان";
            // 
            // btnStatement
            // 
            this.btnStatement.BackColor = System.Drawing.Color.DarkSlateGray;
            this.btnStatement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStatement.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStatement.ForeColor = System.Drawing.Color.White;
            this.btnStatement.Location = new System.Drawing.Point(27, 100);
            this.btnStatement.Name = "btnStatement";
            this.btnStatement.Size = new System.Drawing.Size(99, 32);
            this.btnStatement.TabIndex = 6;
            this.btnStatement.Text = "كشف حساب";
            this.btnStatement.UseVisualStyleBackColor = false;
            this.btnStatement.Click += new System.EventHandler(this.btnStatement_Click);
            // 
            // btndelete
            // 
            this.btndelete.BackColor = System.Drawing.Color.ForestGreen;
            this.btndelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btndelete.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btndelete.ForeColor = System.Drawing.Color.White;
            this.btndelete.Location = new System.Drawing.Point(148, 100);
            this.btndelete.Name = "btndelete";
            this.btndelete.Size = new System.Drawing.Size(75, 32);
            this.btndelete.TabIndex = 8;
            this.btndelete.Text = "حذف";
            this.btndelete.UseVisualStyleBackColor = false;
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            // 
            // btnedit
            // 
            this.btnedit.BackColor = System.Drawing.Color.ForestGreen;
            this.btnedit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnedit.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnedit.ForeColor = System.Drawing.Color.White;
            this.btnedit.Location = new System.Drawing.Point(244, 100);
            this.btnedit.Name = "btnedit";
            this.btnedit.Size = new System.Drawing.Size(75, 32);
            this.btnedit.TabIndex = 7;
            this.btnedit.Text = "تعديل";
            this.btnedit.UseVisualStyleBackColor = false;
            this.btnedit.Click += new System.EventHandler(this.btnedit_Click);
            // 
            // btnadd
            // 
            this.btnadd.BackColor = System.Drawing.Color.ForestGreen;
            this.btnadd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnadd.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnadd.ForeColor = System.Drawing.Color.White;
            this.btnadd.Location = new System.Drawing.Point(346, 100);
            this.btnadd.Name = "btnadd";
            this.btnadd.Size = new System.Drawing.Size(75, 32);
            this.btnadd.TabIndex = 3;
            this.btnadd.Text = "اضافة";
            this.btnadd.UseVisualStyleBackColor = false;
            this.btnadd.Click += new System.EventHandler(this.btnadd_Click);
            // 
            // PhoneTXT
            // 
            this.PhoneTXT.Location = new System.Drawing.Point(27, 38);
            this.PhoneTXT.Name = "PhoneTXT";
            this.PhoneTXT.Size = new System.Drawing.Size(131, 29);
            this.PhoneTXT.TabIndex = 2;
            this.PhoneTXT.Enter += new System.EventHandler(this.NameTXT_Enter);
            this.PhoneTXT.Leave += new System.EventHandler(this.NameTXT_Leave);
            // 
            // AddressTXT
            // 
            this.AddressTXT.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.AddressTXT.Location = new System.Drawing.Point(290, 38);
            this.AddressTXT.Name = "AddressTXT";
            this.AddressTXT.Size = new System.Drawing.Size(131, 29);
            this.AddressTXT.TabIndex = 1;
            this.AddressTXT.Enter += new System.EventHandler(this.NameTXT_Enter);
            this.AddressTXT.Leave += new System.EventHandler(this.NameTXT_Leave);
            // 
            // NameTXT
            // 
            this.NameTXT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NameTXT.Location = new System.Drawing.Point(563, 38);
            this.NameTXT.Name = "NameTXT";
            this.NameTXT.Size = new System.Drawing.Size(131, 29);
            this.NameTXT.TabIndex = 0;
            this.NameTXT.Enter += new System.EventHandler(this.NameTXT_Enter);
            this.NameTXT.Leave += new System.EventHandler(this.NameTXT_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(164, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 21);
            this.label3.TabIndex = 2;
            this.label3.Text = "رقم الهاتف";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(700, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "اسم المورد";
            // 
            // CustContainer
            // 
            this.CustContainer.BackColor = System.Drawing.Color.Transparent;
            this.CustContainer.Controls.Add(this.custData);
            this.CustContainer.Controls.Add(this.groupBox1);
            this.CustContainer.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CustContainer.Location = new System.Drawing.Point(4, 30);
            this.CustContainer.Name = "CustContainer";
            this.CustContainer.Padding = new System.Windows.Forms.Padding(3);
            this.CustContainer.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.CustContainer.Size = new System.Drawing.Size(792, 416);
            this.CustContainer.TabIndex = 1;
            this.CustContainer.Text = "العملاء";
            // 
            // custData
            // 
            this.custData.AllowUserToAddRows = false;
            this.custData.AllowUserToDeleteRows = false;
            this.custData.AllowUserToResizeColumns = false;
            this.custData.AllowUserToResizeRows = false;
            this.custData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.custData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.custData.Location = new System.Drawing.Point(3, 156);
            this.custData.Name = "custData";
            this.custData.ReadOnly = true;
            this.custData.Size = new System.Drawing.Size(786, 257);
            this.custData.TabIndex = 2;
            this.custData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.custData_CellClick);
            this.custData.MouseDown += new System.Windows.Forms.MouseEventHandler(this.custData_MouseDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.CUSTSEARCHTXT);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.custStatement);
            this.groupBox1.Controls.Add(this.deleteCust);
            this.groupBox1.Controls.Add(this.editCust);
            this.groupBox1.Controls.Add(this.ADDCUST);
            this.groupBox1.Controls.Add(this.CustPhoneTXT);
            this.groupBox1.Controls.Add(this.CUSTAddressTXT);
            this.groupBox1.Controls.Add(this.custNameTXT);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox1.Size = new System.Drawing.Size(786, 153);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "بيانات المورد";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(720, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 21);
            this.label5.TabIndex = 11;
            this.label5.Text = "بحث";
            // 
            // CUSTSEARCHTXT
            // 
            this.CUSTSEARCHTXT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CUSTSEARCHTXT.Location = new System.Drawing.Point(475, 103);
            this.CUSTSEARCHTXT.Name = "CUSTSEARCHTXT";
            this.CUSTSEARCHTXT.Size = new System.Drawing.Size(219, 29);
            this.CUSTSEARCHTXT.TabIndex = 4;
            this.CUSTSEARCHTXT.TextChanged += new System.EventHandler(this.CUSTSEARCHTXT_TextChanged);
            this.CUSTSEARCHTXT.Enter += new System.EventHandler(this.custNameTXT_Enter);
            this.CUSTSEARCHTXT.Leave += new System.EventHandler(this.custNameTXT_Leave);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(427, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 21);
            this.label6.TabIndex = 1;
            this.label6.Text = "العنوان";
            // 
            // custStatement
            // 
            this.custStatement.BackColor = System.Drawing.Color.DarkSlateGray;
            this.custStatement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.custStatement.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.custStatement.ForeColor = System.Drawing.Color.White;
            this.custStatement.Location = new System.Drawing.Point(27, 100);
            this.custStatement.Name = "custStatement";
            this.custStatement.Size = new System.Drawing.Size(99, 32);
            this.custStatement.TabIndex = 5;
            this.custStatement.Text = "كشف حساب";
            this.custStatement.UseVisualStyleBackColor = false;
            this.custStatement.Click += new System.EventHandler(this.custStatement_Click);
            // 
            // deleteCust
            // 
            this.deleteCust.BackColor = System.Drawing.Color.ForestGreen;
            this.deleteCust.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteCust.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteCust.ForeColor = System.Drawing.Color.White;
            this.deleteCust.Location = new System.Drawing.Point(148, 100);
            this.deleteCust.Name = "deleteCust";
            this.deleteCust.Size = new System.Drawing.Size(75, 32);
            this.deleteCust.TabIndex = 8;
            this.deleteCust.Text = "حذف";
            this.deleteCust.UseVisualStyleBackColor = false;
            this.deleteCust.Click += new System.EventHandler(this.deleteCust_Click);
            // 
            // editCust
            // 
            this.editCust.BackColor = System.Drawing.Color.ForestGreen;
            this.editCust.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.editCust.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editCust.ForeColor = System.Drawing.Color.White;
            this.editCust.Location = new System.Drawing.Point(244, 100);
            this.editCust.Name = "editCust";
            this.editCust.Size = new System.Drawing.Size(75, 32);
            this.editCust.TabIndex = 6;
            this.editCust.Text = "تعديل";
            this.editCust.UseVisualStyleBackColor = false;
            this.editCust.Click += new System.EventHandler(this.editCust_Click);
            // 
            // ADDCUST
            // 
            this.ADDCUST.BackColor = System.Drawing.Color.ForestGreen;
            this.ADDCUST.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ADDCUST.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ADDCUST.ForeColor = System.Drawing.Color.White;
            this.ADDCUST.Location = new System.Drawing.Point(346, 100);
            this.ADDCUST.Name = "ADDCUST";
            this.ADDCUST.Size = new System.Drawing.Size(75, 32);
            this.ADDCUST.TabIndex = 3;
            this.ADDCUST.Text = "اضافة";
            this.ADDCUST.UseVisualStyleBackColor = false;
            this.ADDCUST.Click += new System.EventHandler(this.ADDCUST_Click);
            // 
            // CustPhoneTXT
            // 
            this.CustPhoneTXT.Location = new System.Drawing.Point(27, 38);
            this.CustPhoneTXT.Name = "CustPhoneTXT";
            this.CustPhoneTXT.Size = new System.Drawing.Size(131, 29);
            this.CustPhoneTXT.TabIndex = 2;
            this.CustPhoneTXT.Enter += new System.EventHandler(this.custNameTXT_Enter);
            this.CustPhoneTXT.Leave += new System.EventHandler(this.custNameTXT_Leave);
            // 
            // CUSTAddressTXT
            // 
            this.CUSTAddressTXT.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.CUSTAddressTXT.Location = new System.Drawing.Point(290, 38);
            this.CUSTAddressTXT.Name = "CUSTAddressTXT";
            this.CUSTAddressTXT.Size = new System.Drawing.Size(131, 29);
            this.CUSTAddressTXT.TabIndex = 1;
            this.CUSTAddressTXT.Enter += new System.EventHandler(this.custNameTXT_Enter);
            this.CUSTAddressTXT.Leave += new System.EventHandler(this.custNameTXT_Leave);
            // 
            // custNameTXT
            // 
            this.custNameTXT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.custNameTXT.Location = new System.Drawing.Point(563, 38);
            this.custNameTXT.Name = "custNameTXT";
            this.custNameTXT.Size = new System.Drawing.Size(131, 29);
            this.custNameTXT.TabIndex = 0;
            this.custNameTXT.Enter += new System.EventHandler(this.custNameTXT_Enter);
            this.custNameTXT.Leave += new System.EventHandler(this.custNameTXT_Leave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(164, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 21);
            this.label7.TabIndex = 2;
            this.label7.Text = "رقم الهاتف";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(700, 41);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 21);
            this.label8.TabIndex = 0;
            this.label8.Text = "اسم العميل";
            // 
            // People
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.Name = "People";
            this.Text = "People";
            this.Load += new System.EventHandler(this.People_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.People_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.Suppcontainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.data)).EndInit();
            this.supplierDetails.ResumeLayout(false);
            this.supplierDetails.PerformLayout();
            this.CustContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.custData)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Suppcontainer;
        private System.Windows.Forms.TabPage CustContainer;
        private System.Windows.Forms.GroupBox supplierDetails;
        private System.Windows.Forms.TextBox PhoneTXT;
        private System.Windows.Forms.TextBox AddressTXT;
        private System.Windows.Forms.TextBox NameTXT;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStatement;
        private System.Windows.Forms.Button btndelete;
        private System.Windows.Forms.Button btnedit;
        private System.Windows.Forms.Button btnadd;
        private System.Windows.Forms.DataGridView data;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox search;
        private System.Windows.Forms.DataGridView custData;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox CUSTSEARCHTXT;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button custStatement;
        private System.Windows.Forms.Button deleteCust;
        private System.Windows.Forms.Button editCust;
        private System.Windows.Forms.Button ADDCUST;
        private System.Windows.Forms.TextBox CustPhoneTXT;
        private System.Windows.Forms.TextBox CUSTAddressTXT;
        private System.Windows.Forms.TextBox custNameTXT;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}