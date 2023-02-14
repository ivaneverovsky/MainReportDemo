using LiveCharts;
using LiveCharts.Wpf;
using MainReportDemo.UIModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

            MonthCounter(dbDataMonth, contract);
            QuarterCounter(dbDataQuarter, contract);
            YearCounter(dbDataYear, contract);
            CountSLA();

            var report = new Report(color, colorYear, contract, reportAmount, reportQuarter, reportAmountYear, critical, criticalYear, targetSla, slaMonth, slaState, slaValue, slaQuarter, slaQuarterState, slaQuarterValue, slaYear, slaYearState, slaYearValue, SLABreakCounter, SLABreakCounterQuarter, SLABreakCounterYear, requestsAccess, accessPerc, requestsChange, changePerc, requestsUsage, usagePerc, incidents, incidentsPerc, incidentsIS, incidentsISPerc, requestsAdvice, advicePerc, plannedWork, plannedWorkPerc, five, fivePerc, four, fourPerc, three, threePerc, two, twoPerc, noMark, noMarkPerc, restart, restartPerc);

            _stor.AddReport(report);
        }
        //requests builder
        public void RequestsBuilder(List<object> dbDataValues, string dateType)
        {
            int counter = dbDataValues.Count;
            int closed = 0;
            int crisis = 0;
            int brokenSLA = 0;

            for (int i = 0; i < dbDataValues.Count; i++)
            {
                object[] value = (object[])dbDataValues[i];

                if (value[7].ToString() == "Закрыто")
                    closed++;

                if (value[6].ToString() == "True")
                    crisis++;

                if (value[13].ToString() != "")
                    brokenSLA++;
            }

            double SLA = Math.Round((1 - brokenSLA / (double)counter) * 100, 2);

            var request = new Requests(counter, closed, SLA, crisis, dateType);
            _stor.AddRequest(request);
        }
        //graph builder
        public void GraphBuilder(List<object> l1, List<object> l2, List<object> l3, List<object> l4, List<object> l5, List<object> l6, List<object> l7,
            List<object> l8, List<object> l9, List<object> l10, List<object> l11, List<object> l12, List<object> l13, List<object> l14, List<object> l15,
            string contract)
        {
            rs1(l1, contract);
            rs2(l2, contract);
            rs3(l3, contract);
            rs4(l4, contract);
            rs5(l5, contract);
            rs6(l6, contract);
            rs7(l7, contract);
            rs8(l8, contract);
            rs9(l9, contract);
            rs10(l10, contract);
            rs11(l11, contract);
            rs12(l12, contract);
            rs13(l13, contract);
            rs14(l14, contract);
            rs15(l15, contract);

            var graph = new Graph(contract, report1, report2, report3, report4, report5, report6, report7, report8, report9, report10, report11, report12,
                report13, report14, report15, sla1, sla2, sla3, sla4, sla5, sla6, sla7, sla8, sla9, sla10, sla11, sla12, sla13, sla14, sla15);

            _stor.AddGraph(graph);

            GraphResetValues();
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
                reportQuarter += reports[i].ReportAmountQuarter;
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

                //store united contracts with new contract name
                _stor.AddUnitedContract(reports[i], newContractName);
            }

            CountStats();
            CountSLA();
            CheckCrisis();

            var report = new Report(color, colorYear, newContractName, reportAmount, reportQuarter, reportAmountYear, critical, criticalYear, targetSla, slaMonth, slaState, slaValue, slaQuarter, slaQuarterState, slaQuarterValue, slaYear, slaYearState, slaYearValue, SLABreakCounter, SLABreakCounterQuarter, SLABreakCounterYear, requestsAccess, accessPerc, requestsChange, changePerc, requestsUsage, usagePerc, incidents, incidentsPerc, incidentsIS, incidentsISPerc, requestsAdvice, advicePerc, plannedWork, plannedWorkPerc, five, fivePerc, four, fourPerc, three, threePerc, two, twoPerc, noMark, noMarkPerc, restart, restartPerc);

            _stor.AddReport(report);

            return report;
        }
        public bool CheckContractName(string nameForCheck)
        {
            if (_stor.UnitedContracts.ContainsValue(nameForCheck))
                return true;

            foreach (var item in _stor.Reports)
                if (item.ContractName == nameForCheck)
                    return true;

            return false;
        }
        public bool DetachContract(Report report, string contractName)
        {
            bool bull;
            bull = _stor.DetachUnitedContract(report, contractName);
            return bull;
        }
        public List<Report> CheckDic(Report report)
        {
            List<Report> content = new List<Report>();

            if (_stor.UnitedContracts.ContainsValue(report.ContractName))
            {
                for (int i = 0; i < _stor.UnitedContracts.Count; i++)
                {
                    var item = _stor.UnitedContracts.ElementAt(i);

                    if (item.Value == report.ContractName)
                        content.Add(item.Key);
                }

                return content;
            }
            else
            {
                MessageBox.Show("Дополнительной информации о контракте " + report.ContractName + " нет.", "Внимание");
                return content;
            }
        }
        public bool RenameContract(Report report, string newContractName)
        {
            bool bull;
            bull = CheckContractName(newContractName);

            if (!bull)
            {
                //change name in crisis data
                foreach (var item in _stor.CrisisData)
                    if (item.ContractName == report.ContractName)
                    {
                        item.ContractName = newContractName;
                        break;
                    }

                //change name in graph data
                foreach (var item in _stor.GraphData)
                {
                    if (item.ContractName == report.ContractName)
                    {
                        item.ContractName = newContractName;
                        break;
                    }
                }

                //change name in united reports
                for (int i = 0; i < _stor.UnitedContracts.Count; i++)
                {
                    var item = _stor.UnitedContracts.ElementAt(i);
                    if (item.Value == report.ContractName)
                        _stor.UnitedContracts[item.Key] = newContractName;
                }

                //change name in reports
                foreach (var item in _stor.Reports)
                    if (item.ContractName == report.ContractName)
                    {
                        item.ContractName = newContractName;
                        break;
                    }

                return true;
            }
            else
            {
                MessageBox.Show("Данное имя уже занято", "Внимание");
                return false;
            }
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
            string DH;

            if (contractName == "ДС8 ИК \"Сибинтек\" РН-IaaS" || contractName == "ДС13 ИК \"Сибинтек\" SAP HANA" ||
                contractName == "ДС9 ИК «Сибинтек» ИС Predix" || contractName == "ДС7 ИК \"Сибинтек\" ИС ГеоПАК")
                DH = "DH-2";
            else
                DH = "DH-1";

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

                    if (value[23].ToString() != "")
                    {
                        //check IS
                        bool bull = CheckIS(value[23].ToString(), DH, "Month");
                        if (bull)
                        {
                            //Add IS incident
                            var isinfo = new IS(value[23].ToString(), DH, 1, 0, 0);
                            _stor.AddIS(isinfo);
                        }
                    }

                    if (value[32].ToString() != "" && value[32].ToString() != "0")
                        restart++;
                }
            }
            CountStats();
        }
        //count quarter
        private void QuarterCounter(List<object> dbDataQuarter, string contractName)
        {
            string DH;

            if (contractName == "ДС8 ИК \"Сибинтек\" РН-IaaS" || contractName == "ДС13 ИК \"Сибинтек\" SAP HANA" ||
                contractName == "ДС9 ИК «Сибинтек» ИС Predix" || contractName == "ДС7 ИК \"Сибинтек\" ИС ГеоПАК")
                DH = "DH-2";
            else
                DH = "DH-1";

            for (int i = 0; i < dbDataQuarter.Count; i++)
            {
                object[] value = (object[])dbDataQuarter[i];

                if (value[11].ToString() == contractName)
                {
                    reportQuarter++;

                    if (value[13].ToString() != "")
                        SLABreakCounterQuarter++;

                    if (value[23].ToString() != "")
                    {
                        //check IS
                        bool bull = CheckIS(value[23].ToString(), DH, "Quarter");
                        if (bull)
                        {
                            //Add IS incident
                            var isinfo = new IS(value[23].ToString(), DH, 0, 1, 0);
                            _stor.AddIS(isinfo);
                        }
                    }
                }
            }
        }
        //count year
        private void YearCounter(List<object> dbDataYear, string contractName)
        {
            string DH;

            if (contractName == "ДС8 ИК \"Сибинтек\" РН-IaaS" || contractName == "ДС13 ИК \"Сибинтек\" SAP HANA" ||
                contractName == "ДС9 ИК «Сибинтек» ИС Predix" || contractName == "ДС7 ИК \"Сибинтек\" ИС ГеоПАК")
                DH = "DH-2";
            else
                DH = "DH-1";

            for (int i = 0; i < dbDataYear.Count; i++)
            {
                object[] value = (object[])dbDataYear[i];

                if (value[11].ToString() == contractName)
                {
                    reportAmountYear++;

                    if (value[6].ToString() == "True")
                    {
                        //check crisis
                        bool bull = false;
                        foreach (var item in _stor.CrisisData)
                        {
                            if (item.RegNum == value[0].ToString())
                            {
                                bull = true;
                                break;
                            }
                        }

                        if (!bull)
                        {
                            //create crisis incident
                            var ci = new CI(value[0].ToString(), contractName, value[7].ToString(), value[14].ToString(), value[17].ToString(), "Old");
                            _stor.AddCrisis(ci);
                        }
                    }

                    if (value[13].ToString() != "")
                        SLABreakCounterYear++;

                    if (value[23].ToString() != "")
                    { 
                        //check IS
                        bool bull = CheckIS(value[23].ToString(), DH, "Year");
                        if (bull)
                        {
                            //Add IS incident
                            var isinfo = new IS(value[23].ToString(), DH, 0, 0, 1);
                            _stor.AddIS(isinfo);
                        }
                    }
                }
            }
        }

        //count crisis
        public bool CrisisCounter(string period, string contractName)
        {
            if (period == "Old")
            {
                for (int i = 0; i < _stor.Reports.Count; i++)
                {
                    if (_stor.Reports[i].ContractName == contractName)
                    {
                        _stor.Reports[i].CriticalYear++;

                        if (_stor.Reports[i].CriticalYear <= 1)
                            _stor.Reports[i].ColorYear = "#eda711";
                        else if (_stor.Reports[i].CriticalYear >= 2)
                            _stor.Reports[i].ColorYear = "Red";
                        return true;
                    }
                }

                MessageBox.Show("Кризисный инцидент не был добавлен. Сделайте перерасчет и добавьте КИ в необъединенный контракт.", "Внимание");
                return false;
            }
            else
            {
                for (int i = 0; i < _stor.Reports.Count; i++)
                {
                    if (_stor.Reports[i].ContractName == contractName)
                    {
                        _stor.Reports[i].Critical++;

                        if (_stor.Reports[i].Critical <= 1)
                            _stor.Reports[i].Color = "#eda711";
                        else if (_stor.Reports[i].Critical >= 2)
                            _stor.Reports[i].Color = "Red";
                        return true;
                    }
                }

                MessageBox.Show("Кризисный инцидент не был добавлен. Сделайте перерасчет и добавьте КИ в необъединенный контракт.", "Внимание");
                return false;
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

            if (slaMonth.ToString() == "NaN" && SLABreakCounter == 0)
                slaMonth = 100;

            //table month
            slaValue = Math.Round(100 - slaMonth, 2);

            if (slaMonth <= 100 && slaMonth >= 97.12)
                slaState = "Green";
            else if (slaMonth < 97.12 && slaMonth >= targetSla)
                slaState = "#eda711";
            else
                slaState = "Red";

            //quarter
            slaQuarter = Math.Round((1 - SLABreakCounterQuarter / (double)reportQuarter) * 100, 2);

            if (slaQuarter.ToString() == "NaN" && SLABreakCounterQuarter == 0)
                slaQuarter = 100;

            //table quarter
            slaQuarterValue = Math.Round(100 - slaQuarter, 2);

            if (slaQuarter <= 100 && slaQuarter >= 97.12)
                slaQuarterState = "Green";
            else if (slaQuarter < 97.12 && slaQuarter >= targetSla)
                slaQuarterState = "#eda711";
            else
                slaQuarterState = "Red";

            //year
            slaYear = Math.Round((1 - SLABreakCounterYear / (double)reportAmountYear) * 100, 2);

            if (slaYear.ToString() == "NaN" && SLABreakCounterYear == 0)
                slaYear = 100;

            //table year
            slaYearValue = Math.Round(100 - slaYear, 2);

            if (slaYear <= 100 && slaYear >= 97.12)
                slaYearState = "Green";
            else if (slaYear < 97.12 && slaYear >= targetSla)
                slaYearState = "#eda711";
            else
                slaYearState = "Red";
        }
        //check crisis
        private void CheckCrisis()
        {
            if (critical == 1)
                color = "#eda711";
            else if (critical >= 2)
                color = "Red";

            if (criticalYear == 1)
                colorYear = "#eda711";
            else if (criticalYear >= 2)
                colorYear = "Red";
        }
        //check IS
        private bool CheckIS(string ibcat1, string dh, string period)
        {
            foreach (var item in _stor.ISData)
            {
                if (item.IBCat1 == ibcat1 && period == "Month" && item.DH == dh)
                {
                    item.Month++;
                    return false;
                }
                else if (item.IBCat1 == ibcat1 && period == "Quarter" && item.DH == dh)
                {
                    item.Quarter++;
                    return false;
                }
                else if (item.IBCat1 == ibcat1 && period == "Year" && item.DH == dh)
                {
                    item.Year++;
                    return false;
                }
            }

            return true;
        }

        //check db values (Marks) for dbList
        public List<object> DBCheck(List<object> list)
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
            GraphResetValues();

            foreach (var item in _stor.GraphData)
            {
                report1 += item.Report1;
                report2 += item.Report2;
                report3 += item.Report3;
                report4 += item.Report4;
                report5 += item.Report5;
                report6 += item.Report6;
                report7 += item.Report7;
                report8 += item.Report8;
                report9 += item.Report9;
                report10 += item.Report10;
                report11 += item.Report11;
                report12 += item.Report12;
                report13 += item.Report13;
                report14 += item.Report14;
                report15 += item.Report15;

                sla1 += item.SLA1;
                sla2 += item.SLA2;
                sla3 += item.SLA3;
                sla4 += item.SLA4;
                sla5 += item.SLA5;
                sla6 += item.SLA6;
                sla7 += item.SLA7;
                sla8 += item.SLA8;
                sla9 += item.SLA9;
                sla10 += item.SLA10;
                sla11 += item.SLA11;
                sla12 += item.SLA12;
                sla13 += item.SLA13;
                sla14 += item.SLA14;
                sla15 += item.SLA15;
            }

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Поступило обращений",
                    Values = new ChartValues<int> { report1, report2, report3, report4, report5, report6, report7, report8, report9, report10, report11,
                    report12, report13, report14, report15},
                    Fill = Brushes.CornflowerBlue
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

        //collect reports from storage
        public List<Report> CollectReports()
        {
            return _stor.Reports;
        }
        //collect requests from storage
        public List<Requests> CollectRequests()
        {
            return _stor.Requests;
        }
        //collect crisis incidents from storage
        public List<CI> CollectCI()
        {
            return _stor.CrisisData;
        }
        //collect IS from storage
        public List<IS> CollectIS()
        {
            return _stor.ISData;
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
        //delete united report
        public void DropUnitedReport(Report report)
        {
            _stor.DropUnitedContract(report);
        }
        //delete graph info
        public void DropGraph(Report report) 
        {
            for (int i = 0; i < _stor.GraphData.Count; i++)
            {
                var item = _stor.GraphData[i];
                if (report.ContractName == item.ContractName)
                {
                    _stor.DropGraph(item);
                    i--;
                }
            }

            for (int i = 0; i < _stor.UnitedContracts.Count; i++)
            {
                var item = _stor.UnitedContracts.ElementAt(i);
                if (item.Value == report.ContractName)
                {
                    for (int j = 0; j < _stor.GraphData.Count; j++)
                    {
                        var graph = _stor.GraphData[j];
                        if (item.Key.ContractName == graph.ContractName)
                        {
                            _stor.DropGraph(graph);
                            j--;
                        }
                    }
                }
            }
        }
        //insert Report by index
        public void InsertReport(int index, Report report)
        {
            _stor.InsertReport(index, report);
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
