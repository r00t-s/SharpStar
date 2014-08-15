﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpStar.Lib.Server
{
    public class StarboundUDPServer : IServer
    {

        private readonly UdpClient udpServer;
        private readonly UdpClient udpClient;

        private CancellationTokenSource tokenSource;

        private readonly IPAddress sharpStarBind;
        private readonly IPAddress starboundBind;

        private readonly int serverPort;
        private readonly int listenPort;

        private volatile bool isRunning;

        private readonly object locker = new object();

        public StarboundUDPServer()
        {
            string shBind = SharpStarMain.Instance.Config.ConfigFile.SharpStarBind;

            if (shBind == "*")
                sharpStarBind = IPAddress.Any;
            else
                sharpStarBind = IPAddress.Parse(shBind);

            string sbBind = SharpStarMain.Instance.Config.ConfigFile.StarboundBind;

            if (string.IsNullOrEmpty(sbBind))
                starboundBind = IPAddress.Parse("127.0.0.1");
            else
                starboundBind = IPAddress.Parse(sbBind);

            serverPort = SharpStarMain.Instance.Config.ConfigFile.ServerPort;
            listenPort = SharpStarMain.Instance.Config.ConfigFile.ListenPort;

            udpClient = new UdpClient();
            udpServer = new UdpClient(listenPort);
        }

        public void Start()
        {
            if (tokenSource != null)
            {
                Stop();
            }

            new Thread(() =>
            {

                var sIpe = new IPEndPoint(sharpStarBind, listenPort);
                var cIpe = new IPEndPoint(starboundBind, serverPort);

                udpClient.Connect(cIpe);

                lock (locker)
                {
                    isRunning = true;
                }

                while (true)
                {

                    lock (locker)
                    {
                        if (!isRunning)
                            break;
                    }

                    byte[] clientData = udpServer.Receive(ref sIpe);

                    udpClient.Send(clientData, clientData.Length);

                    byte[] recvData = udpClient.Receive(ref cIpe);

                    udpServer.Send(recvData, recvData.Length, sIpe);
                }


            }).Start();

        }

        public void Stop()
        {
            lock (locker)
            {
                isRunning = false;
            }
        }
    }
}
