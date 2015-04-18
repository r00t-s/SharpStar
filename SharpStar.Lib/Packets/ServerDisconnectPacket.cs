using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    class ServerDisconnectPacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte)KnownPacket.ServerDisconnect; }
        }

        public string reason { get; set; }

        public override void Read(IStarboundStream stream)
        {
            reason = stream.ReadString();
        }

        public override void Write(IStarboundStream stream)
        {
            stream.WriteString(reason);
        }
    }
}
