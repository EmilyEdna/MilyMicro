using BeetleX.Buffers;
using BeetleX.Clients;
using BeetleX.Packets;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Clinet.ClientRpc.RpcSetting
{
    public class RcpClientPacket : FixeHeaderClientPacket
    {
        public IMessageTypeHeader TypeHeader { get; set; }
        public override IClientPacket Clone()
        {
            TypeHeader = new TypeHandler();
            return new RcpClientPacket();
        }

        protected override object OnRead(IClient client, PipeStream stream)
        {
            return MessagePackSerializer.NonGeneric.Deserialize(TypeHeader.ReadType(stream), stream, MessagePack.Resolvers.ContractlessStandardResolver.Instance);
        }

        protected override void OnWrite(object data, IClient client, PipeStream stream)
        {
            TypeHeader.WriteType(data, stream);
            MessagePackSerializer.NonGeneric.Serialize(data.GetType(), stream, data, MessagePack.Resolvers.ContractlessStandardResolver.Instance);
        }
    }
}
