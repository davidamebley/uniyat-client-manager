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
    public partial class frmModifyUser : Form
    {
        string conStr = "datasource=localhost;port=3306;username=root;password=Password1;database=uniyat_project_1";
        string userId = ChangeUserClass.userId;
        bool isUserActive;
        bool userActiveState;
        string userType;
        bool isSaveSuccess = false;
        public frmModifyUser()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmModifyUser_Load(object sender, EventArgs e)
        {
            PopulateUserTypeCombo();
            this.cboUserType.SelectedItem = GetUserType();
            this.txtId.Text = userId;
            if (isUserActive)
            {
                chkUserActive.CheckState = CheckState.Checked;
            }
            else
            {
                chkUserActive.CheckState = CheckState.Unchecked;
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

        string GetUserType()
        {
            MySqlConnection dbConnection = new MySqlConnection(conStr);
            string dbQuery = "SELECT `user_type`, `is_user_active` FROM user WHERE `user_id` = '"+userId+"'";
            MySqlCommand dbCommand = new MySqlCommand(dbQuery, dbConnection);
            string tempUserType = "";

            dbCommand.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader myDataReader = dbCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    while (myDataReader.Read())
                    {
                        tempUserType = myDataReader.GetString(0);
                        isUserActive = myDataReader.GetBoolean(1);
                    }

                }
                else if (!myDataReader.HasRows)
                {
                    MessageBox.Show("Error getting user");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return tempUserType;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateUser();
            if (isSaveSuccess)
            {
                this.Dispose();
                UserClass.isUserTableChanged = true;
            }
            
        }


        //Function to cause Update User
        void UpdateUser()
        {
            userActiveState = Convert.ToBoolean(chkUserActive.CheckState);
            userType = cboUserType.SelectedItem.ToString();
            MySqlConnection dbConnection = new MySqlConnection(conStr);
            string updateQuery = "UPDATE `user` SET `user_type` = '" + userType + "', `is_user_active` = '"+userActiveState+"' WHERE user_id = '"+userId+"'";
            MySqlCommand updateCommand = new MySqlCommand(updateQuery, dbConnection);

            updateCommand.CommandTimeout = 60;
            try
            {
                dbConnection.Open();
                if (updateCommand.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Update success");
                    isSaveSuccess = true;
                }
                else
                    MessageBox.Show("Error while updating data. Try again");
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
