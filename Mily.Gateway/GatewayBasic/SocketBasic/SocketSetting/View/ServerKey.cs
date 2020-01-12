using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Gateway.GatewayBasic.SocketBasic.SocketSetting.View
{
    public class ServerKey
    {
        public SocketTypeEnum NetType { get; set; }
        public String ServName { get; set; }
        public static ServerKey SetValue(SocketTypeEnum Net, String Serv)
        {
            return new ServerKey { NetType = Net, ServName = Serv };
        }
    }
}
