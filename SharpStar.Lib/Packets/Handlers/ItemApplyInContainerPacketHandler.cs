using System.Threading.Tasks;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    class ItemApplyInContainerPacketHandler : PacketHandler<ItemApplyInContainerPacket>
    {
        public override Task Handle(ItemApplyInContainerPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("itemApplyInContainerResult", packet, client);

            return base.Handle(packet, client);
        }

        public override Task HandleAfter(ItemApplyInContainerPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterItemApplyInContainerResult", packet, client);

            return base.HandleAfter(packet, client);
        }
    }
}
