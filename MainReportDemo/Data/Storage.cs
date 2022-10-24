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

        private List<CDS> _marks = new List<CDS>();
        public List<CDS> Marks { get { return _marks; } }
        public void AddMark(CDS cds)
        {
            _marks.Add(cds);
        }

        private List<Graph> _graphData = new List<Graph>();
        public List<Graph> GraphData { get { return _graphData; } }
        public void AddGraph(Graph graph)
        {
            _graphData.Add(graph);
        }

        private List<CI> _crisisData = new List<CI>();
        public List<CI> CrisisData { get { return _crisisData; } }
        public void AddCrisis(CI ci)
        {
            _crisisData.Add(ci);
        }
        public void DropCrisis(CI ci)
        {
            _crisisData.Remove(ci);
        }

        public void ClearLists()
        {
            _reports.Clear();
            _graphData.Clear();
            _marks.Clear();
            _crisisData.Clear();
        }
    }
}
