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
        /// 服务器地址
        /// </summary>
        public string ServerPath { get; set; }
        /// <summary>
        /// 服务器端口
        /// </summary>
        public int ServerPort { get; set; }
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
        /// 初始化Socket
        /// </summary>
        /// <param name="Action"></param>
        public static void InitSocket(Action<SocketBasic> Action, bool UseServer = false)
        {
            SocketBasic Client = new SocketBasic();
            Action(Client);
            if (UseServer)
                Client.InitInternalSocket(Client.ServerPath, Client.ServerPort, DependencyExecute.Instance.FindLibrary());

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
                File.AppendAllText(Path.Combine(Path.Combine(AppContext.BaseDirectory, @"SocketLogger\"), "SocketError.log"), ExceptionInfomations);
                Console.WriteLine(ExceptionInfomations);
            };
            if (MiddleData.SendType == SendTypeEnum.Init)
                ClientAsnyc.Send(MiddleData);
        }
    }
}
