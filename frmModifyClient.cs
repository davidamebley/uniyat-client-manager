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
using System.Text.RegularExpressions;

namespace prjUniyatProject1
{
    public partial class frmModifyClient : Form
    {
        int numNationalities=0;
        string[] arrNationality;
        string clientId = "";
        string name = "";
        string phone = "";
        string location = "";
        string occupation = "";
        string nationality = "";
        string address = "";
        public frmModifyClient()
        {
            InitializeComponent();
        }

        private void frmModifyClient_Load(object sender, EventArgs e)
        {

            //Get Id of This Client
            clientId = ClientClass.client_id;

            string conStr = "datasource=localhost;port=3306;username=root;password=Password1;database=uniyat_project_1";
            MySqlConnection dbConnection = new MySqlConnection(conStr);
            string selectClientQuery = "SELECT * FROM client WHERE `client_id`='"+clientId+"'";
            string selectQuery = "SELECT * FROM nationality";
            MySqlCommand dbSelectNationalityCommand = new MySqlCommand(selectQuery, dbConnection);
            MySqlCommand dbCommand = new MySqlCommand(selectClientQuery, dbConnection);
            dbCommand.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader myDataReader = dbCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    while (myDataReader.Read())
                    {
                        name = myDataReader.GetString(2);
                        phone = myDataReader.GetString(3);
                        location = myDataReader.GetString(4);
                        occupation = myDataReader.GetString(5);
                        nationality = myDataReader.GetString(6);
                        address = myDataReader.GetString(7);
                    }
                }
                //Populate Fields
                this.txtId.Text = clientId;
                this.txtName.Text = name;
                this.txtPhone.Text = phone;
                this.txtLocation.Text = location;
                this.txtOccupation.Text = occupation;
                this.cboNationality.SelectedItem = nationality;
                this.txtAddress.Text = address;

                myDataReader.Close();
                //Get ComboBox Nationalities
                MySqlDataReader nationalityDataReader = dbSelectNationalityCommand.ExecuteReader();
                if (nationalityDataReader.HasRows)
                {
                    while (nationalityDataReader.Read())
                    {
                        this.cboNationality.Items.Add(nationalityDataReader.GetString(1));
                        numNationalities++;
                    }
                    //Create Nationlity Array
                    for (int i = 0; i < numNationalities; i++)
                    {
                        //arrNationality[i] = this.cboNationality.Items[i].ToString();
                        if (this.cboNationality.Items[i].ToString()==nationality)
                        {
                            this.cboNationality.SelectedIndex = i;
                        }
                    }
                    
                }

                nationalityDataReader.Close();


                dbConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
            dbConnection.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get Id of This Client
            clientId = this.txtId.Text;
            if (txtPhone.TextLength > 9) {
                string conStr = "datasource=localhost;port=3306;username=root;password=Password1;database=uniyat_project_1";
                MySqlConnection dbConnection = new MySqlConnection(conStr);
                string updateQuery = "";
                MySqlCommand dbCommand;

                try
                {
                    dbConnection.Open();
                    updateQuery = "UPDATE `client` SET `name` = '" + txtName.Text + "', `phone` = '" + txtPhone.Text + "', `location` = '" + txtLocation.Text + "', `occupation` = '" + txtOccupation.Text + "', `nationality` = '" + this.cboNationality.SelectedItem.ToString() + "', `address` = '" + txtAddress.Text + "' WHERE `client`.`client_id` = '" + clientId + "'";
                    dbCommand = new MySqlCommand(updateQuery, dbConnection);
                    dbCommand.CommandTimeout = 60;

                    //Update
                    if (dbCommand.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Data updated successfully");
                        //Send a Broadcast Msg to Client Class
                        ClientClass.isClientTableChanged = true;
                    }
                    else
                    {
                        MessageBox.Show("Error updating data. Try again");
                    }
                    //Populate Fields
                    this.txtId.Text = clientId;
                    this.txtName.Text = name;
                    this.txtPhone.Text = phone;
                    this.txtLocation.Text = location;
                    this.txtOccupation.Text = occupation;
                    this.cboNationality.SelectedItem = nationality;
                    this.txtAddress.Text = address;

                    dbConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
                dbConnection.Close();
                this.Close();
            }
            else if(txtPhone.TextLength < 10)
            {
                MessageBox.Show("Phone number cannot be less than 10 digits");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult;
            dialogResult = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            string dummy = txtPhone.Text;
            Regex pattern = new Regex(@"^[\d]+$");
            string replaceText = Regex.Replace(dummy, @"[^\d]+", "");
            txtPhone.Text = replaceText;
            MatchCollection allMatches = pattern.Matches(dummy);
            if (allMatches.Count < 1)
            {
                MessageBox.Show("Please enter only numbers");
            }
        }
    }
}
