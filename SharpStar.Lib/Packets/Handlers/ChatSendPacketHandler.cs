using System.Threading.Tasks;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    class ChatSendPacketHandler : PacketHandler<ChatSendPacket>
    {
        public override Task Handle(ChatSendPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("chatSendResult", packet, client);

            return base.Handle(packet, client);
        }

        public override Task HandleAfter(ChatSendPacket packet, SharpStarClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterChatSendResult", packet, client);

            return base.HandleAfter(packet, client);
        }
    }
}
