using System;

namespace MainReportDemo.UIModels
{
    [Serializable]
    internal class Report
    {
        public string Color { get; set; }
        public string ColorYear { get; set; }
        public string ContractName { get; set; }
        public int ReportAmount { get; set; }
        public int ReportAmountYear { get; set; }
        public int Critical { get; set; }
        public int CriticalYear { get; set; }
        public double TargetSLA { get; set; }
        public double SLAMonth { get; set; }
        public string SLAState { get; set; }
        public double SLAValue { get; set; }
        public double SLAQuarter { get; set; }
        public string SLAQuarterState { get; set; }
        public double SLAQuarterValue { get; set; }
        public double SLAYear { get; set; }
        public string SLAYearState { get; set; }
        public double SLAYearValue { get; set; }
        public int RequestsAccess { get; set; }
        public double AccessPerc { get; set; }
        public int RequestsChange { get; set; }
        public double ChangePerc { get; set; }
        public int RequestsUsage { get; set; }
        public double UsagePerc { get; set; }
        public int Incidents { get; set; }
        public double IncidentsPerc { get; set; }
        public int IncidentsIS { get; set; }
        public double IncidentsISPerc { get; set; }
        public int RequestsAdvice { get; set; }
        public double AdvicePerc { get; set; }
        public int PlannedWork { get; set; }
        public double PlannedWorkPerc { get; set; }
        public double Five { get; set; }
        public double Four { get; set; }
        public double Three { get; set; }
        public double Two { get; set; }
        public double NoMark { get; set; }
        public double Restart { get; set; }

        public Report(string color, string colorYear, string contractName, int reportAmount, int reportAmountYear, int critical, int criticalYear, double targetSLA, double sLAMonth, string sLAState, double sLAValue, double sLAQuarter, string sLAQuarterState, double sLAQuarterValue, double sLAYear, string sLAYearState, double sLAYearValue, int requestsAccess, double accessPerc, int requestsChange, double changePerc, int requestsUsage, double usagePerc, int incidents, double incidentsPerc, int incidentsIS, double incidentsISPerc, int requestsAdvice, double advicePerc, int plannedWork, double plannedWorkPerc, double five, double four, double three, double two, double noMark, double restart)
        {
            Color = color;
            ColorYear = colorYear;
            ContractName = contractName;
            ReportAmount = reportAmount;
            ReportAmountYear = reportAmountYear;
            Critical = critical;
            CriticalYear = criticalYear;
            TargetSLA = targetSLA;
            SLAMonth = sLAMonth;
            SLAState = sLAState;
            SLAValue = sLAValue;
            SLAQuarter = sLAQuarter;
            SLAQuarterState = sLAQuarterState;
            SLAQuarterValue = sLAQuarterValue;
            SLAYear = sLAYear;
            SLAYearState = sLAYearState;
            SLAYearValue = sLAYearValue;
            RequestsAccess = requestsAccess;
            AccessPerc = accessPerc;
            RequestsChange = requestsChange;
            ChangePerc = changePerc;
            RequestsUsage = requestsUsage;
            UsagePerc = usagePerc;
            Incidents = incidents;
            IncidentsPerc = incidentsPerc;
            IncidentsIS = incidentsIS;
            IncidentsISPerc = incidentsISPerc;
            RequestsAdvice = requestsAdvice;
            AdvicePerc = advicePerc;
            PlannedWork = plannedWork;
            PlannedWorkPerc = plannedWorkPerc;
            Five = five;
            Four = four;
            Three = three;
            Two = two;
            NoMark = noMark;
            Restart = restart;
        }
    }
}
