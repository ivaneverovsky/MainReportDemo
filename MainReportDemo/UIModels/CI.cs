using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainReportDemo.UIModels
{
    [Serializable]
    internal class CI
    {
        public string Check { get; set; }
        public string RegNum { get; set; }
        public string ContractName { get; set; }
        public string Status { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }

        public CI(string check, string regNum, string contractName, string status, string serviceName, string description)
        {
            Check = check;
            RegNum = regNum;
            ContractName = contractName;
            Status = status;
            ServiceName = serviceName;
            Description = description;
        }
    }
}
