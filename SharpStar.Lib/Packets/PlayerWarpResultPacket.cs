
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    public class PlayerWarpResultPacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte)KnownPacket.PlayerWarpResult; }
        }

        public bool success { set; get; }
        public byte[] finishwriting { set; get; } //todo finish the rest when docs aren't ambiguous
        //public bool WarpActionInvalid { get; set; }

        public override void Read(IStarboundStream stream)
        {
            success = stream.ReadBoolean();
            finishwriting = stream.ReadToEnd();
            //WarpActionInvalid = stream.ReadBoolean();
        }

        public override void Write(IStarboundStream stream)
        {
            stream.WriteBoolean(success);
            stream.Write(finishwriting,0,finishwriting.Length);
            //stream.WriteBoolean(WarpActionInvalid);
        }
    }
}
