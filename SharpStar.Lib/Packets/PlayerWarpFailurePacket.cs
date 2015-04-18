using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    public class PlayerWarpFailurePacket : Packet
    {
        public override byte PacketId
        {
           get { return (byte) KnownPacket.PlayerWarpResult; }
        }

        public string WarptoWorld { get; set; }
        public string WarptoPlayer { get; set; }
        public WarpAlias WarpAlias { get; set; }
        public bool WarpActionInvalid { get; set; }

        public override void Read(IStarboundStream stream)
        {
            WarptoWorld = stream.ReadString();
            WarptoPlayer = stream.ReadString();
            WarpAlias = (WarpAlias)stream.ReadInt32();
            WarpActionInvalid = stream.ReadBoolean();
        }

        public override void Write(IStarboundStream stream)
        {
            stream.WriteString(WarptoWorld);
            stream.WriteString(WarptoPlayer);
            stream.WriteInt32((int)WarpAlias);
            stream.WriteBoolean(WarpActionInvalid);
        }
    }

    public enum WarpAlias2
    {
        MoveShip = 1,
        WarpUp = 2,
        WarpOtherShip = 3,
        WarpDown = 4,
        WarpHome = 5
    }
}
