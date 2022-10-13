using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainReportDemo.UIModels
{
    internal class OutputDataModel
    {
        public string ReportDateMonth { get { return ReturnMonth(); } }
        public string ReportDateYear { get { return DateTime.Now.Year.ToString() + " года"; } }
        public DateTime StartReportDate { get { return ReportDateTimeStart(); } }
        public DateTime FinalReportDate { get { return ReportDateTimeFinal(); } }


        private static List<string> MonthList = new List<string>() {"Январь", "Февраль", "Март", "Апрель", "Май", "Июнь"
            , "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"};

        static string ReturnMonth()
        {
            DateTime time = DateTime.Now;
            string month = MonthList[time.Month - 1];
            return month;
        }

        //count start date for program
        static DateTime ReportDateTimeStart()
        {
            DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            return dateTime;
        }
        
        //count final date for program
        static DateTime ReportDateTimeFinal()
        {
            DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1);
            return dateTime.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
        }
    }
}
