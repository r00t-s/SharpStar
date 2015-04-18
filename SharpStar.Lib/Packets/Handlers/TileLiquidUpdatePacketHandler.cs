using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    class TileLiquidUpdatePacketHandler : PacketHandler<TileLiquidUpdatePacket>
    {
        public override Task Handle(TileLiquidUpdatePacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("tileLiquidUpdateResult", packet, client);

            return base.Handle(packet, client);
        }

        public override Task HandleAfter(TileLiquidUpdatePacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterTileLiquidUpdateResult", packet, client);

            return base.HandleAfter(packet, client);
        }
    }
}
