using MainReportDemo.Data;
using MainReportDemo.UIModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MainReportDemo
{
    public partial class MainWindow : Window
    {
        OutputDataModel _odm = new OutputDataModel();
        DBConnection _db = new DBConnection();
        Calculations _calc = new Calculations();
        FileLoader _fl = new FileLoader();

        //store contracts from db 
        private List<object> dbData = new List<object>();

        //store sorted data for contracts by date from db
        private List<object> dbDataMonth = new List<object>();
        private List<object> dbDataQuarter = new List<object>();
        private List<object> dbDataYear = new List<object>();

        //store sorted data for graph by date from db
        private List<object> l1 = new List<object>();
        private List<object> l2 = new List<object>();
        private List<object> l3 = new List<object>();
        private List<object> l4 = new List<object>();
        private List<object> l5 = new List<object>();
        private List<object> l6 = new List<object>();
        private List<object> l7 = new List<object>();
        private List<object> l8 = new List<object>();
        private List<object> l9 = new List<object>();
        private List<object> l10 = new List<object>();
        private List<object> l11 = new List<object>();
        private List<object> l12 = new List<object>();
        private List<object> l13 = new List<object>();
        private List<object> l14 = new List<object>();
        private List<object> l15 = new List<object>();

        //store contracts
        private List<object> contractsList = new List<object>();
        private List<object> deletedContracts = new List<object>();

        //store crisis incidents
        private List<CI> crisisList = new List<CI>();
        private List<CI> selectedCrisisList = new List<CI>();

        //store data from cds file
        Dictionary<object, object> fileRows = new Dictionary<object, object>();

        //datetime for contracts
        private DateTime yearDate;
        private DateTime QuarterSDate;
        private DateTime QuaterFDate;
        private DateTime MonthDate;

        //Quarter value
        string quarter;

        //datetime for graph
        private DateTime d1;
        private DateTime d2;
        private DateTime d3;
        private DateTime d4;
        private DateTime d5;
        private DateTime d6;
        private DateTime d7;
        private DateTime d8;
        private DateTime d9;
        private DateTime d10;
        private DateTime d11;
        private DateTime d12;
        private DateTime d13;
        private DateTime d14;
        private DateTime d15;

        //requests to db for contracts
        private string contractsRequest;
        private string monthRequest;
        private string quarterRequest;
        private string yearRequest;

        //requests to db for graph
        private string r1;
        private string r2;
        private string r3;
        private string r4;
        private string r5;
        private string r6;
        private string r7;
        private string r8;
        private string r9;
        private string r10;
        private string r11;
        private string r12;
        private string r13;
        private string r14;
        private string r15;

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

            //try load program state
            try
            {
                _calc.LoadData();
                Restore();
            }
            catch
            {
                MessageBox.Show("Последнее состояние программы не было восстановлено. Программа продолжит работу.", "Внимание");
            }
        }

        //get contracts list from db
        private async void GetContractsListFromDB(string request)
        {
            await _db.CreateConnection();
            dbData = await _db.SendCommandRequest(request);
            _db.CloseConnection();
        }

        //get data for contracts from db
        private async Task GetContractsData()
        {
            await _db.CreateConnection();
            dbDataMonth = await _db.SendCommandRequest(monthRequest);
            dbDataQuarter = await _db.SendCommandRequest(quarterRequest);
            dbDataYear = await _db.SendCommandRequest(yearRequest);
            _db.CloseConnection();
        }

        //get data for graph
        private async Task GetGraphData()
        {
            await _db.CreateConnection();
            l1 = await _db.SendCommandRequest(r1);
            l2 = await _db.SendCommandRequest(r2);
            l3 = await _db.SendCommandRequest(r3);
            l4 = await _db.SendCommandRequest(r4);
            l5 = await _db.SendCommandRequest(r5);
            l6 = await _db.SendCommandRequest(r6);
            l7 = await _db.SendCommandRequest(r7);
            l8 = await _db.SendCommandRequest(r8);
            l9 = await _db.SendCommandRequest(r9);
            l10 = await _db.SendCommandRequest(r10);
            l11 = await _db.SendCommandRequest(r11);
            l12 = await _db.SendCommandRequest(r12);
            l13 = await _db.SendCommandRequest(r13);
            l14 = await _db.SendCommandRequest(r14);
            l15 = await _db.SendCommandRequest(r15);
            _db.CloseConnection();
        }

        //button "Вычислить", count everything
        private async void Count(object sender, RoutedEventArgs e) 
        {
            Cleaning();

            //build cds marks model
            _calc.FileBuilder(fileRows);

            //get contracts and graph info
            await GetContractsData();
            await GetGraphData();

            //check values with file and rewrite month list from db
            dbDataMonth = _calc.dbCheck(dbDataMonth);

            //count contracts, graph and crisis incidents
            CountReports();
            BuildGraph();
            CountCrisis();
            
            //save program state
            _calc.SaveData();

            txtBoxFilePath.Text = "Готово. Кризисные инциденты добавляются вручную.";
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
            foreach (object contract in contractsList)
                _calc.GraphBuilder(l1, l2, l3, l4, l5, l6, l7, l8, l9, l10, l11, l12, l13, l14, l15, contract.ToString());

            _calc.BuildGraph();
            graphSLA.Series = _calc.SeriesCollection;
            graphSLA.AxisX[0].Labels = _calc.Labels;
            graphSLA.AxisY[1].LabelFormatter = _calc.Formatter;
        }

        //show crisis
        private void CountCrisis()
        {
            List<CI> ciList = _calc.CollectCI();

            for (int i = 0; i < ciList.Count; i++)
            {
                crisisList.Add(ciList[i]);
                crisisListView.Items.Add(ciList[i]);
            }
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
            //date for contracts
            yearDate = new DateTime(DateTime.Now.Year, 1, 1);
            MonthDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            if (DateTime.Now.Month >= 1 && DateTime.Now.Month <= 3)
            {
                QuarterSDate = new DateTime(DateTime.Now.Year, 1, 1);
                QuaterFDate = new DateTime(DateTime.Now.Year, 3, 31);
                quarter = "I";
            }
            else if (DateTime.Now.Month >= 4 && DateTime.Now.Month <= 6)
            {
                QuarterSDate = new DateTime(DateTime.Now.Year, 4, 1);
                QuaterFDate = new DateTime(DateTime.Now.Year, 6, 30);
                quarter = "II";
            }
            else if (DateTime.Now.Month >= 7 && DateTime.Now.Month <= 9)
            {
                QuarterSDate = new DateTime(DateTime.Now.Year, 7, 1);
                QuaterFDate = new DateTime(DateTime.Now.Year, 9, 30);
                quarter = "III";
            }
            else
            {
                QuarterSDate = new DateTime(DateTime.Now.Year, 10, 1);
                QuaterFDate = new DateTime(DateTime.Now.Year, 12, 31);
                quarter = "IV";
            }

            //date for graph
            d1 = MonthDate.AddMonths(-14);
            _calc.Labels.Add(_odm.ReturnMonthGraph(d1));
            d2 = MonthDate.AddMonths(-13);
            _calc.Labels.Add(_odm.ReturnMonthGraph(d2));
            d3 = MonthDate.AddMonths(-12);
            _calc.Labels.Add(_odm.ReturnMonthGraph(d3));
            d4 = MonthDate.AddMonths(-11);
            _calc.Labels.Add(_odm.ReturnMonthGraph(d4));
            d5 = MonthDate.AddMonths(-10);
            _calc.Labels.Add(_odm.ReturnMonthGraph(d5));
            d6 = MonthDate.AddMonths(-9);
            _calc.Labels.Add(_odm.ReturnMonthGraph(d6));
            d7 = MonthDate.AddMonths(-8);
            _calc.Labels.Add(_odm.ReturnMonthGraph(d7));
            d8 = MonthDate.AddMonths(-7);
            _calc.Labels.Add(_odm.ReturnMonthGraph(d8));
            d9 = MonthDate.AddMonths(-6);
            _calc.Labels.Add(_odm.ReturnMonthGraph(d9));
            d10 = MonthDate.AddMonths(-5);
            _calc.Labels.Add(_odm.ReturnMonthGraph(d10));
            d11 = MonthDate.AddMonths(-4);
            _calc.Labels.Add(_odm.ReturnMonthGraph(d11));
            d12 = MonthDate.AddMonths(-3);
            _calc.Labels.Add(_odm.ReturnMonthGraph(d12));
            d13 = MonthDate.AddMonths(-2);
            _calc.Labels.Add(_odm.ReturnMonthGraph(d13));
            d14 = MonthDate.AddMonths(-1);
            _calc.Labels.Add(_odm.ReturnMonthGraph(d14));
            d15 = MonthDate;
            _calc.Labels.Add(_odm.ReturnMonthGraph(d15));
        }

        //set db requests
        private void SetRequests()
        {
            //contract list
            contractsRequest = @"SELECT DISTINCT ServiceContractTitle FROM dbo.RequestsFull";
            
            //contract details
            monthRequest = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + MonthDate + "'";
            quarterRequest = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + QuarterSDate + "' AND CAST([CreateDate] AS date) <= '" + QuaterFDate + "'";
            yearRequest = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + yearDate + "'";

            //graph details
            r1 = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + d1 + "' AND CAST([CreateDate] AS date) < '" + d2 + "'";
            r2 = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + d2 + "' AND CAST([CreateDate] AS date) < '" + d3 + "'";
            r3 = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + d3 + "' AND CAST([CreateDate] AS date) < '" + d4 + "'";
            r4 = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + d4 + "' AND CAST([CreateDate] AS date) < '" + d5 + "'";
            r5 = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + d5 + "' AND CAST([CreateDate] AS date) < '" + d6 + "'";
            r6 = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + d6 + "' AND CAST([CreateDate] AS date) < '" + d7 + "'";
            r7 = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + d7 + "' AND CAST([CreateDate] AS date) < '" + d8 + "'";
            r8 = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + d8 + "' AND CAST([CreateDate] AS date) < '" + d9 + "'";
            r9 = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + d9 + "' AND CAST([CreateDate] AS date) < '" + d10 + "'";
            r10 = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + d10 + "' AND CAST([CreateDate] AS date) < '" + d11 + "'";
            r11 = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + d11 + "' AND CAST([CreateDate] AS date) < '" + d12 + "'";
            r12 = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + d12 + "' AND CAST([CreateDate] AS date) < '" + d13 + "'";
            r13 = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + d13 + "' AND CAST([CreateDate] AS date) < '" + d14 + "'";
            r14 = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + d14 + "' AND CAST([CreateDate] AS date) < '" + d15 + "'";
            r15 = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + d15 + "'";
        }

        //erase data
        private void Cleaning()
        {
            //clear list views
            reportListView.Items.Clear();
            slaListView.Items.Clear();
            crisisListView.Items.Clear();

            //clear graph
            graphSLA.Update();

            //clear storage
            _calc.ClearData();

            //clear contracts lists
            dbDataMonth.Clear();
            dbDataQuarter.Clear();
            dbDataYear.Clear();

            //clear graph lists
            l1.Clear();
            l2.Clear();
            l3.Clear();
            l4.Clear();
            l5.Clear();
            l6.Clear();
            l7.Clear();
            l8.Clear();
            l9.Clear();
            l10.Clear();
            l11.Clear();
            l12.Clear();
            l13.Clear();
            l14.Clear();
            l15.Clear();
        }

        //restore saved data
        private void Restore()
        {
            //collect reports
            List<Report> restoredReports = _calc.CollectReports();
            for (int i = 0; i < restoredReports.Count; i++)
            {
                reportListView.Items.Add(restoredReports[i]);
                slaListView.Items.Add(restoredReports[i]);
            }

            //collect graphs
            List<Graph> restoredGraph = _calc.CollectGraph();

            //because of program logic, last graph contains actual info
            Graph last = restoredGraph.Last();
            _calc.GraphLastState(last);

            graphSLA.Series = _calc.SeriesCollection;
            graphSLA.AxisX[0].Labels = _calc.Labels;
            graphSLA.AxisY[1].LabelFormatter = _calc.Formatter;

            //restore crisis
            CountCrisis();

            //cds info
            txtBoxFilePath.Text = "Оценки ЦДС были восстановлены. Для обновления загрузите новый файл.";
        }

        //export data to file
        private void Export(object sender, RoutedEventArgs e)
        {
            try
            {
                StreamWriter sw = new StreamWriter("result.csv", false, Encoding.Default);
                sw.WriteLine("Color;ColorYear;ContractName;ReportAmount;ReportAmountYear;Critical;CriticalYear;SLAMonth;SLAQuarter;SLAYear;" +
                    "RequestsAccess;RequestsChange;RequestsUsage;Incidents;IncidentsIS;RequestsAdvice;PlannedWork;Five;Four;Three;Two;" +
                    "NoMark;Restart;");
                foreach (var item in _calc.CollectReports())
                    sw.WriteLine(item.Color + ";" + item.ColorYear + ";" + item.ContractName + ";" + item.ReportAmount + ";" + item.ReportAmountYear + ";" 
                        + item.Critical + ";" + item.CriticalYear + ";" + item.SLAMonth + ";" + item.SLAQuarter + ";" + item.SLAYear + ";" 
                        + item.RequestsAccess + ";" + item.RequestsChange + ";" + item.RequestsUsage + ";" + item.Incidents + ";" + item.IncidentsIS + ";" 
                        + item.RequestsAdvice + ";" + item.PlanedWork + ";" + item.Five + ";" + item.Four + ";" + item.Three + ";" + item.Two + ";" 
                        + item.NoMark + ";" + item.Restart + ";");

                sw.WriteLine("\n\nДанные за текущий месяц: " + MonthDate.Month.ToString() + "." + MonthDate.Year.ToString());
                sw.WriteLine("Квартал: " + QuarterSDate.Day.ToString() + "." + QuarterSDate.Month.ToString() + "." + QuarterSDate.Year.ToString() + " - " + QuaterFDate.Day.ToString() + "." + QuaterFDate.Month.ToString() + "." + QuaterFDate.Year.ToString() + " (" + quarter + ")");
                sw.WriteLine("Год: " + yearDate.Year.ToString());
                sw.Close();

                MessageBox.Show("Файл записан.", "Готово");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        //load file with CDS marks
        private void LoadFile(object sender, RoutedEventArgs e)
        {
            //clear cds dictionary first
            fileRows.Clear();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel files (*.xlsx)|*.xlsx";
            if (ofd.ShowDialog() == true)
                txtBoxFilePath.Text = ofd.FileName;

            fileRows = _fl.LoadFile(ofd);
        }

        //reload crisis incidents
        private void Reload(object sender, RoutedEventArgs e)
        {
            foreach (CI item in crisisListView.SelectedItems)
                selectedCrisisList.Add(item);

            foreach (CI item in selectedCrisisList)
            {
                crisisList.Remove(item);
                crisisListView.Items.Remove(item);
            }

            foreach (CI contract in selectedCrisisList)
                _calc.CrisisCounter(contract.Period, contract.ContractName);

            selectedCrisisList.Clear();

            //refresh UI list views
            reportListView.Items.Refresh();
            slaListView.Items.Refresh();

            txtBoxFilePath.Text = "Готово. Кризисные инциденты добавлены.";
        }
    }
}
