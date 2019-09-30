using BeetleX.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Service.ViewSetting.SocketSetting
{
    public class HitWeight
    {
        public int Hits { get; set; }
        public Dictionary<int,SessionReceiveEventArgs> SessionEventMap { get; set; }
        public SessionReceiveEventArgs SessionEvnet => SessionEventMap[Hits];
    }
}
