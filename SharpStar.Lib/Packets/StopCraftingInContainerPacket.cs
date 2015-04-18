using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    class StopCraftingInContainerPacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte)KnownPacket.StopCraftingInContainer; }
        }

        public int EntityId { get; set; }

        public override void Read(IStarboundStream stream)
        {
            EntityId = stream.ReadInt32();
        }

        public override void Write(IStarboundStream stream)
        {
            stream.WriteInt32(EntityId);
        }
    }
}
