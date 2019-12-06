using BeetleX.EventArgs;
using Mily.Service.CenterApi.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XExten.Common;
using XExten.CacheFactory;

namespace Mily.Service.CenterRpc.RpcSetting.Handler
{
    public class EventCache
    {
        private static Dictionary<String, PacketDecodeCompletedEventArgs> PacketCache = new Dictionary<String, PacketDecodeCompletedEventArgs>();
        public static void SetPacketCache(ResultProvider Provider, PacketDecodeCompletedEventArgs Event)
        {
            var Key = (ServerKey)Provider.ObjectProvider;
            if (!PacketCache.ContainsKey(Key.ServName))
            {
                PacketCache.Add(Key.ServName, Event);
                SetMongoCache(Key.ServName, Event);
            }
        }
        public static PacketDecodeCompletedEventArgs GetPacketCache(String Key)
        {
            return PacketCache.ContainsKey(Key) ? PacketCache[Key] : null;
        }
        private static void SetMongoCache(String ServiceProvider, PacketDecodeCompletedEventArgs Event)
        {
            ServerCondition Condition = new ServerCondition
            {
                No = (int)Event.Session.ID,
                ServiceName = ServiceProvider,
                Host = Event.Session.Socket.RemoteEndPoint.ToString().Split(":")[0],
                TcpPort = Event.Session.Socket.RemoteEndPoint.ToString().Split(":")[1],
                HttpPort = "",
                Stutas = 1
            };
            Caches.MongoDBCacheSet(Condition);
        }
    }
}
