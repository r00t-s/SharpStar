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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mono.Addins;
using Mono.Addins.Setup;
using SharpStar.Lib.Attributes;
using SharpStar.Lib.Logging;
using SharpStar.Lib.Mono;
using SharpStar.Lib.Packets;
using SharpStar.Lib.Server;

[assembly: AddinRoot("SharpStar.Lib", "1.0")]

namespace SharpStar.Lib.Plugins
{
    public sealed class CSPluginManager
    {

        private static readonly SharpStarLogger Logger = SharpStarLogger.DefaultLogger;

        private readonly object _commandLocker = new object();

        public readonly List<Tuple<string, string, string, bool>> Commands;

        public readonly Dictionary<string, string> ConsoleCommands;

        private readonly Dictionary<string, ICSPlugin> _csPlugins;

        private const string AddinRepoUrl = "http://sharpstar.org/sharpstar-addins";

        private readonly SetupService setupService;

        public List<ICSPlugin> Plugins
        {
            get
            {
                return _csPlugins.Values.ToList();
            }
        }

        public const string CSPluginDirectory = "addins";

        public CSPluginManager()
        {
            _csPlugins = new Dictionary<string, ICSPlugin>();
            Commands = new List<Tuple<string, string, string, bool>>();
            ConsoleCommands = new Dictionary<string, string>();

            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolved;

            if (!AddinManager.IsInitialized)
            {
                AddinManager.Initialize(".", "./addins");
                AddinManager.AddExtensionNodeHandler(typeof(ICSPlugin), OnExtensionChanged);
            }

            setupService = new SetupService(AddinManager.Registry);

            if (!setupService.Repositories.ContainsRepository(AddinRepoUrl))
                setupService.Repositories.RegisterRepository(new ConsoleProgressStatus(false), AddinRepoUrl, false);

            setupService.Repositories.UpdateAllRepositories(new ConsoleProgressStatus(false));

        }

        private static Assembly AssemblyResolved(object sender, ResolveEventArgs args)
        {
            var asm = (from a in AppDomain.CurrentDomain.GetAssemblies()
                       where a.GetName().FullName == args.Name
                       select a).FirstOrDefault();

            return asm;
        }

        public void LoadPlugins()
        {
            if (!Directory.Exists(CSPluginDirectory))
                Directory.CreateDirectory(CSPluginDirectory);

            AddinManager.Registry.Update(null);

            if (SharpStarMain.Instance.Config.ConfigFile.AutoUpdatePlugins)
            {
                UpdatePlugins();
            }

            foreach (Addin addin in AddinManager.Registry.GetAddins())
            {
                var property = addin.Properties.SingleOrDefault(p => p.Name.Equals("sharpstar", StringComparison.OrdinalIgnoreCase));

                if (property != null)
                {
                    Version ver = Assembly.GetEntryAssembly().GetName().Version;

                    string verStr = String.Format("{0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision);

                    if (Addin.CompareVersions(verStr, property.Value) <= 0)
                    {
                        AddinManager.Registry.EnableAddin(addin.Id);
                    }
                    else
                    {
                        Logger.Error("Plugin {0} requires SharpStar version {1}+. Load failed!", addin.Description.LocalId, property.Value);
                    }
                }
                else
                {
                    Logger.Error("Plugin {0} does not define a minimum SharpStar version requirement. Update failed!", addin.Description.LocalId);
                }
            }

        }

        public void UpdatePlugins()
        {

            foreach (Addin addin in AddinManager.Registry.GetAddins())
            {
                Addin refreshedAddin = AddinManager.Registry.GetAddin(addin.Id);

                UpdatePlugin(refreshedAddin.Description.LocalId);
            }

            AddinManager.Registry.Update();
        }

        public bool UpdatePlugin(string name)
        {

            setupService.Repositories.UpdateAllRepositories(new ProgressStatus());

            Addin addin = AddinManager.Registry.GetAddins().SingleOrDefault(p => p.Description.LocalId.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (addin == null)
            {
                Logger.Error("The plugin {0} is not installed!", name);

                return false;
            }

            AddinRepositoryEntry[] entries = setupService.Repositories.GetAvailableAddinUpdates(addin.LocalId, RepositorySearchFlags.LatestVersionsOnly);

            if (entries.Any())
            {
                var entry = entries.First();

                var property = entry.Addin.Properties.SingleOrDefault(p => p.Name.Equals("sharpstar", StringComparison.OrdinalIgnoreCase));

                if (property != null)
                {

                    Version ver = Assembly.GetEntryAssembly().GetName().Version;

                    string verStr = String.Format("{0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision);

                    if (Addin.CompareVersions(verStr, property.Value) <= 0)
                    {
                        Logger.Info("Plugin {0} is now updating to version {1}!", addin.Description.LocalId, entry.Addin.Version);

                        setupService.Install(new ProgressStatus(), entries);

                        AddinManager.Registry.EnableAddin(entry.Addin.Id);
                        AddinManager.Registry.Update();
                    }
                    else
                    {
                        Logger.Error("Plugin {0} now requires SharpStar version {1}+. Update failed!", addin.Description.LocalId, property.Value);
                    }

                    return true;
                }
                else
                {
                    Logger.Error("Plugin {0} does not define a minimum SharpStar version requirement. Update failed!", addin.Description.LocalId);
                }
            }

            return false;
        }

        public void InstallPlugin(string name)
        {

            Addin addin = AddinManager.Registry.GetAddins().SingleOrDefault(p => p.Description.LocalId.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (addin != null)
            {
                Logger.Error("Plugin {0} is already installed!");

                return;
            }

            var addins = setupService.Repositories.GetAvailableAddins(RepositorySearchFlags.LatestVersionsOnly).Where(p => p.Addin.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToArray();

            if (addins.Any())
            {
                AddinRepositoryEntry avAddin = addins.First();

                var property = avAddin.Addin.Properties.SingleOrDefault(p => p.Name.Equals("sharpstar", StringComparison.OrdinalIgnoreCase));

                if (property != null)
                {
                    Version ver = Assembly.GetEntryAssembly().GetName().Version;

                    string verStr = String.Format("{0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision);

                    if (Addin.CompareVersions(verStr, property.Value) <= 0)
                    {
                        setupService.Install(new ProgressStatus(), addins);

                        AddinManager.Registry.EnableAddin(avAddin.Addin.Id);
                    }
                    else
                    {
                        Logger.Error("Plugin {0} now requires SharpStar version {1}+. Update failed!", avAddin.Addin.Id, property.Value);
                    }
                }
                else
                {
                    Logger.Error("Plugin {0} does not define a minimum SharpStar version requirement. Installation failed!", avAddin.Addin.Id);
                }
            }
            else
            {
                Logger.Error("Could not find plugin by the name \"{0}\"", name);
            }
        }

        public void UninstallPlugin(string name)
        {
            Addin addin = AddinManager.Registry.GetAddins().SingleOrDefault(p => p.Description.LocalId.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (addin != null)
            {
                setupService.Uninstall(new ProgressStatus(), addin.Id);
                AddinManager.Registry.Update();
            }
            else
            {
                Logger.Error("The plugin {0} is not installed", name);
            }
        }

        public async Task CallEvent(IPacket packet, SharpStarClient client, bool isAfter = false)
        {
            foreach (ICSPlugin csPlugin in _csPlugins.Values.ToList())
            {
                await csPlugin.OnEventOccurred(packet, client, isAfter);
            }
        }

        public async Task<bool> PassChatCommand(SharpStarClient client, string command, string[] args)
        {
            bool result = false;

            foreach (ICSPlugin plugin in _csPlugins.Values)
            {
                if (await plugin.OnChatCommandReceived(client, command, args).ConfigureAwait(false))
                    result = true;
            }

            return result;
        }

        public bool PassConsoleCommand(string command, string[] args)
        {
            bool result = false;

            foreach (ICSPlugin plugin in _csPlugins.Values)
            {
                if (plugin.OnConsoleCommand(command, args))
                    result = true;
            }

            return result;
        }

        public void UnloadPlugins()
        {
            if (AddinManager.IsInitialized)
            {
                foreach (ICSPlugin plugin in _csPlugins.Values)
                {
                    plugin.OnUnload();

                    Parallel.ForEach(_csPlugins.Values.Except(new[] { plugin }), otherPlugin => otherPlugin.OnPluginUnloaded(plugin));
                }

                foreach (Addin addin in AddinManager.Registry.GetAddins())
                {
                    AddinManager.Registry.DisableAddin(addin.Id);
                }
            }
        }

        private void OnExtensionChanged(object sender, ExtensionNodeEventArgs args)
        {

            var plugin = args.ExtensionObject as ICSPlugin;

            if (plugin != null)
            {
                if (args.Change == ExtensionChange.Add)
                {

                    plugin.OnLoad();

                    Parallel.ForEach(_csPlugins.Values, p => p.OnPluginLoaded(plugin));

                    _csPlugins.Add(Addin.GetFullId(null, args.ExtensionNode.Addin.Id, args.ExtensionNode.Addin.Version), plugin);

                    Logger.Info("Loaded CSharp Plugin \"{0}\"", plugin.Name);

                }
                else if (args.Change == ExtensionChange.Remove)
                {

                    plugin.OnUnload();

                    Parallel.ForEach(_csPlugins.Values, p => p.OnPluginUnloaded(plugin));

                    _csPlugins.Remove(Addin.GetFullId(null, args.ExtensionNode.Addin.Id, args.ExtensionNode.Addin.Version));

                    Logger.Info("Unloaded CSharp Plugin \"{0}\"", plugin.Name);

                }

                RefreshCommands();

            }

        }

        public void LoadPlugin(ICSPlugin plugin)
        {

            var plugins = _csPlugins.Where(p => p.Value.Equals(plugin)).ToList();

            if (plugins.Count > 1)
            {
                Logger.Error("Error loading plugin \"{0}\"", plugin.Name);
            }
            else if (plugins.Count > 0)
            {
                AddinManager.Registry.EnableAddin(plugins[0].Key);
            }

        }

        public void UnloadPlugin(ICSPlugin plugin)
        {
            var plugins = _csPlugins.Where(p => p.Value.Equals(plugin)).ToList();

            if (plugins.Count > 1)
            {
                Logger.Error("Error unloading plugin \"{0}\"", plugin.Name);
            }
            else if (plugins.Count > 0)
            {
                AddinManager.Registry.DisableAddin(plugins[0].Key);
            }
        }

        private void RefreshCommands()
        {
            lock (_commandLocker)
            {
                Commands.Clear();
                ConsoleCommands.Clear();

                var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                foreach (var assm in assemblies.Where(p => p.GetTypes().Any(x => typeof(ICSPlugin).IsAssignableFrom(x))))
                {
                    Type[] types = assm.GetTypes();

                    foreach (Type type in types)
                    {

                        foreach (MethodInfo mi in type.GetMethods())
                        {

                            var cmdAttribs = mi.GetCustomAttributes(typeof(CommandAttribute), false).ToList();
                            var consoleCmdAttribs = mi.GetCustomAttributes(typeof(ConsoleCommandAttribute), false).ToList();

                            if (consoleCmdAttribs.Count == 1)
                            {
                                ConsoleCommandAttribute cCmdAttr = (ConsoleCommandAttribute)cmdAttribs[0];

                                ConsoleCommands.Add(cCmdAttr.CommandName, cCmdAttr.CommandDescription);
                            }
                            else if (cmdAttribs.Count == 1)
                            {
                                var permAttribs = mi.GetCustomAttributes(typeof(CommandPermissionAttribute), false).ToList();

                                CommandAttribute cmdAttrib = (CommandAttribute)cmdAttribs[0];

                                if (permAttribs.Count == 1)
                                {
                                    CommandPermissionAttribute permAttrib = (CommandPermissionAttribute)permAttribs[0];

                                    Commands.Add(Tuple.Create(cmdAttrib.CommandName, cmdAttrib.CommandDescription, permAttrib.Permission, permAttrib.Admin));
                                }
                                else
                                {
                                    Commands.Add(Tuple.Create(cmdAttrib.CommandName, cmdAttrib.CommandDescription, String.Empty, false));
                                }
                            }

                        }

                    }

                }
            }
        }

    }
}
