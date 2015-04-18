using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    class UpdateTileProtectionPacketHandler : PacketHandler<UpdateTileProtectionPacket>
    {
        public override Task Handle(UpdateTileProtectionPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("updateTileProtectionResult", packet, client);

            return base.Handle(packet, client);
        }

        public override Task HandleAfter(UpdateTileProtectionPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterUpdateTileProtectionResult", packet, client);

            return base.HandleAfter(packet, client);
        }
    }
}
