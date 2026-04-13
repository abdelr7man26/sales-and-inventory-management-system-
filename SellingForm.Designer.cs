namespace Auto_Parts_Store
{
    partial class SellingForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.container = new System.Windows.Forms.TableLayoutPanel();
            this.headerpanel = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.add = new System.Windows.Forms.Button();
            this.cmbParts = new System.Windows.Forms.ComboBox();
            this.lblUnitPrice = new System.Windows.Forms.Label();
            this.customernamelabel = new System.Windows.Forms.Label();
            this.custoomername = new System.Windows.Forms.ComboBox();
            this.quantitylabel = new System.Windows.Forms.Label();
            this.quantity = new System.Windows.Forms.NumericUpDown();
            this.categorysearchcmbbox = new System.Windows.Forms.ComboBox();
            this.categorysearch = new System.Windows.Forms.Label();
            this.dgvSales = new System.Windows.Forms.DataGridView();
            this.CID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.footerpanel = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.savebtn = new System.Windows.Forms.Button();
            this.paymentmethod = new System.Windows.Forms.ComboBox();
            this.paymentmethodlabel = new System.Windows.Forms.Label();
            this.totallbl = new System.Windows.Forms.Label();
            this.container.SuspendLayout();
            this.headerpanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.quantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).BeginInit();
            this.footerpanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // container
            // 
            this.container.ColumnCount = 1;
            this.container.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.container.Controls.Add(this.headerpanel, 0, 0);
            this.container.Controls.Add(this.dgvSales, 0, 1);
            this.container.Controls.Add(this.footerpanel, 0, 2);
            this.container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.container.Location = new System.Drawing.Point(0, 0);
            this.container.Name = "container";
            this.container.RowCount = 3;
            this.container.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.container.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.container.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 91F));
            this.container.Size = new System.Drawing.Size(1423, 579);
            this.container.TabIndex = 0;
            // 
            // headerpanel
            // 
            this.headerpanel.Controls.Add(this.label6);
            this.headerpanel.Controls.Add(this.label1);
            this.headerpanel.Controls.Add(this.add);
            this.headerpanel.Controls.Add(this.cmbParts);
            this.headerpanel.Controls.Add(this.lblUnitPrice);
            this.headerpanel.Controls.Add(this.customernamelabel);
            this.headerpanel.Controls.Add(this.custoomername);
            this.headerpanel.Controls.Add(this.quantitylabel);
            this.headerpanel.Controls.Add(this.quantity);
            this.headerpanel.Controls.Add(this.categorysearchcmbbox);
            this.headerpanel.Controls.Add(this.categorysearch);
            this.headerpanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerpanel.Location = new System.Drawing.Point(3, 3);
            this.headerpanel.Name = "headerpanel";
            this.headerpanel.Size = new System.Drawing.Size(1417, 104);
            this.headerpanel.TabIndex = 0;
            this.headerpanel.Click += new System.EventHandler(this.headerpanel_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(441, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 21);
            this.label6.TabIndex = 14;
            this.label6.Text = "سعر القطعه";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(838, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 21);
            this.label1.TabIndex = 13;
            this.label1.Text = "البحث عن قطعه ";
            // 
            // add
            // 
            this.add.BackColor = System.Drawing.Color.ForestGreen;
            this.add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.add.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.add.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.add.Location = new System.Drawing.Point(72, 55);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(82, 35);
            this.add.TabIndex = 4;
            this.add.Text = "اضافه";
            this.add.UseVisualStyleBackColor = false;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // cmbParts
            // 
            this.cmbParts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbParts.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbParts.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbParts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbParts.FormattingEnabled = true;
            this.cmbParts.Location = new System.Drawing.Point(345, 9);
            this.cmbParts.Name = "cmbParts";
            this.cmbParts.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmbParts.Size = new System.Drawing.Size(441, 21);
            this.cmbParts.TabIndex = 1;
            this.cmbParts.SelectedIndexChanged += new System.EventHandler(this.cmbParts_SelectedIndexChanged);
            this.cmbParts.TextChanged += new System.EventHandler(this.cmbParts_TextChanged);
            this.cmbParts.Enter += new System.EventHandler(this.categorysearchcmbbox_Enter_1);
            this.cmbParts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbParts_KeyDown);
            this.cmbParts.Leave += new System.EventHandler(this.categorysearchcmbbox_Leave_1);
            // 
            // lblUnitPrice
            // 
            this.lblUnitPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUnitPrice.AutoSize = true;
            this.lblUnitPrice.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnitPrice.Location = new System.Drawing.Point(341, 62);
            this.lblUnitPrice.Name = "lblUnitPrice";
            this.lblUnitPrice.Size = new System.Drawing.Size(50, 21);
            this.lblUnitPrice.TabIndex = 11;
            this.lblUnitPrice.Text = "00.00";
            // 
            // customernamelabel
            // 
            this.customernamelabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.customernamelabel.AutoSize = true;
            this.customernamelabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customernamelabel.Location = new System.Drawing.Point(1278, 62);
            this.customernamelabel.Name = "customernamelabel";
            this.customernamelabel.Size = new System.Drawing.Size(53, 21);
            this.customernamelabel.TabIndex = 8;
            this.customernamelabel.Text = "العميل";
            // 
            // custoomername
            // 
            this.custoomername.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.custoomername.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.custoomername.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.custoomername.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.custoomername.FormattingEnabled = true;
            this.custoomername.Location = new System.Drawing.Point(783, 62);
            this.custoomername.Name = "custoomername";
            this.custoomername.Size = new System.Drawing.Size(336, 21);
            this.custoomername.TabIndex = 3;
            this.custoomername.Enter += new System.EventHandler(this.categorysearchcmbbox_Enter_1);
            this.custoomername.KeyDown += new System.Windows.Forms.KeyEventHandler(this.custoomername_KeyDown);
            this.custoomername.Leave += new System.EventHandler(this.categorysearchcmbbox_Leave_1);
            // 
            // quantitylabel
            // 
            this.quantitylabel.AutoSize = true;
            this.quantitylabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quantitylabel.Location = new System.Drawing.Point(255, 12);
            this.quantitylabel.Name = "quantitylabel";
            this.quantitylabel.Size = new System.Drawing.Size(49, 21);
            this.quantitylabel.TabIndex = 5;
            this.quantitylabel.Text = "الكميه";
            // 
            // quantity
            // 
            this.quantity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.quantity.Location = new System.Drawing.Point(124, 13);
            this.quantity.Name = "quantity";
            this.quantity.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.quantity.Size = new System.Drawing.Size(112, 20);
            this.quantity.TabIndex = 2;
            this.quantity.Enter += new System.EventHandler(this.categorysearchcmbbox_Enter_1);
            this.quantity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.quantity_KeyDown);
            this.quantity.Leave += new System.EventHandler(this.categorysearchcmbbox_Leave_1);
            // 
            // categorysearchcmbbox
            // 
            this.categorysearchcmbbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.categorysearchcmbbox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.categorysearchcmbbox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.categorysearchcmbbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.categorysearchcmbbox.FormattingEnabled = true;
            this.categorysearchcmbbox.Location = new System.Drawing.Point(1062, 9);
            this.categorysearchcmbbox.Name = "categorysearchcmbbox";
            this.categorysearchcmbbox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.categorysearchcmbbox.Size = new System.Drawing.Size(169, 21);
            this.categorysearchcmbbox.TabIndex = 0;
            this.categorysearchcmbbox.SelectedIndexChanged += new System.EventHandler(this.categorysearchcmbbox_SelectedIndexChanged);
            this.categorysearchcmbbox.Enter += new System.EventHandler(this.categorysearchcmbbox_Enter_1);
            this.categorysearchcmbbox.Leave += new System.EventHandler(this.categorysearchcmbbox_Leave_1);
            // 
            // categorysearch
            // 
            this.categorysearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.categorysearch.AutoSize = true;
            this.categorysearch.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categorysearch.Location = new System.Drawing.Point(1269, 9);
            this.categorysearch.Name = "categorysearch";
            this.categorysearch.Size = new System.Drawing.Size(127, 21);
            this.categorysearch.TabIndex = 2;
            this.categorysearch.Text = "تصفيه حسب الفئه";
            // 
            // dgvSales
            // 
            this.dgvSales.AllowUserToAddRows = false;
            this.dgvSales.AllowUserToResizeColumns = false;
            this.dgvSales.AllowUserToResizeRows = false;
            this.dgvSales.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSales.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CID,
            this.CName,
            this.CPrice,
            this.CQty,
            this.CTotal,
            this.CDelete});
            this.dgvSales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSales.Location = new System.Drawing.Point(3, 113);
            this.dgvSales.Name = "dgvSales";
            this.dgvSales.ReadOnly = true;
            this.dgvSales.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dgvSales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSales.Size = new System.Drawing.Size(1417, 372);
            this.dgvSales.TabIndex = 10;
            this.dgvSales.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSales_CellContentClick);
            this.dgvSales.Click += new System.EventHandler(this.headerpanel_Click);
            // 
            // CID
            // 
            this.CID.HeaderText = "كود الصنف";
            this.CID.Name = "CID";
            this.CID.ReadOnly = true;
            this.CID.Visible = false;
            // 
            // CName
            // 
            this.CName.HeaderText = "اسم القطعه";
            this.CName.Name = "CName";
            this.CName.ReadOnly = true;
            // 
            // CPrice
            // 
            dataGridViewCellStyle3.Format = "N2";
            this.CPrice.DefaultCellStyle = dataGridViewCellStyle3;
            this.CPrice.HeaderText = "السعر";
            this.CPrice.Name = "CPrice";
            this.CPrice.ReadOnly = true;
            // 
            // CQty
            // 
            this.CQty.HeaderText = "الكمية";
            this.CQty.Name = "CQty";
            this.CQty.ReadOnly = true;
            // 
            // CTotal
            // 
            dataGridViewCellStyle4.Format = "N2";
            this.CTotal.DefaultCellStyle = dataGridViewCellStyle4;
            this.CTotal.HeaderText = "الاجمالي";
            this.CTotal.Name = "CTotal";
            this.CTotal.ReadOnly = true;
            // 
            // CDelete
            // 
            this.CDelete.HeaderText = "حذف";
            this.CDelete.Name = "CDelete";
            this.CDelete.ReadOnly = true;
            this.CDelete.Text = "حذف";
            this.CDelete.UseColumnTextForButtonValue = true;
            // 
            // footerpanel
            // 
            this.footerpanel.Controls.Add(this.label5);
            this.footerpanel.Controls.Add(this.label4);
            this.footerpanel.Controls.Add(this.label3);
            this.footerpanel.Controls.Add(this.textBox1);
            this.footerpanel.Controls.Add(this.label2);
            this.footerpanel.Controls.Add(this.savebtn);
            this.footerpanel.Controls.Add(this.paymentmethod);
            this.footerpanel.Controls.Add(this.paymentmethodlabel);
            this.footerpanel.Controls.Add(this.totallbl);
            this.footerpanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.footerpanel.Location = new System.Drawing.Point(3, 491);
            this.footerpanel.Name = "footerpanel";
            this.footerpanel.Size = new System.Drawing.Size(1417, 85);
            this.footerpanel.TabIndex = 2;
            this.footerpanel.Click += new System.EventHandler(this.headerpanel_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(764, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 21);
            this.label5.TabIndex = 15;
            this.label5.Text = "00.00";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(838, 34);
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
            this.label3.Location = new System.Drawing.Point(1058, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 21);
            this.label3.TabIndex = 13;
            this.label3.Text = "المدفوع";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Location = new System.Drawing.Point(952, 35);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 5;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.Enter += new System.EventHandler(this.categorysearchcmbbox_Enter_1);
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            this.textBox1.Leave += new System.EventHandler(this.categorysearchcmbbox_Leave_1);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1290, 34);
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
            this.savebtn.Location = new System.Drawing.Point(72, 27);
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
            this.paymentmethod.Location = new System.Drawing.Point(345, 37);
            this.paymentmethod.Name = "paymentmethod";
            this.paymentmethod.Size = new System.Drawing.Size(121, 21);
            this.paymentmethod.TabIndex = 6;
            this.paymentmethod.Enter += new System.EventHandler(this.categorysearchcmbbox_Enter_1);
            this.paymentmethod.Leave += new System.EventHandler(this.categorysearchcmbbox_Leave_1);
            // 
            // paymentmethodlabel
            // 
            this.paymentmethodlabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.paymentmethodlabel.AutoSize = true;
            this.paymentmethodlabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.paymentmethodlabel.Location = new System.Drawing.Point(503, 34);
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
            this.totallbl.Location = new System.Drawing.Point(1223, 34);
            this.totallbl.Name = "totallbl";
            this.totallbl.Size = new System.Drawing.Size(50, 21);
            this.totallbl.TabIndex = 0;
            this.totallbl.Text = "00.00";
            // 
            // SellingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1423, 579);
            this.Controls.Add(this.container);
            this.KeyPreview = true;
            this.Name = "SellingForm";
            this.Text = "SellingForm";
            this.Load += new System.EventHandler(this.SellingForm_Load_1);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SellingForm_KeyDown);
            this.container.ResumeLayout(false);
            this.headerpanel.ResumeLayout(false);
            this.headerpanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.quantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).EndInit();
            this.footerpanel.ResumeLayout(false);
            this.footerpanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel container;
        private System.Windows.Forms.Panel headerpanel;
        private System.Windows.Forms.Label quantitylabel;
        private System.Windows.Forms.NumericUpDown quantity;
        private System.Windows.Forms.ComboBox categorysearchcmbbox;
        private System.Windows.Forms.Label categorysearch;
        private System.Windows.Forms.Label paymentmethodlabel;
        private System.Windows.Forms.ComboBox paymentmethod;
        private System.Windows.Forms.Label customernamelabel;
        private System.Windows.Forms.ComboBox custoomername;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.DataGridView dgvSales;
        private System.Windows.Forms.Panel footerpanel;
        private System.Windows.Forms.Button savebtn;
        private System.Windows.Forms.Label totallbl;
        private System.Windows.Forms.Label lblUnitPrice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbParts;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridViewTextBoxColumn CID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn CQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn CTotal;
        private System.Windows.Forms.DataGridViewButtonColumn CDelete;
    }
}