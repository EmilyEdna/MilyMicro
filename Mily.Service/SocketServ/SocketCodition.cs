using BeetleX;
using BeetleX.EventArgs;
using Mily.Service.ViewSetting;
using Mily.Service.ViewSetting.SocketSetting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XExten.XCore;

namespace Mily.Service.SocketServ
{
    public class SocketCodition : ServerHandlerBase
    {
        public static Int32 KeyId { get; set; }
        public static NetType? Boots { get; set; }
        public static List<String> Result = new List<String>();
        private static Dictionary<Int32, SessionReceiveEventArgs> SessionId = new Dictionary<int, SessionReceiveEventArgs>();
        public static Dictionary<String, Dictionary<Int32, SessionReceiveEventArgs>> Session = new Dictionary<String, Dictionary<Int32, SessionReceiveEventArgs>>();
        public static ManualResetEvent ResetEvent = new ManualResetEvent(true);
        public static SocketCodition NetCodition => new SocketCodition();
        private static object Locker = new object();
        /// <summary>
        /// 服务器端
        /// </summary>
        public static void NetServProvider()
        {
            IServer Server = SocketFactory.CreateTcpServer<SocketCodition>();
            Server.Setting(opt =>
            {
                opt.DefaultListen.Host = Configuration.TCP_Host;
                opt.DefaultListen.Port = Configuration.TCP_Port;
            });
            Server.Open();
        }
        /// <summary>
        /// 接受的数据
        /// </summary>
        /// <param name="Server"></param>
        /// <param name="Event"></param>
        public override void SessionReceive(IServer Server, SessionReceiveEventArgs Event)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    lock (Locker)
                    {
                        SocketInit(Server, Event);
                        base.SessionReceive(Server, Event);
                    }
                    ResetEvent.WaitOne();
                    ResetEvent.Reset();
                }
            });
        }
        /// <summary>
        /// 断开链接
        /// </summary>
        /// <param name="Server"></param>
        /// <param name="Event"></param>
        public override void Disconnect(IServer Server, SessionEventArgs Event)
        {
            base.Disconnect(Server, Event);
            if (!string.IsNullOrEmpty(Event.Session.Name))
            {
                string result = ParseCmd.Create(NetType.DisConnect, Event.Session.Name + "断开链接");
                SendToOnlines(Server, result);
            }
        }
        private void SendToOnlines(IServer Server, string Content, bool Poll = true)
        {
            if (Poll)
                Server.GetOnlines().ToEach<ISession>(item =>
                {
                    item.Stream.ToPipeStream().WriteLine(Content);
                    item.Stream.Flush();
                    item.Stream.Close();
                });
            else
            {
                ISession Session = Server.GetSession(KeyId);
                Session.Stream.ToPipeStream().WriteLine(Content);
                Session.Stream.Flush();
                Session.Stream.Close();
            }
        }
        private void SocketInit(IServer Server, SessionReceiveEventArgs Event)
        {
            string ClientResult = Event.Stream.ToPipeStream().ReadLine();
            Commanding NetTypes = ParseCmd.Parse(ClientResult);
            //接受数据
            if (NetTypes.Type != 0)
            {
                switch (NetTypes.Type)
                {
                    case NetType.Connect:
                        AddClient(NetTypes.Content, Event);
                        SendToOnlines(Server, ParseCmd.Create(NetType.Listen, "链接成功，开始监听>>>"));
                        break;
                    case NetType.Listen:
                        Result.Clear();
                        Result.Add(NetTypes.Content);
                        break;
                }
            }
            //发送数据
            if (Boots.HasValue)
            {
                switch (Boots)
                {
                    case NetType.Listen:
                        SendToOnlines(Server, ParseCmd.Create(NetType.Listen, ParseCmd.HashData), false);
                        Boots = null;
                        break;
                }
            }
        }
        private void AddClient(String Content, SessionReceiveEventArgs Event)
        {
            if (Content.IsContainsIn("|"))
            {
                String ServName = Content.Split("|")[1];
                if (!SessionId.ContainsKey((int)Event.Session.ID))
                {
                    SessionId.Add((int)Event.Session.ID, Event);
                    if (!Session.ContainsKey(ServName.ToUpper()))
                        Session.Add(ServName.ToUpper(), SessionId);
                }
            }
        }
    }
}
