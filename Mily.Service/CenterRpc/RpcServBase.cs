using BeetleX;
using BeetleX.EventArgs;
using Mily.Service.CenterRpc.RpcSetting.Handler;
using XExten.Common;
using XExten.CacheFactory;

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
            ResultProvider Provider = ProxyHandler.InitProxy((ResultProvider)Event.Message);
            Caches.MongoDBCacheSet(Event);
            Server.Send(Provider, Event.Session);
        }
    }
}
