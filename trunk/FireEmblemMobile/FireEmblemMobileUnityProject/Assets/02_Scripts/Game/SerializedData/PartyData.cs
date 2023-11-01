using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Items;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Monsters;
using Game.Systems;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Players
{
    [System.Serializable]
    public class PartyData
    {
        [SerializeField] public List<UnitData> humanData;
        [SerializeField] public int money;
        [SerializeField] public ConvoyData convoy;
        [SerializeField] public int activeUnitIndex;

        [SerializeField] public string currentEncounterNodeId;
        [SerializeField] public List<string> movedEncounterIds;
        public bool activatedEncounter;
        [SerializeField]public ConvoyData storage;
        public List<string> visitedEvents;
        public List<string> visitedMaps;
        public int areaIndex;

        public PartyData(Party party)
        {
            SaveData(party);
        }
        

        // public void LoadEncounterAreaData(Party party, List<Column> columns){
        //     int columnIndex = currentEncounterNodeId.x;
        //     int childIndex = currentEncounterNodeId.y;
        //     party.EncounterNode = columns[columnIndex].children[childIndex];
        //     for (int i = 0; i < movedEncounterIds.Count; i++)
        //     {
        //         party.MovedEncounters.Add(columns[i].children[movedEncounterIds[i]]);
        //         Debug.Log("MovedEncounters: "+columns[i].children[movedEncounterIds[i]]);
        //     }
        // }

        public void SaveData(Party party)
        {
            if (party == null)
                return;
            currentEncounterNodeId =party.EncounterComponent.EncounterNodeId;
            movedEncounterIds = new List<string>();
            activatedEncounter = party.EncounterComponent.activatedEncounter;
           
            foreach (var encounter in party.EncounterComponent.MovedEncounterIds)
            {
                movedEncounterIds.Add(encounter);
            }

            visitedEvents = new List<string>();
            foreach (var id in party.VisitedEvents)
            {
                visitedEvents.Add(id.name);
            }
            visitedMaps = new List<string>();
            foreach (var id in party.VisitedMaps)
            {
                visitedMaps.Add(id.name);
            }
            activeUnitIndex = party.ActiveUnitIndex;
            convoy = new ConvoyData(party.Convoy);
            storage = new ConvoyData(party.Storage);
            money = party.Money;
            areaIndex = party.AreaIndex;
            humanData = new List<UnitData>();
            
            foreach (var member in party.members)
            {
                humanData.Add(new UnitData(member));
            }
        }
    }
}