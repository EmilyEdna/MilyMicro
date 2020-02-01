using BeetleX.Clients;
using Mily.Socket.SocketConfig;
using Mily.Socket.SocketEnum;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Mily.Socket.SocketInterface;
using Mily.Socket.SocketInterface.DefaultImpl;
using XExten.XCore;
using Mily.Socket.SocketConfig.ConstConfig;
using Mily.Socket.SocketAbstract;

namespace Mily.Socket.SocketCall
{
    public class Call
    {
        public static AsyncTcpClient SocketClient { get; set; }
        /// <summary>
        /// 发送内部通信
        /// </summary>
        /// <param name="Param"></param>
        /// <param name="Session"></param>
        public static void SendInternalInfo(SocketSerializeData SerializeData,ISocketSession Session = null)
        {
            ISocketResult Param = new SocketResultDefault() { Router = SerializeData.Route,SocketJsonData= SerializeData.Providor?.ToJson() };
           SocketClient.Send(SocketMiddleData.Middle(SendTypeEnum.InternalInfo, Param, Session, SocketConstConfig.ClientPort).ToJson());
        }
        /// <summary>
        /// 处理数据然后回发数据
        /// </summary>
        /// <param name="Param"></param>
        /// <param name="CallHandle"></param>
        public static void CallBackHandler(SocketMiddleData Param, CallHandleAbstract CallHandle)
        {
            if (Param.SendType == SendTypeEnum.RequestInfo)
            {
                var ResultData = CallHandle.ExecuteCallFuncHandler(Param);
                CallBackInternalInfo(ResultData,Param.SendPort);
            }
        }
        /// <summary>
        /// 回调数据
        /// </summary>
        /// <param name="Param"></param>
        /// <param name="SendPort"></param>
        private static void CallBackInternalInfo(ISocketResult Param,int? SendPort)
        {
            SocketClient.Send(SocketMiddleData.Middle(SendTypeEnum.CallBack, Param,null,SendPort).ToJson());
        }
    }
}
