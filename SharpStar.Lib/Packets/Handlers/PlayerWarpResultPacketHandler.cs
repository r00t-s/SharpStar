
using System.Threading.Tasks;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    class PlayerWarpResultPacketHandler : PacketHandler<PlayerWarpResultPacket>
    {
        public override Task Handle(PlayerWarpResultPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("playerWarpResult", packet, client);

            return base.Handle(packet, client);
        }

        public override Task HandleAfter(PlayerWarpResultPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterPlayerWarpResult", packet, client);

            return base.HandleAfter(packet, client);
        }
    }
}
