using BeetleX.EventArgs;
using Mily.Service.MiddleView;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net;
using XExten.XCore;

namespace Mily.Service.MiddleHandler.IntegrationHandler
{
    public class CallHandler
    {
        /// <summary>
        /// 执行回调数据
        /// </summary>
        internal static void ExecuteSocketCallBack(List<PacketDecodeCompletedEventArgs> Packets, SocketMiddleData Param)
        {
            var Event = Packets.Where(Item => (Item.Session.Socket.RemoteEndPoint as IPEndPoint).Port == Param.SendPort).FirstOrDefault();
            Event.Server.Send(Param.ToJson(), Event.Session);
        }
    }
}
