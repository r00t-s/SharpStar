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

using System.IO;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.DataTypes
{
    /// <summary>
    /// A class representing a 'Net Tile' - Unfinished
    /// </summary>
    public class NetTile : IWriteable
    {

        public ushort background { get; set; }

        public byte backgroundHueShift { get; set; }

        public byte backgroundColorVariant { get; set; }

        public ushort backgroundMod { get; set; }

        public byte backgroundModHueShift { get; set; }

        public ushort foreground { get; set; }

        public byte foregroundHueShift { get; set; }

        public byte foregroundColorVariant { get; set; }

        public ushort foregroundMod { get; set; }

        public byte foregroundModHueShift { get; set; }

        public byte collision { get; set; }

        public byte blockBiomeIndex { get; set; }

        public byte environmentBiomeIndex { get; set; }

        public byte LiquidLevel { get; set; }

        public float Gravity { get; set; }

        public ushort DungeonID { get; set; } //uint16
        
        
        public NetTile()
        {

        }
       

        public static NetTile FromStream(IStarboundStream stream)
        {
            var netT = new NetTile();
            netT.background = stream.ReadUInt16();
            netT.backgroundHueShift = stream.ReadUInt8();
            netT.backgroundColorVariant = stream.ReadUInt8();
            netT.backgroundMod  = stream.ReadUInt16();
            netT.backgroundModHueShift = stream.ReadUInt8();
            netT.foreground = stream.ReadUInt16();
            netT.foregroundHueShift =stream.ReadUInt8();
            netT.foregroundColorVariant =stream.ReadUInt8();
            netT.foregroundMod =stream.ReadUInt16();
            netT.foregroundModHueShift =stream.ReadUInt8();
            netT.collision =stream.ReadUInt8();
            netT.blockBiomeIndex =stream.ReadUInt8();
            netT.environmentBiomeIndex =stream.ReadUInt8();
            netT.LiquidLevel =stream.ReadUInt8();
            netT.Gravity = stream.ReadFloat();
            netT.DungeonID = stream.ReadUInt16();

            return netT;
        }

        public void WriteTo(IStarboundStream stream)
        {
            stream.WriteUInt16(background);
            stream.WriteUInt8(backgroundHueShift);
            stream.WriteUInt8(backgroundColorVariant);
            stream.WriteUInt16(backgroundMod);
            stream.WriteUInt8(backgroundModHueShift);
            stream.WriteUInt16(foreground);
            stream.WriteUInt8(foregroundHueShift);
            stream.WriteUInt8(foregroundColorVariant);
            stream.WriteUInt16(foregroundMod);
            stream.WriteUInt8(foregroundModHueShift);
            stream.WriteUInt8(collision);
            stream.WriteUInt8(blockBiomeIndex);
            stream.WriteUInt8(environmentBiomeIndex);
            stream.WriteUInt8(LiquidLevel);
            stream.WriteFloat(Gravity);
            stream.WriteUInt16(DungeonID);
        }
    }
}
