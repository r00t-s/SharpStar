using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using SharpStar.Lib.Misc;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    class ConnectSuccessPacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte)KnownPacket.ConnectSuccess; }
        }

       // public ulong ClientId { get; set; }

        public uint ClientId { get; set; }
        public byte[] UUID;
        //public List<CelestialInfo> CelestialInfos { get; set; }
        public CelestialInfo cInfo { get; set; }

        public override void Read(IStarboundStream stream)
        {
            ClientId = stream.ReadUInt32();
            UUID = stream.ReadUInt8Array(16);
            cInfo = CelestialInfo.FromStream(stream);

            /*
            CelestialInfos = new List<CelestialInfo>();
            
            ulong length = stream.ReadVLQ();

            for (ulong i = 0; i < length; i++)
            {
                CelestialInfos.Add(CelestialInfo.FromStream(stream));
            }
             */

        }

        public override void Write(IStarboundStream stream)
        {
            stream.WriteUInt32(ClientId);
            stream.WriteUInt8Array(UUID, false);
            cInfo.WriteTo(stream);

            /*
            stream.WriteVLQ((ulong)CelestialInfos.Count);

            
            foreach (CelestialInfo cInfo in CelestialInfos)
            {
                cInfo.WriteTo(stream);
            }
             */
        }
    }
}
