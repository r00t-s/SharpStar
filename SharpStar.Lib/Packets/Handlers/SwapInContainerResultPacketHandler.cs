using System.Threading.Tasks;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    class SwapInContainerResultPacketHandler : PacketHandler<SwapInContainerResultPacket>
    {
        public override Task Handle(SwapInContainerResultPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("swapInContainerResultResult", packet, client);

            return base.Handle(packet, client);
        }

        public override Task HandleAfter(SwapInContainerResultPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterSwapInContainerResultResult", packet, client);

            return base.HandleAfter(packet, client);
        }
    }
}
