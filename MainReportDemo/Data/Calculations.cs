using LiveCharts.Wpf;
using LiveCharts;
using MainReportDemo.UIModels;
using System;
using System.Collections.Generic;
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

        private double slaMonth;
        private double slaQuarter;
        private double slaYear;
        private int SLABreakCounter;
        private int SLABreakCounterQuarter;
        private int SLABreakCounterYear;

        private int requestsAccess;
        private int requestsChange;
        private int requestsUsage;
        private int requestsAdvice;
        private int planedWork;

        private int incidents;
        private int incidentsIS;

        private double five;
        private double four;
        private double three;
        private double two;
        private double noMark;
        private double restart;

        //graph fields
        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> Formatter { get; set; } = value => value.ToString() + "%";
        public List<string> Labels = new List<string> { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь", "Октябрь", "Ноябрь", "Декабрь" };
        
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

            var report = new Report(color, colorYear, contract, reportAmount, reportAmountYear, critical, criticalYear, slaMonth, slaQuarter, slaYear, requestsAccess,
                requestsChange, requestsUsage, incidents, incidentsIS, requestsAdvice, planedWork, five, four, three, two, noMark, restart);

            _stor.AddReport(report);
        }

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

        //count month 1
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

        //count month 2
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

        //count month 3
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

        //count month 4
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

        //count month 5
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

        //count month 6
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

        //count month 7
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

        //count month 8
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

        //count month 9
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

        //count month 10
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

        //count month 11
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

        //count month 12
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

        //count month 13
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

        //count month 14
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

        //count month 15
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
                        planedWork++;
                    else if (value[3].ToString() == "Инцидент ИБ")
                        incidentsIS++;
                    else if (value[3].ToString() == "Запрос на изменение")
                        requestsChange++;
                    else if (value[3].ToString() == "Запрос на доступ")
                        requestsAccess++;

                    if (value[6].ToString() == "True")
                        critical++;

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

            if (critical >= 2)
                color = "Red";
            else if (critical == 1)
                color = "Yellow";

            slaMonth = Math.Round((1 - SLABreakCounter / (double)reportAmount) * 100, 2);
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

            slaQuarter = Math.Round((1 - SLABreakCounterQuarter / (double)reportQuarter) * 100, 2);
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
                        criticalYear++;

                    if (value[13].ToString() != "")
                        SLABreakCounterYear++;
                }
            }

            if (criticalYear >= 2)
                colorYear = "Red";
            else if (criticalYear == 1)
                colorYear = "Yellow";

            slaYear = Math.Round((1 - SLABreakCounterYear / (double)reportAmountYear) * 100, 2);
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
                    Fill = Brushes.Gray,
                },

                new LineSeries
                {
                    Title = "% обращений с нарушенным SLA",
                    Values = new ChartValues<double> { Math.Round((sla1 / (double)report1) * 100, 2), Math.Round((sla2 / (double)report2) * 100, 2),
                        Math.Round((sla3 / (double)report3) * 100, 2), Math.Round((sla4 / (double)report4) * 100, 2), Math.Round((sla5 / (double)report5) * 100, 2),
                        Math.Round((sla6 / (double)report6) * 100, 2), Math.Round((sla7 / (double)report7) * 100, 2), Math.Round((sla8 / (double)report8) * 100, 2),
                        Math.Round((sla9 / (double)report9) * 100, 2), Math.Round((sla10 / (double)report10) * 100, 2), Math.Round((sla11 / (double)report11) * 100, 2),
                        Math.Round((sla12 / (double)report12) * 100, 2), Math.Round((sla13 / (double)report13) * 100, 2), Math.Round((sla14 / (double)report14) * 100, 2),
                        Math.Round((sla15 / (double)report15) * 100, 2)},
                    Fill = Brushes.Transparent,
                    StrokeThickness = 1,
                    LineSmoothness = 0,
                    Stroke = Brushes.DarkBlue,
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

            slaMonth = 0;
            slaQuarter = 0;
            slaYear = 0;
            SLABreakCounter = 0;
            SLABreakCounterQuarter = 0;
            SLABreakCounterYear = 0;

            requestsAccess = 0;
            requestsChange = 0;
            requestsUsage = 0;
            requestsAdvice = 0;
            planedWork = 0;

            incidents = 0;
            incidentsIS = 0;

            five = 0;
            four = 0;
            three = 0;
            two = 0;
            noMark = 0;
            restart = 0;
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

        //clear lists
        public void ClearData()
        {
            GraphResetValues();
            _stor.ClearLists();
        }
    }
}
