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
    public partial class frmLogin : Form
    {
        string conStr = "datasource=localhost;port=3306;username=root;password=Password1;database=uniyat_project_1";
        string email = "", password = "";
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult;
            dialogResult = MessageBox.Show("Are you sure? Cancelling this dialog box will \nexit the application.", "Confirm", MessageBoxButtons.YesNo);
            if (dialogResult==DialogResult.Yes)
            {
                this.Dispose();
                Application.Exit();
            }
            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            PerformLogin();
        }


        //Function for Login
        void PerformLogin()
        {
            string passString = txtPassword.Text;
            email = txtUserEmail.Text;
            //Setup Password Hash Security
            byte[] data = System.Text.Encoding.ASCII.GetBytes(passString);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            string hash = System.Text.Encoding.ASCII.GetString(data);
            password = hash;
            //Query
            MySqlConnection dbConnection = new MySqlConnection(conStr);
            string loginQuery = "SELECT * FROM `user` WHERE `email` = '"+email+"' AND `password` = '"+password+"'";
            MySqlCommand loginCommand = new MySqlCommand(loginQuery, dbConnection);

            loginCommand.CommandTimeout = 60;
            try
            {
                dbConnection.Open();
                MySqlDataReader myDataReader = loginCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    while (myDataReader.Read())
                    {
                        UserClass.id = myDataReader.GetString(0);
                        UserClass.username = myDataReader.GetString(1);
                        UserClass.user_type = myDataReader.GetString(4);
                    }
                    MessageBox.Show("Login success!");
                    txtPassword.ResetText();
                    new Form1().Show();
                    this.Hide();
                }
                else
                    MessageBox.Show("Failed to login. Check your username or password");
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
