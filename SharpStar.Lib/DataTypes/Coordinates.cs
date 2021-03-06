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
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using SharpStar.Lib.Misc;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.DataTypes
{
    public class SystemCoordinate
    {

        public string Sector { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public SystemCoordinate()
        {
        }

        public SystemCoordinate(string sector, int x, int y, int z)
        {
            Sector = sector;
            X = x;
            Y = y;
            Y = z;
        }

        public static SystemCoordinate FromStream(IStarboundStream stream)
        {

            SystemCoordinate coord = new SystemCoordinate();
            coord.Sector = stream.ReadString();
            coord.X = stream.ReadInt32();
            coord.Y = stream.ReadInt32();
            coord.Z = stream.ReadInt32();

            return coord;

        }

        public void WriteTo(IStarboundStream stream)
        {
            stream.WriteString(Sector);
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
        }

    }

    public class WorldCoordinate
    {

       public string Sector { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

        public int Planet { get; set; }

        public int Satellite { get; set; }

        public WorldCoordinate()
        {
           Sector = String.Empty;
            X = 0;
            Y = 0;
            Z = 0;
            Planet = 0;
            Satellite = 0;
        }

        public static WorldCoordinate FromStream(IStarboundStream stream)
        {

            WorldCoordinate wc = new WorldCoordinate();
         //   wc.Sector = stream.ReadString();
            wc.X = stream.ReadInt32();
            wc.Y = stream.ReadInt32();
            wc.Z = stream.ReadInt32();
            wc.Planet = stream.ReadInt32();
            wc.Satellite = stream.ReadInt32();
            wc.Sector = wc.X.ToString() + wc.Y.ToString() + wc.Planet.ToString() + wc.Planet.ToString() + wc.Satellite.ToString();
            return wc;

        }

        public void WriteTo(IStarboundStream stream)
        {
          //  stream.WriteString(Sector);
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
            stream.WriteInt32(Planet);
            stream.WriteInt32(Satellite);
        }

        public static WorldCoordinate GetGlobalCoords(byte[] data)
        {

            for (int i = 0; i < data.Length - 26; i++)
            {
                foreach (var sector in StarboundSector.Sectors.Select(p => p.Data))
                {

                    byte[] buffer = new byte[sector.Length];

                    Buffer.BlockCopy(data, i, buffer, 0, sector.Length);

                    if (sector.SequenceEqual(buffer))
                    {

                        byte[] sectorData = new byte[sector.Length + 21];

                        Buffer.BlockCopy(data, i - 1, sectorData, 0, sector.Length + 21);

                        using (StarboundStream s = new StarboundStream(sectorData))
                        {

                            WorldCoordinate coords = FromStream(s);

                            /*
                            if (string.IsNullOrEmpty(coords.Sector))
                                return null;
                            */

                            return coords;

                        }
                    }

                }
            }

            return null;

        }

        protected bool Equals(WorldCoordinate other)
        {
            return string.Equals(Sector, other.Sector) && X == other.X && Y == other.Y && Z == other.Z && Planet == other.Planet && Satellite == other.Satellite /*(Sector, other.Sector) && */;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WorldCoordinate)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Sector != null ? Sector.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ X;
                hashCode = (hashCode * 397) ^ Y;
                hashCode = (hashCode * 397) ^ Z;
                hashCode = (hashCode * 397) ^ Planet;
                hashCode = (hashCode * 397) ^ Satellite;
                return hashCode;
            }
        }

        public static bool operator ==(WorldCoordinate w1, WorldCoordinate w2)
        {

            if (object.ReferenceEquals(w1, w2))
                return true;

            if (object.ReferenceEquals(w1, null) || object.ReferenceEquals(w2, null))
                return false;

            return w1.Equals(w2);

        }

        public static bool operator !=(WorldCoordinate w1, WorldCoordinate w2)
        {
            return !(w1 == w2);
        }

        public override string ToString()
        {
            return String.Format("[Sector: {0}, X: {1}, Y: {2}, Z: {3}, Planet {4}, Satellite: {5}]", Sector, X, Y, Z, Planet, Satellite);
        }

    }

    public class PlanetCoordinate
    {

        public string Sector { get; set; }

        public ulong X { get; set; }

        public ulong Y { get; set; }

        public ulong Z { get; set; }

        public ulong Planet { get; set; }

        public ulong Satellite { get; set; }

        public PlanetCoordinate()
        {
            Sector = String.Empty;
            X = 0;
            Y = 0;
            Z = 0;
            Planet = 0;
            Satellite = 0;
        }
    }

    public class CelestialCoordinate
    {
        public Vec3I CelestialVec3I { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public static CelestialCoordinate FromStream(IStarboundStream stream)
        {
            var cs = new CelestialCoordinate();

            cs.X = stream.ReadInt32();
            cs.Y = stream.ReadInt32();
            cs.Z = stream.ReadInt32();

            return cs;
        }

        public void WriteTo(IStarboundStream stream)
        {
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
        }
    }
}
