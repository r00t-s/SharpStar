using System.Threading.Tasks;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    class CentralStructureUpdatePacketHandler : PacketHandler<CentralStructureUpdatePacket>
    {
        public override Task Handle(CentralStructureUpdatePacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("centralStructureUpdate", packet, client);

            return base.Handle(packet, client);
        }

        public override Task HandleAfter(CentralStructureUpdatePacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterCentralStructureUpdate", packet, client);

            return base.HandleAfter(packet, client);
        }
    }
}
