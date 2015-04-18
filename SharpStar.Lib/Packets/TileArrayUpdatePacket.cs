
using System.Collections.Generic;
using SharpStar.Lib.DataTypes;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    public class TileArrayUpdatePacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte)KnownPacket.TileArrayUpdate; }
        }

        public Vec2I Position { get; set; }

        public List<NetTile> netTile { get; set; }

        public byte[] temp { get; set; }

        public TileArrayUpdatePacket()
        {
            netTile = new List<NetTile>();
        }

        public override void Read(IStarboundStream stream)
        {
            Position = Vec2I.FromStream(stream);
            temp = stream.ReadToEnd();
            /*
            while ((stream.Length - stream.Position) > 0)
            {
                netTile.Add(NetTile.FromStream(stream));
            }
           */


        }

        public override void Write(IStarboundStream stream)
        {
            Position.WriteTo(stream);
            stream.Write(temp,0,temp.Length);
            /*
            foreach (var sTile in netTile)
            {
               sTile.WriteTo(stream);
            }
              */  
        }
    }
}
