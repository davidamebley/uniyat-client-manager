using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace prjUniyatProject1
{
    static class PopulateListViewClass
    {
        public static void Populate()
        {
            //    int numRows = 0;
            //    ListView listView1;
            //    //listView1.AutoResizeColumns();
            //    string conStr = "datasource=localhost;port=3306;username=root;password=Password1;database=uniyat_project_1";
            //    MySqlConnection dbConnection = new MySqlConnection(conStr);
            //    string dbQuery = "SELECT * FROM client";
            //    MySqlCommand dbCommand = new MySqlCommand(dbQuery, dbConnection);
            //    dbCommand.CommandTimeout = 60;

            //    try
            //    {
            //        dbConnection.Open();
            //        MySqlDataReader myDataReader = dbCommand.ExecuteReader();
            //        if (myDataReader.HasRows)
            //        {
            //            numRows = 0;
            //            if (listView1.Items.Count > 0)
            //            {
            //                listView1.Items.Clear();
            //            }
            //            while (myDataReader.Read())
            //            {
            //                ListViewItem listItem = new ListViewItem(myDataReader.GetString(1));
            //                listItem.SubItems.Add(myDataReader.GetString(2));
            //                listItem.SubItems.Add(myDataReader.GetString(3));
            //                listItem.SubItems.Add(myDataReader.GetString(4));
            //                listItem.SubItems.Add(myDataReader.GetString(5));
            //                listItem.SubItems.Add(myDataReader.GetString(6));
            //                listItem.SubItems.Add(myDataReader.GetString(7));

            //                listView1.Items.Add(listItem);
            //                numRows++;
            //            }
            //        }
            //        else if (!myDataReader.HasRows)
            //        {
            //            MessageBox.Show("No data to show");
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            }
        }
    }
