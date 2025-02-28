using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace prjUniyatProject1
{
    public partial class Form1 : Form
    {
        frmAddContact ContactForm;
        frmUsers UsersForm;
        frmModifyClient UpdateClientForm;
        string clientId = "";
        string currentUser = UserClass.username;
        string currentUserType = UserClass.user_type;
        int numRows = 0;
        public Form1()
        {
            InitializeComponent();
            numRows = GetNumRows();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            //Add new Contact Clicked
            Form1 form1 = new Form1();
            ContactForm = new frmAddContact();
            ContactForm.ShowDialog(form1);
            
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult;
            dialogResult = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
            if (dialogResult == DialogResult.OK)
            {
                UserClass.ResetCredentials();
                this.Hide();
                new frmLogin().Show();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //currentUser = UserClass.username;
            lblCurrentUser.Text = currentUser;
            //Call The Populate ListView Method
            PopulateListView(1, 5);
            ListView1_Resize();
            CheckUserPrivileges();
        }

        //These for Modify Record
        string client_id = "";
        private void btnModify_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 1)
            {
                client_id = listView1.SelectedItems[0].SubItems[0].Text.ToString();
                ClientClass.client_id=client_id;

                //Show Dialog
                Form1 form1 = new Form1();
                UpdateClientForm = new frmModifyClient();
                UpdateClientForm.ShowDialog(new Form1());
            }
            else
            {
                MessageBox.Show("Please select a record first");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            
            if (listView1.SelectedItems.Count==1)
            {
                DialogResult dialogResult;
                dialogResult = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //Get Id of This Client
                    client_id = listView1.SelectedItems[0].SubItems[0].Text.ToString();

                    string conStr = "datasource=localhost;port=3306;username=root;password=Password1;database=uniyat_project_1";
                    MySqlConnection dbConnection = new MySqlConnection(conStr);
                    string deleteQuery = "";
                    MySqlCommand dbCommand;
                    try
                    {
                        dbConnection.Open();
                        deleteQuery = "DELETE FROM `client` WHERE `client_id` = '" + client_id + "'";
                        dbCommand = new MySqlCommand(deleteQuery, dbConnection);
                        dbCommand.CommandTimeout = 60;
                        if (dbCommand.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("Record deleted successfully");
                            //Call The Populate ListView Method
                            PopulateListView(1, 5);
                        }
                        else
                        {
                            MessageBox.Show("Error inserting record");
                        }
                        dbConnection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);

                    }
                    dbConnection.Close();
                }

            }
            else
            {
                MessageBox.Show("Please select a record first");
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }


        public void PopulateListView(int limit, int offset)
        {
            //listView1.AutoResizeColumns();
            string conStr = "datasource=localhost;port=3306;username=root;password=Password1;database=uniyat_project_1";
            MySqlConnection dbConnection = new MySqlConnection(conStr);
            string dbQuery = "SELECT * FROM client ORDER BY id ASC LIMIT " + ((limit - 1) * 5) + ", " + offset + "";
            //string dbQuery = "SELECT * FROM client";
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
                        ListViewItem listItem = new ListViewItem(myDataReader.GetString(1));
                        listItem.SubItems.Add(myDataReader.GetString(2));
                        listItem.SubItems.Add(myDataReader.GetString(3));
                        listItem.SubItems.Add(myDataReader.GetString(4));
                        listItem.SubItems.Add(myDataReader.GetString(5));
                        listItem.SubItems.Add(myDataReader.GetString(6));
                        listItem.SubItems.Add(myDataReader.GetString(7));

                        listView1.Items.Add(listItem);
                        //numRows++;
                    }

                    //Display Nav Buttons
                    flowLayoutPanel1.Controls.Clear();
                    PopulateWithButton();

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
            ListView1_Resize();
        }

        public void PopulateWithButton()
        {
            numRows = GetNumRows();
            //Create Buttons
            numRows = (int)Math.Ceiling((Decimal)numRows / 5);

            for (int i = 0; i < numRows; i++)
            {
                //Create Navigation Button
                Button navButton = new Button();
                navButton.Name = "btn" + (i + 1);
                navButton.Text = (i + 1).ToString();
                navButton.Size = new Size(new Point(20, 20));
                navButton.Margin = new Padding(3, 3, 0, 3);
                navButton.Click += new EventHandler(navButton_Click);

                flowLayoutPanel1.Controls.Add(navButton);
            }
        }

        //Function to Get Total Number of Records
        public int GetNumRows()
        {
            string conStr = "datasource=localhost;port=3306;username=root;password=Password1;database=uniyat_project_1";
            MySqlConnection dbConnection = new MySqlConnection(conStr);
            string numRowsQuery = "SELECT COUNT(*) FROM client";
            using (MySqlCommand cmd = new MySqlCommand(numRowsQuery, dbConnection))
            {
                dbConnection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }


        void ListView1_Resize()
        {
            decimal width = listView1.Width;
            //MessageBox.Show(listView1.Columns.Count.ToString());
            //MessageBox.Show(width.ToString());
            foreach (ColumnHeader column in listView1.Columns)
            {
                //if (column.Text == "Client ID")
                //{
                //    column.Width = 40;
                //}
                //else
                //{
                column.Width = -2;
                //column.Width = (int)Math.Floor((width - 40) / (listView1.Columns.Count-1)) - 5;
                //}

            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            //Populate ListView if change occured
            if (ClientClass.isClientTableChanged == true)
            {
                PopulateListView(1, 5);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //MessageBox.Show("Form shown");
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
           // MessageBox.Show("Form deactivated");
            ClientClass.isClientTableChanged = false;
        }

        private void navButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            //MessageBox.Show(btn.Text);
            //"SELECT * FROM `gadget` LIMIT " + ((Convert.ToInt32(btn.Text) - 1) * 2) + ",2", true);
            int navPage = Convert.ToInt32(btn.Text);
            //MessageBox.Show(btn.Text);
            PopulateListView(navPage, 5);
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            //Add new Contact Clicked
            Form1 form1 = new Form1();
            UsersForm = new frmUsers();
            UsersForm.ShowDialog(form1);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }


        //Function to Check User Access Rights
        void CheckUserPrivileges()
        {
            if (currentUserType=="Data Entry")
            {
                this.panel4.Controls.Remove(btnDelete);
                this.panel4.Controls.Remove(btnModify);
                this.panel6.Controls.Remove(btnUsers);
            }
            else if (currentUserType == "Normal User")
            {
                this.panel4.Controls.Remove(btnDelete);
                this.panel6.Controls.Remove(btnUsers);
            }
        }
    }
}
