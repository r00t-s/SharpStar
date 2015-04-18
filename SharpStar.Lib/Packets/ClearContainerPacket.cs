
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    public class ClearContainerPacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte)KnownPacket.ClearContainer; }
        }

        public long EntityId { get; set; }

        public override void Read(IStarboundStream stream)
        {
            EntityId = stream.ReadSignedVLQ();
        }

        public override void Write(IStarboundStream stream)
        {
            stream.WriteSignedVLQ(EntityId);
        }

    }
}
