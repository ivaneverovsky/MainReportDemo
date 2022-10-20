using System;

namespace MainReportDemo.UIModels
{
    [Serializable]
    internal class CI
    {
        public string RegNum { get; set; }
        public string ContractName { get; set; }
        public string Status { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string Period { get; set; }

        public CI(string regNum, string contractName, string status, string serviceName, string description, string period)
        {
            RegNum = regNum;
            ContractName = contractName;
            Status = status;
            ServiceName = serviceName;
            Description = description;
            Period = period;
        }
    }
}
