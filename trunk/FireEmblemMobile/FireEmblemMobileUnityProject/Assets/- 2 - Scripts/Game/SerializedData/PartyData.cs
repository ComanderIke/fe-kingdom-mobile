using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Items;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Monsters;
using Game.Systems;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Players
{
    [System.Serializable]
    public class PartyData
    {
        [SerializeField]
        public List<HumanData> humanData;
        [SerializeField]
        public int money;
        [SerializeField]
        public Convoy convoy;
        [SerializeField]
        public int activeUnitIndex;
        [SerializeField]
        public Vector2Int currentEncounterNodeId;
        [SerializeField]
        public List<int> movedEncounterIds;

        public PartyData(Party party)
        {
            SaveData(party);
            
        }

        public void Load(Party party)
        {
            // party.name = name;
            // party.members = new List<Unit>();
            // party.location = WorldMapGameManager.Instance.World.Locations.FirstOrDefault(l=> l.UniqueId==locationId);
            //
            //
            // // Debug.Log(party.members[0].name);
            // // Debug.Log("loading Party: "+party.members[0].name+" "+party.location.worldMapPosition.name);
            // party.TurnStateManager = turnStateManager;
            // foreach (var data in humanData)
            // {
            //     var unit = ScriptableObject.CreateInstance<Human>();
            //     data.Load(unit);
            //     unit.Initialize();
            //     party.members.Add(unit);
            //
            // }
            // foreach (var data in monsterData)
            // {
            //     var unit = ScriptableObject.CreateInstance<Monster>();
            //     data.Load(unit);
            //     unit.Initialize();
            //     party.members.Add(unit);
            // }
        }

        public void SaveData(Party party)
        {
            if (party == null)
                return;
            currentEncounterNodeId = new Vector2Int(party.EncounterNode.depth, party.EncounterNode.childIndex);
            movedEncounterIds = new List<int>();
            foreach (var encounter in party.MovedEncounters)
            {
                movedEncounterIds.Add(encounter.childIndex);
            }

            convoy = party.Convoy;
            money = party.money;
            humanData = new List<HumanData>();
            foreach (var member in party.members)
            {
                if (member is Human human)
                {
                    humanData.Add(new HumanData(human));
                }
            }
            // name = party.name;
            // humanData = new List<HumanData>();
            // monsterData = new List<MonsterData>();
            // // Debug.Log("Saving Party: " +party.name);
            // // Debug.Log(party.members.Count);
            // // Debug.Log(party.members[0].name);
            // // Debug.Log(party.location.name);
            // foreach (var member in party.members)
            // {
            //     if (member is Human human)
            //     {
            //         humanData.Add(new HumanData(human));
            //     }
            //
            //     if (member is Monster monster)
            //     {
            //         monsterData.Add(new MonsterData(monster));
            //     }
            // }
            //
            // turnStateManager = party.TurnStateManager;
            // locationId = party.location.UniqueId;
        }
    }
}