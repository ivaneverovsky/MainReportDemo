using MainReportDemo.UIModels;
using System.Collections.Generic;

namespace MainReportDemo.Data
{
    internal class Storage
    {
        private List<Report> _reports = new List<Report>();
        public List<Report> Reports { get { return _reports; } }
        public void AddReport(Report report)
        {
            _reports.Add(report);
        }



        private List<object> _dbData = new List<object>();
        public List<object> dbData { get { return _dbData; } }
        public void AddData(object row)
        {
            _dbData.Add(row);
        }



        public void ClearLists()
        {
            _reports.Clear();
            _dbData.Clear();
        }
    }
}
