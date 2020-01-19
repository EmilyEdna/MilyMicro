using BeetleX.Clients;
using Mily.Socket.SocketConfig;
using Mily.Socket.SocketEnum;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Mily.Socket.SocketInterface;

namespace Mily.Socket.SocketEvent
{
    public class CallEvent
    {
        public static AsyncTcpClient SocketClient { get; set; }
        /// <summary>
        /// 发送内部通信
        /// </summary>
        /// <param name="Param"></param>
        /// <param name="Session"></param>
        public static void SendInternalInfo(ISocketResult Param, ISocketSession Session = null)
        {
            SocketClient.Send(SocketMiddleData.Middle(SendTypeEnum.InternalInfo, Param, Session));
        }
        /// <summary>
        /// 处理数据然后回发数据
        /// </summary>
        /// <param name="Param"></param>
        public static void CallBackHandler(SocketMiddleData Param)
        {
            if (Param.SendType == SendTypeEnum.RequestInfo)
            {
                var ResultData = CallHandlerEvent.ExecuteCallFuncHandler(Param);
                CallBackInternalInfo(ResultData);
            }
        }
        /// <summary>
        /// 回调数据
        /// </summary>
        /// <param name="Param"></param>
        private static void CallBackInternalInfo(ISocketResult Param)
        {
            SocketClient.Send(SocketMiddleData.Middle(SendTypeEnum.CallBack, Param));
        }
    }
}
