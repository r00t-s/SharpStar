using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.DataTypes
{
    /// <summary>
    /// Contains X, Y, Z coordinates as integers
    /// </summary>
    public class Vec3I : IWriteable
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

        public static Vec3I FromStream(IStarboundStream stream)
        {
            var vec = new Vec3I();
            vec.X = stream.ReadInt32();
            vec.Y = stream.ReadInt32();
            vec.Z = stream.ReadInt32();

            return vec;
        }

        public void WriteTo(IStarboundStream stream)
        {
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
            stream.WriteInt32(Z);
        }
    }
}
