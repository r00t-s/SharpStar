﻿using System;
using SharpStar.Lib.DataTypes;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    public class GiveItemPacket : IPacket
    {
        public byte PacketId
        {
            get { return 21; }
        }

        public bool Ignore { get; set; }

        public string ItemName { get; set; }

        public ulong Count { get; set; }

        public VariantDict ItemProperties { get; set; }

        public GiveItemPacket()
        {
            ItemName = String.Empty;
            ItemProperties = new VariantDict();
        }

        public void Read(IStarboundStream stream)
        {
            int discarded;

            ItemName = stream.ReadString();
            Count = stream.ReadVLQ(out discarded);
            ItemProperties = (VariantDict) stream.ReadVariant().Value;
        }

        public void Write(IStarboundStream stream)
        {
            stream.WriteString(ItemName);
            stream.WriteVLQ(Count);
            stream.WriteVariant(new Variant(ItemProperties));
        }
    }
}