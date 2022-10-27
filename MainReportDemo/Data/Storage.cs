using MainReportDemo.UIModels;
using System;
using System.Collections.Generic;

namespace MainReportDemo.Data
{
    [Serializable]
    internal class Storage
    {
        private List<Report> _reports = new List<Report>(); //reports
        public List<Report> Reports { get { return _reports; } }
        public void AddReport(Report report)
        {
            _reports.Add(report);
        }
        public void DropReport(Report report)
        {
            _reports.Remove(report);
        }
        private List<CDS> _marks = new List<CDS>(); //marks
        public List<CDS> Marks { get { return _marks; } }
        public void AddMark(CDS cds)
        {
            _marks.Add(cds);
        }
        private List<Graph> _graphData = new List<Graph>(); //graph
        public List<Graph> GraphData { get { return _graphData; } }
        public void AddGraph(Graph graph)
        {
            _graphData.Add(graph);
        }
        private List<CI> _crisisData = new List<CI>(); //crisis
        public List<CI> CrisisData { get { return _crisisData; } }
        public void AddCrisis(CI ci)
        {
            _crisisData.Add(ci);
        }
        public void DropCrisis(CI ci)
        {
            _crisisData.Remove(ci);
        }
        public void ClearLists() //clear store
        {
            _reports.Clear();
            _graphData.Clear();
            _marks.Clear();
            _crisisData.Clear();
        }
    }
}
