﻿using System;
using System.Collections.Generic;
using Packages.pack.guerro.shared.Scripts.Clients;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.iOS;

namespace Packet.Guerro.Shared.Inputs
{    
    public partial class CInputManager : ComponentSystem
    {        
        // -------- -------- -------- -------- -------- -------- -------- -------- -------- /.
        // Register
        // -------- -------- -------- -------- -------- -------- -------- -------- -------- /.
        public void RegisterFromFile(string path)
        {
            
        }

        public void RegisterFromString(string @string)
        {
            
        }

        public void RegisterFromList(List<InputSettingBase> informationMap)
        {
            for (int i = 0; i != informationMap.Count; i++)
            {
                RegisterSingle(informationMap[i]);
            }
        }

        public void RegisterSingle(InputSettingBase setting)
        {
            var id = GetStockIdInternal(setting.NameId);
            var map = new Map
            {
                Id = id,
                NameId = setting.NameId,
                DefaultSettings = setting
            };

            switch (setting)
            {
                case Settings.Axis1D axis1D:
                {
                    map.SettingType = Settings.EType.Axis1D;
                    break;
                }
                case Settings.Axis2D axis2D:
                {
                    map.SettingType = Settings.EType.Axis2D;
                    break;
                }
                case Settings.Push push:
                {
                    map.SettingType = Settings.EType.Push;
                    break;
                }
            }

            s_Maps[id] = map;
        }
        
        // -------- -------- -------- -------- -------- -------- -------- -------- -------- /.
        // Get
        // -------- -------- -------- -------- -------- -------- -------- -------- -------- /.
        public int GetId(string inputMapName)
        {
            return s_MapsStringLookup[inputMapName];
        }

        public void Get<TInput>(int id)
        {
        }

        public ref Map GetMap(int id)
        {
            return ref s_Maps.RefGet(id);
        }

        public Map GetMap(string nameId)
        {
            return GetMap(s_MapsStringLookup[nameId]);
        }

        protected override void OnCreateManager(int capacity)
        {
            s_Maps = new FastDictionary<int, Map>();
            s_MapsStringLookup = new FastDictionary<string, int>();
        }

        protected override void OnUpdate()
        {
            
        }

        protected override void OnDestroyManager()
        {
            s_Maps.Clear();
            s_MapsStringLookup.Clear();

            s_Maps = null;
            s_MapsStringLookup = null;
        }
    }
    
    // -------- -------- -------- -------- -------- -------- -------- -------- -------- /.
    // Internals
    // -------- -------- -------- -------- -------- -------- -------- -------- -------- /.  
    public partial class CInputManager
    {
        private int GetStockIdInternal(string nameId)
        {
            if (s_MapsStringLookup.ContainsKey(nameId))
            {
                throw new InputAlreadyRegisteredException();
            }
            
            s_MapsStringLookup[nameId] = s_Maps.Count;
            
            return s_Maps.Count;
        }
    }

    // -------- -------- -------- -------- -------- -------- -------- -------- -------- /.
    // Statics
    // -------- -------- -------- -------- -------- -------- -------- -------- -------- /.    
    public partial class CInputManager
    {
        private static FastDictionary<string, int> s_MapsStringLookup;
        private static FastDictionary<int, Map> s_Maps;
    }
    
    // -------- -------- -------- -------- -------- -------- -------- -------- -------- /.
    // Sub-classes
    // -------- -------- -------- -------- -------- -------- -------- -------- -------- /.  
    public partial class CInputManager
    {
        public struct Map : IEquatable<Map>
        {
            public string NameId;
            public int Id;
            
            public InputSettingBase DefaultSettings;

            public Settings.EType SettingType;

            //< -------- -------- -------- -------- -------- -------- -------- ------- //
            // IEquatable methods
            //> -------- -------- -------- -------- -------- -------- -------- ------- //
            public bool Equals(Map other)
            {
                return Id == other.Id;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is Map && Equals((Map) obj);
            }

            public override int GetHashCode()
            {
                return Id;
            }
        }
        
        public class InputAlreadyRegisteredException : Exception
        {
        }
    }
}