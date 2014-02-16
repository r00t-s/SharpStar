﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpStar.Networking;

namespace SharpStar.Packets
{
    public class HeartbeatPacket : ServerPacket
    {
        public override byte PacketId
        {
            get
            {
                return 46;
            }
            set
            {
            }
        }

        public override bool Ignore { get; set; }

        public ulong CurrentStep { get; set; }

        public override void Read(StarboundStream stream)
        {

            int discarded;

            CurrentStep = stream.ReadVLQ(out discarded);
        
        }

        public override void Write(StarboundStream stream)
        {
            stream.WriteVLQ(CurrentStep);
        }
    }
}
