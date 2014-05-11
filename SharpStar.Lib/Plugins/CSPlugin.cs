﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SharpStar.Lib.Packets;
using SharpStar.Lib.Server;

namespace SharpStar.Lib.Plugins
{
    public abstract class CSPlugin : ICSPlugin
    {

        public abstract string Name { get; }

        private readonly Dictionary<string, Action<IPacket, StarboundClient>> _registeredEvents;

        private readonly Dictionary<string, Action<StarboundClient, string[]>> _registeredCommands;

        public readonly Dictionary<object, Dictionary<Tuple<string, string>, Action<StarboundClient, string[]>>> RegisteredCommandObjects;

        public readonly Dictionary<object, Dictionary<string, Action<IPacket, StarboundClient>>> RegisteredEventObjects;

        protected CSPlugin()
        {
            _registeredEvents = new Dictionary<string, Action<IPacket, StarboundClient>>();
            _registeredCommands = new Dictionary<string, Action<StarboundClient, string[]>>();
            RegisteredCommandObjects = new Dictionary<object, Dictionary<Tuple<string, string>, Action<StarboundClient, string[]>>>();
            RegisteredEventObjects = new Dictionary<object, Dictionary<string, Action<IPacket, StarboundClient>>>();
        }

        public virtual void OnLoad()
        {
        }

        public virtual void OnUnload()
        {
        }

        public void RegisterEvent(string name, Action<IPacket, StarboundClient> toCall)
        {

            if (!_registeredEvents.ContainsKey(name))
            {
                _registeredEvents.Add(name, toCall);
            }

        }

        public void RegisterCommandObject(object obj)
        {

            if (!RegisteredCommandObjects.ContainsKey(obj))
            {

                var dict = new Dictionary<Tuple<string, string>, Action<StarboundClient, string[]>>();

                MethodInfo[] methods = obj.GetType().GetMethods();

                foreach (var mi in methods)
                {

                    var attribs = mi.GetCustomAttributes(typeof(CommandAttribute), false).ToList();

                    if (attribs.Count == 1)
                    {

                        CommandAttribute attrib = (CommandAttribute)attribs[0];

                        dict.Add(Tuple.Create(attrib.CommandName, attrib.CommandDescription), (Action<StarboundClient, string[]>)Delegate.CreateDelegate(typeof(Action<StarboundClient, string[]>), obj, mi));

                    }

                }

                RegisteredCommandObjects.Add(obj, dict);

            }

        }

        public void RegisterEventObject(object obj)
        {

            if (!RegisteredEventObjects.ContainsKey(obj))
            {

                var dict = new Dictionary<string, Action<IPacket, StarboundClient>>();

                MethodInfo[] methods = obj.GetType().GetMethods();

                foreach (var mi in methods)
                {

                    var attribs = mi.GetCustomAttributes(typeof(EventAttribute), false).ToList();

                    if (attribs.Count == 1)
                    {

                        EventAttribute attrib = (EventAttribute)attribs[0];

                        var act = (Action<IPacket, StarboundClient>)Delegate.CreateDelegate(typeof(Action<IPacket, StarboundClient>), obj, mi);

                        foreach (string evt in attrib.EventNames)
                            dict.Add(evt, act);

                    }

                }

                RegisteredEventObjects.Add(obj, dict);

            }

        }

        public void RegisterCommand(string name, Action<StarboundClient, string[]> toCall)
        {

            if (!_registeredCommands.ContainsKey(name))
            {
                _registeredCommands.Add(name, toCall);
            }

        }

        public void UnregisterCommand(string name)
        {

            if (_registeredCommands.ContainsKey(name))
            {
                _registeredCommands.Remove(name);
            }

        }

        public virtual void OnEventOccurred(string evtName, IPacket packet, StarboundClient client, params object[] args)
        {

            if (_registeredEvents.ContainsKey(evtName))
            {
                _registeredEvents[evtName](packet, client);
            }

            foreach (var kvp in RegisteredEventObjects)
            {

                var val = kvp.Value;

                if (val.ContainsKey(evtName))
                {
                    val[evtName](packet, client);
                }

            }

        }

        public virtual bool OnChatCommandReceived(StarboundClient client, string command, string[] args)
        {

            var cmd = _registeredCommands.SingleOrDefault(p => p.Key.Equals(command, StringComparison.OrdinalIgnoreCase));

            bool contained = false;

            foreach (var kvp in RegisteredCommandObjects)
            {

                var val = kvp.Value.SingleOrDefault(p => p.Key.Item1.Equals(command, StringComparison.OrdinalIgnoreCase));

                if (val.Value != null)
                {

                    val.Value(client, args);

                    contained = true;

                }

            }

            if (cmd.Value != null)
            {

                cmd.Value(client, args);

                contained = true;

            }

            return contained;

        }
    }
}
