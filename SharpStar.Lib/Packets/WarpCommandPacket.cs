﻿// SharpStar
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
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    public class WarpCommandPacket : Packet
    {
        /*
        public override byte PacketId
        {
            get { return (byte)KnownPacket.WarpCommand; }
        }
        */
        public WarpType WarpType { get; set; }

        public WorldCoordinate Coordinates { get; set; }

        public string Player { get; set; }

        public WarpCommandPacket()
        {
            Player = String.Empty;
            Coordinates = new WorldCoordinate();
        }

        public override void Read(IStarboundStream stream)
        {
            WarpType = (WarpType) stream.ReadUInt32();
            Coordinates = stream.ReadWorldCoordinate();
            Player = stream.ReadString();
        }

        public override void Write(IStarboundStream stream)
        {
            stream.WriteUInt32((uint) WarpType);
            stream.WriteWorldCoordinate(Coordinates);
            stream.WriteString(Player);
        }
    }

    public enum WarpType
    {
        MoveShip = 1,
        WarpUp = 2,
        WarpOtherShip = 3,
        WarpDown = 4,
        WarpHome = 5
    }
}