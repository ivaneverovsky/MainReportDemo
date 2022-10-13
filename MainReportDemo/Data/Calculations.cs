using MainReportDemo.UIModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace MainReportDemo.Data
{
    internal class Calculations
    {
        Storage _stor = new Storage();

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

        //count contracts details for Report
        public void ResultBuilder(List<object> dbDataMonth, List<object> dbDataQuarter, List<object> dbDataYear, string contract)
        {
            ResetValues();

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

        //collect reports from storage
        public List<Report> CollectReports()
        {
            return _stor.Reports;
        }

        //reset counters
        private void ResetValues()
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

        //clear lists
        public void ClearData()
        {
            _stor.ClearLists();
        }
    }
}
