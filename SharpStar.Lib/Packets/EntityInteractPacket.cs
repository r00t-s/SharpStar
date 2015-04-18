using SharpStar.Lib.DataTypes;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    class EntityInteractPacket :Packet
    {
        public override byte PacketId
        {
            get { return (byte)KnownPacket.EntityInteract; }
        }

        public int sEntityId { get; set; }

        public Vec2F SourcePosition { get; set; }

        public int dEntityId { get; set; }

        public override void Read(IStarboundStream stream)
        {
            sEntityId = stream.ReadInt32();
            SourcePosition = Vec2F.FromStream(stream);
            dEntityId = stream.ReadInt32();
        }

        public override void Write(IStarboundStream stream)
        {
            stream.WriteInt32(sEntityId);
            SourcePosition.WriteTo(stream);
            stream.WriteInt32(dEntityId);
        }
    }
}
