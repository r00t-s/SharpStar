﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpStar.Networking;

namespace SharpStar.Packets
{
    public class OpenContainerPacket : IPacket
    {
        public byte PacketId
        {
            get
            {
                return 31;
            }
            set
            {
            }
        }
        public bool Ignore { get; set; }

        public long EntityId { get; set; }

        public void Read(StarboundStream stream)
        {

            int discarded;

            EntityId = stream.ReadSignedVLQ(out discarded);
        
        }

        public void Write(StarboundStream stream)
        {
            stream.WriteSignedVLQ(EntityId);
        }
    }
}