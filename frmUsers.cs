using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjUniyatProject1
{
    public partial class frmUsers : Form
    {
        frmAddUser AddUserForm;
        string changeUserId = "";
        public frmUsers()
        {
            InitializeComponent();
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
            {
                MessageBox.Show("Please select a record first");
            }
            else
            {

            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            PopulateListView();
        }

        public void PopulateListView()
        {
            //listView1.AutoResizeColumns();
            string conStr = "datasource=localhost;port=3306;username=root;password=Password1;database=uniyat_project_1";
            MySqlConnection dbConnection = new MySqlConnection(conStr);
            string dbQuery = "SELECT * FROM user";
            MySqlCommand dbCommand = new MySqlCommand(dbQuery, dbConnection);

            dbCommand.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader myDataReader = dbCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    //numRows = 0;
                    if (listView1.Items.Count > 0)
                    {
                        listView1.Items.Clear();
                    }
                    while (myDataReader.Read())
                    {
                        ListViewItem listItem = new ListViewItem(myDataReader.GetString(0));
                        listItem.SubItems.Add(myDataReader.GetString(1));
                        listItem.SubItems.Add(myDataReader.GetString(2));
                        listItem.SubItems.Add(myDataReader.GetString(4));
                        if (myDataReader.GetBoolean(5)==true)
                        {
                            listItem.SubItems.Add("Active");
                        }
                        else
                        {
                            listItem.SubItems.Add("Inactive");
                        }

                        listView1.Items.Add(listItem);
                        //numRows++;
                    }

                }
                else if (!myDataReader.HasRows)
                {
                    MessageBox.Show("No data to show");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            foreach (ColumnHeader item in listView1.Columns)
            {
                item.Width = -2;
            }
        }

        private void frmUsers_Activated(object sender, EventArgs e)
        {
            if (UserClass.isUserTableChanged==true)
            {
                PopulateListView();
            }
        }

        private void frmUsers_Deactivate(object sender, EventArgs e)
        {
            UserClass.isUserTableChanged = false;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnModify_Click_1(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
            {
                MessageBox.Show("Please select a record first");
            }
            else
            {
                changeUserId = listView1.SelectedItems[0].SubItems[0].Text.ToString();
                ChangeUserClass.userId = changeUserId;
                frmModifyUser ModifyUserForm = new frmModifyUser();
                ModifyUserForm.ShowDialog(new frmUsers());
            }
        }

        private void btnAddNew_Click_1(object sender, EventArgs e)
        {
            frmUsers UsersForm = new frmUsers();
            AddUserForm = new frmAddUser();
            AddUserForm.ShowDialog(UsersForm);
            UsersForm.Hide();
        }
    }
}
