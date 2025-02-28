using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace prjUniyatProject1
{
    static class LastInsertedIDClass
    {
        public static string lastId = "";

        public static int GetLastID()
        {
            int lastId = 0;
            string selectQuery = "SELECT `last_id` FROM `last_id`";
            string conStr = "datasource=localhost;port=3306;username=root;password=Password1;database=uniyat_project_1";
            MySqlConnection dbConnection = new MySqlConnection(conStr);
            MySqlCommand dbCommand = new MySqlCommand(selectQuery, dbConnection);
            dbCommand.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader myDataReader = dbCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    
                    while (myDataReader.Read())
                    {
                        lastId = myDataReader.GetInt32(0);
                    }
                }
                else
                {
                    lastId = 0;
                }
                myDataReader.Close();
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbConnection.Close();
            return lastId;
        }

        
    }
}
