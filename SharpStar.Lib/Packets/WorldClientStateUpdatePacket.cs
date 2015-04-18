using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    public class WorldClientStateUpdatePacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte)KnownPacket.WorldClientStateUpdate; }
        }

        public byte[] worldClientStateDelta { get; set; }

        public override void Read(IStarboundStream stream)
        {
            worldClientStateDelta = stream.ReadToEnd();
        }

        public override void Write(IStarboundStream stream)
        {
            stream.Write(worldClientStateDelta,0,worldClientStateDelta.Length);
        }
    }
}
