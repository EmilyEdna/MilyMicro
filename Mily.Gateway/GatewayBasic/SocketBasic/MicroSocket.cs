using BeetleX;
using BeetleX.EventArgs;
using Mily.Gateway.GatewayBasic.SocketBasic.SocketSetting.Handle;
using Mily.Gateway.GatewayBasic.SocketBasic.SocketSetting.View;
using System;
using System.Collections.Generic;
using System.Text;
using XExten.Common;

namespace Mily.Gateway.GatewayBasic.SocketBasic
{
    public class MicroSocket : ServerHandlerBase
    {
        public override void Disconnect(IServer Server, SessionEventArgs Event)
        {
            base.Disconnect(Server, Event);
            Server.CloseSession(Event.Session);
        }
        public override void SessionPacketDecodeCompleted(IServer Server, PacketDecodeCompletedEventArgs Event)
        {
            ResultProvider Provider = SocketProxy.InitProxy((ResultProvider)Event.Message);
            SocketCache.SetPacketCache(Provider, Event);
            if (((ServerKey)Provider.ObjectProvider).NetType != SocketTypeEnum.CallBack)
                Server.Send(Provider, Event.Session);
        }
    }
}
