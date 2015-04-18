using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpStar.Lib.DataTypes;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    public class TileUpdatePacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte)KnownPacket.TileUpdate; }
        }

        public Vec2I Position {get; set;}

        public NetTile NetTile { get; set; }

        public override void Read(IStarboundStream stream)
        {
            Position=Vec2I.FromStream(stream);
            NetTile = NetTile.FromStream(stream);
            //NetTile = stream.

        }

        public override void Write(IStarboundStream stream)
        {
            Position.WriteTo(stream);
            NetTile.WriteTo(stream);
        }
    }
}
