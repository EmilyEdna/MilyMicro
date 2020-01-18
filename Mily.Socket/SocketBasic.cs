using BeetleX;
using BeetleX.Clients;
using Mily.Socket.SocketConfig;
using Mily.Socket.SocketDependency;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XExten.XCore;

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
            {
                DependencyExecute.FindLibrary();
                Client.InitRpcProviderCustomer(Client.ServerPath, Client.ServerPort);
            }
        }
        /// <summary>
        /// 初始化Rpc
        /// </summary>
        /// <param name="Ip"></param>
        /// <param name="Port"></param>
        protected virtual void InitRpcProviderCustomer(string Ip, int Port)
        {
            AsyncTcpClient ClientAsnyc = SocketFactory.CreateClient<AsyncTcpClient, SocketPacket>(Ip, Port);
            if (!ClientPath.IsNullOrEmpty() && ClientPort.HasValue)
                ClientAsnyc.LocalEndPoint = new IPEndPoint(IPAddress.Parse(ClientPath), ClientPort.Value);
            ClientAsnyc.Connect(out bool Connect);
            ClientAsnyc.PacketReceive = (Client, Data) =>
            {

            };
            ClientAsnyc.ClientError = (Client, Error) =>
            {

            };

        }
    }
}
