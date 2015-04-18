// SharpStar
// Copyright (C) 2014 Mitchell Kutchuk
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using SharpStar.Lib.DataTypes;
using SharpStar.Lib.Logging;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    public class GiveItemPacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte) KnownPacket.GiveItem; }
        }

       // public ItemDescriptor Itemdescriptor { get; set; }
        public byte[] Unknown { get; set; }

        public override void Read(IStarboundStream stream)
        {
           // Itemdescriptor = ItemDescriptor.FromStream(stream);
            Unknown = stream.ReadToEnd();
            string s = null;

            foreach (var bleh in Unknown)
            {
                s += (char)bleh;
            }

            SharpStarLogger.DefaultLogger.Info(string.Format("{0}:{1}","uk",s));
        }

        public override void Write(IStarboundStream stream)
        {
            stream.Write(Unknown, 0, Unknown.Length);
          //  Itemdescriptor.WriteTo(stream);
        }
    }
}

/*
        public string ItemName { get; set; }

        public ulong Count { get; set; }

       // public VariantDict ItemProperties { get; set; }
        public byte[] ItemProperties { get; set; }

        public GiveItemPacket()
        {
            ItemName = String.Empty;
            Count = 0;
            //ItemProperties = new VariantDict();

        }

        public override void Read(IStarboundStream stream)
        {
            ItemName = stream.ReadString();
            Count = stream.ReadVLQ();
            SharpStarLogger.DefaultLogger.Info(string.Format("ItemName:{0},Count:{1}",ItemName,Count));
         //   ItemProperties = (VariantDict) stream.ReadVariant().Value;
            ItemProperties = stream.ReadToEnd();
        }

        public override void Write(IStarboundStream stream)
        {
            stream.WriteString(ItemName);
            stream.WriteVLQ(Count);
          //  stream.WriteVariant(new Variant(ItemProperties));
            stream.WriteUInt8Array(ItemProperties);
        }
         */