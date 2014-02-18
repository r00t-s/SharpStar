﻿using SharpStar.Networking;

namespace SharpStar.Packets
{
    public class TileDamageUpdatePacket : IPacket
    {
        public byte PacketId
        {
            get
            {
                return 17;
            }
        }

        public bool Ignore { get; set; }

        public int TileX { get; set; }

        public int TileY { get; set; }

        public byte Unknown { get; set; }

        public float Damage { get; set; }

        public float DamageEffect { get; set; }

        public float SourcePosX { get; set; }

        public float SourcePosY { get; set; }

        public byte DamageType { get; set; }

        public TileDamageUpdatePacket()
        {
            TileX = 0;
            TileY = 0;
            Unknown = 0;
            Damage = 0;
            DamageEffect = 0;
            SourcePosX = 0;
            SourcePosY = 0;
            DamageType = 0;
        }

        public void Read(StarboundStream stream)
        {
            TileX = stream.ReadInt32();
            TileY = stream.ReadInt32();
            Unknown = stream.ReadUInt8();
            Damage = stream.ReadSingle();
            DamageEffect = stream.ReadSingle();
            SourcePosX = stream.ReadSingle();
            SourcePosY = stream.ReadSingle();
            DamageType = stream.ReadUInt8();
        }

        public void Write(StarboundStream stream)
        {
            stream.WriteInt32(TileX);
            stream.WriteInt32(TileY);
            stream.WriteUInt8(Unknown);
            stream.WriteSingle(Damage);
            stream.WriteSingle(DamageEffect);
            stream.WriteSingle(SourcePosX);
            stream.WriteSingle(SourcePosY);
            stream.WriteUInt8(DamageType);
        }
    }
}