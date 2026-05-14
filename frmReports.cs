using Auto_Parts_Store.Helpers;
using Auto_Parts_Store.Models;
using Auto_Parts_Store.Repositories;
using Auto_Parts_Store.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Auto_Parts_Store
{
    public class frmReports : Form
    {
        // ── Palette ────────────────────────────────────────────────
        private static readonly Color NavyDark  = Color.FromArgb(20,  25,  72);
        private static readonly Color NavyDeep  = Color.FromArgb(30,  34,  90);
        private static readonly Color NavyMid   = Color.FromArgb(45,  52, 120);
        private static readonly Color Accent    = Color.FromArgb(0,  149, 255);
        private static readonly Color PageBg    = Color.FromArgb(245, 246, 250);
        private static readonly Color CardWhite = Color.White;

        // ── Services ───────────────────────────────────────────────
        private readonly IReportsService _svc;

        // ── State ──────────────────────────────────────────────────
        private string  _activeReport = string.Empty;
        private Button  _activeBtn;

        // ── Layout controls ────────────────────────────────────────
        private Panel      pnlHeader;
        private Panel      pnlSidebar;
        private Panel      pnlMain;
        private Panel      pnlFilter;
        private Panel      pnlCards;
        private DataGridView dgv;
        private Label      lblReportTitle;

        // Filter controls
        private DateTimePicker dtpFrom, dtpTo;
        private ComboBox       cmbCategory;
        private Button         btnLoad;

        // Summary-card labels (value / caption pairs)
        private Label[] _cardValues   = new Label[4];
        private Label[] _cardCaptions = new Label[4];

        // ──────────────────────────────────────────────────────────

        public frmReports()
        {
            _svc = new ReportsService(new ReportsRepository());
            Build();
        }

        // ══════════════════════════════════════════════════════════
        //  UI CONSTRUCTION
        // ══════════════════════════════════════════════════════════

        private void Build()
        {
            SuspendLayout();

            Text             = "التقارير والإحصائيات";
            Size             = new Size(1240, 760);
            MinimumSize      = new Size(1000, 620);
            StartPosition    = FormStartPosition.CenterScreen;
            BackColor        = PageBg;
            Font             = new Font("Segoe UI", 10);
            RightToLeft      = RightToLeft.Yes;

            BuildHeader();
            BuildSidebar();
            BuildMain();

            ResumeLayout(true);
        }

        // ── Header ─────────────────────────────────────────────────

        private void BuildHeader()
        {
            pnlHeader = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 58,
                BackColor = NavyDark
            };

            lblReportTitle = MakeLabel("📊  التقارير والإحصائيات", 15, FontStyle.Bold, Color.White);
            lblReportTitle.AutoSize = true;
            lblReportTitle.Location = new Point(20, 14);

            var btnExport = MakeBtn("⬇  تصدير Excel", Color.FromArgb(40, 167, 69), 130, 34);
            btnExport.Anchor   = AnchorStyles.Top | AnchorStyles.Right;
            btnExport.Click   += BtnExportCsv_Click;

            var btnClose = MakeBtn("✕", Color.FromArgb(180, 40, 40), 38, 34);
            btnClose.Anchor   = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Click   += (s, e) => Close();

            pnlHeader.Controls.AddRange(new Control[] { lblReportTitle, btnExport, btnClose });
            pnlHeader.SizeChanged += (s, e) =>
            {
                btnClose.Location  = new Point(pnlHeader.Width - 50,  12);
                btnExport.Location = new Point(pnlHeader.Width - 195, 12);
            };
            Controls.Add(pnlHeader);
        }

        // ── Sidebar ────────────────────────────────────────────────

        private void BuildSidebar()
        {
            pnlSidebar = new Panel
            {
                Dock      = DockStyle.Left,
                Width     = 210,
                BackColor = NavyDeep,
                Padding   = new Padding(0, 8, 0, 0)
            };

            var items = new (string Icon, string Label, string Key, bool IsSection)[]
            {
                ("", "📦  المخزون",         "",                true),
                ("", "جرد المخزن الكامل",   "StockAudit",      false),
                ("", "الأصناف الراكدة",     "Stagnant",        false),
                ("", "تنبيهات النقص",       "LowStock",        false),
                ("", "💰  المبيعات",        "",                true),
                ("", "الربح والخسارة",      "ProfitReport",    false),
                ("", "الأكثر مبيعاً",       "TopSelling",      false),
                ("", "🏦  المالية",         "",                true),
                ("", "سجل الخزينة",         "SafeLedger",      false),
                ("", "الدائنون (مبيعات)",   "Receivable",      false),
                ("", "المدينون (مشتريات)",  "Payable",         false),
            };

            int y = 8;
            foreach (var item in items)
            {
                if (item.IsSection)
                {
                    var lbl = MakeLabel(item.Label, 9, FontStyle.Bold, Color.FromArgb(130, 145, 190));
                    lbl.Location = new Point(14, y + 6);
                    lbl.AutoSize = true;
                    pnlSidebar.Controls.Add(lbl);
                    y += 30;
                }
                else
                {
                    var btn = new Button
                    {
                        Text      = "  " + item.Label,
                        Tag       = item.Key,
                        Location  = new Point(0, y),
                        Size      = new Size(210, 36),
                        FlatStyle = FlatStyle.Flat,
                        BackColor = NavyDeep,
                        ForeColor = Color.FromArgb(200, 210, 240),
                        Font      = new Font("Segoe UI", 10),
                        TextAlign = ContentAlignment.MiddleRight,
                        Cursor    = Cursors.Hand
                    };
                    btn.FlatAppearance.BorderSize         = 0;
                    btn.FlatAppearance.MouseOverBackColor = NavyMid;
                    btn.Click += NavBtn_Click;

                    pnlSidebar.Controls.Add(btn);
                    y += 38;
                }
            }

            Controls.Add(pnlSidebar);
        }

        // ── Main content area ──────────────────────────────────────

        private void BuildMain()
        {
            pnlMain = new Panel { Dock = DockStyle.Fill, BackColor = PageBg, Padding = new Padding(14, 10, 14, 10) };

            // Filter bar
            pnlFilter = new Panel { Dock = DockStyle.Top, Height = 64, BackColor = CardWhite, Padding = new Padding(10, 8, 10, 8) };
            pnlFilter.Paint += PaintCard;
            BuildFilterBar();

            // Summary cards row
            pnlCards = new Panel { Dock = DockStyle.Top, Height = 92, BackColor = Color.Transparent };
            BuildCards();

            // Data grid
            dgv = new DataGridView { Dock = DockStyle.Fill, BackgroundColor = CardWhite, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            dgv.ApplyCustomStyle();

            pnlMain.Controls.Add(dgv);
            pnlMain.Controls.Add(pnlCards);
            pnlMain.Controls.Add(pnlFilter);
            Controls.Add(pnlMain);
        }
        private void BuildFilterBar()
        {
            var lblFrom = MakeLabel("من:", 9, FontStyle.Regular, Color.Gray);
            dtpFrom = new DateTimePicker { Format = DateTimePickerFormat.Short, Size = new Size(115, 28) };
            dtpFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            var lblTo = MakeLabel("إلى:", 9, FontStyle.Regular, Color.Gray);
            dtpTo = new DateTimePicker { Format = DateTimePickerFormat.Short, Size = new Size(115, 28) };
            dtpTo.Value = DateTime.Today;

            var lblCat = MakeLabel("الفئة:", 9, FontStyle.Regular, Color.Gray);
            cmbCategory = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Size          = new Size(130, 28)
            };
            cmbCategory.Items.Add("كل الفئات");
            cmbCategory.SelectedIndex = 0;

            btnLoad = MakeBtn("🔄  تحديث", Accent, 100, 32);
            btnLoad.Anchor   = AnchorStyles.Top | AnchorStyles.Left;
            btnLoad.Location = new Point(10, 16);
            btnLoad.Click   += (s, e) => LoadCurrentReport();

            pnlFilter.Controls.AddRange(new Control[] { lblFrom, dtpFrom, lblTo, dtpTo, lblCat, cmbCategory, btnLoad });

            // Reposition right-aligned controls once pnlFilter has its actual width
            pnlFilter.SizeChanged += (s, e) =>
            {
                int w = pnlFilter.ClientSize.Width;
                if (w <= 0) return;
                lblFrom.Location     = new Point(w - 110, 20);
                dtpFrom.Location     = new Point(w - 230, 18);
                lblTo.Location       = new Point(w - 360, 20);
                dtpTo.Location       = new Point(w - 480, 18);
                lblCat.Location      = new Point(w - 600, 20);
                cmbCategory.Location = new Point(w - 734, 18);
            };
        }
        private void BuildCards()
        {
            string[] captions = { "إجمالي الأصناف", "إجمالي التكلفة", "إجمالي الربح", "تنبيهات النقص" };
            Color[]  accents  =
            {
                Color.FromArgb(0, 149, 255),
                Color.FromArgb(40, 167, 69),
                Color.FromArgb(255, 153, 0),
                Color.FromArgb(220, 53, 69)
            };

            // FlowLayoutPanel so cards reflow instead of clipping at small window sizes
            var flow = new System.Windows.Forms.FlowLayoutPanel
            {
                Dock          = DockStyle.Fill,
                FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight,
                WrapContents  = false,
                Padding       = new Padding(0, 8, 0, 0)
            };

            for (int i = 0; i < 4; i++)
            {
                int idx = i;
                var card = new Panel
                {
                    Size      = new Size(220, 76),
                    BackColor = CardWhite,
                    Margin    = new Padding(0, 0, 10, 0)
                };
                card.Paint += (s, e) => PaintCard(s, e);

                var accent = new Panel { Size = new Size(220, 4), Location = new Point(0, 0), BackColor = accents[idx] };

                _cardValues[i] = MakeLabel("—", 18, FontStyle.Bold, NavyDark);
                _cardValues[i].Location = new Point(10, 10);
                _cardValues[i].AutoSize = true;

                _cardCaptions[i] = MakeLabel(captions[i], 8, FontStyle.Regular, Color.Gray);
                _cardCaptions[i].Location = new Point(10, 50);
                _cardCaptions[i].AutoSize = true;

                card.Controls.AddRange(new Control[] { accent, _cardValues[i], _cardCaptions[i] });
                flow.Controls.Add(card);
            }

            pnlCards.Controls.Add(flow);
        }
        // ══════════════════════════════════════════════════════════
        //  NAVIGATION
        // ══════════════════════════════════════════════════════════

        private void NavBtn_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            string key = btn.Tag?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(key)) return;

            SetActiveButton(btn);
            _activeReport = key;
            LoadCurrentReport();
        }

        private void SetActiveButton(Button btn)
        {
            if (_activeBtn != null)
            {
                _activeBtn.BackColor = NavyDeep;
                _activeBtn.ForeColor = Color.FromArgb(200, 210, 240);
            }
            _activeBtn           = btn;
            btn.BackColor        = NavyMid;
            btn.ForeColor        = Color.White;
        }

        // ══════════════════════════════════════════════════════════
        //  DATA LOADING
        // ══════════════════════════════════════════════════════════

        private async void LoadCurrentReport()
        {
            if (string.IsNullOrEmpty(_activeReport)) return;

            btnLoad.Enabled  = false;
            btnLoad.Text     = "⌛ جاري التحميل...";
            dgv.DataSource   = null;
            ResetCards();

            try
            {
                switch (_activeReport)
                {
                    case "StockAudit":   await LoadStockAudit();   break;
                    case "Stagnant":     await LoadStagnant();     break;
                    case "LowStock":     await LoadLowStock();     break;
                    case "ProfitReport": await LoadProfitReport(); break;
                    case "TopSelling":   await LoadTopSelling();   break;
                    case "SafeLedger":   await LoadSafeLedger();   break;
                    case "Receivable":   await LoadReceivable();   break;
                    case "Payable":      await LoadPayable();      break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل البيانات:\n" + ex.Message, "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLoad.Enabled = true;
                btnLoad.Text    = "🔄  تحديث";
            }
        }

        // ── Report loaders ─────────────────────────────────────────

        private async System.Threading.Tasks.Task LoadStockAudit()
        {
            lblReportTitle.Text = "📊  التقارير والإحصائيات  ›  جرد المخزن الكامل";

            var items   = await _svc.GetStockAuditAsync();
            var summary = await _svc.GetStockSummaryAsync();

            BindGrid(items.Select(i => new
            {
                رقم_القطعة   = i.PartNumber,
                اسم_القطعة   = i.PartName,
                الفئة         = i.Category,
                الكمية        = i.Quantity,
                سعر_الشراء   = i.PurchasePrice.ToString("N2"),
                سعر_البيع    = i.SellingPrice.ToString("N2"),
                إجمالي_التكلفة = i.TotalCostValue.ToString("N2"),
                إجمالي_البيع  = i.TotalSellingValue.ToString("N2"),
                الربح_المتوقع  = i.PotentialProfit.ToString("N2"),
                الحد_الأدنى   = i.MinimumStock,
                الحالة         = i.StockStatus
            }));

            SetCards(
                summary.TotalItems.ToString(),
                "جنيه " + summary.TotalCostValue.ToString("N0"),
                "جنيه " + summary.TotalPotentialProfit.ToString("N0"),
                summary.LowStockCount + " + " + summary.OutOfStockCount + " نفد"
            );

            ColorStockStatusColumn();
        }

        private async System.Threading.Tasks.Task LoadStagnant()
        {
            lblReportTitle.Text = "📊  التقارير والإحصائيات  ›  الأصناف الراكدة";
            var items = await _svc.GetStagnantItemsAsync();

            BindGrid(items.Select(i => new
            {
                رقم_القطعة    = i.PartNumber,
                اسم_القطعة    = i.PartName,
                الفئة          = i.Category,
                الكمية         = i.Quantity,
                سعر_الشراء    = i.PurchasePrice.ToString("N2"),
                إجمالي_التكلفة = i.TotalValue.ToString("N2"),
                أيام_بدون_بيع = i.DaysSinceLastSale < 0 ? "لم يُباع قط" : i.DaysSinceLastSale.ToString()
            }));

            SetCards(items.Count.ToString(), items.Sum(x => x.TotalValue).ToString("N0"),
                     items.Count(x => x.DaysSinceLastSale < 0).ToString(), "—");
        }

        private async System.Threading.Tasks.Task LoadLowStock()
        {
            lblReportTitle.Text = "📊  التقارير والإحصائيات  ›  تنبيهات النقص";
            var items = await _svc.GetLowStockAlertsAsync();

            BindGrid(items.Select(i => new
            {
                رقم_القطعة       = i.PartNumber,
                اسم_القطعة       = i.PartName,
                الفئة             = i.Category,
                المخزون_الحالي    = i.CurrentStock,
                الحد_الأدنى       = i.MinimumStock,
                النقص             = i.Shortage,
                تكلفة_الإعادة     = i.EstimatedReplenishmentCost.ToString("N2")
            }));

            SetCards(items.Count.ToString(),
                     items.Count(x => x.CurrentStock == 0).ToString(),
                     "جنيه " + items.Sum(x => x.EstimatedReplenishmentCost).ToString("N0"),
                     "—");
        }

        private async System.Threading.Tasks.Task LoadProfitReport()
        {
            lblReportTitle.Text = "📊  التقارير والإحصائيات  ›  الربح والخسارة";
            var items   = await _svc.GetProfitReportAsync(dtpFrom.Value, dtpTo.Value);
            var summary = await _svc.GetProfitSummaryAsync(dtpFrom.Value, dtpTo.Value);

            BindGrid(items.Select(i => new
            {
                رقم_الفاتورة    = i.InvoiceID,
                التاريخ          = i.SaleDate.ToString("yyyy-MM-dd"),
                اسم_القطعة      = i.PartName,
                الفئة            = i.Category,
                الكمية           = i.Quantity,
                سعر_البيع       = i.SellingPrice.ToString("N2"),
                سعر_الشراء      = i.PurchasePrice.ToString("N2"),
                الإيراد          = i.Revenue.ToString("N2"),
                الربح_الإجمالي   = i.GrossProfit.ToString("N2"),
                هامش_الربح_بالمئة = i.ProfitMarginPct.ToString("N1") + " %"
            }));

            SetCards(
                "جنيه " + summary.TotalRevenue.ToString("N0"),
                "جنيه " + summary.TotalGrossProfit.ToString("N0"),
                summary.AvgMarginPct.ToString("N1") + " %",
                summary.TotalTransactions.ToString()
            );
        }

        private async System.Threading.Tasks.Task LoadTopSelling()
        {
            lblReportTitle.Text = "📊  التقارير والإحصائيات  ›  الأكثر مبيعاً";
            var items = await _svc.GetTopSellingItemsAsync(dtpFrom.Value, dtpTo.Value);

            BindGrid(items.Select(i => new
            {
                رقم_القطعة     = i.PartNumber,
                اسم_القطعة     = i.PartName,
                الفئة           = i.Category,
                الكمية_المباعة  = i.TotalSold,
                إجمالي_الإيراد  = i.TotalRevenue.ToString("N2"),
                إجمالي_الربح   = i.TotalProfit.ToString("N2"),
                متوسط_الهامش   = i.AvgMarginPct.ToString("N1") + " %"
            }));

            SetCards(items.Count.ToString(),
                     "جنيه " + items.Sum(x => x.TotalRevenue).ToString("N0"),
                     "جنيه " + items.Sum(x => x.TotalProfit).ToString("N0"),
                     "—");
        }

        private async System.Threading.Tasks.Task LoadSafeLedger()
        {
            lblReportTitle.Text = "📊  التقارير والإحصائيات  ›  سجل الخزينة";
            var items = await _svc.GetSafeLedgerAsync(dtpFrom.Value, dtpTo.Value);

            BindGrid(items.Select(i => new
            {
                التاريخ         = i.TransactionDate.ToString("yyyy-MM-dd"),
                النوع            = i.TransactionType,
                المبلغ           = i.Amount.ToString("N2"),
                الرصيد_الجاري   = i.RunningBalance.ToString("N2"),
                البيان           = i.Description,
                المستخدم         = i.UserName,
                نوع_الفاتورة    = i.InvoiceType
            }));

            decimal totalIn  = items.Where(x => x.TransactionType == "إيداع").Sum(x => x.Amount);
            decimal totalOut = items.Where(x => x.TransactionType != "إيداع").Sum(x => x.Amount);
            decimal balance  = items.Count > 0 ? items.Last().RunningBalance : 0;

            SetCards("جنيه " + totalIn.ToString("N0"),
                     "جنيه " + totalOut.ToString("N0"),
                     "جنيه " + balance.ToString("N0"),
                     items.Count.ToString());
        }

        private async System.Threading.Tasks.Task LoadReceivable()
        {
            lblReportTitle.Text = "📊  التقارير والإحصائيات  ›  الدائنون (مبالغ للتحصيل)";
            var items = await _svc.GetAccountsReceivableAsync();
            BindPersonBalances(items);
            SetCards(items.Count.ToString(),
                     "جنيه " + items.Sum(x => x.OutstandingBalance).ToString("N0"),
                     "—", "—");
        }

        private async System.Threading.Tasks.Task LoadPayable()
        {
            lblReportTitle.Text = "📊  التقارير والإحصائيات  ›  المدينون (مبالغ مستحقة)";
            var items = await _svc.GetAccountsPayableAsync();
            BindPersonBalances(items);
            SetCards(items.Count.ToString(),
                     "جنيه " + items.Sum(x => x.OutstandingBalance).ToString("N0"),
                     "—", "—");
        }

        // ── Grid helpers ───────────────────────────────────────────

        private void BindGrid<T>(IEnumerable<T> source)
        {
            dgv.DataSource = source.ToList();
            dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void BindPersonBalances(IList<PersonBalance> items)
        {
            BindGrid(items.Select(i => new
            {
                الاسم          = i.PersonName,
                الهاتف          = i.Phone,
                العنوان         = i.Address,
                المبلغ_المستحق  = i.OutstandingBalance.ToString("N2"),
                عدد_الفواتير   = i.InvoiceCount
            }));
        }

        private void ColorStockStatusColumn()
        {
            if (!dgv.Columns.Contains("الحالة")) return;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                string status = row.Cells["الحالة"]?.Value?.ToString() ?? string.Empty;
                if (status == "نفد المخزون")
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 220, 220);
                else if (status == "تحت الحد الأدنى")
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 245, 200);
                else
                    row.DefaultCellStyle.BackColor = Color.White;
            }
        }

        // ── Summary card helpers ───────────────────────────────────

        private void SetCards(string v0, string v1, string v2, string v3)
        {
            string[] vals = { v0, v1, v2, v3 };
            for (int i = 0; i < 4; i++) _cardValues[i].Text = vals[i];
        }

        private void ResetCards()
        {
            for (int i = 0; i < 4; i++) _cardValues[i].Text = "—";
        }

        // ══════════════════════════════════════════════════════════
        //  EXPORT
        // ══════════════════════════════════════════════════════════

        private void BtnExportCsv_Click(object sender, EventArgs e)
        {
            if (dgv.RowCount == 0) { MessageBox.Show("لا توجد بيانات للتصدير.", "تنبيه"); return; }

            using (var dlg = new SaveFileDialog
            {
                Filter   = "CSV (*.csv)|*.csv",
                FileName = _activeReport + "_" + DateTime.Today.ToString("yyyyMMdd") + ".csv"
            })
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;

                var sb = new StringBuilder();
                var cols = dgv.Columns.Cast<DataGridViewColumn>().Select(c => c.HeaderText);
                sb.AppendLine(string.Join(",", cols));

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    var cells = row.Cells.Cast<DataGridViewCell>().Select(c => "\"" + (c.Value ?? "") + "\"");
                    sb.AppendLine(string.Join(",", cells));
                }

                File.WriteAllText(dlg.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show("تم التصدير بنجاح:\n" + dlg.FileName, "نجاح",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  PAINT HELPERS
        // ══════════════════════════════════════════════════════════

        private static void PaintCard(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            var p = (Panel)sender;
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(220, 224, 235)), 0, 0, p.Width - 1, p.Height - 1);
        }

        // ══════════════════════════════════════════════════════════
        //  FACTORY HELPERS
        // ══════════════════════════════════════════════════════════

        private static Label MakeLabel(string text, float size, FontStyle style, Color fore)
            => new Label { Text = text, Font = new Font("Segoe UI", size, style), ForeColor = fore, AutoSize = true };

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frmReports
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmReports";
            this.ResumeLayout(false);

        }

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
    }
}
