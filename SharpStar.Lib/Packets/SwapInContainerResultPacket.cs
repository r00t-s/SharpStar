using SharpStar.Lib.DataTypes;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    class SwapInContainerResultPacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte)KnownPacket.SwapInContainerResult; }
        }

        public ItemDescriptor item { get; set; }

        public override void Read(IStarboundStream stream)
        {
            item = ItemDescriptor.FromStream(stream);
        }

        public override void Write(IStarboundStream stream)
        {
            item.WriteTo(stream);
        }
    }
}
