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
        public static NetType? Boots { get; set; }
        public static Dictionary<String, SessionReceiveEventArgs> Session = new Dictionary<String, SessionReceiveEventArgs>();
        public static List<String> Result = new List<String>();
        public static SocketCodition NetCodition => new SocketCodition();
        public static ManualResetEvent ResetEvent = new ManualResetEvent(true);
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
                    string Key = Event.Session.ID.ToString();
                    if (!Session.ContainsKey(Key))
                        Session.Add(Key, Event);
                    SocketInit(Server, Event);
                    Console.WriteLine(Server);
                    base.SessionReceive(Server, Event);
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
        private void SendToOnlines(IServer Server, string Content)
        {
            foreach (ISession item in Server.GetOnlines())
            {
                item.Stream.ToPipeStream().WriteLine(Content);
                item.Stream.Flush();
                item.Stream.Close();
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
                        SendToOnlines(Server, ParseCmd.Create(NetType.Listen, ParseCmd.HashData));
                        Boots = null;
                        break;
                }
            }
        }
    }
}
