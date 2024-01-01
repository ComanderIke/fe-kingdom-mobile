﻿using System.Collections.Generic;
using UnityEngine;

namespace Game.WorldMapStuff.Model
{
    public class EncounterPosition
    {
        public List<string> MovedEncounterIds { get; set; }
        public string EncounterNodeId { get; set; }

        private EncounterNode encounterNode;
        public EncounterNode EncounterNode
        {
            get
            {
                if (encounterNode == null)
                {
                    encounterNode = EncounterTree.Instance.GetEncounterNodeById(EncounterNodeId);
                }
                return encounterNode;
            }
            set
            {
                encounterNode = value;
                if(encounterNode!=null)
                    EncounterNodeId = encounterNode.GetId();
                else
                {
                    EncounterNodeId = "";
                }
            }
        }

        public bool activatedEncounter = false;
      

        public void AddMovedEncounter(EncounterNode node)
        {
           
           MovedEncounterIds.Add(node.GetId());
            // MovedEncounterIds.Add(node.GetId());DONT DO THIS ENDLESS LOOP
        }
        public EncounterPosition()
        {
            
            MovedEncounterIds = new List<string>();
        }

        public void RemoveMovedEncounterAt(int i)
        {
           
            MovedEncounterIds.RemoveAt(i);
        }

        public void Reset()
        {
            MovedEncounterIds.Clear();
            activatedEncounter = false;
            EncounterNode = null;
        }
    }
}