using MainReportDemo.UIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MainReportDemo.Data
{
    internal class Calculations
    {
        Storage _stor = new Storage();

        public void ResultBuilder(List<object> dbDataMonth, List<object> dbDataQuarter, List<object> dbDataYear, string contract)
        {
            bool status = true;
            string color = "Green";
            string contractName = contract;
            int reportAmount = 0;
            int reportAmountYear = 10;
            int critical = 0;
            double slaMonth = 0;
            double slaQuarter = 0;
            double slaYear = 0;
            int requestsAccess = 0;
            int requestsChange = 0;
            int requestsUsage = 0;
            int incidents = 0;
            int incidentsIS = 0;
            int requestsAdvice = 0;
            int planedWork = 0;
            double five = 0;
            double four = 0;
            double three = 0;
            double two = 0;
            double noMark = 0;
            double restart = 0;

            if (contractName.Length < 10)
            {
                color = "Red";
            }
            var report = new Report(status, color, contractName, reportAmount, reportAmountYear, critical, slaMonth, slaQuarter, slaYear, requestsAccess, 
                requestsChange, requestsUsage, incidents, incidentsIS, requestsAdvice, planedWork, five, four, three, two, noMark, restart);

            _stor.AddReport(report);

            //actualSLA = Math.Round((1 - SLABreakCounter / (double)reportAmount) * 100, 2);
        }

        //collect reports from storage
        public List<Report> CollectReports()
        {
            return _stor.Reports;
        }

        //clear lists
        public void ClearData()
        {
            _stor.ClearLists();
        }
    }
}
