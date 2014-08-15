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
using System.IO;
using System.Reflection;
using System.Timers;
using log4net;
using SharpStar.Lib.Config;
using SharpStar.Lib.Database;
using SharpStar.Lib.Logging;
using SharpStar.Lib.Misc;
using SharpStar.Lib.Plugins;
using SharpStar.Lib.Server;

namespace SharpStar.Lib
{
    public sealed class SharpStarMain
    {
        private static readonly object SyncRoot = new object();

        private static SharpStarMain _instance;

        public static SharpStarMain Instance
        {
            get
            {
                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new SharpStarMain();
                    }

                    return _instance;
                }
            }
        }

        public static string AssemblyPath = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;

        private const string ConfigFile = "sharpstar.json";

        private const int DefaultListenPort = 21025;
        private const int DefaultServerPort = 21024;

        private Timer addinUpdateChecker;


        public SharpStarConfig Config { get; set; }

        public StarboundServer Server { get; private set; }

        public StarboundUDPServer UDPServer { get; private set; }

        public PluginManager PluginManager { get; set; }

        public SharpStarDb Database { get; private set; }


        public void Start()
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ConfigFile);

            if (!File.Exists(configFile))
            {
                Config = new SharpStarConfig();

                Config.ConfigFile.ListenPort = DefaultListenPort;
                Config.ConfigFile.ServerPort = DefaultServerPort;
                Config.ConfigFile.EnableAccounts = true;

                Config.ConfigFile.AllowedStarboundCommands.Add("pvp");
                Config.ConfigFile.AllowedStarboundCommands.Add("w");

                Config.Save(configFile);
            }
            else
            {
                Config = SharpStarConfig.Load(configFile);
            }

            if (string.IsNullOrEmpty(Config.ConfigFile.PythonLibLocation))
            {

                string pythonLoc = Python.GetPythonInstallDir();

                if (!string.IsNullOrEmpty(pythonLoc))
                    Config.ConfigFile.PythonLibLocation = Path.Combine(pythonLoc, "Lib");

                Config.Save(configFile);

            }

            Database = new SharpStarDb("SharpStar.db");
            Database.CreateTables();

            SharpStarLogger.DefaultLogger.Info("Listening on port {0}", Config.ConfigFile.ListenPort);

            Server = new StarboundServer(Config.ConfigFile.ListenPort, Config.ConfigFile.ServerPort);
            Server.Start();

            UDPServer = new StarboundUDPServer();
            UDPServer.Start();

            PluginManager = new PluginManager();
            PluginManager.LoadPlugins();

            if (Config.ConfigFile.AutoUpdatePlugins)
            {
                addinUpdateChecker = new Timer();
                addinUpdateChecker.Interval = TimeSpan.FromHours(2).TotalMilliseconds;
                addinUpdateChecker.Elapsed += (s, e) => PluginManager.CSPluginManager.UpdatePlugins();
            }


        }

        public void Shutdown()
        {

            if (addinUpdateChecker != null)
            {
                addinUpdateChecker.Stop();
                addinUpdateChecker.Dispose();
                addinUpdateChecker = null;
            }

            PluginManager.UnloadPlugins();

            Server.Stop();

        }
    }
}