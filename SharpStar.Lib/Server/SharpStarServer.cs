﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SharpStar.Lib.Logging;
using SharpStar.Lib.Packets;
using SharpStar.Lib.Packets.Handlers;

namespace SharpStar.Lib.Server
{
    public sealed class SharpStarServer : IServer, IDisposable
    {

        private int m_numConnections;
        private BufferManager m_bufferManager;
        private Socket listenSocket;
        private SocketAsyncEventArgsPool m_readWritePool;
        private int m_numConnectedSockets;
        private Semaphore m_maxNumberAcceptedClients;

        private IPEndPoint sbServerEndPoint;

        private List<SharpStarServerClient> _clients;

        private readonly string _starboundBind = SharpStarMain.Instance.Config.ConfigFile.StarboundBind;
        private readonly string _sharpstarBind = SharpStarMain.Instance.Config.ConfigFile.SharpStarBind;

        public event EventHandler<ClientConnectedEventArgs> ClientConnected;

        public List<SharpStarServerClient> Clients
        {
            get
            {
                return _clients.ToList();
            }
        }

        public List<IPacketHandler> DefaultPacketHandlers = new List<IPacketHandler>
        {
            new UnknownPacketHandler(),
            new ProtocolVersionPacketHandler(),
            new ClientConnectPacketHandler(),
            new ClientDisconnectPacketHandler(),
            new HandshakeResponsePacketHandler(),
            new ChatSentPacketHandler(),
            new RequestDropPacketHandler(),
            new WarpCommandPacketHandler(),
            new OpenContainerPacketHandler(),
            new CloseContainerPacketHandler(),
            new DamageNotificationPacketHandler(),
            new ConnectionResponsePacketHandler(),
            new HandshakeChallengePacketHandler(),
            new DisconnectResponsePacketHandler(),
            new ChatReceivedPacketHandler(),
            new EntityInteractResultPacketHandler(),
            new UniverseTimeUpdatePacketHandler(),
            new ClientContextUpdatePacketHandler(),
            new WorldStartPacketHandler(),
            new WorldStopPacketHandler(),
            //new TileDamageUpdatePacketHandler(),
            new GiveItemPacketHandler(),
            new EnvironmentUpdatePacketHandler(),
            new EntityCreatePacketHandler(),
            new EntityUpdatePacketHandler(),
            new EntityDestroyPacketHandler(),
            new UpdateWorldPropertiesPacketHandler(),
            new HeartbeatPacketHandler(),
            new SpawnEntityPacketHandler()
        };

        public SharpStarServer(int sbPort, int numConnections, int receiveBufferSize)
        {
            m_numConnectedSockets = 0;
            m_numConnections = numConnections;
            m_bufferManager = new BufferManager(receiveBufferSize * numConnections, receiveBufferSize);

            m_readWritePool = new SocketAsyncEventArgsPool(numConnections);
            m_maxNumberAcceptedClients = new Semaphore(numConnections, numConnections);

            if (!string.IsNullOrEmpty(_starboundBind))
                sbServerEndPoint = new IPEndPoint(IPAddress.Parse(_starboundBind), sbPort);
            else
                sbServerEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), sbPort);

            _clients = new List<SharpStarServerClient>();

            Init();
        }

        public void Init()
        {
            m_bufferManager.InitBuffer();

            for (int i = 0; i < m_numConnections; i++)
            {
                SocketAsyncEventArgs readWriteEventArg = new SocketAsyncEventArgs();
                readWriteEventArg.UserToken = new AsyncUserToken();

                m_bufferManager.SetBuffer(readWriteEventArg);

                m_readWritePool.Push(readWriteEventArg);
            }
        }

        public void Start(IPEndPoint localEndPoint)
        {
            listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(localEndPoint);
            listenSocket.Listen(100);

            StartAccept(null);
        }

        public void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            try
            {
                if (acceptEventArg == null)
                {
                    acceptEventArg = new SocketAsyncEventArgs();
                    acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
                }
                else
                {
                    acceptEventArg.AcceptSocket = null;
                }

                m_maxNumberAcceptedClients.WaitOne();
                bool willRaiseEvent = listenSocket.AcceptAsync(acceptEventArg);
                if (!willRaiseEvent)
                {
                    ProcessAccept(acceptEventArg);
                }
            }
            catch
            {
            }
        }

        void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            Interlocked.Increment(ref m_numConnectedSockets);

            SocketAsyncEventArgs readEventArgs = m_readWritePool.Pop();
            ((AsyncUserToken)readEventArgs.UserToken).Socket = e.AcceptSocket;

            SharpStarLogger.DefaultLogger.Info("Connection from {0}", e.AcceptSocket.RemoteEndPoint);

            SharpStarClient client = new SharpStarClient(readEventArgs, Direction.Client);

            SharpStarServerClient ssc = new SharpStarServerClient(client);
            ssc.SClientConnected += ssc_SClientConnected;
            ssc.ClientId = m_numConnectedSockets;

            foreach (IPacketHandler packetHandler in DefaultPacketHandlers)
            {
                ssc.RegisterPacketHandler(packetHandler);
            }

            ssc.Connect(sbServerEndPoint);

            StartAccept(e);
        }

        void ssc_SClientConnected(object sender, ClientConnectedEventArgs e)
        {
            SharpStarServerClient ssc = e.Client.Server;

            _clients.Add(ssc);

            if (ClientConnected != null)
                ClientConnected(this, new ClientConnectedEventArgs(ssc.PlayerClient));

            ssc.ServerClient.ClientDisconnected += ServerClient_ClientDisconnected;
            ssc.PlayerClient.ClientDisconnected += PlayerClient_ClientDisconnected;
        }

        private void ServerClient_ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            if (e.Client.Server.Player == null) //client was unable to connect to the Starbound server
            {
                if (e.Client.Server.PlayerClient.Connected)
                {
                    //TODO: Send rejection message
                }

            }

            if (e.Client.Server != null)
            {
                _clients.Remove(e.Client.Server);
            }

            try
            {
                e.Client.Dispose();
            }
            catch
            {
            }

        }

        void PlayerClient_ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            if (e.Client.Server != null && e.Client.Server.PlayerClient != null)
            {
                if (e.Client.Server.Player == null)
                {
                    SharpStarLogger.DefaultLogger.Info("{0} has disconnected", e.Client.Server.PlayerClient.RemoteEndPoint);
                }
                else
                {
                    SharpStarLogger.DefaultLogger.Info("Player {0} ({1}) has disconnected", e.Client.Server.Player.Name, e.Client.Server.PlayerClient.RemoteEndPoint);
                }

                _clients.Remove(e.Client.Server);
            }

            try
            {
                e.Client.Dispose();
            }
            catch
            {
            }

        }


        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            foreach (SharpStarServerClient ssc in _clients)
            {
                ssc.PlayerClient.ForceDisconnect();
                ssc.ServerClient.ForceDisconnect();

                ssc.PlayerClient.Dispose();
                ssc.ServerClient.Dispose();
            }

            try
            {
                listenSocket.Shutdown(SocketShutdown.Send);
            }
            catch
            {
            }
            finally
            {
                listenSocket.Close();
                _clients.Clear();
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stop();

                m_readWritePool.Dispose();
            }

            listenSocket = null;
            m_bufferManager = null;
            m_readWritePool = null;

        }

        ~SharpStarServer()
        {
            Dispose(false);
        }

    }

    public class ClientConnectedEventArgs : EventArgs
    {

        public SharpStarClient Client { get; set; }

        public ClientConnectedEventArgs(SharpStarClient client)
        {
            Client = client;
        }

    }

    public class ClientDisconnectedEventArgs : EventArgs
    {

        public SharpStarClient Client { get; set; }

        public ClientDisconnectedEventArgs(SharpStarClient client)
        {
            Client = client;
        }

    }

}