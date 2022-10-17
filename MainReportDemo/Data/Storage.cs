using MainReportDemo.UIModels;
using System;
using System.Collections.Generic;

namespace MainReportDemo.Data
{
    [Serializable]
    internal class Storage
    {
        private List<Report> _reports = new List<Report>();
        public List<Report> Reports { get { return _reports; } }
        public void AddReport(Report report)
        {
            _reports.Add(report);
        }

        private List<Graph> _graphData = new List<Graph>();
        public List<Graph> GraphData { get { return _graphData; } }
        public void AddGraph(Graph graph)
        {
            _graphData.Add(graph);
        }

        public void ClearLists()
        {
            _reports.Clear();
            _graphData.Clear();
        }
    }
}
