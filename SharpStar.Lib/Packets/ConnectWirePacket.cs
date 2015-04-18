using SharpStar.Lib.DataTypes;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    class ConnectWirePacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte)KnownPacket.ConnectWire; }
        }

        public Vec2I OutputObjectLocation { get; set; }
        public Vec2I OutputConnectorLocation { get; set; }
        public Vec2I InputObjectLocation { get; set; }
        public Vec2I InputConnectorLocation { get; set; }

        public override void Read(IStarboundStream stream)
        {
            OutputConnectorLocation = Vec2I.FromStream(stream);
            OutputConnectorLocation = Vec2I.FromStream(stream);
            InputObjectLocation = Vec2I.FromStream(stream);
            InputConnectorLocation  = Vec2I.FromStream(stream);
        }

        public override void Write(IStarboundStream stream)
        {
            OutputObjectLocation.WriteTo(stream);
            OutputConnectorLocation.WriteTo(stream);
            InputObjectLocation.WriteTo(stream);
            InputConnectorLocation.WriteTo(stream);
        }
    }
}
