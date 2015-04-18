using SharpStar.Lib.DataTypes;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    class DisconnectAllWiresPacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte)KnownPacket.DisconnectAllWires; }
        }

        public Vec2I ObjectLocation { get; set; }
        public Vec2I ConnectorLocation { get; set; }

        public override void Read(IStarboundStream stream)
        {
            ObjectLocation = Vec2I.FromStream(stream);
            ConnectorLocation = Vec2I.FromStream(stream);
        }

        public override void Write(IStarboundStream stream)
        {
            ObjectLocation.WriteTo(stream);
            ConnectorLocation.WriteTo(stream);
        }
    }
}
