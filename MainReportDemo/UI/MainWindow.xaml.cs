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

        private List<object> dbData = new List<object>(); //store contracts from db 
        private List<object> dbDataMonth = new List<object>();  //store sorted data for contracts by date from db
        private List<object> dbDataQuarter = new List<object>();
        private List<object> dbDataYear = new List<object>();

        private List<object> l1 = new List<object>(); //store sorted data for graph by date from db
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

        private List<object> contractsList = new List<object>(); //store contracts
        private List<object> deletedContracts = new List<object>();

        private List<CI> crisisList = new List<CI>(); //store crisis incidents
        private List<CI> selectedCrisisList = new List<CI>();

        Dictionary<object, object> fileRows = new Dictionary<object, object>(); //store data from cds file

        private DateTime yearDate; //datetime for contracts
        private DateTime QuarterSDate;
        private DateTime QuaterFDate;
        private DateTime MonthDate;

        string quarter;  //Quarter value

        private DateTime d1; //datetime for graph
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

        private string contractsRequest; //requests to db for contracts
        private string monthRequest;
        private string quarterRequest;
        private string yearRequest;

        private string r1; //requests to db for graph
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
            SetDate(); //set data for program
            SetRequests();
            GetContractsListFromDB(contractsRequest); //list contracts
            MakeContracts(dbData);
            reportDateMonth.Text = _odm.ReportDateMonth; //shows report date
            reportDateYear.Text = _odm.ReportDateYear;
            try //try load program state
            {
                _calc.LoadData();
                Restore();
            }
            catch
            {
                MessageBox.Show("Последнее состояние программы не было восстановлено. Программа продолжит работу.", "Внимание");
            }
        }
        private void Restore() //restore saved data
        {
            List<Report> restoredReports = _calc.CollectReports(); //collect reports
            for (int i = 0; i < restoredReports.Count; i++)
            {
                reportListView.Items.Add(restoredReports[i]);
                slaListView.Items.Add(restoredReports[i]);
            }
            List<Graph> restoredGraph = _calc.CollectGraph(); //collect graphs
            Graph last = restoredGraph.Last(); //because of program logic, last graph contains actual info
            _calc.GraphLastState(last);
            graphSLA.Series = _calc.SeriesCollection;
            graphSLA.AxisX[0].Labels = _calc.Labels;
            graphSLA.AxisY[1].LabelFormatter = _calc.Formatter;
            CountCrisis(); //restore crisis
            txtBoxFilePath.Text = "Оценки ЦДС были восстановлены. Для обновления загрузите новый файл."; //cds info
        }

        private async void GetContractsListFromDB(string request) //get contracts list from db
        {
            await _db.CreateConnection();
            dbData = await _db.SendCommandRequest(request);
            _db.CloseConnection();
        }
        private async Task GetContractsData() //get data for contracts from db
        {
            await _db.CreateConnection();
            dbDataMonth = await _db.SendCommandRequest(monthRequest);
            dbDataQuarter = await _db.SendCommandRequest(quarterRequest);
            dbDataYear = await _db.SendCommandRequest(yearRequest);
            _db.CloseConnection();
        }
        private async Task GetGraphData() //get data for graph
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

        private async void Count(object sender, RoutedEventArgs e) //button "Вычислить", count everything
        {
            if (contractsListView.Items.Count == 0)
            {
                MessageBox.Show("Список контрактов для расчета пуст! Добавьте контракт!", "Внимание");
                return;
            }
            Cleaning();
            await GetContractsData(); //get contracts and graph info
            await GetGraphData();
            if (fileRows.Count != 0) //build cds marks model
            {
                _calc.FileBuilder(fileRows);
                dbDataMonth = _calc.dbCheck(dbDataMonth); //check values with file and rewrite month list from db
            }
            CountReports(); //count contracts, graph and crisis incidents
            BuildGraph();
            CountCrisis();
            _calc.SaveData(); //save program state
            txtBoxFilePath.Text = "Готово. Кризисные инциденты добавляются вручную (при наличии).";
        }
        private void Add(object sender, RoutedEventArgs e) //add selected contracts to contract list and contractsListView
        {
            if (deletedContractsListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Доступные контракты не выбраны!", "Внимание");
                return;
            }
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
        private void Delete(object sender, RoutedEventArgs e) //delete selected contracts from contract list and contractListview
        {
            if (contractsListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Контракты для исключения из расчета не выбраны!", "Внимание");
                return;
            }

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
        private void Export(object sender, RoutedEventArgs e) //export data to file
        {
            try
            {
                StreamWriter sw = new StreamWriter("result.csv", false, Encoding.Default);
                sw.WriteLine("ContractName;ReportAmount;ReportAmountYear;Critical;CriticalYear;SLAMonth;SLAQuarter;SLAYear;" +
                    "RequestsAccess;RequestsChange;RequestsUsage;Incidents;IncidentsIS;RequestsAdvice;PlannedWork;Five;Four;Three;Two;" +
                    "NoMark;Restart;");
                foreach (var item in _calc.CollectReports())
                    sw.WriteLine(item.ContractName + ";" + item.ReportAmount + ";" + item.ReportAmountYear + ";"
                        + item.Critical + ";" + item.CriticalYear + ";" + item.SLAMonth + ";" + item.SLAQuarter + ";" + item.SLAYear + ";"
                        + item.RequestsAccess + ";" + item.RequestsChange + ";" + item.RequestsUsage + ";" + item.Incidents + ";" + item.IncidentsIS + ";"
                        + item.RequestsAdvice + ";" + item.PlannedWork + ";" + item.Five + ";" + item.Four + ";" + item.Three + ";" + item.Two + ";"
                        + item.NoMark + ";" + item.Restart + ";");
                sw.WriteLine("\n\nДанные за текущий месяц: " + MonthDate.Month.ToString() + "." + MonthDate.Year.ToString());
                sw.WriteLine("Квартал: " + QuarterSDate.Day.ToString() + "." + QuarterSDate.Month.ToString() + "." + QuarterSDate.Year.ToString() + " - " + QuaterFDate.Day.ToString() + "." + QuaterFDate.Month.ToString() + "." + QuaterFDate.Year.ToString() + " (" + quarter + ")");
                sw.WriteLine("Год: " + yearDate.Year.ToString());
                sw.Close();
                MessageBox.Show("Файл записан!", "Готово");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
        private void LoadFile(object sender, RoutedEventArgs e) //load file with CDS marks
        {
            fileRows.Clear(); //clear cds dictionary first
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel files (*.xlsx)|*.xlsx";
            if (ofd.ShowDialog() == true)
                txtBoxFilePath.Text = ofd.FileName;
            if (ofd.FileName == "")
            {
                MessageBox.Show("Файл ЦДС не выбран!", "Внимание");
                return;
            }
            fileRows = _fl.LoadFile(ofd);
        }
        private void Reload(object sender, RoutedEventArgs e) //reload crisis incidents
        {
            if (crisisListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Кризисные инциденты не выбраны!", "Внимание");
                return;
            }
            foreach (CI item in crisisListView.SelectedItems)
                selectedCrisisList.Add(item);
            foreach (CI item in selectedCrisisList)
            {
                crisisList.Remove(item);
                crisisListView.Items.Remove(item);
                _calc.DropCI(item);
            }
            foreach (CI contract in selectedCrisisList)
                _calc.CrisisCounter(contract.Period, contract.ContractName);
            selectedCrisisList.Clear();
            reportListView.Items.Refresh(); //refresh UI list views
            slaListView.Items.Refresh();
            _calc.SaveData();
            txtBoxFilePath.Text = "Готово. Кризисные инциденты добавлены.";
        }
        private void Unite(object sender, RoutedEventArgs e) //unite selected contracts
        {
            if (reportListView.SelectedItems.Count == 0) //checkers
            {
                MessageBox.Show("Контракты для объединения не выбраны!", "Внимание");
                return;
            }
            if (reportListView.SelectedItems.Count == 1)
            {
                MessageBox.Show("Выберите как минимум 2 контракта для объединения!", "Внимание");
                return;
            }
            if (txtNewContr.Text == "")
            {
                MessageBox.Show("Название нового контракта не указано!", "Внимание");
                return;
            }
            List<Report> newContract = new List<Report>(); //add info to list and string
            string newContractName = txtNewContr.Text;
            foreach (Report item in reportListView.SelectedItems)
                newContract.Add(item);
            foreach (var item in newContract)  //drop contarct from store and UI
            {
                reportListView.Items.Remove(item);
                slaListView.Items.Remove(item);
                _calc.DropReport(item);
            }
            Report newReport = _calc.NewContract(newContract, newContractName); //build new contract
            reportListView.Items.Add(newReport); //add contract to UI list views
            slaListView.Items.Add(newReport);
            _calc.SaveData();
            txtNewContr.Text = "";
            txtBoxFilePath.Text = "Готово. Контракты объединены.";
        }

        private void CountReports() //count reports
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
        private void BuildGraph() //build graph
        {
            foreach (object contract in contractsList)
                _calc.GraphBuilder(l1, l2, l3, l4, l5, l6, l7, l8, l9, l10, l11, l12, l13, l14, l15, contract.ToString());
            _calc.BuildGraph();
            graphSLA.Series = _calc.SeriesCollection;
            graphSLA.AxisX[0].Labels = _calc.Labels;
            graphSLA.AxisY[1].LabelFormatter = _calc.Formatter;
        }
        private void CountCrisis() //show crisis
        {
            List<CI> ciList = _calc.CollectCI();
            for (int i = 0; i < ciList.Count; i++)
            {
                crisisList.Add(ciList[i]);
                crisisListView.Items.Add(ciList[i]);
            }
        }
        private void MakeContracts(List<object> dbData) //parse db data to contracts list
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
        
        private void SetDate() //set date for year, quarter and month
        {
            yearDate = new DateTime(DateTime.Now.Year, 1, 1); //date for contracts
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
            d1 = MonthDate.AddMonths(-14); //date for graph
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
        private void SetRequests() //set db requests
        {
            contractsRequest = @"SELECT DISTINCT ServiceContractTitle FROM dbo.RequestsFull"; //contract list
            monthRequest = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + MonthDate + "'"; //contract details
            quarterRequest = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + QuarterSDate + "' AND CAST([CreateDate] AS date) <= '" + QuaterFDate + "'";
            yearRequest = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + yearDate + "'";
            r1 = @"SELECT * FROM dbo.RequestsFull WHERE CAST([CreateDate] AS date) >= '" + d1 + "' AND CAST([CreateDate] AS date) < '" + d2 + "'"; //graph details
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
        
        private void Cleaning() //erase data
        {
            reportListView.Items.Clear(); //clear list views
            slaListView.Items.Clear();
            crisisListView.Items.Clear();
            graphSLA.Update(); //clear graph
            _calc.ClearData(); //clear storage
            dbDataMonth.Clear(); //clear contracts lists
            dbDataQuarter.Clear();
            dbDataYear.Clear();
            l1.Clear(); //clear graph lists
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
    }
}