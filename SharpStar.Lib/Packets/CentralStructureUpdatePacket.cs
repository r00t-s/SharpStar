using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpStar.Lib.DataTypes;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    public class CentralStructureUpdatePacket : Packet
    {
        public override byte PacketId 
        {
            get { return (byte) KnownPacket.CentralStructureUpdate; }
        }

        //public Json StructureData { get; set; }
        public byte[] sdata { get; set; }

        public override void Read(IStarboundStream stream)
        {
          //  StructureData = Json.FromStream(stream);
            sdata = stream.ReadToEnd();
        }

        public override void Write(IStarboundStream stream)
        {
            //StructureData.WriteTo(stream);
            stream.Write(sdata,0,sdata.Length);
        }
    }
}
