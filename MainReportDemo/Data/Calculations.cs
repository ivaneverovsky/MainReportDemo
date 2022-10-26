using LiveCharts;
using LiveCharts.Wpf;
using MainReportDemo.UIModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MainReportDemo.Data
{
    internal class Calculations
    {
        Storage _stor = new Storage();

        //contract fields
        private string color;
        private string colorYear;

        private int critical;
        private int criticalYear;

        private int reportAmount;
        private int reportQuarter;
        private int reportAmountYear;

        private double targetSla;
        private double slaMonth;
        private string slaState;
        private double slaValue;
        private double slaQuarter;
        private string slaQuarterState;
        private double slaQuarterValue;
        private double slaYear;
        private string slaYearState;
        private double slaYearValue;
        private int SLABreakCounter;
        private int SLABreakCounterQuarter;
        private int SLABreakCounterYear;

        private int requestsAccess;
        private double accessPerc;
        private int requestsChange;
        private double changePerc;
        private int requestsUsage;
        private double usagePerc;
        private int requestsAdvice;
        private double advicePerc;
        private int plannedWork;
        private double plannedWorkPerc;

        private int incidents;
        private double incidentsPerc;
        private int incidentsIS;
        private double incidentsISPerc;

        private int five;
        private int four;
        private int three;
        private int two;
        private int noMark;
        private int restart;

        private double fivePerc;
        private double fourPerc;
        private double threePerc;
        private double twoPerc;
        private double noMarkPerc;
        private double restartPerc;

        //graph fields
        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> Formatter { get; set; } = value => value.ToString() + "%";
        public List<string> Labels = new List<string>();

        private int report1;
        private int report2;
        private int report3;
        private int report4;
        private int report5;
        private int report6;
        private int report7;
        private int report8;
        private int report9;
        private int report10;
        private int report11;
        private int report12;
        private int report13;
        private int report14;
        private int report15;

        private int sla1;
        private int sla2;
        private int sla3;
        private int sla4;
        private int sla5;
        private int sla6;
        private int sla7;
        private int sla8;
        private int sla9;
        private int sla10;
        private int sla11;
        private int sla12;
        private int sla13;
        private int sla14;
        private int sla15;

        //count contracts details for Report
        public void ResultBuilder(List<object> dbDataMonth, List<object> dbDataQuarter, List<object> dbDataYear, string contract)
        {
            ContractResetValues();

            //count values
            Task[] tasks = new Task[3]
            {
                new Task(() => MonthCounter(dbDataMonth, contract)),
                new Task(() => QuarterCounter(dbDataQuarter, contract)),
                new Task(() => YearCounter(dbDataYear, contract))
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

            CountSLA();

            var report = new Report(color, colorYear, contract, reportAmount, reportQuarter, reportAmountYear, critical, criticalYear, targetSla, slaMonth, slaState, slaValue, slaQuarter, slaQuarterState, slaQuarterValue, slaYear, slaYearState, slaYearValue, SLABreakCounter, SLABreakCounterQuarter, SLABreakCounterYear, requestsAccess, accessPerc, requestsChange, changePerc, requestsUsage, usagePerc, incidents, incidentsPerc, incidentsIS, incidentsISPerc, requestsAdvice, advicePerc, plannedWork, plannedWorkPerc, five, fivePerc, four, fourPerc, three, threePerc, two, twoPerc, noMark, noMarkPerc, restart, restartPerc);

            _stor.AddReport(report);
        }

        //graph builder
        public void GraphBuilder(List<object> l1, List<object> l2, List<object> l3, List<object> l4, List<object> l5, List<object> l6, List<object> l7,
            List<object> l8, List<object> l9, List<object> l10, List<object> l11, List<object> l12, List<object> l13, List<object> l14, List<object> l15,
            string contract)
        {
            Task[] tasks = new Task[15]
            {
                new Task(() => rs1(l1, contract)),
                new Task(() => rs2(l2, contract)),
                new Task(() => rs3(l3, contract)),
                new Task(() => rs4(l4, contract)),
                new Task(() => rs5(l5, contract)),
                new Task(() => rs6(l6, contract)),
                new Task(() => rs7(l7, contract)),
                new Task(() => rs8(l8, contract)),
                new Task(() => rs9(l9, contract)),
                new Task(() => rs10(l10, contract)),
                new Task(() => rs11(l11, contract)),
                new Task(() => rs12(l12, contract)),
                new Task(() => rs13(l13, contract)),
                new Task(() => rs14(l14, contract)),
                new Task(() => rs15(l15, contract))
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

            var graph = new Graph(contract, report1, report2, report3, report4, report5, report6, report7, report8, report9, report10, report11, report12,
                report13, report14, report15, sla1, sla2, sla3, sla4, sla5, sla6, sla7, sla8, sla9, sla10, sla11, sla12, sla13, sla14, sla15);

            _stor.AddGraph(graph);
        }

        //file builder
        public void FileBuilder(Dictionary<object, object> file)
        {
            foreach (var row in file)
            {
                if (row.Value.ToString() != "")
                {
                    var mark = new CDS(row.Key.ToString(), row.Value.ToString());
                    _stor.AddMark(mark);
                }
            }
        }

        //build new contract
        public Report NewContract(List<Report> reports, string newContractName)
        {
            ContractResetValues();
            for (int i = 0; i < reports.Count; i++)
            {
                reportAmount += reports[i].ReportAmount;
                reportQuarter += reports[i].SLABreakCounterQuarter;
                reportAmountYear += reports[i].ReportAmountYear;
                critical += reports[i].Critical;
                criticalYear += reports[i].CriticalYear;
                targetSla = reports[i].TargetSLA;
                requestsAccess += reports[i].RequestsAccess;
                requestsChange += reports[i].RequestsChange;
                requestsUsage += reports[i].RequestsUsage;
                incidents += reports[i].Incidents;
                incidentsIS += reports[i].IncidentsIS;
                requestsAdvice += reports[i].RequestsAdvice;
                plannedWork += reports[i].PlannedWork;
                five += reports[i].Five;
                four += reports[i].Four;
                three += reports[i].Three;
                two += reports[i].Two;
                noMark += reports[i].NoMark;
                restart += reports[i].Restart;
                SLABreakCounter += reports[i].SLABreakCounter;
                SLABreakCounterQuarter += reports[i].SLABreakCounterQuarter;
                SLABreakCounterYear += reports[i].SLABreakCounterYear;
            }

            CountStats();
            CountSLA();
            CheckCrisis();

            Task[] tasks = new Task[3]
            {
                new Task(() => CountStats()),
                new Task(() => CountSLA()),
                new Task(() => CheckCrisis())
            };

            foreach(Task task in tasks)
                task.Start();

            try
            {
                Task.WaitAll(tasks);
            }
            catch (AggregateException ae)
            {
                MessageBox.Show(ae.Message, "Ошибка");
            }

            var report = new Report(color, colorYear, newContractName, reportAmount, reportQuarter, reportAmountYear, critical, criticalYear, targetSla, slaMonth, slaState, slaValue, slaQuarter, slaQuarterState, slaQuarterValue, slaYear, slaYearState, slaYearValue, SLABreakCounter, SLABreakCounterQuarter, SLABreakCounterYear, requestsAccess, accessPerc, requestsChange, changePerc, requestsUsage, usagePerc, incidents, incidentsPerc, incidentsIS, incidentsISPerc, requestsAdvice, advicePerc, plannedWork, plannedWorkPerc, five, fivePerc, four, fourPerc, three, threePerc, two, twoPerc, noMark, noMarkPerc, restart, restartPerc);

            _stor.AddReport(report);

            return report;
        }

        //count months (1-15)
        private void rs1(List<object> l, string contractName)
        {
            for (int i = 0; i < l.Count; i++)
            {
                object[] value = (object[])l[i];
                if (value[11].ToString() == contractName)
                {
                    report1++;
                    if (value[13].ToString() != "")
                        sla1++;
                }
            }
        }
        private void rs2(List<object> l, string contractName)
        {
            for (int i = 0; i < l.Count; i++)
            {
                object[] value = (object[])l[i];
                if (value[11].ToString() == contractName)
                {
                    report2++;
                    if (value[13].ToString() != "")
                        sla2++;
                }
            }
        }
        private void rs3(List<object> l, string contractName)
        {
            for (int i = 0; i < l.Count; i++)
            {
                object[] value = (object[])l[i];
                if (value[11].ToString() == contractName)
                {
                    report3++;
                    if (value[13].ToString() != "")
                        sla3++;
                }
            }
        }
        private void rs4(List<object> l, string contractName)
        {
            for (int i = 0; i < l.Count; i++)
            {
                object[] value = (object[])l[i];
                if (value[11].ToString() == contractName)
                {
                    report4++;
                    if (value[13].ToString() != "")
                        sla4++;
                }
            }
        }
        private void rs5(List<object> l, string contractName)
        {
            for (int i = 0; i < l.Count; i++)
            {
                object[] value = (object[])l[i];
                if (value[11].ToString() == contractName)
                {
                    report5++;
                    if (value[13].ToString() != "")
                        sla5++;
                }
            }
        }
        private void rs6(List<object> l, string contractName)
        {
            for (int i = 0; i < l.Count; i++)
            {
                object[] value = (object[])l[i];
                if (value[11].ToString() == contractName)
                {
                    report6++;
                    if (value[13].ToString() != "")
                        sla6++;
                }
            }
        }
        private void rs7(List<object> l, string contractName)
        {
            for (int i = 0; i < l.Count; i++)
            {
                object[] value = (object[])l[i];
                if (value[11].ToString() == contractName)
                {
                    report7++;
                    if (value[13].ToString() != "")
                        sla7++;
                }
            }
        }
        private void rs8(List<object> l, string contractName)
        {
            for (int i = 0; i < l.Count; i++)
            {
                object[] value = (object[])l[i];
                if (value[11].ToString() == contractName)
                {
                    report8++;
                    if (value[13].ToString() != "")
                        sla8++;
                }
            }
        }
        private void rs9(List<object> l, string contractName)
        {
            for (int i = 0; i < l.Count; i++)
            {
                object[] value = (object[])l[i];
                if (value[11].ToString() == contractName)
                {
                    report9++;
                    if (value[13].ToString() != "")
                        sla9++;
                }
            }
        }
        private void rs10(List<object> l, string contractName)
        {
            for (int i = 0; i < l.Count; i++)
            {
                object[] value = (object[])l[i];
                if (value[11].ToString() == contractName)
                {
                    report10++;
                    if (value[13].ToString() != "")
                        sla10++;
                }
            }
        }
        private void rs11(List<object> l, string contractName)
        {
            for (int i = 0; i < l.Count; i++)
            {
                object[] value = (object[])l[i];
                if (value[11].ToString() == contractName)
                {
                    report11++;
                    if (value[13].ToString() != "")
                        sla11++;
                }
            }
        }
        private void rs12(List<object> l, string contractName)
        {
            for (int i = 0; i < l.Count; i++)
            {
                object[] value = (object[])l[i];
                if (value[11].ToString() == contractName)
                {
                    report12++;
                    if (value[13].ToString() != "")
                        sla12++;
                }
            }
        }
        private void rs13(List<object> l, string contractName)
        {
            for (int i = 0; i < l.Count; i++)
            {
                object[] value = (object[])l[i];
                if (value[11].ToString() == contractName)
                {
                    report13++;
                    if (value[13].ToString() != "")
                        sla13++;
                }
            }
        }
        private void rs14(List<object> l, string contractName)
        {
            for (int i = 0; i < l.Count; i++)
            {
                object[] value = (object[])l[i];
                if (value[11].ToString() == contractName)
                {
                    report14++;
                    if (value[13].ToString() != "")
                        sla14++;
                }
            }
        }
        private void rs15(List<object> l, string contractName)
        {
            for (int i = 0; i < l.Count; i++)
            {
                object[] value = (object[])l[i];
                if (value[11].ToString() == contractName)
                {
                    report15++;
                    if (value[13].ToString() != "")
                        sla15++;
                }
            }
        }
        
        //count month
        private void MonthCounter(List<object> dbDataMonth, string contractName)
        {
            for (int i = 0; i < dbDataMonth.Count; i++)
            {
                object[] value = (object[])dbDataMonth[i];
                if (value[11].ToString() == contractName)
                {
                    reportAmount++;

                    if (value[3].ToString() == "Запрос на обслуживание")
                        requestsUsage++;
                    else if (value[3].ToString() == "Консультация")
                        requestsAdvice++;
                    else if (value[3].ToString() == "Инцидент")
                        incidents++;
                    else if (value[3].ToString() == "Регламентная работа")
                        plannedWork++;
                    else if (value[3].ToString() == "Инцидент ИБ")
                        incidentsIS++;
                    else if (value[3].ToString() == "Запрос на изменение")
                        requestsChange++;
                    else if (value[3].ToString() == "Запрос на доступ")
                        requestsAccess++;

                    if (value[6].ToString() == "True")
                    {   //create crisis incident
                        var ci = new CI(value[0].ToString(), contractName, value[7].ToString(), value[14].ToString(), value[17].ToString(), "Actual");
                        _stor.AddCrisis(ci);
                    }

                    if (value[13].ToString() != "")
                        SLABreakCounter++;

                    if (value[18].ToString() == "0")
                        noMark++;
                    else if (value[18].ToString() == "5")
                        five++;
                    else if (value[18].ToString() == "4")
                        four++;
                    else if (value[18].ToString() == "3")
                        three++;
                    else if (value[18].ToString() == "2")
                        two++;

                    if (value[32].ToString() != "" && value[32].ToString() != "0")
                        restart++;
                }
            }
            CountStats();
        }

        //count quarter
        private void QuarterCounter(List<object> dbDataQuarter, string contractName)
        {
            for (int i = 0; i < dbDataQuarter.Count; i++)
            {
                object[] value = (object[])dbDataQuarter[i];

                if (value[11].ToString() == contractName)
                {
                    reportQuarter++;

                    if (value[13].ToString() != "")
                        SLABreakCounterQuarter++;
                }
            }
        }

        //count year
        private void YearCounter(List<object> dbDataYear, string contractName)
        {
            for (int i = 0; i < dbDataYear.Count; i++)
            {
                object[] value = (object[])dbDataYear[i];

                if (value[11].ToString() == contractName)
                {
                    reportAmountYear++;

                    if (value[6].ToString() == "True")
                    {   //create crisis incident
                        var ci = new CI(value[0].ToString(), contractName, value[7].ToString(), value[14].ToString(), value[17].ToString(), "Old");
                        _stor.AddCrisis(ci);
                    }

                    if (value[13].ToString() != "")
                        SLABreakCounterYear++;
                }
            }
        }

        //count crisis
        public void CrisisCounter(string period, string contractName)
        {
            if (period == "Old")
            {
                for (int i = 0; i < _stor.Reports.Count; i++)
                {
                    if (_stor.Reports[i].ContractName == contractName)
                    {
                        _stor.Reports[i].CriticalYear++;
                        if (_stor.Reports[i].CriticalYear <= 1)
                            _stor.Reports[i].ColorYear = "Yellow";
                        else if (_stor.Reports[i].CriticalYear >= 2)
                            _stor.Reports[i].ColorYear = "Red";

                        break;
                    }
                }
            }
            else if (period == "Actual")
            {
                for (int i = 0; i < _stor.Reports.Count; i++)
                {
                    if (_stor.Reports[i].ContractName == contractName)
                    {
                        _stor.Reports[i].Critical++;
                        if (_stor.Reports[i].Critical <= 1)
                            _stor.Reports[i].Color = "Yellow";
                        else if (_stor.Reports[i].Critical >= 2)
                            _stor.Reports[i].Color = "Red";

                        break;
                    }
                }
            }
        }

        //statistic counter
        private void CountStats()
        {
            double sum = noMark + five + four + three + two;
            if (sum == 0)
            {
                fivePerc = 0;
                fourPerc = 0;
                threePerc = 0;
                twoPerc = 0;
                noMarkPerc = 0;
            }
            else
            {
                fivePerc = Math.Round(five / sum * 100, 1);
                fourPerc = Math.Round(four / sum * 100, 1);
                threePerc = Math.Round(three / sum * 100, 1);
                twoPerc = Math.Round(two / sum * 100, 1);
                noMarkPerc = Math.Round(noMark / sum * 100, 1);
            }

            restartPerc = Math.Round(restart / (double)reportAmount * 100, 1);
            if (restartPerc.ToString() == "NaN")
                restartPerc = 0;

            double sumRequests = requestsAccess + requestsChange + requestsUsage + requestsAdvice + plannedWork + incidents + incidentsIS;
            if (sumRequests == 0)
            {
                accessPerc = 0;
                changePerc = 0;
                usagePerc = 0;
                advicePerc = 0;
                plannedWorkPerc = 0;
                incidentsPerc = 0;
                incidentsISPerc = 0;
            }
            else
            {
                accessPerc = Math.Round(requestsAccess / sumRequests * 100, 1);
                changePerc = Math.Round(requestsChange / sumRequests * 100, 1);
                usagePerc = Math.Round(requestsUsage / sumRequests * 100, 1);
                advicePerc = Math.Round(requestsAdvice / sumRequests * 100, 1);
                plannedWorkPerc = Math.Round(plannedWork / sumRequests * 100, 1);
                incidentsPerc = Math.Round(incidents / sumRequests * 100, 1);
                incidentsISPerc = Math.Round(incidentsIS / sumRequests * 100, 1);
            }
        }

        //sla counter
        private void CountSLA()
        {
            targetSla = 90.00;

            //month
            slaMonth = Math.Round((1 - SLABreakCounter / (double)reportAmount) * 100, 2);
            if (slaMonth.ToString() == "NaN" && SLABreakCounter == 0 || SLABreakCounter == reportAmount)
                slaMonth = 100;

            //table month
            slaValue = Math.Round(100 - slaMonth, 2);
            if (slaMonth <= 100 && slaMonth >= 97.12)
                slaState = "Green";
            else if (slaMonth < 97.12 && slaMonth >= targetSla)
                slaState = "Yellow";
            else
                slaState = "Red";

            //quarter
            slaQuarter = Math.Round((1 - SLABreakCounterQuarter / (double)reportQuarter) * 100, 2);
            if (slaQuarter.ToString() == "NaN" && SLABreakCounterQuarter == 0 || SLABreakCounterQuarter == reportQuarter) //SLA = count in cases when unite contacts
                slaQuarter = 100;

            //table quarter
            slaQuarterValue = Math.Round(100 - slaQuarter, 2);
            if (slaQuarter <= 100 && slaQuarter >= 97.12)
                slaQuarterState = "Green";
            else if (slaQuarter < 97.12 && slaQuarter >= targetSla)
                slaQuarterState = "Yellow";
            else
                slaQuarterState = "Red";

            //year
            slaYear = Math.Round((1 - SLABreakCounterYear / (double)reportAmountYear) * 100, 2);
            if (slaYear.ToString() == "NaN" && SLABreakCounterYear == 0 || SLABreakCounterYear == reportAmountYear)
                slaYear = 100;

            //table year
            slaYearValue = Math.Round(100 - slaYear, 2);
            if (slaYear <= 100 && slaYear >= 97.12)
                slaYearState = "Green";
            else if (slaYear < 97.12 && slaYear >= targetSla)
                slaYearState = "Yellow";
            else
                slaYearState = "Red";
        }

        //check crisis
        private void CheckCrisis()
        {
            if (critical == 1)
                color = "Yellow";
            else if (critical >= 2)
                color = "Red";

            if (criticalYear == 1)
                colorYear = "Yellow";
            else if (criticalYear >= 2)
                colorYear = "Red";
        }

        //check db values (Marks) for dbList
        public List<object> dbCheck(List<object> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                object[] value = (object[])list[i];
                if (value[1].ToString() != "")
                    for (int j = 0; j < _stor.Marks.Count; j++)
                        if (_stor.Marks[j].RegNum == value[1].ToString())
                        {
                            value[18] = _stor.Marks[j].Mark;
                            list.RemoveAt(i);
                            list.Add(value);
                        }
            }
            return list;
        }

        //build graph
        public void BuildGraph()
        {
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Поступило обращений",
                    Values = new ChartValues<int> { report1, report2, report3, report4, report5, report6, report7, report8, report9, report10, report11,
                    report12, report13, report14, report15},
                    Fill = Brushes.CornflowerBlue,
                },

                new LineSeries
                {
                    Title = "% обращений с нарушенным SLA",
                    Values = new ChartValues<double> { Math.Round(sla1 / (double)report1 * 100, 2), Math.Round(sla2 / (double)report2 * 100, 2),
                        Math.Round(sla3 / (double)report3 * 100, 2), Math.Round(sla4 / (double)report4 * 100, 2), Math.Round(sla5 / (double)report5 * 100, 2),
                        Math.Round(sla6 / (double)report6 * 100, 2), Math.Round(sla7 / (double)report7 * 100, 2), Math.Round(sla8 / (double)report8 * 100, 2),
                        Math.Round(sla9 / (double)report9 * 100, 2), Math.Round(sla10 / (double)report10 * 100, 2), Math.Round(sla11 / (double)report11 * 100, 2),
                        Math.Round(sla12 / (double)report12 * 100, 2), Math.Round(sla13 / (double)report13 * 100, 2), Math.Round(sla14 / (double)report14 * 100, 2),
                        Math.Round(sla15 / (double)report15 * 100, 2)},
                    Fill = Brushes.Transparent,
                    StrokeThickness = 1,
                    LineSmoothness = 0,
                    Stroke = Brushes.Yellow,
                    ScalesYAt = 1
                }
            };
        }

        //last graph state
        public void GraphLastState(Graph last)
        {
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Поступило обращений",
                    Values = new ChartValues<int> { last.Report1, last.Report2, last.Report3, last.Report4, last.Report5, last.Report6, last.Report7,
                        last.Report8, last.Report9, last.Report10, last.Report11, last.Report12, last.Report13, last.Report14, last.Report15},
                    Fill = Brushes.CornflowerBlue,
                },

                new LineSeries
                {
                    Title = "% обращений с нарушенным SLA",
                    Values = new ChartValues<double> { Math.Round(last.SLA1 / (double)last.Report1 * 100, 2), Math.Round(last.SLA2 / (double)last.Report2 * 100, 2),
                        Math.Round(last.SLA3 / (double)last.Report3 * 100, 2), Math.Round(last.SLA4 / (double)last.Report4 * 100, 2),
                        Math.Round(last.SLA5 / (double)last.Report5 * 100, 2), Math.Round(last.SLA6 / (double)last.Report6 * 100, 2),
                        Math.Round(last.SLA7 / (double)last.Report7 * 100, 2), Math.Round(last.SLA8 / (double)last.Report8 * 100, 2),
                        Math.Round(last.SLA9 / (double)last.Report9 * 100, 2), Math.Round(last.SLA10 / (double)last.Report10 * 100, 2),
                        Math.Round(last.SLA11 / (double)last.Report11 * 100, 2), Math.Round(last.SLA12 / (double)last.Report12 * 100, 2),
                        Math.Round(last.SLA13 / (double)last.Report13 * 100, 2), Math.Round(last.SLA14 / (double)last.Report14 * 100, 2),
                        Math.Round(last.SLA15 / (double)last.Report15 * 100, 2)},
                    Fill = Brushes.Transparent,
                    StrokeThickness = 1,
                    LineSmoothness = 0,
                    Stroke = Brushes.Yellow,
                    ScalesYAt = 1
                }
            };
        }

        //collect reports from storage
        public List<Report> CollectReports()
        {
            return _stor.Reports;
        }

        //collect graph data from storage
        public List<Graph> CollectGraph()
        {
            return _stor.GraphData;
        }

        //collect crisis incidents from storage
        public List<CI> CollectCI()
        {
            return _stor.CrisisData;
        }

        //reset contract counters
        private void ContractResetValues()
        {
            color = "Green";
            colorYear = "Green";

            critical = 0;
            criticalYear = 0;

            reportAmount = 0;
            reportQuarter = 0;
            reportAmountYear = 0;

            targetSla = 0;
            slaMonth = 0;
            slaState = "Green";
            slaValue = 0;
            slaQuarter = 0;
            slaQuarterState = "Green";
            slaQuarterValue = 0;
            slaYear = 0;
            slaYearState = "Green";
            slaYearValue = 0;
            SLABreakCounter = 0;
            SLABreakCounterQuarter = 0;
            SLABreakCounterYear = 0;

            requestsAccess = 0;
            accessPerc = 0;
            requestsChange = 0;
            changePerc = 0;
            requestsUsage = 0;
            usagePerc = 0;
            requestsAdvice = 0;
            advicePerc = 0;
            plannedWork = 0;
            plannedWorkPerc = 0;

            incidents = 0;
            incidentsPerc = 0;
            incidentsIS = 0;
            incidentsISPerc = 0;

            five = 0;
            four = 0;
            three = 0;
            two = 0;
            noMark = 0;
            restart = 0;

            fivePerc = 0;
            fourPerc = 0;
            threePerc = 0;
            twoPerc = 0;
            noMarkPerc = 0;
            restartPerc = 0;
        }

        //reset graph counters
        private void GraphResetValues()
        {
            report1 = 0;
            report2 = 0;
            report3 = 0;
            report4 = 0;
            report5 = 0;
            report6 = 0;
            report6 = 0;
            report7 = 0;
            report8 = 0;
            report9 = 0;
            report10 = 0;
            report11 = 0;
            report12 = 0;
            report13 = 0;
            report14 = 0;
            report15 = 0;

            sla1 = 0;
            sla2 = 0;
            sla3 = 0;
            sla4 = 0;
            sla5 = 0;
            sla6 = 0;
            sla7 = 0;
            sla8 = 0;
            sla9 = 0;
            sla10 = 0;
            sla11 = 0;
            sla12 = 0;
            sla13 = 0;
            sla14 = 0;
            sla15 = 0;
        }

        //delete CI if count
        public void DropCI(CI ci)
        {
            _stor.DropCrisis(ci);
        }

        //delete Report
        public void DropReport(Report report)
        {
            _stor.DropReport(report);
        }

        //clear lists
        public void ClearData()
        {
            GraphResetValues();
            _stor.ClearLists();
        }

        //save program state
        public void SaveData()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream("data.dat", FileMode.Create, FileAccess.Write);
            formatter.Serialize(fs, _stor);
            fs.Close();
        }

        //load program state
        public void LoadData()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream("data.dat", FileMode.Open, FileAccess.Read);
            _stor = (Storage)formatter.Deserialize(fs);
            fs.Close();
        }
    }
}
