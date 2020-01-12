﻿using BeetleX.Buffers;
using BeetleX.Clients;
using BeetleX.Packets;
using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Extension.ClientRpc.RpcSetting
{
    public class RcpClientPacket : FixeHeaderClientPacket
    {
        public IMessageTypeHeader TypeHeader { get; set; }

        public RcpClientPacket()
        {
            TypeHeader = new TypeHandler();
        }

        public override IClientPacket Clone()
        {
            return new RcpClientPacket();
        }

        protected override object OnRead(IClient client, PipeStream stream)
        {
            return MessagePackSerializer.Deserialize(TypeHeader.ReadType(stream), stream, MessagePackSerializerOptions.Standard.WithResolver(ContractlessStandardResolver.Instance));
        }

        protected override void OnWrite(object data, IClient client, PipeStream stream)
        {
            TypeHeader.WriteType(data, stream);
            MessagePackSerializer.Serialize(data.GetType(), stream, data, MessagePackSerializerOptions.Standard.WithResolver(ContractlessStandardResolver.Instance));
        }
    }
}
