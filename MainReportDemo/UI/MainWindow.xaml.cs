using MainReportDemo.Data;
using MainReportDemo.UIModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace MainReportDemo
{
    public partial class MainWindow : Window
    {
        OutputDataModel _odm = new OutputDataModel();
        DBConnection _db = new DBConnection();
        Calculations _calc = new Calculations();


        //store data from db 
        private List<object> dbData = new List<object>();

        public List<object> contractsList = new List<object>();
        public List<object> deletedContracts = new List<object>();


        public MainWindow()
        {
            InitializeComponent();
            
            //shows report date
            reportDateMonth.Text = _odm.ReportDateMonth;
            reportDateYear.Text = _odm.ReportDateYear;

            //list contracts
            Connect(@"SELECT DISTINCT ServiceContractTitle FROM dbo.RequestsFull");
            MakeContracts(dbData);
            for (int i = 0; i < contractsList.Count; i++)
                contractsListView.Items.Add(contractsList[i]);
        }

        //count contracts data
        private void Count(object sender, RoutedEventArgs e) 
        {
            Cleaning();

            Connect(@"SELECT * FROM [dbo].RequestsFull WHERE CAST([CreateDate] AS date) >= '" + _odm.StartReportDate + "' AND CAST([CreateDate] AS date) <= '" + _odm.FinalReportDate + "'");

            CountReports();
        }

        //connect to db
        private async void Connect(string request)
        {
            await _db.CreateConnection();
            dbData = await _db.SendCommandRequest(request);
            _db.CloseConnection();
        }

        private void CountReports()
        {
            foreach (var contract in contractsList)
                _calc.ResultBuilder(dbData, contract.ToString());

            List<Report> reportList = _calc.CollectReports();

            for (int i = 0; i < reportList.Count; i++)
                reportListView.Items.Add(reportList[i]);

            Task[] tasks = new Task[2]
            {
                new Task(() => CountSLA()),
                new Task(() => CountCrisis())
            };

            foreach (Task task in tasks)
                task.Start();

            try
            {
                Task.WaitAll(tasks);
            }
            catch (AggregateException ae)
            {
                MessageBox.Show(ae.Message, "Ошибка");
            }
        }

        //count sla
        private void CountSLA()
        {
            //MessageBox.Show("SLA");
        }

        //count crisis incidents
        private void CountCrisis()
        {
            //MessageBox.Show("Crisis");
        }

        //parse db data to contracts list
        private void MakeContracts(List<object> dbData)
        {
            for (int i = 0; i < dbData.Count; i++)
            {
                object[] item = (object[])dbData[i];

                if (item[0].ToString() != "")
                    contractsList.Add(item[0]);
            }
        }

        //add selected contracts to contractsListView
        private void Add(object sender, RoutedEventArgs e)
        {
            foreach (var item in deletedContractsListView.SelectedItems)
            {
                contractsList.Add(item);
                contractsListView.Items.Add(item);
            }

            foreach (var item in contractsList)
            {
                deletedContracts.Remove(item);
                deletedContractsListView.Items.Remove(item);
            }
        }

        //delete selected contracts from contract list and listview
        private void Delete(object sender, RoutedEventArgs e)
        {
            foreach (var item in contractsListView.SelectedItems)
            {
                deletedContracts.Add(item);
                deletedContractsListView.Items.Add(item);
            }

            foreach (var item in deletedContracts)
            {
                contractsList.Remove(item);
                contractsListView.Items.Remove(item);
            }
        }

        //erase data
        private void Cleaning()
        {
            reportListView.Items.Clear();
            _calc.ClearData();
            dbData.Clear();
        }
    }
}
