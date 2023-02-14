using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Threading.Tasks;
using System.Windows;

namespace MainReportDemo.Data
{
    internal class DBConnection
    {
        public string Login { get; set; }
        public string Pass { get; set; }
        public string DB { get; set; }

        public DBConnection(string login, string pass, string dB)
        {
            Login = login;
            Pass = pass;
            DB = dB;
        }

        OleDbConnection connection;

        public async Task CreateConnection()
        {
            //connection = new OleDbConnection(@"Provider = SQLOLEDB.1; Persist Security Info = False; Trusted_Connection = Yes; User ID = " + Login + "; Password = " + Pass + "; Initial Catalog = SnegirSoft_Prod; Data Source = " + DB);
            connection = new OleDbConnection(@"Provider=MSOLEDBSQL.1;Initial Catalog=TestData;Data Source=(localdb)\MSSQLLocalDB;Trusted_Connection=Yes;Persist Security Info=False");

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

            List<object> dbData = new List<object>(); //store data from db here

            try
            {
                OleDbDataReader reader = (OleDbDataReader)await command.ExecuteReaderAsync(); //read data from db

                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount]; //create row
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
