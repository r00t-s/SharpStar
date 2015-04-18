using System.Threading.Tasks;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    public class WorldClientStateUpdatePacketHandler : PacketHandler<WorldClientStateUpdatePacket>
    {
        public override Task Handle(WorldClientStateUpdatePacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("WorldClientStateUpdate", packet, client);

            return base.Handle(packet, client);
        }

        public override Task HandleAfter(WorldClientStateUpdatePacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterWorldClientStateUpdate", packet, client);

            return base.HandleAfter(packet, client);
        }
    }
}
