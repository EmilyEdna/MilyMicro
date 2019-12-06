using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Extension.ClientRpc.RpcSetting.View
{
    public class ClientKey
    {
        public NetTypeEnum NetType { get; set; }
        public String ServName { get; set; }
        public static ClientKey SetValue(NetTypeEnum Net, String Serv)
        {
            return new ClientKey { NetType = Net, ServName = Serv };
        }
    }
}
