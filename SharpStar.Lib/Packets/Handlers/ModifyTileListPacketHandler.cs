using System.Threading.Tasks;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    class ModifyTileListPacketHandler : PacketHandler<ModifyTileListPacket>
    {
        public override Task Handle(ModifyTileListPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("modifyTileListResult", packet, client);

            return base.Handle(packet, client);
        }

        public override Task HandleAfter(ModifyTileListPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterModifyTileListResult", packet, client);

            return base.HandleAfter(packet, client);
        }
    }
}
