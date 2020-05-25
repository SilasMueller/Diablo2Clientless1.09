﻿using D2NG.D2GS.Act.Packet;
using D2NG.D2GS.Objects;
using D2NG.D2GS.Packet;
using Serilog;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace D2NG.D2GS.Act
{
    class ActData
    {
        public Act Act { get; set; }
        public uint MapId { get; set; }
        public Area Area { get; set; }

        private readonly ConcurrentDictionary<Act, List<Tile>> _tiles = new ConcurrentDictionary<Act, List<Tile>>();
        private readonly ConcurrentDictionary<Act, ConcurrentDictionary<(uint, EntityType), WorldObject>> _worldObjects
            = new ConcurrentDictionary<Act, ConcurrentDictionary<(uint, EntityType), WorldObject>>();

        public List<Tile> Tiles { get => _tiles.GetOrAdd(Act, new List<Tile>()); }

        public ConcurrentDictionary<(uint, EntityType), WorldObject> WorldObjects { get => _worldObjects.GetOrAdd(Act, new ConcurrentDictionary<(uint, EntityType), WorldObject>()); }
        
        internal void LoadActData(ActDataPacket packet)
        {
            Act = packet.Act;
            Area = packet.Area;
            MapId = packet.MapId;
        }

        internal void UpdateNPCLocation(uint entityId, Point location)
        {
            var npc = WorldObjects[(entityId,EntityType.NPC)];
            if(npc != null)
            {
                npc.Location = location;
            }
        }

        internal void UpdateNPCState(NpcStatePacket packet)
        {
            var npc = WorldObjects[(packet.EntityId,EntityType.NPC)];
            npc.State = packet.EntityState;
            if (packet.EntityState == EntityState.BeingHit || packet.EntityState == EntityState.Dead)
            {
                npc.Location = packet.Location;
            }
        }
        internal void AddNPC(AssignNpcPacket packet)
        {
            var npc = new WorldObject(EntityType.NPC, packet.EntityId, 0, packet.Location, EntityState.Alive, 0);
            npc.NPCCode = packet.UniqueCode;
            AddWorldObject(npc);
        }

        internal void RemoveWorldObject(uint entityId, EntityType entityType)
        {
            WorldObjects.Remove((entityId,entityType), out var value);
        }

        internal void AddWorldObject(WorldObject obj)
        {
            WorldObjects[(obj.Id, obj.Type)] = obj;
        }

        internal void HandleMapRevealPacket(MapRevealPacket p)
        {
            Area = p.Area;
            var tile = new Tile(p.X, p.Y, p.Area);
            if(!Tiles.Any(item => item.Equals(tile)))
            {
                lock(Tiles)
                {
                    Tiles.Add(tile);
                    foreach(var t in Tiles)
                    {
                        if(t.IsNorthOf(tile))
                        {
                            tile.North = t;
                            t.South = tile;
                        }
                        if(t.IsEastOf(tile))
                        {
                            tile.East = t;
                            t.West = tile;
                        }
                        if (t.IsSouthOf(tile))
                        {
                            tile.South = t;
                            t.North = tile;
                        }
                        if (t.IsWestOf(tile))
                        {
                            tile.West = t;
                            t.East = tile;
                        }
                    }
                }
            }
        }
    }
}
