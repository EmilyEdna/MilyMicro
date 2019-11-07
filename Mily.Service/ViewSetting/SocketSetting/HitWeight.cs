using BeetleX.EventArgs;
using System.Collections.Generic;

namespace Mily.Service.ViewSetting.SocketSetting
{
    public class HitWeight
    {
        public static Dictionary<int, int> HitsRecord = new Dictionary<int, int>();
        public static int Hits { get; set; }
        public static SessionReceiveEventArgs SessionEvnet { get; set; }
    }
}