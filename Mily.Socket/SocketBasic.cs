using BeetleX;
using BeetleX.Clients;
using Mily.Socket.SocketConfig;
using Mily.Socket.SocketDependency;
using Mily.Socket.SocketEnum;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using XExten.XCore;
using System.IO;
using Mily.Socket.SocketEvent;

namespace Mily.Socket
{
    public class SocketBasic
    {
        #region Basic Config
        /// <summary>
        /// 通信中心IP
        /// </summary>
        public string SockInfoIP { get; set; }
        /// <summary>
        /// 通信中心端口
        /// </summary>
        public int SockInfoPort { get; set; }
        /// <summary>
        /// 客服端地址
        /// </summary>
        public string ClientPath { get; set; }
        /// <summary>
        /// 客服端端口
        /// </summary>
        public int? ClientPort { get; set; }
        #endregion

        /// <summary>
        /// 初始化通信中心Socket
        /// </summary>
        /// <param name="Action"></param>
        public static void InitInternalSocket(Action<SocketBasic> Action, bool UseServer = false)
        {
            SocketBasic Client = new SocketBasic();
            Action(Client);
            if (UseServer)
                Client.InitInternalSocket(Client.SockInfoIP, Client.SockInfoPort, DependencyExecute.Instance.FindLibrary());
        }
        /// <summary>
        /// 重新连接通信中心
        /// </summary>
        /// <param name="Ip"></param>
        /// <param name="Port"></param>
        public static void ReOpenInternalSocket(string Ip,int Port) {
            SocketBasic Client = new SocketBasic();
            if (CallEvent.SocketClient.IsConnected)
                CallEvent.SocketClient.DisConnect();
            Client.InitInternalSocket(Ip, Port, DependencyExecute.Instance.FindLibrary());
        }
        /// <summary>
        /// 初始化内部通信
        /// </summary>
        /// <param name="Ip"></param>
        /// <param name="Port"></param>
        protected virtual void InitInternalSocket(string Ip, int Port, SocketMiddleData MiddleData)
        {
            AsyncTcpClient ClientAsnyc = SocketFactory.CreateClient<AsyncTcpClient, SocketPacket>(Ip, Port);
            if (!ClientPath.IsNullOrEmpty() && ClientPort.HasValue)
                ClientAsnyc.LocalEndPoint = new IPEndPoint(IPAddress.Parse(ClientPath), ClientPort.Value);
            ClientAsnyc.Connect(out bool Connect);
            CallEvent.SocketClient = ClientAsnyc;
            ClientAsnyc.PacketReceive = (Client, Data) =>
            {
                if (Client.IsConnected && ((SocketMiddleData)Data).MiddleResult != null)
                    CallEvent.CallBackHandler((SocketMiddleData)Data);
            };
            ClientAsnyc.ClientError = (Client, Error) =>
            {
                String ExceptionInfomations = $"Service errored with exception：【{Error.Message}】====write time：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\n";
                var Diretories = Path.Combine(AppContext.BaseDirectory, "SocketError");
                if (!Directory.Exists(Diretories))
                    Directory.CreateDirectory(Diretories);
                File.AppendAllText(Path.Combine(Diretories, "SocketErrorInfo.log"), ExceptionInfomations);
                Console.WriteLine(ExceptionInfomations);
            };
            if (MiddleData.SendType == SendTypeEnum.Init)
                ClientAsnyc.Send(MiddleData.ToJson());
        }
    }
}
