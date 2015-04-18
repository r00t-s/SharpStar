using SharpStar.Lib.Logging;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    class PlayerWarpPacket : Packet
    {
        public override byte PacketId
        {
            get
            {
                return (byte) KnownPacket.PlayerWarp;
            }
        }

        public string WarptoWorld { get; set; }
        public string WarptoPlayer { get; set; }
        public WarpAlias WarpAlias { get; set; }
        public byte[] poop { get; set; }

        public override void Read(IStarboundStream stream)
        {
            /*
            WarptoWorld = stream.ReadString();
            WarptoPlayer = stream.ReadString();
            //WarpAlias = (WarpAlias) stream.ReadUInt8();
             */
            poop = stream.ReadToEnd();
            string s = null;
            foreach (var VARIABLE in poop)
            {
                 s += VARIABLE;
            }

            SharpStarLogger.DefaultLogger.Info("Warpaction_stream:"+s);
        }

        public override void Write(IStarboundStream stream)
        {
            /*
            stream.WriteString(WarptoWorld);
            stream.WriteString(WarptoPlayer);
             */
            stream.Write(poop,0,poop.Length);
           // stream.WriteUInt8((byte) WarpAlias);
        }
    }

    public enum WarpAlias
    {
        //todo this is likely uint. need to test
        Return,
        OrbitedWorld,
        OwnShip
    }
}
