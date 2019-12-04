using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Service.CenterApi.ViewModel
{
    public class ServerCondition:BaseCondition
    {
        public int No { get; set; }
        public string Addr { get; set; }
        public string Port { get; set; }
        public int Stutas { get; set; }
    }
}
