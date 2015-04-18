using System.Collections.Generic;
using SharpStar.Lib.DataTypes;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    public class CollectLiquidPacket : Packet
    {
         public override byte PacketId
        {
            get { return (byte)KnownPacket.CollectLiquid; }
        }

        public List<Vec2I> tilePositions { get; set; }

        public byte[] temp { get; set; }

        public CollectLiquidPacket()
        {
            tilePositions = new List<Vec2I>();
        }

        public override void Read(IStarboundStream stream)
        {
            temp = stream.ReadToEnd();
            /*
            while ((stream.Length - stream.Position) > 0)
            {
                tilePositions.Add(Vec2I.FromStream(stream));
            }
           */
        }

        public override void Write(IStarboundStream stream)
        {
            stream.Write(temp,0,temp.Length);
            /*
            foreach (var vector in tilePositions)
            {
               vector.WriteTo(stream);
            }
              */  
        }
    }
}
