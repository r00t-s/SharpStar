
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    public class StartCraftingInContainerPacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte)KnownPacket.StartCraftingInContainer; }
        }

        public int EntityId { get; set; }

        public override void Read(IStarboundStream stream)
        {
            EntityId = stream.ReadInt32();
        }

        public override void Write(IStarboundStream stream)
        {
            stream.WriteInt32(EntityId);
        }
    }
}
