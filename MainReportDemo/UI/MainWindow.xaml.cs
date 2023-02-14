using MainReportDemo.Data;
using MainReportDemo.UI;
using MainReportDemo.UIModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MainReportDemo
{
    public partial class MainWindow : Window
    {
        OutputDataModel _odm = new OutputDataModel();
        DBConnection _db;
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

        List<string> droppedContracts = new List<string>();

        private List<CI> crisisList = new List<CI>(); //store crisis incidents
        private List<CI> selectedCrisisList = new List<CI>();

        Dictionary<object, object> fileRows = new Dictionary<object, object>(); //store data from cds file
        List<object> fileColumns = new List<object>(); //store data from cds file

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

        //initializing
        public MainWindow()
        {
            InitializeComponent();
            SetDate(); //set data for program
            SetRequests();

            reportDateMonth.Text = _odm.ReportDateMonth; //shows report date
            reportDateYear.Text = _odm.ReportDateYear;

            try //try load program state
            {
                _calc.LoadData();
                BuildUI();
                txtBoxFilePath.Text = "Данные восстановлены.";
            }
            catch
            {
                //MessageBox.Show("Последнее состояние программы не было восстановлено. Программа продолжит работу.", "Внимание");
                txtBoxFilePath.Text = "Выполните подключение.";
            }
        }

        //db connection
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

        //buttons 
        private void Connect(object sender, RoutedEventArgs e) //connect to db get contracts
        {
            if (txtLogin.Text == "" || txtDB.Text == "")
            {
                MessageBox.Show("Введите данные для подключения к базе данных.", "Внимание");
                return;
            }

            _db = new DBConnection(txtLogin.Text, txtPass.Password, txtDB.Text);

            GetContractsListFromDB(contractsRequest); //list contracts
            MakeContracts(dbData);

            txtBoxFilePath.Text = "Список контрактов получен.";
        }
        private async void Count(object sender, RoutedEventArgs e) //button "Вычислить", count everything
        {
            if (contractsListView.Items.Count == 0)
            {
                MessageBox.Show("Список контрактов для расчета пуст. Добавьте контракт.", "Внимание");
                return;
            }

            Cleaning();

            await GetContractsData(); //get contracts and graph info
            await GetGraphData();

            if (fileRows.Count != 0) //build cds marks model
            {
                _calc.FileBuilder(fileRows);
                dbDataMonth = _calc.DBCheck(dbDataMonth); //check values with file and rewrite month list from db
            }

            CountReports(); //count contracts, requests, graph and crisis incidents
            CountRequests();
            BuildGraph();
            BuildUI();

            _calc.SaveData(); //save program state

            txtBoxFilePath.Text = "Готово. Кризисные инциденты добавляются вручную (при наличии).";
        }
        private void Add(object sender, RoutedEventArgs e) //add selected contracts to contract list and contractsListView
        {
            if (deletedContractsListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Доступные контракты не выбраны.", "Внимание");
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

            txtFilter.Clear();
            txtFilter.Focus();
        }
        private void Delete(object sender, RoutedEventArgs e) //delete selected contracts from contract list and contractListview
        {
            if (contractsListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Контракты для исключения из расчета не выбраны.", "Внимание");
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
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            StreamWriter sw = new StreamWriter(filePath + @"\result.csv", false, Encoding.Default);

            try
            {
                //contracts
                sw.WriteLine("ContractName;ReportAmount;ReportAmountYear;Critical;CriticalYear;SLAMonth;SLAQuarter;SLAYear;" +
                    "RequestsAccess;RequestsChange;RequestsUsage;Incidents;IncidentsIS;RequestsAdvice;PlannedWork;Five;Four;Three;Two;" +
                    "NoMark;Restart; ;RQ;Inc");

                foreach (var item in _calc.CollectReports())
                {
                    double sum = item.Five + item.Four + item.Three + item.Two + item.NoMark + item.Restart;
                    double sumRQ = item.RequestsAccess + item.RequestsAdvice + item.RequestsChange + item.RequestsUsage;
                    double sumInc = item.Incidents + item.IncidentsIS;
                    double zapros = sumRQ + sumInc;
                    double RQPerc;
                    double IncPerc;

                    if (zapros == 0)
                    {
                        RQPerc = 0;
                        IncPerc = 0;
                    }
                    else
                    {
                        RQPerc = sumRQ / zapros;
                        IncPerc = 1 - RQPerc;
                    }

                    sw.WriteLine(item.ContractName + ";" + item.ReportAmount + ";" + item.ReportAmountYear + ";"
                        + item.Critical + ";" + item.CriticalYear + ";" + (item.SLAMonth / 100).ToString().Replace("." , ",") + ";" + (item.SLAQuarter / 100).ToString().Replace(".", ",") + ";" + (item.SLAYear / 100).ToString().Replace(".", ",") + ";"
                        + item.RequestsAccess + ";" + item.RequestsChange + ";" + item.RequestsUsage + ";" + item.Incidents + ";" + item.IncidentsIS + ";"
                        + item.RequestsAdvice + ";" + item.PlannedWork + ";" + Math.Round(item.Five / sum, 2).ToString().Replace(".", ",") + ";" + Math.Round(item.Four / sum, 2).ToString().Replace(".", ",") + ";"
                        + Math.Round(item.Three / sum, 2).ToString().Replace(".", ",") + ";" + Math.Round(item.Two / sum, 2).ToString().Replace(".", ",") + ";" + Math.Round(item.NoMark / sum, 2).ToString().Replace(".", ",") + ";" + Math.Round(item.Restart / sum, 2).ToString().Replace(".", ",") + ";" + ";" + Math.Round(RQPerc, 2).ToString().Replace(".", ",") + ";" + Math.Round(IncPerc, 2).ToString().Replace(".", ",") + ";");
                }

                //Requests
                sw.WriteLine("\nОбращения;\nПериод;Получено;Закрыто;SLA;КИ");
                foreach (var item in _calc.CollectRequests())
                {
                    sw.WriteLine(item.DateType + ";" + item.GetRequests + ";" + item.ClosedRequests + ";" + (item.TotalSLA / 100).ToString().Replace(".", ",") + ";" + item.CrisisCount + ";");
                }

                //IS
                sw.WriteLine("\nDH-1;\nНазвание строк;Месяц;Квартал;Год");
                foreach (var item in _calc.CollectIS())
                    if (item.DH == "DH-1")
                        sw.WriteLine(item.IBCat1 + ";" + item.Month + ";" + item.Quarter + ";" + item.Year);

                sw.WriteLine("Общий итог\nSLA нарушен");

                sw.WriteLine("\nDH-2;\nНазвание строк;Месяц;Квартал;Год");
                foreach (var item in _calc.CollectIS())
                    if (item.DH == "DH-2")
                        sw.WriteLine(item.IBCat1 + ";" + item.Month + ";" + item.Quarter + ";" + item.Year);

                sw.WriteLine("Общий итог\nSLA нарушен");

                //CI
                sw.WriteLine("\nКризисные инциденты - накопительный итог\nВсего с начала года;Количество часов простоя с начала года;Нарушение инструкций с начала года;Нарушение инструкций за отчетный период\n\n");

                sw.WriteLine("\nКоличество КИ с начала года\nМР;Всего нарушений");
                foreach (var item in _calc.CollectReports())
                    sw.WriteLine(item.ContractName + ";" + item.CriticalYear);

                sw.WriteLine("\n\nВремя простоя по КИ с начала года\nРПУ;I кв.;II кв.;III кв.;IV кв.;Всего");
                foreach (var item in _calc.CollectReports())
                    sw.WriteLine(item.ContractName);

                sw.WriteLine("\n\nИзменение удельного показателя времени простоя в сравнении с предыдущим годом\n ;" + yearDate.AddYears(-1).Year.ToString() + ";" + DateTime.Now.Year.ToString() + ";" + "Цель;Порог\nI кв.\nII кв.\nIII кв.\nIV кв.\nЗа год");


                //info
                sw.WriteLine("\n\nРасчет выгружен: " + DateTime.Now.Day.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Year.ToString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString());
                sw.WriteLine("Квартал: " + QuarterSDate.Day.ToString() + "." + QuarterSDate.Month.ToString() + "." + QuarterSDate.Year.ToString() + " - " + QuaterFDate.Day.ToString() + "." + QuaterFDate.Month.ToString() + "." + QuaterFDate.Year.ToString() + " (" + quarter + ")");

                MessageBox.Show("Файл записан.", "Готово");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка во время экспорта данных. " + ex.Message, "Ошибка");
            }
            finally
            {
                sw.Close();
            }
        }
        private void LoadFile(object sender, RoutedEventArgs e) //load file with CDS marks
        {
            fileRows.Clear(); //clear cds dictionary first
            fileColumns.Clear();

            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx"
            };
            ofd.ShowDialog();

            if (ofd.FileName == "")
            {
                MessageBox.Show("Файл с оценками ЦДС не выбран.", "Внимание");
                return;
            }
            fileColumns = _fl.LoadColumns(ofd);

            CDSWindow cdsw = new CDSWindow
            {
                DataContext = fileColumns,
                Owner = this,
                Title = "Оценки ЦДС"
            };
            cdsw.ShowDialog();

            if (!cdsw.Bull)
            {
                MessageBox.Show("Данные не были обработаны. Для учета оценок ЦДС перезагрузите файл и выберите столбцы для расчета в открывшемся окне.", "Внимание");
                fileColumns.Clear();
                return;
            }
            fileRows = _fl.LoadFile(ofd, cdsw.StartIndex, cdsw.EndIndex);

            txtBoxFilePath.Text = "Файл с оценками ЦДС загружен.";
        }
        private void Reload(object sender, RoutedEventArgs e) //reload crisis incidents
        {
            if (crisisListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Кризисные инциденты не выбраны.", "Внимание");
                return;
            }

            bool bull = false;

            foreach (CI item in crisisListView.SelectedItems)
                selectedCrisisList.Add(item);

            foreach (CI contract in selectedCrisisList)
                bull = _calc.CrisisCounter(contract.Period, contract.ContractName);

            if (bull)
            {
                foreach (CI item in selectedCrisisList)
                {
                    crisisList.Remove(item);
                    crisisListView.Items.Remove(item);
                    _calc.DropCI(item);
                }

                selectedCrisisList.Clear();

                reportListView.Items.Refresh(); //refresh UI list views
                slaListView.Items.Refresh();

                _calc.SaveData();

                txtBoxFilePath.Text = "Готово. Кризисные инциденты добавлены.";
            }
            else
                selectedCrisisList.Clear();
        }
        private void Unite(object sender, RoutedEventArgs e) //unite selected contracts
        {
            bool bull;

            //checkers
            if (reportListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Контракты для объединения не выбраны.", "Внимание");
                return;
            }

            if (reportListView.SelectedItems.Count == 1)
            {
                MessageBox.Show("Выберите как минимум 2 контракта для объединения.", "Внимание");
                return;
            }

            if (txtNewContr.Text == "")
            {
                MessageBox.Show("Название нового контракта не указано.", "Внимание");
                return;
            }

            bull = _calc.CheckContractName(txtNewContr.Text);

            if (bull)
            {
                MessageBox.Show("Контракт с названием \"" + txtNewContr.Text + "\" уже существует.", "Внимание");
                txtNewContr.Focus();
                return;
            }

            //add info to list and string
            List<Report> newContract = new List<Report>();
            string newContractName = txtNewContr.Text; //🔒

            foreach (Report item in reportListView.SelectedItems)
                newContract.Add(item);

            //drop contract from store and UI
            foreach (var item in newContract)
            {
                reportListView.Items.Remove(item);
                slaListView.Items.Remove(item);

                _calc.DropReport(item);
            }

            //build new contract
            Report newReport = _calc.NewContract(newContract, newContractName);

            //add contract to UI list views
            reportListView.Items.Add(newReport);
            slaListView.Items.Add(newReport);

            _calc.SaveData();

            txtNewContr.Text = string.Empty;
            txtBoxFilePath.Text = "Готово. Контракты объединены.";
        }
        private void Detach(object sender, RoutedEventArgs e) //detach selected contracts
        {
            var selection = reportListView.SelectedIndex;
            bool bull;

            if (selection != -1)
            {
                var itemToDetach = reportListView.SelectedItem as Report;
                bull = _calc.DetachContract(itemToDetach, itemToDetach.ContractName);

                if (bull)
                {
                    _calc.SaveData();

                    reportListView.Items.Clear();
                    slaListView.Items.Clear();

                    List<Report> reportList = _calc.CollectReports();

                    for (int i = 0; i < reportList.Count; i++)
                    {
                        reportListView.Items.Add(reportList[i]);
                        slaListView.Items.Add(reportList[i]);
                    }

                    txtBoxFilePath.Text = "Готово. Контракты разъединены.";
                    return;
                }
            }
            else
            {
                MessageBox.Show("Контракт для разъединения не выбран.", "Внимание");
                return;
            }
        }
        private void Rename(object sender, RoutedEventArgs e) //rename selected contract
        {
            var selection = reportListView.SelectedIndex;

            if (selection == -1)
            {
                MessageBox.Show("Контракт для переименования не выбран.", "Внимание");
                return;
            }
            if (txtNewContr.Text == "")
            {
                MessageBox.Show("Не указано новое название контракта.", "Внимание");
                return;
            }

            bool bull;
            var itemToRename = reportListView.SelectedItem as Report;
            bull = _calc.RenameContract(itemToRename, txtNewContr.Text);

            if (bull)
            {
                _calc.SaveData();

                reportListView.Items.Clear();
                slaListView.Items.Clear();

                List<Report> reportList = _calc.CollectReports();

                for (int i = 0; i < reportList.Count; i++)
                {
                    reportListView.Items.Add(reportList[i]);
                    slaListView.Items.Add(reportList[i]);
                }

                txtNewContr.Text = string.Empty;
                txtBoxFilePath.Text = "Готово. Контракт переименован.";

                return;
            }
        }
        private void Up(object sender, RoutedEventArgs e) //moves the contract up
        {
            var selection = reportListView.SelectedIndex;

            if (selection > 0 && selection != -1)
            {
                var itemToMoveReportListView = reportListView.Items[selection];
                var itemToMoveSLAListView = slaListView.Items[selection];
                var selectedReport = reportListView.SelectedItem as Report;

                reportListView.Items.Remove(itemToMoveReportListView);
                slaListView.Items.Remove(itemToMoveSLAListView);
                _calc.DropReport(selectedReport);

                reportListView.Items.Insert(selection - 1, itemToMoveReportListView);
                slaListView.Items.Insert(selection - 1, itemToMoveSLAListView);
                _calc.InsertReport(selection - 1, selectedReport);

                reportListView.SelectedIndex = selection - 1;

                _calc.SaveData();

                return;
            }
            else if (selection == 0)
            {
                MessageBox.Show("Контракт в начале списка.", "Внимание");
                return;
            }
            else
            {
                MessageBox.Show("Не выбран контракт для перемещения вверх.", "Внимание");
                return;
            }
        }
        private void Down(object sender, RoutedEventArgs e) //moves the contract down
        {
            var selection = reportListView.SelectedIndex;

            if (selection + 1 < reportListView.Items.Count && selection != -1)
            {
                var itemToMoveReportListView = reportListView.Items[selection];
                var itemToMoveSLAListView = slaListView.Items[selection];
                var selectedReport = reportListView.SelectedItem as Report;

                reportListView.Items.Remove(itemToMoveReportListView);
                slaListView.Items.Remove(itemToMoveSLAListView);
                _calc.DropReport(selectedReport);

                reportListView.Items.Insert(selection + 1, itemToMoveReportListView);
                slaListView.Items.Insert(selection + 1, itemToMoveSLAListView);
                _calc.InsertReport(selection + 1, selectedReport);

                reportListView.SelectedIndex = selection + 1;

                _calc.SaveData();

                return;
            }
            else if (selection == reportListView.Items.Count - 1)
            {
                MessageBox.Show("Контракт в конце списка.", "Внимание");
                return;
            }
            else
            {
                MessageBox.Show("Не выбран контракт для перемещения вниз.", "Внимание");
                return;
            }
        }
        private void Cross(object sender, RoutedEventArgs e) //drops the contract
        {
            var selection = reportListView.SelectedIndex;

            if (selection != -1)
            {
                var itemToRemoveReportListView = reportListView.Items[selection];
                var itemToRemoveSLAListView = slaListView.Items[selection];
                var selectedReport = reportListView.SelectedItem as Report;

                reportListView.Items.Remove(itemToRemoveReportListView);
                slaListView.Items.Remove(itemToRemoveSLAListView);
                _calc.DropReport(selectedReport);
                _calc.DropGraph(selectedReport);
                _calc.DropUnitedReport(selectedReport);

                //rebuild graph
                _calc.BuildGraph();

                graphSLA.Series = _calc.SeriesCollection;
                graphSLA.AxisX[0].Labels = _calc.Labels;
                graphSLA.AxisY[1].LabelFormatter = _calc.Formatter;

                _calc.SaveData();
            }
            else
                MessageBox.Show("Не выбран контракт для удаления.", "Внимание");
        }
        private void Expand(object sender, RoutedEventArgs e) //expands the Requests Tab
        {
            ResultsWindow rw = new ResultsWindow
            {
                Owner = this
            };
            rw.Show();
        }
        private void FilterContracts(object sender, TextChangedEventArgs e) //filters the contracts
        {
            if (txtFilter.Text != "")
            {
                droppedContracts.Clear();
                deletedContractsListView.Items.Clear();

                for (int i = 0; i < deletedContracts.Count; i++)
                    deletedContractsListView.Items.Add(deletedContracts[i]);

                foreach (string item in deletedContractsListView.Items)
                {
                    if (!item.ToLower().Contains(txtFilter.Text.ToLower()))
                        droppedContracts.Add(item);
                }

                foreach (string item in droppedContracts)
                    deletedContractsListView.Items.Remove(item);
            }
            else
            {
                droppedContracts.Clear();
                deletedContractsListView.Items.Clear();

                for (int i = 0; i < deletedContracts.Count; i++)
                    deletedContractsListView.Items.Add(deletedContracts[i]);
            }
        }
        private void CleanFilter(object sender, RoutedEventArgs e) //cleans filter
        {
            txtFilter.Clear();
            txtFilter.Focus();
        }
        private void ShowContractInfo(object sender, RoutedEventArgs e) //shows selected contract's info
        {
            List<Report> content;
            var selection = reportListView.SelectedIndex;

            if (selection != -1)
            {
                var selectedReport = reportListView.SelectedItem as Report;
                content = _calc.CheckDic(selectedReport);

                if (content.Count == 0)
                    return;

                var window = new ContractInfoWindow
                {
                    DataContext = content,
                    Owner = this,
                    Title = "Информация по контракту: " + selectedReport.ContractName
                };
                window.Show();
            }
            else
                MessageBox.Show("Не выбран контракт для вывода информации.", "Внимание");
        }

        //building methods
        private void BuildUI()
        {
            List<Report> reportList = _calc.CollectReports();

            for (int i = 0; i < reportList.Count; i++)
            {
                reportListView.Items.Add(reportList[i]);
                slaListView.Items.Add(reportList[i]);
            }

            _calc.BuildGraph();
            graphSLA.Series = _calc.SeriesCollection;
            graphSLA.AxisX[0].Labels = _calc.Labels;
            graphSLA.AxisY[1].LabelFormatter = _calc.Formatter;

            List<CI> ciList = _calc.CollectCI();

            for (int i = 0; i < ciList.Count; i++)
            {
                crisisList.Add(ciList[i]);
                crisisListView.Items.Add(ciList[i]);
            }
        }
        private void CountReports() //count reports
        {
            foreach (object contract in contractsList)
                _calc.ResultBuilder(dbDataMonth, dbDataQuarter, dbDataYear, contract.ToString());
        }
        private void CountRequests()
        {
            _calc.RequestsBuilder(dbDataMonth, "Month");
            _calc.RequestsBuilder(dbDataQuarter, "Quarter");
            _calc.RequestsBuilder(dbDataYear, "Year");
        }
        private void BuildGraph() //build graph
        {
            foreach (object contract in contractsList)
                _calc.GraphBuilder(l1, l2, l3, l4, l5, l6, l7, l8, l9, l10, l11, l12, l13, l14, l15, contract.ToString());
        }
        private void MakeContracts(List<object> dbData) //parse db data to contracts list
        {
            contractsList.Clear();
            contractsListView.Items.Clear();
            deletedContracts.Clear();
            deletedContractsListView.Items.Clear();

            for (int i = 0; i < dbData.Count; i++)
            {
                object[] item = (object[])dbData[i];

                if (item[0].ToString() != "")
                    deletedContracts.Add(item[0]);
            }

            for (int i = 0; i < deletedContracts.Count; i++)
                deletedContractsListView.Items.Add(deletedContracts[i]);

            dbData.Clear();
        }

        //programm settings
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
            CultureInfo.CurrentCulture = new CultureInfo("en-US"); //change localization

            //change here tables (left join tables with data)
            string table = "dbo.Sd_ServiceDeskRequests r (nolock)\r\n\r\nleft join Sd_ServiceDeskTypes ty (nolock) on r.[Type] = ty.Oid\r\nleft join Sd_ServiceDeskPriorities pr (nolock) on r.ServiceDeskPriority = pr.Oid\r\nleft join Sd_ServiceDeskRequestImpact im (nolock) on r.Impact = im.Oid\r\nleft join Sd_ServiceDeskStatuses st (nolock) on r.Status = st.Oid\r\nleft join Sd_ServiceDeskRequestStatusReasons str (nolock) on r.StatusReason = str.Oid\r\nleft join Sys_Organizations org (nolock) on r.OwnerOrganization = org.Oid\r\nleft join Sd_ServiceDeskServiceContracts sc (nolock) on r.ServiceContract = sc.Oid\r\nleft join CM_ConfigurationItems items (nolock) on r.ServiceAsset = items.Oid\r\nleft join CM_ServiceAssetParameters sp (nolock) on r.ServiceAssetParameter = sp.Oid\r\nleft join Sd_ServiceDeskSolutionCodesRequest scodes (nolock) on r.SolutionCode = scodes.Oid\r\nleft join Rn_RequestInformationSecurityComponent rsc (nolock) on r.Oid = rsc.Request\r\nleft join Rn_InformationSecurityCategoryComponent isc (nolock) on rsc.CategoryComponent = isc.Oid\r\nleft join Rn_InformationSecurityCategory1 cat1 (nolock) on isc.Category1 = cat1.Oid\r\nleft join Sys_Individuals ind (nolock) on r.Owner = ind.Oid\r\nleft join Sd_ServiceDeskRequestGroups gr (nolock) on r.[Group] = gr.Oid\r\nleft join Sys_Individuals indSp (nolock) on r.Specialist = indSp.Oid\r\nleft join Sys_Departments dept (nolock) on indSp.Department = dept.Oid\r\nleft join Sd_ServiceDeskRequestReasons reasons (nolock) on r.Reason = reasons.Oid";

            //change here selection (35 columns)
            string selection = "r.RegNumText,\r\nr.ExternalRegnum,\r\nr.CreateDate,\r\nty.Title TypeTitle,\r\npr.Title PriorityTytle,\r\nim.Title Impact,\r\nr.IsCrisis,\r\nst.Title Status,\r\nstr.Title StatusReason,\r\nr.GetMethod,\r\norg.Title Company,\r\nsc.Title ServiceContractTitle,\r\nr.Deadline,\r\nr.BrokenSLADate,\r\nitems.Title Service,\r\nsp.Name ServiceParametr,\r\nr.Subject,\r\nr.Description,\r\nr.Rating,\r\nr.UserSolutionDate,\r\nr.UsersSolution,\r\nr.ClosedDate,\r\nscodes.Title SilutionCodeTitle,\r\ncat1.Title IBCat1,\r\nind.FullName Owner,\r\ndept.Title Department,\r\ngr.Title [Group],\r\nindSp.FullName Specialist,\r\nr.TargetResponseTime,\r\nr.ActualResponseTime,\r\nr.ReassignedGroupNumber,\r\nr.ReClassificationNumber,\r\nr.ReturnedToProcessNumber,\r\nreasons.Title Reason,\r\nr.TechnicalBrokenSLA";

            //contract list
            contractsRequest = @"SELECT DISTINCT sc.Title ServiceContractTitle FROM " + table;

            //contract details
            monthRequest = @"SELECT " + selection + " FROM " + table + " WHERE CAST(r.[CreateDate] AS date) >= '" + MonthDate + "'";
            quarterRequest = @"SELECT " + selection + " FROM " + table + " WHERE CAST(r.[CreateDate] AS date) >= '" + QuarterSDate + "' AND CAST(r.[CreateDate] AS date) <= '" + QuaterFDate + "'";
            yearRequest = @"SELECT " + selection + " FROM " + table + " WHERE CAST(r.[CreateDate] AS date) >= '" + yearDate + "'";

            // graph details
            r1 = @"SELECT " + selection + " FROM " + table + " WHERE CAST(r.[CreateDate] AS date) >= '" + d1 + "' AND CAST(r.[CreateDate] AS date) < '" + d2 + "'";
            r2 = @"SELECT " + selection + " FROM " + table + " WHERE CAST(r.[CreateDate] AS date) >= '" + d2 + "' AND CAST(r.[CreateDate] AS date) < '" + d3 + "'";
            r3 = @"SELECT " + selection + " FROM " + table + " WHERE CAST(r.[CreateDate] AS date) >= '" + d3 + "' AND CAST(r.[CreateDate] AS date) < '" + d4 + "'";
            r4 = @"SELECT " + selection + " FROM " + table + " WHERE CAST(r.[CreateDate] AS date) >= '" + d4 + "' AND CAST(r.[CreateDate] AS date) < '" + d5 + "'";
            r5 = @"SELECT " + selection + " FROM " + table + " WHERE CAST(r.[CreateDate] AS date) >= '" + d5 + "' AND CAST(r.[CreateDate] AS date) < '" + d6 + "'";
            r6 = @"SELECT " + selection + " FROM " + table + " WHERE CAST(r.[CreateDate] AS date) >= '" + d6 + "' AND CAST(r.[CreateDate] AS date) < '" + d7 + "'";
            r7 = @"SELECT " + selection + " FROM " + table + " WHERE CAST(r.[CreateDate] AS date) >= '" + d7 + "' AND CAST(r.[CreateDate] AS date) < '" + d8 + "'";
            r8 = @"SELECT " + selection + " FROM " + table + " WHERE CAST(r.[CreateDate] AS date) >= '" + d8 + "' AND CAST(r.[CreateDate] AS date) < '" + d9 + "'";
            r9 = @"SELECT " + selection + " FROM " + table + " WHERE CAST(r.[CreateDate] AS date) >= '" + d9 + "' AND CAST(r.[CreateDate] AS date) < '" + d10 + "'";
            r10 = @"SELECT " + selection + " FROM " + table + " WHERE CAST(r.[CreateDate] AS date) >= '" + d10 + "' AND CAST(r.[CreateDate] AS date) < '" + d11 + "'";
            r11 = @"SELECT " + selection + " FROM " + table + " WHERE CAST(r.[CreateDate] AS date) >= '" + d11 + "' AND CAST(r.[CreateDate] AS date) < '" + d12 + "'";
            r12 = @"SELECT " + selection + " FROM " + table + " WHERE CAST(r.[CreateDate] AS date) >= '" + d12 + "' AND CAST(r.[CreateDate] AS date) < '" + d13 + "'";
            r13 = @"SELECT " + selection + " FROM " + table + " WHERE CAST(r.[CreateDate] AS date) >= '" + d13 + "' AND CAST(r.[CreateDate] AS date) < '" + d14 + "'";
            r14 = @"SELECT " + selection + " FROM " + table + " WHERE CAST(r.[CreateDate] AS date) >= '" + d14 + "' AND CAST(r.[CreateDate] AS date) < '" + d15 + "'";
            r15 = @"SELECT " + selection + " FROM " + table + " WHERE CAST(r.[CreateDate] AS date) >= '" + d15 + "'";
        }

        //cleaner
        private void Cleaning() //erase data
        {
            reportListView.Items.Clear(); //clear list views
            slaListView.Items.Clear();
            crisisListView.Items.Clear();

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