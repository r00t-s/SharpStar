using System.Threading.Tasks;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    class SwapInContainerPacketHandler : PacketHandler<SwapInContainerPacket>
    {
        public override Task Handle(SwapInContainerPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("swapInContainerResult", packet, client);

            return base.Handle(packet, client);
        }

        public override Task HandleAfter(SwapInContainerPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterSwapInContainerResult", packet, client);

            return base.HandleAfter(packet, client);
        }
    }
}
