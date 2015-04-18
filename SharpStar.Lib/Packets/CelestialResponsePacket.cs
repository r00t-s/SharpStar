
using System.Collections.Generic;
using SharpStar.Lib.DataTypes;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    public class CelestialResponsePacket : Packet
    {
        public List<Vec3I> cel { set; get; }
        public byte[] Unknown { get; set; }

        public override byte PacketId
        {
            get { return (byte)KnownPacket.CelestialResponse; }
        }

        public override void Read(IStarboundStream stream)
        {
            //cel.Add(Vec3I.FromStream(stream)); 
            Unknown = stream.ReadToEnd();
        }

        public override void Write(IStarboundStream stream)
        {
            stream.Write(Unknown, 0, Unknown.Length);
        }
    }
}
