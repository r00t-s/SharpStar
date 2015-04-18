using System.Threading.Tasks;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    class EntityInteractPacketHandler : PacketHandler<EntityInteractPacket>
    {
        public override Task Handle(EntityInteractPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("entityInteractResult", packet, client);

            return base.Handle(packet, client);
        }

        public override Task HandleAfter(EntityInteractPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterEntityInteractResult", packet, client);

            return base.HandleAfter(packet, client);
        }
    }
}
