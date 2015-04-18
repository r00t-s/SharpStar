
using System;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.DataTypes
{
    public class ItemDescriptor
    {
        public ItemDescriptor ItemDescriptors { get; set; }

        public String name { get; set; }
        public ulong count { get; set; }
        Json parameters { get; set; }

        public static ItemDescriptor FromStream(IStarboundStream stream)
        {
            ItemDescriptor idescr = new ItemDescriptor();
            idescr.name = stream.ReadString();
            idescr.count = stream.ReadVLQ();
           // SharpStarLogger.DefaultLogger.Info("name:"+idescr.name+",count"+idescr.count);
            idescr.parameters = Json.FromStream(stream);

            return idescr;
        }

        public void WriteTo(IStarboundStream stream)
        {
            stream.WriteString(name);
            stream.WriteVLQ(count);
            parameters.WriteTo(stream);
        }
    }
}
