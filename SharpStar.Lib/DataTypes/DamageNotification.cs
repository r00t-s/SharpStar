using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.DataTypes
{
    public class DamageNotification
    {
        public string damSourceKind { get; set; }
        public Vec2F position { get; set; }

        public static DamageNotification FromStream(IStarboundStream stream)
        {
            var writeme = new DamageNotification();
            writeme.damSourceKind = stream.ReadString();
            writeme.position = Vec2F.FromStream(stream);

            return writeme;

        }

        public void WriteTo(IStarboundStream stream)
        {
            stream.WriteString(damSourceKind);
            position.WriteTo(stream);
        }

        /*
        public static DamageNotification FromStream(IStarboundStream stream)
        {
            
        }
        */
    }
}
