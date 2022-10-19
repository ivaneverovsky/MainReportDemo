using System;

namespace MainReportDemo.UIModels
{
    [Serializable]
    internal class CDS
    {
        public string RegNum { get; set; }
        public string Mark { get; set; }

        public CDS(string regNum, string mark)
        {
            RegNum = regNum;
            Mark = mark;
        }
    }
}
