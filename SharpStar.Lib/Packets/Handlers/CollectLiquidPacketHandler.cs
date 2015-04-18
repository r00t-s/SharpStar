using System.Threading.Tasks;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    class CollectLiquidPacketHandler : PacketHandler<CollectLiquidPacket>
    {
        public override Task Handle(CollectLiquidPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("collectLiquidResult", packet, client);

            return base.Handle(packet, client);
        }

        public override Task HandleAfter(CollectLiquidPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterCollectLiquidResult", packet, client);

            return base.HandleAfter(packet, client);
        }
    }
}
