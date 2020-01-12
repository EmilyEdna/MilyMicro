using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Gateway.GatewaySetting.ViewModel
{
    public class ServerGroupCondition
    {
        public string ServiceName { get; set; }
        public List<ServerCondition> Conditions { get; set; }
    }
}
