using System.Threading.Tasks;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    class DamageRequestPacketHandler: PacketHandler<DamageRequestPacket>
    {
        public override Task Handle(DamageRequestPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("damageRequestResult", packet, client);

            return base.Handle(packet, client);
        }

        public override Task HandleAfter(DamageRequestPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterDamageRequestResult", packet, client);

            return base.HandleAfter(packet, client);
        }
    }
}
