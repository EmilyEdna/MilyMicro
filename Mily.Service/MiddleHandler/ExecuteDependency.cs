using Mily.Service.MiddleView;
using Mily.Service.MiddleView.ViewEnum;
using XExten.XCore;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
using BeetleX.EventArgs;
using System.Linq;
using Mily.Service.MiddleHandler.IntegrationHandler;
using BeetleX;

namespace Mily.Service.MiddleHandler
{
    public class ExecuteDependency
    {
        /// <summary>
        /// 缓存Seesion
        /// </summary>
        private readonly static Dictionary<String, PacketDecodeCompletedEventArgs> PacketCache = new Dictionary<String, PacketDecodeCompletedEventArgs>();

        /// <summary>
        /// 缓存Session
        /// </summary>
        /// <param name="Provider"></param>
        /// <param name="Event"></param>
        public static void ExecutePacketCache(SocketMiddleData Provider, PacketDecodeCompletedEventArgs Event)
        {
            if (Provider.SendType == SendTypeEnum.Init)
            {
                var Keys = Provider.MiddleResult.SocketJsonData.ToModel<Dictionary<string, List<string>>>().Keys.ToList();
                PacketCache.Add(string.Join(",", Keys), Event);
            }
        }
        /// <summary>
        /// 处理内部消息
        /// </summary>
        /// <param name="Param"></param>
        public static void ExecuteInternalInfo(PacketDecodeCompletedEventArgs Event,SocketMiddleData Param)
        {

            switch (Param.SendType)
            {
                case SendTypeEnum.Init:
                    InitHandler.ExecuteSocketApiJson(Param.MiddleResult);
                    break;
                case SendTypeEnum.InternalInfo:
                    InternalHandler.ExecuteSocketIniternalInfo(Param, PacketCache);
                    break;
                case SendTypeEnum.RequestInfo:
                    Event.Server.Send(Param.ToJson(),Event.Session);
                    break;
                case SendTypeEnum.CallBack:
                    break;
                default:
                    break;
            }
        }

    }
}
