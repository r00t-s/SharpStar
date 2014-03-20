﻿using System;
using SharpStar.Lib.Entities;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    public class ClientConnectPacketHandler : PacketHandler<ClientConnectPacket>
    {
        public override void Handle(ClientConnectPacket packet, StarboundClient client)
        {
            client.Server.Player = new StarboundPlayer(packet.PlayerName,
                BitConverter.ToString(packet.UUID, 0).Replace("-", String.Empty).ToLower());

            SharpStarMain.Instance.PluginManager.CallEvent("clientConnected", packet, client);
        }

        public override void HandleAfter(ClientConnectPacket packet, StarboundClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterClientConnected", packet, client);
        }
    }
}