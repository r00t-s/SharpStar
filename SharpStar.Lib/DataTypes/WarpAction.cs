using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.DataTypes
{
    /*
    class WarpAction
    {
        public string WarptoWorld { get; set; }
        public string WarptoPlayer { get; set; }
        public WarpAlias WarpAlias { get; set; }



        public static WarpAction FromStream(IStarboundStream stream)
        {
            WarptoWorld = stream.ReadString();

            WarpAlias (WarpAlias) = stream.ReadToEnd();
        }

        public void WriteTo(IStarboundStream stream)
        {
            stream.WriteInt32(X);
            stream.WriteInt32(Y);
        }

    }

    public enum WarpAlias
    {
        //todo this is likely uint8. need to test
        Return,
        OrbitedWorld,
        OwnShip
    }
     * */
}
