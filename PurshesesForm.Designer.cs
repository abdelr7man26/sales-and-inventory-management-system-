namespace Auto_Parts_Store
{
    partial class PurshesesForm
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
            this.container = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.notestxt = new System.Windows.Forms.RichTextBox();
            this.supplierscmb = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbCategories = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPartName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtPartNumber = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPurchasePrice = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.Quantity = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.Add = new System.Windows.Forms.Button();
            this.dgvPurshes = new System.Windows.Forms.DataGridView();
            this.PartNumberCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartIDCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartNameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PurchasePriceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SellingPriceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QuantityCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NotesCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.categoryCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.footerpanel = new System.Windows.Forms.Panel();
            this.Remaining = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Paid = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.savebtn = new System.Windows.Forms.Button();
            this.paymentmethod = new System.Windows.Forms.ComboBox();
            this.paymentmethodlabel = new System.Windows.Forms.Label();
            this.totallbl = new System.Windows.Forms.Label();
            this.container.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Quantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurshes)).BeginInit();
            this.footerpanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // container
            // 
            this.container.ColumnCount = 1;
            this.container.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.container.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.container.Controls.Add(this.dgvPurshes, 0, 1);
            this.container.Controls.Add(this.footerpanel, 0, 2);
            this.container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.container.Location = new System.Drawing.Point(0, 0);
            this.container.Name = "container";
            this.container.RowCount = 3;
            this.container.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.container.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.container.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 91F));
            this.container.Size = new System.Drawing.Size(1491, 605);
            this.container.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 109F));
            this.tableLayoutPanel1.Controls.Add(this.notestxt, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.supplierscmb, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.label21, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.label8, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbCategories, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtPartName, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label10, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtPartNumber, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label6, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtPurchasePrice, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.label12, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.Quantity, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label9, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.Add, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1485, 214);
            this.tableLayoutPanel1.TabIndex = 26;
            // 
            // notestxt
            // 
            this.notestxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.notestxt.Location = new System.Drawing.Point(3, 56);
            this.notestxt.Name = "notestxt";
            this.notestxt.Size = new System.Drawing.Size(385, 101);
            this.notestxt.TabIndex = 50;
            this.notestxt.Text = "";
            this.notestxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.notestxt_KeyDown);
            // 
            // supplierscmb
            // 
            this.supplierscmb.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.supplierscmb.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.supplierscmb.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.supplierscmb.FormattingEnabled = true;
            this.supplierscmb.Location = new System.Drawing.Point(1080, 176);
            this.supplierscmb.Name = "supplierscmb";
            this.supplierscmb.Size = new System.Drawing.Size(194, 21);
            this.supplierscmb.TabIndex = 43;
            this.supplierscmb.Enter += new System.EventHandler(this.cmbCategories_Enter_1);
            this.supplierscmb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.supplierscmb_KeyDown);
            this.supplierscmb.Leave += new System.EventHandler(this.cmbCategories_Leave_1);
            // 
            // label21
            // 
            this.label21.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(1404, 176);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(50, 21);
            this.label21.TabIndex = 42;
            this.label21.Text = "المورد";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(1409, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 21);
            this.label8.TabIndex = 12;
            this.label8.Text = "الفئه";
            // 
            // cmbCategories
            // 
            this.cmbCategories.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmbCategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategories.FormattingEnabled = true;
            this.cmbCategories.Location = new System.Drawing.Point(1080, 16);
            this.cmbCategories.Name = "cmbCategories";
            this.cmbCategories.Size = new System.Drawing.Size(194, 21);
            this.cmbCategories.TabIndex = 6;
            this.cmbCategories.SelectedIndexChanged += new System.EventHandler(this.cmbCategories_SelectedIndexChanged);
            this.cmbCategories.Enter += new System.EventHandler(this.cmbCategories_Enter_1);
            this.cmbCategories.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbCategories_KeyDown);
            this.cmbCategories.Leave += new System.EventHandler(this.cmbCategories_Leave_1);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(888, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 21);
            this.label1.TabIndex = 5;
            this.label1.Text = "اسم القطعه";
            // 
            // txtPartName
            // 
            this.txtPartName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtPartName.Location = new System.Drawing.Point(583, 16);
            this.txtPartName.Name = "txtPartName";
            this.txtPartName.Size = new System.Drawing.Size(207, 20);
            this.txtPartName.TabIndex = 0;
            this.txtPartName.Enter += new System.EventHandler(this.cmbCategories_Enter_1);
            this.txtPartName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPartName_KeyDown);
            this.txtPartName.Leave += new System.EventHandler(this.cmbCategories_Leave_1);
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(399, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 21);
            this.label10.TabIndex = 11;
            this.label10.Text = "رقم القطعة";
            // 
            // txtPartNumber
            // 
            this.txtPartNumber.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtPartNumber.Location = new System.Drawing.Point(98, 16);
            this.txtPartNumber.Name = "txtPartNumber";
            this.txtPartNumber.Size = new System.Drawing.Size(194, 20);
            this.txtPartNumber.TabIndex = 4;
            this.txtPartNumber.Enter += new System.EventHandler(this.cmbCategories_Enter_1);
            this.txtPartNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPartNumber_KeyDown);
            this.txtPartNumber.Leave += new System.EventHandler(this.cmbCategories_Leave_1);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(1388, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 21);
            this.label6.TabIndex = 8;
            this.label6.Text = "سعر الشراء";
            // 
            // txtPurchasePrice
            // 
            this.txtPurchasePrice.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtPurchasePrice.Location = new System.Drawing.Point(1067, 96);
            this.txtPurchasePrice.Name = "txtPurchasePrice";
            this.txtPurchasePrice.Size = new System.Drawing.Size(221, 20);
            this.txtPurchasePrice.TabIndex = 1;
            this.txtPurchasePrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPurchasePrice.Enter += new System.EventHandler(this.cmbCategories_Enter_1);
            this.txtPurchasePrice.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPurchasePrice_KeyDown);
            this.txtPurchasePrice.Leave += new System.EventHandler(this.cmbCategories_Leave_1);
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(907, 96);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(49, 21);
            this.label12.TabIndex = 32;
            this.label12.Text = "الكمية";
            // 
            // Quantity
            // 
            this.Quantity.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Quantity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Quantity.Location = new System.Drawing.Point(630, 96);
            this.Quantity.Name = "Quantity";
            this.Quantity.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Quantity.Size = new System.Drawing.Size(112, 20);
            this.Quantity.TabIndex = 30;
            this.Quantity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Quantity.Enter += new System.EventHandler(this.cmbCategories_Enter_1);
            this.Quantity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Quantity_KeyDown);
            this.Quantity.Leave += new System.EventHandler(this.cmbCategories_Leave_1);
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(406, 96);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 21);
            this.label9.TabIndex = 28;
            this.label9.Text = "ملاحظات";
            // 
            // Add
            // 
            this.Add.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Add.BackColor = System.Drawing.Color.ForestGreen;
            this.Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Add.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Add.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Add.Location = new System.Drawing.Point(154, 169);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(82, 35);
            this.Add.TabIndex = 45;
            this.Add.Text = "اضافه";
            this.Add.UseVisualStyleBackColor = false;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // dgvPurshes
            // 
            this.dgvPurshes.AllowUserToAddRows = false;
            this.dgvPurshes.AllowUserToResizeColumns = false;
            this.dgvPurshes.AllowUserToResizeRows = false;
            this.dgvPurshes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPurshes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPurshes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PartNumberCol,
            this.PartIDCol,
            this.PartNameCol,
            this.PurchasePriceCol,
            this.SellingPriceCol,
            this.QuantityCol,
            this.TotalCol,
            this.NotesCol,
            this.categoryCol});
            this.dgvPurshes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPurshes.Location = new System.Drawing.Point(3, 223);
            this.dgvPurshes.Name = "dgvPurshes";
            this.dgvPurshes.ReadOnly = true;
            this.dgvPurshes.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dgvPurshes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPurshes.Size = new System.Drawing.Size(1485, 288);
            this.dgvPurshes.TabIndex = 10;
            this.dgvPurshes.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurshes_CellContentClick);
            this.dgvPurshes.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurshes_CellDoubleClick);
            this.dgvPurshes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvPurshes_KeyDown);
            // 
            // PartNumberCol
            // 
            this.PartNumberCol.HeaderText = "Part Number";
            this.PartNumberCol.Name = "PartNumberCol";
            this.PartNumberCol.ReadOnly = true;
            // 
            // PartIDCol
            // 
            this.PartIDCol.HeaderText = "Part ID";
            this.PartIDCol.Name = "PartIDCol";
            this.PartIDCol.ReadOnly = true;
            this.PartIDCol.Visible = false;
            // 
            // PartNameCol
            // 
            this.PartNameCol.HeaderText = "Part Name";
            this.PartNameCol.Name = "PartNameCol";
            this.PartNameCol.ReadOnly = true;
            // 
            // PurchasePriceCol
            // 
            this.PurchasePriceCol.HeaderText = "Purchase Price";
            this.PurchasePriceCol.Name = "PurchasePriceCol";
            this.PurchasePriceCol.ReadOnly = true;
            // 
            // SellingPriceCol
            // 
            this.SellingPriceCol.HeaderText = "Selling Price";
            this.SellingPriceCol.Name = "SellingPriceCol";
            this.SellingPriceCol.ReadOnly = true;
            // 
            // QuantityCol
            // 
            this.QuantityCol.HeaderText = "Quantity";
            this.QuantityCol.Name = "QuantityCol";
            this.QuantityCol.ReadOnly = true;
            // 
            // TotalCol
            // 
            this.TotalCol.HeaderText = "Total";
            this.TotalCol.Name = "TotalCol";
            this.TotalCol.ReadOnly = true;
            // 
            // NotesCol
            // 
            this.NotesCol.HeaderText = "Notes";
            this.NotesCol.Name = "NotesCol";
            this.NotesCol.ReadOnly = true;
            // 
            // categoryCol
            // 
            this.categoryCol.HeaderText = "CategoryID";
            this.categoryCol.Name = "categoryCol";
            this.categoryCol.ReadOnly = true;
            this.categoryCol.Visible = false;
            // 
            // footerpanel
            // 
            this.footerpanel.Controls.Add(this.Remaining);
            this.footerpanel.Controls.Add(this.label4);
            this.footerpanel.Controls.Add(this.label3);
            this.footerpanel.Controls.Add(this.Paid);
            this.footerpanel.Controls.Add(this.label2);
            this.footerpanel.Controls.Add(this.savebtn);
            this.footerpanel.Controls.Add(this.paymentmethod);
            this.footerpanel.Controls.Add(this.paymentmethodlabel);
            this.footerpanel.Controls.Add(this.totallbl);
            this.footerpanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.footerpanel.Location = new System.Drawing.Point(3, 517);
            this.footerpanel.Name = "footerpanel";
            this.footerpanel.Size = new System.Drawing.Size(1485, 85);
            this.footerpanel.TabIndex = 2;
            // 
            // Remaining
            // 
            this.Remaining.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Remaining.AutoSize = true;
            this.Remaining.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Remaining.Location = new System.Drawing.Point(832, 34);
            this.Remaining.Name = "Remaining";
            this.Remaining.Size = new System.Drawing.Size(50, 21);
            this.Remaining.TabIndex = 15;
            this.Remaining.Text = "00.00";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(906, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 21);
            this.label4.TabIndex = 14;
            this.label4.Text = "الباقي";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1126, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 21);
            this.label3.TabIndex = 13;
            this.label3.Text = "المدفوع";
            // 
            // Paid
            // 
            this.Paid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Paid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Paid.Location = new System.Drawing.Point(1020, 35);
            this.Paid.Name = "Paid";
            this.Paid.Size = new System.Drawing.Size(100, 20);
            this.Paid.TabIndex = 5;
            this.Paid.TextChanged += new System.EventHandler(this.Paid_TextChanged);
            this.Paid.Enter += new System.EventHandler(this.cmbCategories_Enter_1);
            this.Paid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Paid_KeyDown);
            this.Paid.Leave += new System.EventHandler(this.cmbCategories_Leave_1);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1358, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 21);
            this.label2.TabIndex = 11;
            this.label2.Text = "اجمالي الفاتوره";
            // 
            // savebtn
            // 
            this.savebtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.savebtn.BackColor = System.Drawing.Color.ForestGreen;
            this.savebtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.savebtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.savebtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.savebtn.Location = new System.Drawing.Point(156, 27);
            this.savebtn.Margin = new System.Windows.Forms.Padding(10);
            this.savebtn.Name = "savebtn";
            this.savebtn.Size = new System.Drawing.Size(82, 35);
            this.savebtn.TabIndex = 7;
            this.savebtn.Text = "حفظ";
            this.savebtn.UseVisualStyleBackColor = false;
            this.savebtn.Click += new System.EventHandler(this.savebtn_Click);
            // 
            // paymentmethod
            // 
            this.paymentmethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.paymentmethod.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.paymentmethod.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.paymentmethod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.paymentmethod.FormattingEnabled = true;
            this.paymentmethod.Items.AddRange(new object[] {
            "كاش (Cash)",
            "فيزا (Visa / POS)",
            "آجل (On Credit)",
            "instapay",
            "vodafonecash"});
            this.paymentmethod.Location = new System.Drawing.Point(413, 37);
            this.paymentmethod.Name = "paymentmethod";
            this.paymentmethod.Size = new System.Drawing.Size(121, 21);
            this.paymentmethod.TabIndex = 6;
            this.paymentmethod.Enter += new System.EventHandler(this.cmbCategories_Enter_1);
            this.paymentmethod.KeyDown += new System.Windows.Forms.KeyEventHandler(this.paymentmethod_KeyDown);
            this.paymentmethod.Leave += new System.EventHandler(this.cmbCategories_Leave_1);
            // 
            // paymentmethodlabel
            // 
            this.paymentmethodlabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.paymentmethodlabel.AutoSize = true;
            this.paymentmethodlabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.paymentmethodlabel.Location = new System.Drawing.Point(571, 34);
            this.paymentmethodlabel.Name = "paymentmethodlabel";
            this.paymentmethodlabel.Size = new System.Drawing.Size(94, 21);
            this.paymentmethodlabel.TabIndex = 10;
            this.paymentmethodlabel.Text = "طريقه الدفع ";
            // 
            // totallbl
            // 
            this.totallbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.totallbl.AutoSize = true;
            this.totallbl.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totallbl.Location = new System.Drawing.Point(1291, 34);
            this.totallbl.Name = "totallbl";
            this.totallbl.Size = new System.Drawing.Size(50, 21);
            this.totallbl.TabIndex = 0;
            this.totallbl.Text = "00.00";
            // 
            // PurshesesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1491, 605);
            this.Controls.Add(this.container);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "PurshesesForm";
            this.Text = "PurshesesForm";
            this.Load += new System.EventHandler(this.PurshesesForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PurshesesForm_KeyDown);
            this.container.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Quantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurshes)).EndInit();
            this.footerpanel.ResumeLayout(false);
            this.footerpanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel container;
        private System.Windows.Forms.DataGridView dgvPurshes;
        private System.Windows.Forms.Panel footerpanel;
        private System.Windows.Forms.Label Remaining;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Paid;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button savebtn;
        private System.Windows.Forms.ComboBox paymentmethod;
        private System.Windows.Forms.Label paymentmethodlabel;
        private System.Windows.Forms.Label totallbl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPartName;
        private System.Windows.Forms.TextBox txtPurchasePrice;
        private System.Windows.Forms.ComboBox cmbCategories;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtPartNumber;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown Quantity;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox supplierscmb;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.RichTextBox notestxt;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartNumberCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartIDCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartNameCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn PurchasePriceCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn SellingPriceCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn QuantityCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn NotesCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn categoryCol;
    }
}