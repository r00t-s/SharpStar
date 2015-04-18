using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    public class CallScriptedEntityPacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte)KnownPacket.CallScriptedEntity; }
        }

        public int entityID { get; set; }

        public string function { get; set; }

        public byte[] ScriptBytes { get; set; }

        public override void Read(IStarboundStream stream)
        {
            /*
           entityID = stream.ReadInt32();
            function = stream.ReadString();
             */
            ScriptBytes = stream.ReadToEnd();
        }

        public override void Write(IStarboundStream stream)
        {
            /*
           stream.WriteInt32(entityID);
            stream.WriteString(function);
             */
           // stream.WriteUInt8Array(ScriptBytes);
            stream.Write(ScriptBytes, 0, ScriptBytes.Length);
        }
    }
}
