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
        Graph _graph = new Graph();

        //store contracts from db 
        private List<object> dbData = new List<object>();

        //store sorted date from db
        private List<object> dbDataMonth = new List<object>();
        private List<object> dbDataQuarter = new List<object>();
        private List<object> dbDataYear = new List<object>();

        //store contracts
        private List<object> contractsList = new List<object>();
        private List<object> deletedContracts = new List<object>();

        //datetime for program
        private DateTime yearDate;
        private DateTime QuarterSDate;
        private DateTime QuaterFDate;
        private DateTime MonthDate;

        //requests to db
        private string contractsRequest;
        private string monthRequest;
        private string quarterRequest;
        private string yearRequest;

        public MainWindow()
        {
            InitializeComponent();

            //set data for program
            SetDate();
            SetRequests();

            //list contracts
            GetContractsListFromDB(contractsRequest);
            MakeContracts(dbData);

            //shows report date
            reportDateMonth.Text = _odm.ReportDateMonth;
            reportDateYear.Text = _odm.ReportDateYear;
        }

        //get contracts from db
        private async void GetContractsListFromDB(string request)
        {
            await _db.CreateConnection();
            dbData = await _db.SendCommandRequest(request);
            _db.CloseConnection();
        }

        //get data from db
        private async void GetData(string requestMonth, string requestQuarter, string requestYeat)
        {
            await _db.CreateConnection();
            dbDataMonth = await _db.SendCommandRequest(requestMonth);
            dbDataQuarter = await _db.SendCommandRequest(requestQuarter);
            dbDataYear = await _db.SendCommandRequest(requestYeat);
            _db.CloseConnection();
        }

        //button "Вычислить", count everything
        private void Count(object sender, RoutedEventArgs e) 
        {
            Cleaning();
            GetData(monthRequest, quarterRequest, yearRequest);
            CountReports();
            BuildGraph();
        }

        //count reports
        private void CountReports()
        {
            foreach (object contract in contractsList)
                _calc.ResultBuilder(dbDataMonth, dbDataQuarter, dbDataYear, contract.ToString());

            List<Report> reportList = _calc.CollectReports();

            for (int i = 0; i < reportList.Count; i++)
            {
                reportListView.Items.Add(reportList[i]);
                slaListView.Items.Add(reportList[i]);
            }
        }

        //build graph
        private void BuildGraph()
        {
            _graph.BuildGraph();
            graphSLA.Series = _graph.SeriesCollection;
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

            for (int i = 0; i < contractsList.Count; i++)
                contractsListView.Items.Add(contractsList[i]);

            dbData.Clear();
        }

        //add selected contracts to contract list and contractsListView
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

        //delete selected contracts from contract list and contractListview
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

        //set date for year, quarter and month
        private void SetDate()
        {
            yearDate = new DateTime(DateTime.Now.Year, 1, 1);
            MonthDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            if (DateTime.Now.Month >= 1 && DateTime.Now.Month <= 3)
            {
                QuarterSDate = new DateTime(DateTime.Now.Year, 1, 1);
                QuaterFDate = new DateTime(DateTime.Now.Year, 3, 31);
            }
            else if (DateTime.Now.Month >= 4 && DateTime.Now.Month <= 6)
            {
                QuarterSDate = new DateTime(DateTime.Now.Year, 4, 1);
                QuaterFDate = new DateTime(DateTime.Now.Year, 6, 30);
            }
            else if (DateTime.Now.Month >= 7 && DateTime.Now.Month <= 9)
            {
                QuarterSDate = new DateTime(DateTime.Now.Year, 7, 1);
                QuaterFDate = new DateTime(DateTime.Now.Year, 9, 30);
            }
            else
            {
                QuarterSDate = new DateTime(DateTime.Now.Year, 10, 1);
                QuaterFDate = new DateTime(DateTime.Now.Year, 12, 31);
            }
        }

        //set db requests
        private void SetRequests()
        {
            contractsRequest = @"SELECT DISTINCT ServiceContractTitle FROM dbo.RequestsFull";
            monthRequest = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + MonthDate + "'";
            quarterRequest = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + QuarterSDate + "' AND CAST([CreateDate] AS date) <= '" + QuaterFDate + "'";
            yearRequest = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + yearDate + "'";
        }

        //erase data
        private void Cleaning()
        {
            //clear list views
            reportListView.Items.Clear();
            slaListView.Items.Clear();

            //clear graph
            graphSLA.Update();

            //clear storage
            _calc.ClearData();

            //clear lists
            dbDataMonth.Clear();
            dbDataQuarter.Clear();
            dbDataYear.Clear();
        }
    }
}
