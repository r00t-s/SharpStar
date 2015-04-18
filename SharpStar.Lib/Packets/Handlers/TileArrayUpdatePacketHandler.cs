using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    public class TileArrayUpdatePacketHandler : PacketHandler<TileArrayUpdatePacket>
    {
        public override Task Handle(TileArrayUpdatePacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("tileArrayUpdate", packet, client);

            return base.Handle(packet, client);
        }

        public override Task HandleAfter(TileArrayUpdatePacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterTileArrayUpdate", packet, client);

            return base.Handle(packet, client);
        }
    }
}
