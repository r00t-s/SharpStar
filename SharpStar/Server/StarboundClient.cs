﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Ionic.Zlib;
using SharpStar.DataTypes;
using SharpStar.Entities;
using SharpStar.Extensions;
using SharpStar.Networking;
using SharpStar.Packets;
using SharpStar.Packets.Handlers;

namespace SharpStar.Server
{
    public class StarboundClient : IClient, IDisposable
    {

        public Socket Socket { get; set; }

        public ConcurrentQueue<IPacket> PacketQueue { get; set; }

        public StarboundServerClient Server { get; set; }

        public StarboundClient OtherClient { get; set; }

        public PacketReader PacketReader { get; set; }


        private readonly List<IPacketHandler> _packetHandlers;

        public StarboundClient(Socket socket)
        {

            _packetHandlers = new List<IPacketHandler>();

            Socket = socket;
            PacketQueue = new ConcurrentQueue<IPacket>();

            PacketReader = new PacketReader();
            PacketReader.RegisterPacketType(0, typeof(ProtocolVersionPacket));
            PacketReader.RegisterPacketType(1, typeof(ConnectionResponsePacket));
            PacketReader.RegisterPacketType(2, typeof(DisconnectResponsePacket));
            PacketReader.RegisterPacketType(3, typeof(HandshakeChallengePacket));
            PacketReader.RegisterPacketType(4, typeof(ChatReceivedPacket));
            PacketReader.RegisterPacketType(5, typeof(UniverseTimeUpdatePacket));
            PacketReader.RegisterPacketType(6, typeof(ClientConnectPacket));
            PacketReader.RegisterPacketType(9, typeof(WarpCommandPacket));
            PacketReader.RegisterPacketType(10, typeof(ChatSentPacket));
            PacketReader.RegisterPacketType(11, typeof(ClientContextUpdatePacket));
            PacketReader.RegisterPacketType(12, typeof(WorldStartPacket));
            PacketReader.RegisterPacketType(13, typeof(WorldStopPacket));
            PacketReader.RegisterPacketType(17, typeof(TileDamageUpdatePacket));
            PacketReader.RegisterPacketType(19, typeof(GiveItemPacket));
            PacketReader.RegisterPacketType(45, typeof(UpdateWorldPropertiesPacket));

        }

        public void RegisterPacketHandler<T>(PacketHandler<T> handler) where T : IPacket
        {
            _packetHandlers.Add(handler);
        }

        public void UnregisterPacketHandler<T>(PacketHandler<T> handler) where T : IPacket
        {
            _packetHandlers.Remove(handler);
        }

        #region Connection

        public void StartReceiving()
        {
            Socket.BeginReceive(PacketReader.NetworkBuffer, 0, PacketReader.NetworkBuffer.Length, SocketFlags.None, ClientDataReceived, null);
        }

        public void ForceDisconnect()
        {
            Server.ForceDisconnect();
        }

        public void SendPacket(IPacket packet)
        {
            PacketQueue.Enqueue(packet);
            FlushPackets();
        }

        private void ClientDataReceived(IAsyncResult iar)
        {

            try
            {

                if (Socket == null || (Socket != null && !Socket.IsConnected()))
                {

                    ForceDisconnect();

                    return;

                }

                int length = Socket.EndReceive(iar);

                List<IPacket> packets = PacketReader.UpdateBuffer(length).ToList();

                foreach (var packet in packets)
                {

                    foreach (var handler in _packetHandlers)
                    {
                        if (packet.GetType() == handler.GetPacketType())
                            handler.Handle(packet, OtherClient);
                    }

                    if (!packet.Ignore)
                        OtherClient.SendPacket(packet);

                }

                bool disconnectPacket = false;

                foreach (var packet in packets)
                {

                    foreach (var handler in _packetHandlers)
                    {
                        if (packet.GetType() == handler.GetPacketType())
                            handler.HandleAfter(packet, OtherClient);
                    }

                    if (packet is DisconnectResponsePacket)
                    {

                        disconnectPacket = true;

                        ForceDisconnect();

                    }

                }

                if (!disconnectPacket)
                    Socket.BeginReceive(PacketReader.NetworkBuffer, 0, PacketReader.NetworkBuffer.Length, SocketFlags.None, ClientDataReceived, null);

            }
            catch (Exception)
            {
                ForceDisconnect();
            }

        }

        public void FlushPackets()
        {
            while (PacketQueue.Count > 0)
            {

                IPacket next;

                while (!PacketQueue.TryDequeue(out next))
                {
                }

                var memoryStream = new MemoryStream();

                var stream = new StarboundStream(memoryStream);
                next.Write(stream);
                byte[] buffer = memoryStream.ToArray();

                int length = buffer.Length;
                var compressed = ZlibStream.CompressBuffer(buffer);
                if (compressed.Length < buffer.Length)
                {
                    buffer = compressed;
                    length = -buffer.Length;
                }

                var finalMemStream = new MemoryStream();
                var finalStream = new StarboundStream(finalMemStream);

                finalStream.WriteUInt8(next.PacketId);
                finalStream.WriteSignedVLQ(length);
                finalStream.Write(buffer, 0, buffer.Length);

                byte[] toSend = finalMemStream.ToArray();

                Socket.BeginSend(toSend, 0, toSend.Length, SocketFlags.None, PacketSent, null);

                stream.Dispose();
                finalStream.Dispose();

            }
        }

        private void PacketSent(IAsyncResult iar)
        {
            try
            {
                Socket.EndSend(iar);
            }
            catch (Exception)
            {
                ForceDisconnect();
            }
        }

        #endregion

        #region Commands

        public void WarpTo(string name)
        {
            Server.PlayerClient.SendPacket(new WarpCommandPacket { Player = name, WarpType = WarpType.WarpOtherShip });
        }

        public void WarpTo(StarboundPlayer player)
        {
            WarpTo(player.Name);
        }

        public void WarpUp()
        {
            Server.PlayerClient.SendPacket(new WarpCommandPacket { WarpType = WarpType.WarpUp });
        }

        public void WarpDown()
        {
            Server.PlayerClient.SendPacket(new WarpCommandPacket { WarpType = WarpType.WarpDown });
        }

        public void MoveShip(WorldCoordinate coordinates)
        {
            Server.PlayerClient.SendPacket(new WarpCommandPacket { WarpType = WarpType.MoveShip, Coordinates = coordinates });
        }

        public void SendChatMessage(string message, int channel, string world, string name)
        {
            Server.PlayerClient.SendPacket(new ChatReceivedPacket { Message = message, Channel = (byte)channel, ClientId = 0, Name = name, World = world });
        }

        #endregion

        #region Disposal

        public void Dispose()
        {

            Dispose(true);

            GC.SuppressFinalize(this);

        }

        protected virtual void Dispose(bool disposing)
        {

            if (disposing)
            {

                if (Socket != null)
                    Socket.Close();

                if (PacketReader != null)
                    PacketReader.Dispose();

            }

            Socket = null;

        }

        #endregion

    }
}
