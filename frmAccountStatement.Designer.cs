namespace Auto_Parts_Store
{
    partial class frmAccountStatement
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.To = new System.Windows.Forms.DateTimePicker();
            this.From = new System.Windows.Forms.DateTimePicker();
            this.NameLBL = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DetailsLBL = new System.Windows.Forms.Label();
            this.dgvStatement = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.returnsfinal = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.recieved = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.returns = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.print = new System.Windows.Forms.Button();
            this.lblFinalBalance = new System.Windows.Forms.Label();
            this.lblTotalPaid = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblTotalAccount = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.final = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatement)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgvStatement, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1142, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.To);
            this.panel1.Controls.Add(this.From);
            this.panel1.Controls.Add(this.NameLBL);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.DetailsLBL);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1136, 94);
            this.panel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(636, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 21);
            this.label3.TabIndex = 6;
            this.label3.Text = "الي";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(638, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 21);
            this.label2.TabIndex = 5;
            this.label2.Text = "من";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.ForestGreen;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(56, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 42);
            this.button1.TabIndex = 4;
            this.button1.Text = "عرض";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // To
            // 
            this.To.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.To.Location = new System.Drawing.Point(415, 62);
            this.To.Name = "To";
            this.To.Size = new System.Drawing.Size(200, 20);
            this.To.TabIndex = 3;
            // 
            // From
            // 
            this.From.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.From.Location = new System.Drawing.Point(415, 9);
            this.From.Name = "From";
            this.From.Size = new System.Drawing.Size(200, 20);
            this.From.TabIndex = 2;
            // 
            // NameLBL
            // 
            this.NameLBL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NameLBL.AutoSize = true;
            this.NameLBL.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameLBL.Location = new System.Drawing.Point(896, 40);
            this.NameLBL.Name = "NameLBL";
            this.NameLBL.Size = new System.Drawing.Size(78, 21);
            this.NameLBL.TabIndex = 1;
            this.NameLBL.Text = " ................";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(990, 40);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(137, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "اسم العميل / المورد";
            // 
            // DetailsLBL
            // 
            this.DetailsLBL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DetailsLBL.AutoSize = true;
            this.DetailsLBL.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DetailsLBL.Location = new System.Drawing.Point(968, 40);
            this.DetailsLBL.Name = "DetailsLBL";
            this.DetailsLBL.Size = new System.Drawing.Size(137, 21);
            this.DetailsLBL.TabIndex = 8;
            this.DetailsLBL.Text = "اسم العميل / المورد";
            this.DetailsLBL.Visible = false;
            // 
            // dgvStatement
            // 
            this.dgvStatement.AllowUserToAddRows = false;
            this.dgvStatement.AllowUserToDeleteRows = false;
            this.dgvStatement.AllowUserToResizeColumns = false;
            this.dgvStatement.AllowUserToResizeRows = false;
            this.dgvStatement.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStatement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStatement.Location = new System.Drawing.Point(3, 103);
            this.dgvStatement.MultiSelect = false;
            this.dgvStatement.Name = "dgvStatement";
            this.dgvStatement.ReadOnly = true;
            this.dgvStatement.Size = new System.Drawing.Size(1136, 236);
            this.dgvStatement.TabIndex = 1;
            this.dgvStatement.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStatement_CellDoubleClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.final);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.returnsfinal);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.recieved);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.returns);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.print);
            this.panel2.Controls.Add(this.lblFinalBalance);
            this.panel2.Controls.Add(this.lblTotalPaid);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.lblTotalAccount);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 345);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1136, 102);
            this.panel2.TabIndex = 2;
            // 
            // returnsfinal
            // 
            this.returnsfinal.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.returnsfinal.AutoSize = true;
            this.returnsfinal.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.returnsfinal.Location = new System.Drawing.Point(396, 66);
            this.returnsfinal.Name = "returnsfinal";
            this.returnsfinal.Size = new System.Drawing.Size(50, 21);
            this.returnsfinal.TabIndex = 16;
            this.returnsfinal.Text = "00.00";
            this.returnsfinal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(487, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(121, 21);
            this.label9.TabIndex = 15;
            this.label9.Text = "صافي المرتجعات";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // recieved
            // 
            this.recieved.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.recieved.AutoSize = true;
            this.recieved.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recieved.Location = new System.Drawing.Point(626, 66);
            this.recieved.Name = "recieved";
            this.recieved.Size = new System.Drawing.Size(50, 21);
            this.recieved.TabIndex = 14;
            this.recieved.Text = "00.00";
            this.recieved.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(739, 66);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(135, 21);
            this.label8.TabIndex = 13;
            this.label8.Text = "المستلم من المرتجع";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // returns
            // 
            this.returns.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.returns.AutoSize = true;
            this.returns.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.returns.Location = new System.Drawing.Point(910, 66);
            this.returns.Name = "returns";
            this.returns.Size = new System.Drawing.Size(50, 21);
            this.returns.TabIndex = 12;
            this.returns.Text = "00.00";
            this.returns.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(1045, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 21);
            this.label5.TabIndex = 11;
            this.label5.Text = "المرتجعات";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // print
            // 
            this.print.BackColor = System.Drawing.Color.ForestGreen;
            this.print.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.print.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.print.ForeColor = System.Drawing.Color.White;
            this.print.Location = new System.Drawing.Point(56, 35);
            this.print.Name = "print";
            this.print.Size = new System.Drawing.Size(75, 35);
            this.print.TabIndex = 10;
            this.print.Text = "طباعه";
            this.print.UseVisualStyleBackColor = false;
            this.print.Click += new System.EventHandler(this.print_Click);
            // 
            // lblFinalBalance
            // 
            this.lblFinalBalance.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblFinalBalance.AutoSize = true;
            this.lblFinalBalance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFinalBalance.Location = new System.Drawing.Point(425, 16);
            this.lblFinalBalance.Name = "lblFinalBalance";
            this.lblFinalBalance.Size = new System.Drawing.Size(50, 21);
            this.lblFinalBalance.TabIndex = 8;
            this.lblFinalBalance.Text = "00.00";
            this.lblFinalBalance.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTotalPaid
            // 
            this.lblTotalPaid.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTotalPaid.AutoSize = true;
            this.lblTotalPaid.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalPaid.Location = new System.Drawing.Point(701, 16);
            this.lblTotalPaid.Name = "lblTotalPaid";
            this.lblTotalPaid.Size = new System.Drawing.Size(50, 21);
            this.lblTotalPaid.TabIndex = 7;
            this.lblTotalPaid.Text = "00.00";
            this.lblTotalPaid.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(531, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(122, 21);
            this.label7.TabIndex = 5;
            this.label7.Text = "صافي المتشريات";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(1045, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 21);
            this.label6.TabIndex = 3;
            this.label6.Text = "المشتريات";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTotalAccount
            // 
            this.lblTotalAccount.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTotalAccount.AutoSize = true;
            this.lblTotalAccount.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAccount.Location = new System.Drawing.Point(910, 16);
            this.lblTotalAccount.Name = "lblTotalAccount";
            this.lblTotalAccount.Size = new System.Drawing.Size(50, 21);
            this.lblTotalAccount.TabIndex = 2;
            this.lblTotalAccount.Text = "00.00";
            this.lblTotalAccount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(813, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 21);
            this.label4.TabIndex = 1;
            this.label4.Text = "المدفوع";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(310, 42);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 21);
            this.label10.TabIndex = 17;
            this.label10.Text = "الصافي";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // final
            // 
            this.final.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.final.AutoSize = true;
            this.final.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.final.Location = new System.Drawing.Point(197, 42);
            this.final.Name = "final";
            this.final.Size = new System.Drawing.Size(50, 21);
            this.final.TabIndex = 18;
            this.final.Text = "00.00";
            this.final.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // frmAccountStatement
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1142, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Name = "frmAccountStatement";
            this.Text = "frmAccountStatement";
            this.Load += new System.EventHandler(this.frmAccountStatement_Load_1);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAccountStatement_KeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatement)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label NameLBL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DateTimePicker To;
        private System.Windows.Forms.DateTimePicker From;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvStatement;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblTotalAccount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblTotalPaid;
        private System.Windows.Forms.Label lblFinalBalance;
        private System.Windows.Forms.Button print;
        private System.Windows.Forms.Label DetailsLBL;
        private System.Windows.Forms.Label returns;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label recieved;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label returnsfinal;
        private System.Windows.Forms.Label final;
        private System.Windows.Forms.Label label10;
    }
}