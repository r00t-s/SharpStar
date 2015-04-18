using System.Collections.Generic;
using SharpStar.Lib.Misc;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    class ConnectFailurePacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte) KnownPacket.ConnectFailure; }
        }

        public string RejectionReason { get; set; }

        public override void Read(IStarboundStream stream)
        {
            RejectionReason = stream.ReadString();
        }

        public override void Write(IStarboundStream stream)
        {
            stream.WriteString(RejectionReason);
        }
    }
}
