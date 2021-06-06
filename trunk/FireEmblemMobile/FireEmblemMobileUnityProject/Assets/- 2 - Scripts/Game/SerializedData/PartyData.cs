using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Monsters;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Players
{
    [System.Serializable]
    public class PartyData
    {
        [SerializeField]
        public string name;
        [SerializeField]
        public List<HumanData> humanData;
        [SerializeField]
        public List<MonsterData> monsterData;

        [SerializeField] public string locationId;
        public PartyData(Party party)
        {
            name = party.name;
            humanData = new List<HumanData>();
            monsterData = new List<MonsterData>();
            foreach (var member in party.members)
            {
                if (member is Human human)
                {
                    humanData.Add(new HumanData(human));
                    Debug.Log("Save as Human");
                }

                if (member is Monster monster)
                {
                    monsterData.Add(new MonsterData(monster));
                    Debug.Log("Save as Monster");
                }
            }

            locationId = party.location.UniqueId;
        }

        public void Load(Party party)
        {
            party.name = name;
            party.members = new List<Unit>();
            party.location = WorldMapGameManager.Instance.World.Locations.FirstOrDefault(l=> l.UniqueId==locationId);
            foreach (var data in humanData)
            {
                Debug.Log("Load PartyMember: "+data);
               
                    Debug.Log("Load Human: "+data);
                    var unit = ScriptableObject.CreateInstance<Human>();
                    data.Load(unit);
                    party.members.Add(unit);
                
              
             
               
            }
            foreach (var data in monsterData)
            {
                Debug.Log("Load PartyMember: "+data);
               
                Debug.Log("Load Human: "+data);
                var unit = ScriptableObject.CreateInstance<Monster>();
                data.Load(unit);
                party.members.Add(unit);
                
              
             
               
            }
        }
    }
}