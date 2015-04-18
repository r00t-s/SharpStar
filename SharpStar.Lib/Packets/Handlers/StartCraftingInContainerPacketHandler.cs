using System.Threading.Tasks;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    class StartCraftingInContainerPacketHandler : PacketHandler<StartCraftingInContainerPacket>
    {
        public override Task Handle(StartCraftingInContainerPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("startCraftingInContainerResult", packet, client);

            return base.Handle(packet, client);
        }

        public override Task HandleAfter(StartCraftingInContainerPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterStartCraftingInContainerResult", packet, client);

            return base.HandleAfter(packet, client);
        }
    }
}
