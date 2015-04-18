using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpStar.Lib.Networking;
using SharpStar.Lib.DataTypes;
using SharpStar.Lib.Misc;

namespace SharpStar.Lib.Packets
{
    class FlyShipPacket : Packet
    {
        public override byte PacketId
        {
            get
            {
                return (byte)KnownPacket.FlyShip;

            }
        }

        public CelestialCoordinate Celestial { get; set; } 


        public override void Read(IStarboundStream stream)
        {
            Celestial.X = stream.ReadInt32();
            Celestial.Y = stream.ReadInt32();
            Celestial.Z = stream.ReadInt32();
        }

        public override void Write(IStarboundStream stream)
        {
            stream.WriteInt32(Celestial.X);
            stream.WriteInt32(Celestial.Y);
            stream.WriteInt32(Celestial.Z);
        }
    }
}
