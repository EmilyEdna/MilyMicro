using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Service.CenterRpc.RpcSetting.Handler
{
    public class ServerKey
    {
        public NetTypeEnum NetType { get; set; }
        public String ServName { get; set; }
        public static ServerKey SetValue(NetTypeEnum Net, String Serv)
        {
            return new ServerKey { NetType = Net, ServName = Serv };
        }
    }
}
