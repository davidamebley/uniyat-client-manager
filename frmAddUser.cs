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
    public partial class frmAddUser : Form
    {
        string conStr = "datasource=localhost;port=3306;username=root;password=Password1;database=uniyat_project_1";
        string userId = "", username = "", user_type = "", email = "", password="";
        bool isSaveSuccess = false;

        private void frmAddUser_Load(object sender, EventArgs e)
        {
            PopulateUserTypeCombo();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (txtUsername.TextLength > 1)
            {
                DialogResult dialogResult;
                dialogResult = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo);
                if (dialogResult==DialogResult.Yes)
                {
                    this.Dispose();
                }
            }
            else
            {
                this.Dispose();
            }
        }

        public frmAddUser()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddNewUser();
            if (isSaveSuccess)
            {
                this.Dispose();
            }
        }


        //Function to Add New User
        void AddNewUser()
        {
            username = this.txtUsername.Text.Trim();
            email = this.txtEmail.Text.Trim();
            user_type = this.cboUserType.SelectedItem.ToString().Trim();
            password = this.txtPassword.Text;//Password shouldn't be trimed. Remain as is

            if (username != "" && email != "" && user_type != "" && password != "")
            {

                if (txtPassword.TextLength > 5)
                {
                    //Save clicked & Pass is 6 or more
                    MySqlConnection dbConnection = new MySqlConnection(conStr);
                    string insertQuery = "";
                    string selectEmailQuery = "SELECT * FROM `user` WHERE `email`='" + email + "'";
                    MySqlCommand insertCommand;
                    MySqlCommand selectEmailCommand = new MySqlCommand(selectEmailQuery, dbConnection);
                    selectEmailCommand.CommandTimeout = 60;

                    try
                    {
                        dbConnection.Open();

                        //Search If Email Exists
                        MySqlDataReader emailDataReader = selectEmailCommand.ExecuteReader();
                        if (emailDataReader.HasRows)
                        {
                            MessageBox.Show("Email Already Exists. Try a New One");
                            emailDataReader.Close();
                        }
                        else
                        {//If Email isn't exist
                            emailDataReader.Close();
                            //Setup Password Hash Security
                            string passString = txtPassword.Text;
                            byte[] data = System.Text.Encoding.ASCII.GetBytes(passString);
                            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
                            string hash = System.Text.Encoding.ASCII.GetString(data);
                            password = hash;

                            //Insert New Data
                            insertQuery = "INSERT INTO `uniyat_project_1`.`user` (`username`,`email`,`password`,`user_type`) VALUES ('" + username + "', '" + email + "', '" + password + "', '" + user_type + "')";

                            //Insert New Data
                            insertCommand = new MySqlCommand(insertQuery, dbConnection);
                            insertCommand.CommandTimeout = 60;
                            if (insertCommand.ExecuteNonQuery() == 1)
                            {
                                MessageBox.Show("Data saved successfully");

                                //Call The Populate ListView Class From Form1
                                frmUsers UsersForm = new frmUsers();
                                UsersForm.PopulateListView();
                                //Send a Broadcast Msg to User Class
                                UserClass.isUserTableChanged = true;
                                isSaveSuccess = true;
                            }
                            else
                            {
                                MessageBox.Show("Error inserting data. Try again");
                            }

                        }
                        dbConnection.Close();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                    dbConnection.Close();
                }
                else if (txtPassword.TextLength < 6)
                {
                    MessageBox.Show("Password must be at least 6 characters");
                }
            }
            else
            {
                MessageBox.Show("Please fill all mandatory fields");
            }
        }

        //Function to Populate Usertype Combobox
        void PopulateUserTypeCombo()
        {
            List<string> listUserType;
            MySqlConnection dbConnection = new MySqlConnection(conStr);
            string selectQuery = "SELECT * FROM `user_type`";
            MySqlCommand selectCommand = new MySqlCommand(selectQuery, dbConnection);
            selectCommand.CommandTimeout = 60;
            listUserType = new List<string>();
            string tempUserType = "";

            try
            {
                dbConnection.Open();
                MySqlDataReader myDataReader = selectCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    while (myDataReader.Read())
                    {
                        tempUserType = myDataReader.GetString(1);
                        this.cboUserType.Items.Add(tempUserType);

                    }
                }
                myDataReader.Close();
                dbConnection.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            dbConnection.Close();
        }
    }
}
