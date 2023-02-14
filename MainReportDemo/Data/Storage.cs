using MainReportDemo.UIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MainReportDemo.Data
{
    [Serializable]
    internal class Storage
    {
        //reports
        private List<Report> _reports = new List<Report>(); 
        public List<Report> Reports { get { return _reports; } }
        public void AddReport(Report report)
        {
            _reports.Add(report);
        }
        public void DropReport(Report report)
        {
            _reports.Remove(report);
        }
        public void InsertReport(int index, Report report) //specially for moving logic inside ListView (up/down)
        {
            _reports.Insert(index, report);
        }

        //united reports
        private Dictionary<Report, string> _unitedContracts = new Dictionary<Report, string>();
        public Dictionary<Report, string> UnitedContracts { get { return _unitedContracts; } }
        public void AddUnitedContract(Report report, string contractName)
        {
            _unitedContracts.Add(report, contractName);
        }
        public bool DetachUnitedContract(Report report, string contractName)
        {
            bool bull = false;
            bool drop = false;

            for (int i = 0; i < UnitedContracts.Count; i++)
            {
                var item = UnitedContracts.ElementAt(i);

                if (item.Value == contractName)
                {
                    bull = true;
                    _unitedContracts.Remove(item.Key);
                    i--;
                    AddReport(item.Key); //return back detached Report to common storage

                    if (!drop)
                    {
                        DropReport(report);
                        drop = true;
                    }
                }
            }

            if (!bull)
            {
                MessageBox.Show("Контракт \"" + report.ContractName + "\" не имеет объединений.", "Внимание");
                return false;
            }

            return true;
        }
        public void DropUnitedContract(Report report)
        {
            for (int i = 0; i < UnitedContracts.Count; i++)
            {
                var item = UnitedContracts.ElementAt(i);
                if (item.Value == report.ContractName)
                {
                    _unitedContracts.Remove(item.Key);
                    i--;
                }
            }
        }

        //marks
        private List<CDS> _marks = new List<CDS>(); 
        public List<CDS> Marks { get { return _marks; } }
        public void AddMark(CDS cds)
        {
            _marks.Add(cds);
        }

        //graph
        private List<Graph> _graphData = new List<Graph>(); 
        public List<Graph> GraphData { get { return _graphData; } }
        public void AddGraph(Graph graph)
        {
            _graphData.Add(graph);
        }
        public void DropGraph(Graph graph)
        {
            _graphData.Remove(graph);
        }

        //crisis
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

        //IS
        private List<IS> _isData = new List<IS>();
        public List<IS> ISData { get { return _isData; } }
        public void AddIS(IS isinfo)
        {
            _isData.Add(isinfo);
        }

        //Requests
        private List<Requests> _requests = new List<Requests>();
        public List<Requests> Requests { get { return _requests; } }
        public void AddRequest(Requests request)
        {
            _requests.Add(request);
        }

        //clear store
        public void ClearLists() 
        {
            _reports.Clear();
            _graphData.Clear();
            _marks.Clear();
            _crisisData.Clear();
            _unitedContracts.Clear();
            _isData.Clear();
            _requests.Clear();
        }
    }
}
