using System.Threading.Tasks;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    class TileModificationFailurePacketHandler : PacketHandler<TileModificationFailurePacket>
    {
        public override Task Handle(TileModificationFailurePacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("tileModificationFailureResult", packet, client);

            return base.Handle(packet, client);
        }

        public override Task HandleAfter(TileModificationFailurePacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterTileModificationFailureResult", packet, client);

            return base.HandleAfter(packet, client);
        }
    }
}
