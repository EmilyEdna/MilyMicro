using BeetleX;
using BeetleX.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;

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
            var Result = (XExten.Common.ResultProvider)Event.Message;
            Server.Send(Result, Event.Session);
        }
    }
}
