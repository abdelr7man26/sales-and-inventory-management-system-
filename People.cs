using Auto_Parts_Store.Helpers;
using Auto_Parts_Store.Models;
using Auto_Parts_Store.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Auto_Parts_Store
{
    public partial class People : Form
    {

        private readonly IPersonRepository _repo;
        private int selectedSupplierID = 0;
        private int selectedCustomerID = 0;

        private CancellationTokenSource _cts;

        public People(IPersonRepository repo)
        {
            InitializeComponent();
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));

            this.DoubleBuffered = true;
            this.KeyPreview = true;
        }



        private async void People_Load(object sender, EventArgs e)
        {
            data.ApplyCustomStyle();
            custData.ApplyCustomStyle();

            await Task.WhenAll(
                LoadDataAsync(PersonType.Supplier),
                LoadDataAsync(PersonType.Customer)
            );

            ClearInputs(PersonType.Supplier);
            ClearInputs(PersonType.Customer);
        }


        private async Task LoadDataAsync(PersonType type)
        {
            try
            {
                var dt = await _repo.GetAllPersonsAsync(type);
                if (type == PersonType.Supplier) data.DataSource = dt;
                else custData.DataSource = dt;
            }
            catch (Exception ex) { MessageBox.Show($"خطأ في تحميل البيانات: {ex.Message}"); }
        }

       


        private async void btnadd_Click(object sender, EventArgs e)
        {
            await SavePerson(new Person
            {
                PersonName = NameTXT.Text.Trim(),
                Phone = PhoneTXT.Text.Trim(),
                Address = AddressTXT.Text.Trim(),
                Type = PersonType.Supplier
            }, isUpdate: false);
        }

        private async void btnedit_Click(object sender, EventArgs e)
        {
            if (selectedSupplierID == 0) { MessageBox.Show("اختر مورد أولاً"); return; }
            await SavePerson(new Person
            {
                ID = selectedSupplierID,
                PersonName = NameTXT.Text.Trim(),
                Phone = PhoneTXT.Text.Trim(),
                Address = AddressTXT.Text.Trim(),
                Type = PersonType.Supplier
            }, isUpdate: true);
        }

        private async void btndelete_Click(object sender, EventArgs e)
        {
            if (selectedSupplierID == 0) return;
            await DeletePerson(selectedSupplierID, data.CurrentRow, PersonType.Supplier);
        }

        private void data_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = data.Rows[e.RowIndex];
            selectedSupplierID = Convert.ToInt32(row.Cells["ID"].Value);
            NameTXT.Text = row.Cells["الأسم"].Value.ToString();
            PhoneTXT.Text = row.Cells["التليفون"].Value.ToString();
            AddressTXT.Text = row.Cells["العنوان"].Value.ToString();
            ToggleButtonState(PersonType.Supplier, selectedSupplierID);
        }




        private async void ADDCUST_Click(object sender, EventArgs e)
        {
            await SavePerson(new Person
            {
                PersonName = custNameTXT.Text.Trim(),
                Phone = CustPhoneTXT.Text.Trim(),
                Address = CUSTAddressTXT.Text.Trim(),
                Type = PersonType.Customer
            }, isUpdate: false);
        }
 
        private async void editCust_Click(object sender, EventArgs e)
        {
            if (selectedCustomerID == 0) { MessageBox.Show("اختر عميل أولاً"); return; }
            await SavePerson(new Person
            {
                ID = selectedCustomerID,
                PersonName = custNameTXT.Text.Trim(),
                Phone = CustPhoneTXT.Text.Trim(),
                Address = CUSTAddressTXT.Text.Trim(),
                Type = PersonType.Customer
            }, isUpdate: true);
        }

        private async void deleteCust_Click(object sender, EventArgs e)
        {
            if (selectedCustomerID == 0) return;
            await DeletePerson(selectedCustomerID, custData.CurrentRow, PersonType.Customer);
        }

        private void custData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = custData.Rows[e.RowIndex];
            selectedCustomerID = Convert.ToInt32(row.Cells["ID"].Value);
            custNameTXT.Text = row.Cells["الأسم"].Value.ToString();
            CustPhoneTXT.Text = row.Cells["التليفون"].Value.ToString();
            CUSTAddressTXT.Text = row.Cells["العنوان"].Value.ToString();
            ToggleButtonState(PersonType.Customer, selectedCustomerID);
        }




        private async Task SavePerson(Person person, bool isUpdate)
        {
            try
            {
                ValidationHelper.ValidatePerson(person);

                if (isUpdate) await _repo.UpdatePersonAsync(person);
                else await _repo.AddPersonAsync(person);

                MessageBox.Show("تمت العملية بنجاح");
                ClearInputs(person.Type);
                await LoadDataAsync(person.Type);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private async Task DeletePerson(int id, DataGridViewRow row, PersonType type)
        {
            var confirm = MessageBox.Show("هل أنت متأكد من الحذف؟", "تأكيد", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            try
            {
                decimal balance = Convert.ToDecimal(row.Cells["الرصيد"].Value);
                await _repo.DeletePersonAsync(id, balance);
                MessageBox.Show("تم الحذف");
                ClearInputs(type);
                await LoadDataAsync(type);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void ClearInputs(PersonType type)
        {
            if (type == PersonType.Supplier)
            {
                selectedSupplierID = 0;
                ToggleButtonState(PersonType.Supplier, 0);
                NameTXT.Clear(); PhoneTXT.Clear(); AddressTXT.Clear();
                NameTXT.Focus();
            }
            else
            {
                selectedCustomerID = 0;
                custNameTXT.Clear(); CustPhoneTXT.Clear(); CUSTAddressTXT.Clear();
                ToggleButtonState(PersonType.Customer, 0);
                custNameTXT.Focus();
            }
        }


        private void data_MouseDown(object sender, MouseEventArgs e)
        {
            var hit = ((DataGridView)sender).HitTest(e.X, e.Y);

            if (hit.Type == DataGridViewHitTestType.None)
            {
                ClearInputs(tabControl1.SelectedIndex == 0 ? PersonType.Supplier : PersonType.Customer);

                ((DataGridView)sender).ClearSelection();
            }
        }

        private void People_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                ClearInputs(PersonType.Supplier);
                ClearInputs(PersonType.Customer);
                e.Handled = true;
            }

            if (e.KeyCode == Keys.Enter)
            {
                if (!(ActiveControl is Button))
                {
                    this.SelectNextControl(ActiveControl, true, true, true, true);
                    e.Handled = true;
                    e.SuppressKeyPress = true; 
                }
            }
        }



        private async Task HandleSearch(string text, PersonType type)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            try
            {
                await Task.Delay(300, _cts.Token);

                var result = await _repo.SearchPersonsAsync(type, text);

                if (type == PersonType.Supplier)
                    data.DataSource = result;
                else
                    custData.DataSource = result;
            }
            catch (TaskCanceledException) {  }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private async void search_TextChanged(object sender, EventArgs e)
        {
            await HandleSearch(search.Text, PersonType.Supplier);
        }

        private async void CUSTSEARCHTXT_TextChanged(object sender, EventArgs e)
        {
            await HandleSearch(CUSTSEARCHTXT.Text, PersonType.Customer);
        }


        private void custData_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            var hit = dgv.HitTest(e.X, e.Y);

            if (hit.Type == DataGridViewHitTestType.None)
            {
                PersonType type = (dgv.Name == "data") ? PersonType.Supplier : PersonType.Customer;

                ClearInputs(type);
                dgv.ClearSelection();
            }
        }

        private async void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PersonType currentType = tabControl1.SelectedIndex == 0 ? PersonType.Supplier : PersonType.Customer;

            ClearInputs(currentType);

      
            if (currentType == PersonType.Supplier && data.Rows.Count == 0)
            {
                await LoadDataAsync(PersonType.Supplier);
            }
            else if (currentType == PersonType.Customer && custData.Rows.Count == 0)
            {
                await LoadDataAsync(PersonType.Customer);
            }
        }



        private void ToggleButtonState(PersonType type, int selectedID)
        {
            bool hasSelection = selectedID > 0;

            if (type == PersonType.Supplier)
            {
                btnadd.Enabled = !hasSelection;    
                btnedit.Enabled = hasSelection;    
                btndelete.Enabled = hasSelection; 
                btnStatement.Enabled = hasSelection;
            }
            else
            {
                ADDCUST.Enabled = !hasSelection;
                editCust.Enabled = hasSelection;
                deleteCust.Enabled = hasSelection;
                custStatement.Enabled = hasSelection;
            }
        }




        private void custNameTXT_Enter(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt != null)
            {
                txt.BackColor = Color.LightGoldenrodYellow; 
               
            }
        }

        private void custNameTXT_Leave(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt != null)
            {
                txt.BackColor = Color.White; 
            }
        }

        private void NameTXT_Enter(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt != null)
            {
                txt.BackColor = Color.LightGoldenrodYellow;

            }

        }

        private void NameTXT_Leave(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt != null)
            {
                txt.BackColor = Color.White;
            }
        }

        ISalesRepository repo = new SalesRepository();


        private void custStatement_Click(object sender, EventArgs e)
        {
            if (selectedSupplierID == 0 && selectedCustomerID == 0)
            {
                MessageBox.Show("يرجى اختيار مورد أو عميل أولاً");
                return;
            }

            int targetID = (tabControl1.SelectedTab.Text == "الموردين") ? selectedSupplierID : selectedCustomerID;
            string targetName = (tabControl1.SelectedTab.Text == "الموردين") ? NameTXT.Text : custNameTXT.Text;

            frmAccountStatement stmt = new frmAccountStatement(targetID, targetName , repo);
            stmt.ShowDialog(); 
        }

        private void btnStatement_Click(object sender, EventArgs e)
        {
            if (selectedSupplierID == 0 && selectedCustomerID == 0)
            {
                MessageBox.Show("يرجى اختيار مورد أو عميل أولاً");
                return;
            }

            int targetID = (tabControl1.SelectedTab.Text == "الموردين") ? selectedSupplierID : selectedCustomerID;
            string targetName = (tabControl1.SelectedTab.Text == "الموردين") ? NameTXT.Text : custNameTXT.Text;

            frmAccountStatement stmt = new frmAccountStatement(targetID, targetName,repo);
            stmt.ShowDialog(); 
        }
    }
}
