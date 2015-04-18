using System.Threading.Tasks;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    class ConnectWirePacketHandler : PacketHandler<ConnectWirePacket>
    {
        public override Task Handle(ConnectWirePacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("connectWireResult", packet, client);

            return base.Handle(packet, client);
        }

        public override Task HandleAfter(ConnectWirePacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterConnectWireResult", packet, client);

            return base.HandleAfter(packet, client);
        }
    }
}
