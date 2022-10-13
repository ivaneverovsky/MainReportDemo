using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MainReportDemo.Data
{
    internal class DBConnection
    {

        readonly OleDbConnection connection = new OleDbConnection(@"Provider=MSOLEDBSQL.1;Initial Catalog=TestData;Data Source=(localdb)\MSSQLLocalDB;Trusted_Connection=Yes;Persist Security Info=False");

        public async Task CreateConnection()
        {
            try
            {
                await connection.OpenAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        public async Task<List<object>> SendCommandRequest(string request)
        {
            OleDbCommand command = new OleDbCommand(request, connection);

            //store data from db here
            List<object> dbData = new List<object>();

            try
            {
                //read data from db
                OleDbDataReader reader = (OleDbDataReader)await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    //create row
                    object[] row = new object[reader.FieldCount];

                    reader.GetValues(row);
                    dbData.Add(row);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
            
            command.Dispose();

            return dbData;
        }

        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }
    }
}
