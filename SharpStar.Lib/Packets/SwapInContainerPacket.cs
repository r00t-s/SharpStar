using SharpStar.Lib.DataTypes;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    class SwapInContainerPacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte)KnownPacket.SwapInContainer; }
        }

        public int EntityId { get; set; }
        public ItemDescriptor item { get; set; }
        public ulong slot { get; set; }

        public override void Read(IStarboundStream stream)
        {
            EntityId = stream.ReadInt32();
            item = ItemDescriptor.FromStream(stream);
            slot = stream.ReadVLQ();
        }

        public override void Write(IStarboundStream stream)
        {
            stream.WriteInt32(EntityId);
            item.WriteTo(stream);
            stream.WriteVLQ(slot);
        }
    }
}
