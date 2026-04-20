using System;
using System.Drawing;
using System.Windows.Forms;

namespace Auto_Parts_Store.Helpers
{
    public static class UIHelpers
    {
        public class ButtonState
        {
            public float Opacity { get; set; }
            public bool IsHovered { get; set; }
        }

        public static void ApplyCustomStyle(this DataGridView dgv)
        {
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
          
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            // إعدادات العناوين
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersHeight = 35;


            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.RowTemplate.Height = 32;


            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }
        public static void AddIdColumn(this DataGridView dgv)
        {
            if (!dgv.Columns.Contains("ID_Col"))
            {
                DataGridViewTextBoxColumn idCol = new DataGridViewTextBoxColumn();
                idCol.Name = "ID_Col";
                idCol.HeaderText = "م";
                idCol.ReadOnly = true;
                idCol.Width = 40;
                idCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgv.Columns.Insert(0, idCol);
            }
        }
        public static void SetupFloatingButton(Button btn, Action onClickAction)
        {
            // الألوان
            Color oliveColor = Color.Olive; // أو استخدم Color.FromArgb(128, 128, 0)

            // إعدادات الزرار
            btn.BackColor = oliveColor;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseDownBackColor = oliveColor;
            btn.FlatAppearance.MouseOverBackColor = oliveColor;
            btn.Text = "";
            btn.Tag = new ButtonState { Opacity = 0.7f, IsHovered = false };

            bool isDragging = false;
            Point startPoint = new Point(0, 0);

            btn.Click += (s, e) => {
                if (!isDragging)
                {
                    onClickAction?.Invoke();
                }
            };

            btn.MouseEnter += (s, e) => {
                btn.Tag = new ButtonState { Opacity = 1.0f, IsHovered = true };
                btn.Invalidate();
            };

            btn.MouseLeave += (s, e) => {
                btn.Tag = new ButtonState { Opacity = 0.7f, IsHovered = false };
                btn.Invalidate();
            };

            btn.MouseDown += (s, e) => {
                if (e.Button == MouseButtons.Left)
                {
                    isDragging = true;
                    startPoint = e.Location;
                }
            };

            btn.MouseMove += (s, e) => {
                if (isDragging)
                {
                    Point p = btn.Parent.PointToClient(Control.MousePosition);
                    btn.Location = new Point(p.X - startPoint.X, p.Y - startPoint.Y);
                }
            };

            btn.MouseUp += (s, e) => {
                Timer t = new Timer { Interval = 100 };
                t.Tick += (ts, te) => { isDragging = false; t.Stop(); };
                t.Start();
            };

            btn.Paint += (s, e) => {
                var state = (ButtonState)btn.Tag;
                Graphics g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                using (SolidBrush bgBrush = new SolidBrush(oliveColor))
                    g.FillRectangle(bgBrush, btn.ClientRectangle);

                int margin = state.IsHovered ? 2 : 8;
                int diameter = Math.Min(btn.Width, btn.Height) - (margin * 2);
                Rectangle rect = new Rectangle(margin, margin, diameter, diameter);

                int alpha = (int)(state.Opacity * 255);
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(alpha, 40, 167, 69)))
                    g.FillEllipse(brush, rect);

                using (Font font = new Font("Segoe UI", 18, FontStyle.Bold))
                {
                    SizeF textSize = g.MeasureString("$", font);
                    using (SolidBrush textBrush = new SolidBrush(Color.FromArgb(alpha, Color.White)))
                        g.DrawString("$", font, textBrush, (btn.Width - textSize.Width) / 2, (btn.Height - textSize.Height) / 2);
                }
            };
        }
    }
}
