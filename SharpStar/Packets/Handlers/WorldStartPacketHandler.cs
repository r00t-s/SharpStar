﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpStar.Server;
using SharpStar.DataTypes;

namespace SharpStar.Packets.Handlers
{
    public class WorldStartPacketHandler : PacketHandler<WorldStartPacket>
    {
        public override void Handle(WorldStartPacket packet, StarboundClient client)
        {

            Variant planet = packet.Planet;
            VariantDict planetDict = (VariantDict)planet.Value;

            if (planetDict.ContainsKey("celestialParameters"))
            {

                var celestParamsDict = (VariantDict)planetDict["celestialParameters"].Value;

                if (celestParamsDict != null)
                {

                    var coordinateDict = (VariantDict)celestParamsDict["coordinate"].Value;

                    if (coordinateDict != null)
                    {

                        var loc = (Variant[])coordinateDict["location"].Value;

                        var coords = new PlanetCoordinate();
                        coords.Sector = (string)coordinateDict["sector"].Value;
                        coords.X = (ulong)loc[0].Value;
                        coords.Y = (ulong)loc[1].Value;
                        coords.Z = (ulong)loc[2].Value;
                        coords.Planet = (ulong)coordinateDict["planet"].Value;
                        coords.Satellite = (ulong)coordinateDict["satellite"].Value;

                        client.Server.Player.OnShip = false;
                        client.Server.Player.Coordinates = coords;

                    }

                }
                else
                {
                    client.Server.Player.OnShip = true;
                }

            }

            SharpStarMain.Instance.PluginManager.CallEvent("worldStart", packet, client);

        }

        public override void HandleAfter(WorldStartPacket packet, StarboundClient client)
        {
            SharpStarMain.Instance.PluginManager.CallEvent("afterWorldStart", packet, client);
        }
    }
}
