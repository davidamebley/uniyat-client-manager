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
    public partial class frmAddContact : Form
    {
        int numNationality = 0;
        string clientId = "";
        string name = "";
        string phone = "";
        string location = "";
        string occupation = "";
        string nationality = "";
        string address = "";
        bool isSaveSuccess = false;
        public frmAddContact()
        {
            InitializeComponent();
        }
        List<string> listNationality;
        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult;
            dialogResult = MessageBox.Show("Are you sure?", "Confirm",MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
            }
            
            
        }

        private void frmAddContact_Load(object sender, EventArgs e)
        {
            this.cboNationality.Items.Insert(0, "- Please Select -");
            this.cboNationality.SelectedIndex = 0;

            string conStr = "datasource=localhost;port=3306;username=root;password=Password1;database=uniyat_project_1";
            MySqlConnection dbConnection = new MySqlConnection(conStr);
            string dbQuery = "SELECT * FROM nationality";
            MySqlCommand dbCommand = new MySqlCommand(dbQuery, dbConnection);
            dbCommand.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader myDataReader = dbCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    
                    numNationality = 0;
                    listNationality = new List<string>();
                    string tempNationality = "";

                    while (myDataReader.Read())
                    {
                        tempNationality = myDataReader.GetString(1);
                        this.cboNationality.Items.Add(tempNationality);

                        numNationality++;
                    }
                    myDataReader.Close();
                    this.cboNationality.AutoCompleteSource = AutoCompleteSource.ListItems;
                    dbConnection.Close();
                }
                else if (!myDataReader.HasRows)
                {
                    MessageBox.Show("No data to show");
                }
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbConnection.Close();
            }
            dbConnection.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            TextBox text1 = new TextBox();
            SaveNewContact();
            if (isSaveSuccess)
            {
                foreach (Control item in panel2.Controls)
                {
                    if (item.Name == "cboNationality")
                    {
                        this.cboNationality.Items.Insert(0, "- Please Select -");
                        this.cboNationality.SelectedIndex = 0;
                    }
                    else if (item.GetType().Equals(text1.GetType()))
                    {
                        item.ResetText();
                    }

                }
            }
            isSaveSuccess = false;
            
        }

        private void btnSaveAndExit_Click(object sender, EventArgs e)
        {
            SaveNewContact();
            if (isSaveSuccess)
            {
                this.Dispose();
            }
            
        }


        //Function To Save New Contact
        private void SaveNewContact()
        {
            clientId = this.txtId.Text.Trim();
            name = this.txtName.Text.Trim();
            phone = this.txtPhone.Text.Trim();
            location = this.txtLocation.Text.Trim();
            occupation = this.txtOccupation.Text.Trim();
            nationality = this.cboNationality.SelectedItem.ToString().Trim();
            address = this.txtAddress.Text.Trim();

            if (name != "" && phone != "" && location != "" && nationality != "")
            {

                if (txtPhone.TextLength > 9)
                {
                    //Save clicked & Phone No. is 10 or more
                    string conStr = "datasource=localhost;port=3306;username=root;password=Password1;database=uniyat_project_1";
                    MySqlConnection dbConnection = new MySqlConnection(conStr);
                    string insertQuery = "";
                    string lastInsertIDQuery = "";
                    MySqlCommand insertCommand;
                    MySqlCommand lastIdCommand;

                    try
                    {
                        dbConnection.Open();

                        //Get ID of last row
                        int tempID = 0;

                        tempID = LastInsertedIDClass.GetLastID();
                        if (tempID == 0)
                        {
                            tempID += 1;
                            lastInsertIDQuery = "INSERT INTO `last_id` (`last_id`) VALUES ('" + tempID + "')";
                        }
                        else
                        {
                            tempID += 1;
                            lastInsertIDQuery = "UPDATE `last_id` SET `last_id` = '" + tempID + "'";
                        }
                        if (tempID.ToString().Length == 1)
                        {
                            clientId = "C0" + (tempID);
                        }
                        else
                        {
                            clientId = "C" + (tempID);
                        }

                        //Insert New Data
                        insertQuery = "INSERT INTO `uniyat_project_1`.`client` (`client_id`,`name`,`phone`,`location`,`occupation`,`nationality`,`address`) VALUES ('" + clientId + "', '" + name + "', '" + phone + "', '" + location + "', '" + occupation + "', '" + nationality + "', '" + address + "')";
                        //Setup for Last Insert Query
                        lastIdCommand = new MySqlCommand(lastInsertIDQuery, dbConnection);
                        lastIdCommand.CommandTimeout = 60;
                        if (lastIdCommand.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("Last ID Updated");
                        }


                        //Insert New Data
                        insertCommand = new MySqlCommand(insertQuery, dbConnection);
                        insertCommand.CommandTimeout = 60;
                        if (insertCommand.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("Data saved successfully");
                            isSaveSuccess = true;
                            //Call The Populate ListView Class From Form1
                            Form1 form1 = new Form1();
                            form1.PopulateListView(1, 2);
                            //Send a Broadcast Msg to Client Class
                            ClientClass.isClientTableChanged = true;
                        }
                        else
                        {
                            MessageBox.Show("Error inserting data. Try again");
                        }

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }


                }
                else if (txtPhone.TextLength < 10)
                {
                    MessageBox.Show("Phone number cannot be less than 10 digits");
                }
            }
            else
            {
                MessageBox.Show("Please fill all mandatory fields");
            }
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            string dummy = txtPhone.Text;
            Regex pattern = new Regex(@"^[\d]+$");
            string replaceText = Regex.Replace(dummy, @"[^\d]+", "");
            txtPhone.Text = replaceText;
            MatchCollection allMatches = pattern.Matches(dummy);
            if (allMatches.Count < 1 && txtPhone.TextLength>0)
            {
                MessageBox.Show("Please enter only numbers");
            }
        }

        private void frmAddContact_FormClosing(object sender, FormClosingEventArgs e)
        {
            //DialogResult dialogResult;
            //dialogResult = MessageBox.Show("Are you sure", "Confirm", MessageBoxButtons.YesNo);
            //if (dialogResult==DialogResult.Yes)
            //{
            //    this.Dispose();
            //}
            //else
            //{
                
            //}
        }
    }
}
