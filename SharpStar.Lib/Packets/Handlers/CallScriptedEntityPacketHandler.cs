
using System.Threading.Tasks;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    class CallScriptedEntityPacketHandler : PacketHandler<CallScriptedEntityPacket>
    {
        public override Task Handle(CallScriptedEntityPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("callScriptedEntityResult", packet, client);

            return base.Handle(packet, client);
        }

        public override Task HandleAfter(CallScriptedEntityPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterCallScriptedEntityResult", packet, client);

            return base.HandleAfter(packet, client);
        }
    }
}
