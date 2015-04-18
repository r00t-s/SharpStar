using SharpStar.Lib.DataTypes;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    class TileLiquidUpdatePacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte)KnownPacket.TileLiquidUpdate; }
        }

        public Vec2I Position { get; set; }
        public byte LiquidId { get; set; }
        public byte Level { get; set; }

        public override void Read(IStarboundStream stream)
        {
            Position = Vec2I.FromStream(stream);
            LiquidId = stream.ReadUInt8();
            Level = stream.ReadUInt8();
        }

        public override void Write(IStarboundStream stream)
        {
            Position.WriteTo(stream);
            stream.WriteUInt8(LiquidId);
            stream.WriteUInt8(Level);
        }
    }
}
