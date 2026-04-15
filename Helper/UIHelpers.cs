using System.Drawing;
using System.Windows.Forms;

namespace Auto_Parts_Store.Helpers
{
    public static class UIHelpers
    {
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
    }
}
