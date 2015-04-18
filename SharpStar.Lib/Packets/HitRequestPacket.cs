
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    public class HitRequestPacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte)KnownPacket.HitRequest; }
        }

        public long CausingEntityId { get; set; }

        public long TargetEntityId { get; set; }

        public override void Read(IStarboundStream stream)
        {
           CausingEntityId = stream.ReadSignedVLQ();
           TargetEntityId = stream.ReadSignedVLQ();
        }

        public override void Write(IStarboundStream stream)
        {
            stream.WriteSignedVLQ(CausingEntityId);
            stream.WriteSignedVLQ(TargetEntityId);
        }
    }
}
