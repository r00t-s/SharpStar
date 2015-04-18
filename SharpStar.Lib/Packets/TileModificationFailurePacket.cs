
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    public class TileModificationFailurePacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte) KnownPacket.TileModificationFailure; }
        }

        public byte[] modificationList { get; set; }

        public override void Read(IStarboundStream stream)
        {
            modificationList = stream.ReadToEnd();
        }

        public override void Write(IStarboundStream stream)
        {
            stream.Write(modificationList,0,modificationList.Length);
        }
    }
}
