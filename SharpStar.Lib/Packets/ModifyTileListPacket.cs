using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    class ModifyTileListPacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte)KnownPacket.ModifyTileList; }
        }

        public byte[] unknown { get; set; }

        public override void Read(IStarboundStream stream)
        {
            unknown = stream.ReadToEnd();
        }

        public override void Write(IStarboundStream stream)
        {
            stream.Write(unknown, 0, unknown.Length);
        }
    }
}
