using BeetleX.EventArgs;
using Mily.Gateway.GatewayBasic.SocketBasic.SocketSetting.View;
using Mily.Gateway.GatewaySetting.ViewModel;
using System;
using System.Collections.Generic;
using System.Net;
using XExten.CacheFactory;
using XExten.Common;
using XExten.XExpres;

namespace Mily.Gateway.GatewayBasic.SocketBasic.SocketSetting.Handle
{
    /// <summary>
    /// Socket缓存
    /// </summary>
    public class SocketCache
    {
        /// <summary>
        /// 缓存Seesion
        /// </summary>
        private readonly static Dictionary<String, PacketDecodeCompletedEventArgs> PacketCache = new Dictionary<String, PacketDecodeCompletedEventArgs>();
        public static void SetPacketCache(ResultProvider Provider, PacketDecodeCompletedEventArgs Event)
        {
            var Key = (ServerKey)Provider.ObjectProvider;
            if (!PacketCache.ContainsKey(Key.ServName))
            {
                PacketCache.Add(Key.ServName, Event);
                SetMongoCache(Key.ServName, Event);
            }
        }
        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static PacketDecodeCompletedEventArgs GetPacketCache(String Key)
        {
            return PacketCache.ContainsKey(Key) ? PacketCache[Key] : null;
        }
        /// <summary>
        /// 缓存客户端
        /// </summary>
        /// <param name="ServiceProvider"></param>
        /// <param name="Event"></param>
        private static void SetMongoCache(String ServiceProvider, PacketDecodeCompletedEventArgs Event)
        {
            IPEndPoint Point = (Event.Session.Socket.RemoteEndPoint as IPEndPoint);
            ServerCondition Condition = new ServerCondition
            {
                No = (int)Event.Session.ID,
                ServiceName = ServiceProvider,
                Host = Point.Address.ToString(),
                TcpPort = Point.Port.ToString(),
                ConnetTime = DateTime.Now,
                Stutas = 1
            };
            if (CheckMogodbCache(Condition))
                Caches.MongoDBCacheSet(Condition);
        }
        /// <summary>
        /// 判断是否已经连接过了
        /// </summary>
        /// <param name="Condition"></param>
        /// <returns></returns>
        private static bool CheckMogodbCache(ServerCondition Condition)
        {
            var Express = XExp.GetExpression<ServerCondition>("ServiceName", Condition.ServiceName, QType.Equals)
                 .And(XExp.GetExpression<ServerCondition>("TcpPort", Condition.TcpPort, QType.Equals))
                 .And(XExp.GetExpression<ServerCondition>("Host", Condition.Host, QType.Equals));
            return Caches.MongoDBCacheGet(Express) == null ? true : false;
        }
    }
}
