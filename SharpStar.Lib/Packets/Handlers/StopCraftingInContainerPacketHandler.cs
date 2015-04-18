using System.Threading.Tasks;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    class StopCraftingInContainerPacketHandler : PacketHandler<StopCraftingInContainerPacket>
    {
        public override Task Handle(StopCraftingInContainerPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("stopCraftingInContainerResult", packet, client);

            return base.Handle(packet, client);
        }

        public override Task HandleAfter(StopCraftingInContainerPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterStopCraftingInContainerResult", packet, client);

            return base.HandleAfter(packet, client);
        }
    }
}
