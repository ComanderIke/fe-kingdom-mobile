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
        [SerializeField] public Convoy convoy;
        [SerializeField] public int activeUnitIndex;

        [SerializeField] public string currentEncounterNodeId;
        [SerializeField] public List<string> movedEncounterIds;
        public bool activatedEncounter;

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
            Debug.Log("Save PartyData: "+ party.EncounterComponent.MovedEncounterIds.Count);
            foreach (var encounter in party.EncounterComponent.MovedEncounterIds)
            {
                movedEncounterIds.Add(encounter);
            }

            activeUnitIndex = party.ActiveUnitIndex;
            convoy = party.Convoy;
            money = party.Money;
            humanData = new List<UnitData>();
            
            foreach (var member in party.members)
            {
                humanData.Add(new UnitData(member));
            }
        }
    }
}