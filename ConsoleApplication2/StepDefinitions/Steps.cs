using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinOpsAutomation.StepDefinitions
{
    class Steps
    {
        public class getValOfParams
        {
            public string commercialPeriod { get; set; }
            public string periodNameFormat { get; set; }
            public string from { get; set; }
            public string to { get; set; }
        }
    }
}
