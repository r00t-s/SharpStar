using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    class UpdateTileProtectionPacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte)KnownPacket.UpdateTileProtection; }
        }

        public ushort DungeonId { get; set; }
        public bool IsProtected { get; set; }

        public override void Read(IStarboundStream stream)
        {
            DungeonId = stream.ReadUInt16();
            IsProtected = stream.ReadBoolean();
        }

        public override void Write(IStarboundStream stream)
        {
            stream.WriteUInt16(DungeonId);
            stream.WriteBoolean(IsProtected);
        }
    }
}
