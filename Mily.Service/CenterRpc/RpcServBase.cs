using BeetleX;
using BeetleX.EventArgs;
using Mily.Service.CenterRpc.RpcSetting.Handler;
using XExten.Common;
using XExten.CacheFactory;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Mily.Service.CenterRpc
{
    public class RpcServBase : ServerHandlerBase
    {
        public override void Disconnect(IServer Server, SessionEventArgs Event)
        {
            base.Disconnect(Server, Event);
            Server.CloseSession(Event.Session);
        }
        public override void SessionPacketDecodeCompleted(IServer Server, PacketDecodeCompletedEventArgs Event)
        {
            ResultProvider Provider = ProxyHandler.InitProxy((ResultProvider)Event.Message);
            EventCache.SetPacketCache(Provider, Event);
            if (((ServerKey)Provider.ObjectProvider).NetType != NetTypeEnum.CallBack)
                Server.Send(Provider, Event.Session);
        }
    }
}
