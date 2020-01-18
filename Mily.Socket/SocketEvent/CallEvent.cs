using BeetleX.Clients;
using Mily.Socket.SocketConfig;
using Mily.Socket.SocketEnum;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Socket.SocketEvent
{
    public class CallEvent
    {
        public static AsyncTcpClient SocketClient { get; set; }
        /// <summary>
        /// 发送内部通信
        /// </summary>
        /// <param name="Param"></param>
        public static void SendInternalInfo(object Param)
        {
            SocketClient.Send(SocketMiddleData.Middle(SendTypeEnum.InternalInfo, Param));
        }
        /// <summary>
        /// 处理数据然后回发数据
        /// </summary>
        /// <param name="Param"></param>
        public static void CallBackHandler(SocketMiddleData Param)
        {
            if (Param.MiddleResult.Keys.FirstOrDefault() == SendTypeEnum.RequestInfo)
            {
                CallBackInternalInfo(null);
            }
        }
        /// <summary>
        /// 回调数据
        /// </summary>
        /// <param name="Param"></param>
        private static void CallBackInternalInfo(object Param)
        {
            SocketClient.Send(SocketMiddleData.Middle(SendTypeEnum.CallBack, Param));
        }
    }
}
