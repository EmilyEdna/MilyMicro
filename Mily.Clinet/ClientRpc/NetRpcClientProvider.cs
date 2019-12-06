﻿using BeetleX;
using BeetleX.Clients;
using Mily.Clinet.ClientRpc.RpcSetting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Clinet.ClientRpc
{
    public class NetRpcClientProvider
    {
        /// <summary>
        /// 初始化Rpc
        /// </summary>
        public static void InitRpcProvider()
        {
            AsyncTcpClient Client = SocketFactory.CreateClient<AsyncTcpClient, RcpClientPacket>("127.0.0.1", 9090);
            Client.Connect();
            Client.Socket.SendBufferSize = int.MaxValue;
            Client.Socket.ReceiveBufferSize = int.MaxValue;
            Client.PacketReceive = (Client, Data) => 
            { 
            
            };
            Client.ClientError = (Client, Error) =>
            {

            };
        }
    }
}
