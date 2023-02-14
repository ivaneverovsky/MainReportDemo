using System;

namespace MainReportDemo.UIModels
{
    [Serializable]
    internal class IS
    {
        public string IBCat1 { get; set; }
        public string DH { get; set; }
        public int Month { get; set; }
        public int Quarter { get; set; }
        public int Year { get; set; }
        public IS(string iBCat1, string dH, int month, int quarter, int year)
        {
            IBCat1 = iBCat1;
            DH = dH;
            Month = month;
            Quarter = quarter;
            Year = year;
        }
    }
}
