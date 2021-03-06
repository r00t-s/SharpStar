﻿// SharpStar
// Copyright (C) 2014 Mitchell Kutchuk
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Packets.Handlers
{
    public class HandshakeResponsePacketHandler : PacketHandler<HandshakeResponsePacket>
    {
        public override Task Handle(HandshakeResponsePacket packet, SharpStarClient client)
        {
            if (packet.IsReceive)
            {
                packet.Ignore = true;

                if (client.Server.Player.UserAccount != null && client.Server.Player.UserAccount.Hash != packet.PasswordHash)
                {
                    client.Server.Player.UserAccount = null;
                    client.Server.Player.JoinSuccessful = false;
                }
                else if (client.Server.Player.Guest && SharpStarMain.Instance.Config.ConfigFile.GuestPasswordHash != packet.PasswordHash)
                {
                    client.Server.Player.JoinSuccessful = false;
                }
            }

            //SharpStarMain.Instance.PluginManager.CallEvent("handshakeResponse", packet, client);

            return base.Handle(packet, client);
        }
    }
}
