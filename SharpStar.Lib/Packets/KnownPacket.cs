// SharpStar
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

namespace SharpStar.Lib.Packets
{
    /*
    //pissed off koala 7 packets
    public enum KnownPacket : byte
    {
        ProtocolVersion = 0,
        ConnectionResponse = 1,
        DisconnectResponse = 2,
        HandshakeChallenge = 3,
        ChatReceived = 4,
        UniverseTimeUpdate = 5,
        CelestialResponse = 6,
        ClientConnect = 7,
        ClientDisconnect = 8,
        HandshakeResponse = 9,
        WarpCommand = 10,
        ChatSent = 11,
        CelestialRequest = 12,
        ClientContextUpdate = 13,
        WorldStart = 14,
        WorldStop = 15,
        TileArrayUpdate = 16,
        TileUpdate = 17,
        TileLiquidUpdate = 18,
        TileDamageUpdate = 19,
        TileModificationFailure = 20,
        GiveItem = 21,
        SwapContainerResult = 22,
        EnvironmentUpdate = 23,
        EntityInteractResult = 24,
        ModifyTileList = 25,
        DamageTile = 26,
        DamageTileGroup = 27,
        RequestDrop = 28,
        SpawnEntity = 29,
        EntityInteract = 30,
        ConnectWire = 31,
        DisconnectAllWires = 32,
        OpenContainer = 33,
        CloseContainer = 34,
        SwapContainer = 35,
        ItemApplyContainer = 36,
        StartCraftingContainer = 37,
        StopCraftingContainer = 38,
        BurnContainer = 39,
        ClearContainer = 40,
        WorldClientStateUpdate = 41,
        EntityCreate = 42,
        EntityUpdate = 43,
        EntityDestroy = 44,
        DamageNotification = 45,
        StatusEffectRequest = 46,
        UpdateWorldProperties = 47,
        Heartbeat = 48
    }

    */
    //upbeat giraffe 2 packets

    public enum KnownPacket : byte
    {
        // Packets sent universe server -> universe client
        ProtocolVersion = 0, //done
        ServerDisconnect = 1, //done
        ConnectSuccess = 2, //todo fixme
        ConnectFailure = 3, //done
        HandshakeChallenge = 4, //TEST
        ChatReceive = 5, //test?
        UniverseTimeUpdate = 6, //probably needs to be double
        CelestialResponse = 7,  //todo written as unknown
        PlayerWarpResult = 8, //todo TEST
        // Packets sent universe client -> universe server
        ClientConnect = 9, //todo shipupgrade datatype
        ClientDisconnectRequest = 10, //todo written as unkown packet
        HandshakeResponse = 11, //todo possibly get rid of cliammessage and convert passhash to proper byte array
        PlayerWarp = 12, 
        FlyShip = 13, //done todo test
        ChatSend = 14, //done
        CelestialRequest = 15, //todo written as unknown
        // Packets sent bidirectionally between the universe client and the universe server.
        ClientContextUpdate = 16, //done
        // Packets sent world server -> world client
        WorldStart = 17, //todo test
        WorldStop = 18, //done
        CentralStructureUpdate = 19, //todo write as non byte-array
        TileArrayUpdate = 20,
        TileUpdate = 21, //done
        TileLiquidUpdate = 22,
        TileDamageUpdate = 23,
        TileModificationFailure = 24,
        GiveItem = 25, //todo m_parametersSha256
        SwapInContainerResult = 26,
        EnvironmentUpdate = 27,
        EntityInteractResult = 28,
        UpdateTileProtection = 29,
        // Packets sent world client -> world server
        ModifyTileList = 30,
        DamageTileGroup = 31,
        CollectLiquid = 32,
        RequestDrop = 33,
        SpawnEntity = 34,
        EntityInteract = 35,
        ConnectWire = 36,
        DisconnectAllWires = 37,
        OpenContainer = 38,
        CloseContainer = 39,
        SwapInContainer = 40,
        ItemApplyInContainer = 41,
        StartCraftingInContainer = 42, //done, 
        StopCraftingInContainer = 43, //done 
        BurnContainer = 44, //done
        ClearContainer = 45, //done
        WorldClientStateUpdate = 46, //done
        // Packets sent bidirectionally between world client and world server
        EntityCreate = 47, //done
        EntityUpdate = 48, //done
        EntityDestroy = 49, //might work. 
        HitRequest = 50, //done
        DamageRequest = 51, //written as unknown
        DamageNotification = 52,
        CallScriptedEntity = 53, //done i think?
        UpdateWorldProperties = 54,
        Heartbeat = 55
    }
}
