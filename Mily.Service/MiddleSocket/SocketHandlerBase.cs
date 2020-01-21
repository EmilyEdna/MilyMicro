﻿using BeetleX;
using BeetleX.EventArgs;
using Mily.Service.MiddleHandler;
using Mily.Service.MiddleView;
using System;
using System.Collections.Generic;
using System.Text;
using XExten.XCore;

namespace Mily.Service.MiddleSocket
{
    public class SocketHandlerBase : ServerHandlerBase
    {
        public override void Disconnect(IServer Server, SessionEventArgs Event)
        {
            base.Disconnect(Server, Event);
            Server.CloseSession(Event.Session);
        }
        public override void SessionPacketDecodeCompleted(IServer Server, PacketDecodeCompletedEventArgs Event)
        {
            SocketMiddleData AcceptData;
            if (Event.Message is SocketMiddleData)
                AcceptData = (SocketMiddleData)Event.Message;
            else
                AcceptData = Event.Message.ToString().ToModel<SocketMiddleData>();
            ExecuteDependency.ExecutePacketCache(AcceptData, Event);
            ExecuteDependency.ExecuteInternalInfo(Event, AcceptData);

        }
    }
}
