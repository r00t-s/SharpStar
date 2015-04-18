using System.Threading.Tasks;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    class DisconnectAllWiresPacketHandler : PacketHandler<DisconnectAllWiresPacket>
    {
        public override Task Handle(DisconnectAllWiresPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("disconnectAllWiresResult", packet, client);

            return base.Handle(packet, client);
        }

        public override Task HandleAfter(DisconnectAllWiresPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterDisconnectAllWiresResult", packet, client);

            return base.HandleAfter(packet, client);
        }
    }
}
