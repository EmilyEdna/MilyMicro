using BeetleX;
using BeetleX.Buffers;
using BeetleX.Packets;
using MessagePack;
using MessagePack.Resolvers;
using Mily.Service.CenterRpc.RpcSetting;

namespace Mily.Service.RcpSetting.CenterRpc
{
    public class RcpServerPacket : FixedHeaderPacket
    {
        public IMessageTypeHeader TypeHeader { get; set; }
        public RcpServerPacket()
        {
            TypeHeader = new TypeHandler();
        }
        public override IPacket Clone()
        {
            return new RcpServerPacket();
        }

        protected override object OnReader(ISession session, PipeStream stream)
        {
            return MessagePackSerializer.Deserialize(TypeHeader.ReadType(stream), stream, MessagePackSerializerOptions.Standard.WithResolver(ContractlessStandardResolver.Instance));
        }

        protected override void OnWrite(ISession session, object data, PipeStream stream)
        {
            TypeHeader.WriteType(data, stream);
            MessagePackSerializer.Serialize(data.GetType(), stream, data, MessagePackSerializerOptions.Standard.WithResolver(ContractlessStandardResolver.Instance));
        }
    }
}
