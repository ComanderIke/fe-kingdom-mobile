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

        [SerializeField] public Vector2Int currentEncounterNodeId;
        [SerializeField] public List<int> movedEncounterIds;

        public PartyData(Party party)
        {
            SaveData(party);
        }

        public Party Load()
        {
            Party party = ScriptableObject.CreateInstance<Party>();
            party.members = new List<Unit>();

            foreach (var data in humanData)
            {
                Unit unit=data.Load();
                Debug.Log("Load UnitData!"+unit.name);
                unit.Initialize();
                party.members.Add(unit);
            }

            party.money = money;
            party.Convoy = convoy;
            party.ActiveUnitIndex = activeUnitIndex;
            return party;

        }

        public void LoadEncounterAreaData(Party party, List<Column> columns){
            int columnIndex = currentEncounterNodeId.x;
            int childIndex = currentEncounterNodeId.y;
            party.EncounterNode = columns[columnIndex].children[childIndex];
            for (int i = 0; i < movedEncounterIds.Count; i++)
            {
                party.MovedEncounters.Add(columns[i].children[movedEncounterIds[i]]);
                Debug.Log("MovedEncounters: "+columns[i].children[movedEncounterIds[i]]);
            }
        }

        public void SaveData(Party party)
        {
            if (party == null)
                return;
            if(party.EncounterNode!=null)
                currentEncounterNodeId = new Vector2Int(party.EncounterNode.depth, party.EncounterNode.childIndex);
            movedEncounterIds = new List<int>();
            foreach (var encounter in party.MovedEncounters)
            {
                movedEncounterIds.Add(encounter.childIndex);
            }

            activeUnitIndex = party.ActiveUnitIndex;
            convoy = party.Convoy;
            money = party.money;
            humanData = new List<UnitData>();
            foreach (var member in party.members)
            {
                humanData.Add(new UnitData(member));
            }
        }
    }
}