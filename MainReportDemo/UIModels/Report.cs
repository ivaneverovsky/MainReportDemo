using MainReportDemo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MainReportDemo.UIModels
{
    internal class Report
    {
        public bool Status { get; set; }
        public string Color { get; set; }
        public string ContractName { get; set; }
        public int ReportAmount { get; set; }
        public int ReportAmountYear { get; set; }
        public int Critical { get; set; }
        public double SLAMonth { get; set; }
        public double SLAQuarter { get; set; }
        public double SLAYear { get; set; }
        public int RequestsAccess { get; set; }
        public int RequestsChange { get; set; }
        public int RequestsUsage { get; set; }
        public int Incidents { get; set; }
        public int IncidentsIS { get; set; }
        public int RequestsAdvice { get; set; }
        public int PlanedWork { get; set; }
        public double Five { get; set; }
        public double Four { get; set; }
        public double Three { get; set; }
        public double Two { get; set; }
        public double NoMark { get; set; }
        public double Restart { get; set; }

        public Report(bool status, string color, string contractName, int reportAmount, int reportAmountYear, int critical, double sLAMonth, double sLAQuarter, double sLAYear, int requestsAccess, int requestsChange, int requestsUsage, int incidents, int incidentsIS, int requestsAdvice, int planedWork, double five, double four, double three, double two, double noMark, double restart)
        {
            Status = status;
            Color = color;
            ContractName = contractName;
            ReportAmount = reportAmount;
            ReportAmountYear = reportAmountYear;
            Critical = critical;
            SLAMonth = sLAMonth;
            SLAQuarter = sLAQuarter;
            SLAYear = sLAYear;
            RequestsAccess = requestsAccess;
            RequestsChange = requestsChange;
            RequestsUsage = requestsUsage;
            Incidents = incidents;
            IncidentsIS = incidentsIS;
            RequestsAdvice = requestsAdvice;
            PlanedWork = planedWork;
            Five = five;
            Four = four;
            Three = three;
            Two = two;
            NoMark = noMark;
            Restart = restart;
        }
    }
}
