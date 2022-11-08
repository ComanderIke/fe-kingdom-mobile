using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Game.GameActors.Items;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
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
                return encounterNode;
            }
            set
            {
                encounterNode = value;
                EncounterNodeId = encounterNode.GetId();
            }
        }

        private List<EncounterNode> movedEncounters;

        public IReadOnlyList<EncounterNode> MovedEncounters
        {
            get
            {
                return movedEncounters.AsReadOnly();
            }
        }

        public void AddMovedEncounter(EncounterNode node)
        {
            movedEncounters.Add(node);
            MovedEncounterIds.Add(node.GetId());
        }
        public EncounterPosition()
        {
            movedEncounters = new List<EncounterNode>();
            MovedEncounterIds = new List<string>();
        }

        public void RemoveMovedEncounterAt(int i)
        {
            movedEncounters.RemoveAt(i);
            MovedEncounterIds.RemoveAt(i);
        }
    }
    [CreateAssetMenu(fileName = "Party", menuName = "GameData/Party")]
    public class Party:ScriptableObject
    {

        public event Action<int> onGoldChanged;
        public event Action<int> onSmithingStonesChanged;
        [SerializeField] private bool isPlayerControlled;
        [SerializeField] public List<Unit> members;
        public static Action<Party> PartyDied;
        [SerializeField] int maxSize = 4;
        [SerializeField] public int money = default;

        
        public int MaxSize
        {
            get => maxSize;
            set => maxSize = value;
        }

        public Convoy Convoy;
        int activeUnitIndex=0;
        public int ActiveUnitIndex
        {
            get
            {
                return activeUnitIndex;
            }
            set
            {
                if (value >= members.Count)
                    activeUnitIndex = 0;
                else if (value < 0)
                    activeUnitIndex = members.Count - 1;
                else
                {
                    activeUnitIndex = value;
                }
            }
        }
        public int smithingStones = 2;
        public event Action<Unit> onMemberRemoved;
        public event Action<Unit> onMemberAdded;

        public bool IsPlayerControlled => isPlayerControlled;
    
        public int SmithingStones
        {
            get
            {
                return smithingStones;
            }
            set
            {
                smithingStones = value;
                onSmithingStonesChanged?.Invoke(smithingStones);
            }
        }
        public int Money
        {
            get
            {
                return money;
            }
            set
            {
                money = value;
                onGoldChanged?.Invoke(money);
            }
        }

        public Party():base()
        {
            members = new List<Unit>();
            Convoy = new Convoy();
            EncounterComponent = new EncounterPosition();
        }

        public EncounterPosition EncounterComponent{ get; set; }
     
        public GameObject GameObject { get; set; }
   

        public Unit ActiveUnit
        {
            get { return members[ActiveUnitIndex]; }
        }

      

        public bool IsAlive()
        {
            return members.Count(a => a.IsAlive()) != 0;
        }

        public void Initialize()
        {
            Debug.Log("Init Party");
            foreach (var member in members)
            {
                member.Initialize();
                member.Party = this;
            }
        }

        public override string ToString()
        {
            string party = "";
            
            party += "PartySize: " + members.Count;
            foreach (var member in members)
            {
                party+= member.ToString()+",\n";
            }
            party += "Gold: " + money+"\n";
            party += Convoy.ToString()+"\n";
            party += "EncounterNode: "+EncounterComponent.EncounterNodeId+"\n";
            party += "ActiveUnit: "+ActiveUnitIndex+"\n";
            party += "Moved Encounters: ";
            foreach (var node in EncounterComponent.MovedEncounterIds)
            {
                party+= node.ToString()+", ";
            }
            return party;
        }

        public void AddMember(Unit unit)
        {
            if (IsFull())
            {
                Debug.LogError("Party Size To Big");
                return;
            }

            unit.Party = this;
            members.Add(unit);
            onMemberAdded?.Invoke(unit);
        }

        public void AddGold(int gold)
        {
            Money += gold;
        }

        public void AddSmithingStones(int smithingStones)
        {
            SmithingStones += smithingStones;
        }

        public void RemoveMember(Unit unit)
        {
            unit.Party = null;
            members.Remove(unit);
            onMemberRemoved?.Invoke(unit);
        }

        public bool IsFull()
        {
            return members.Count >= maxSize;
        }

        public void Load(PartyData playerDataPartyData)
        {
            money = playerDataPartyData.money;
            members = new List<Unit>();

            foreach (var data in playerDataPartyData.humanData)
            {
                Unit unit=data.Load();
                Debug.Log("Load UnitData!"+unit.name);
                unit.Initialize();
                members.Add(unit);
            }
            
            Convoy = playerDataPartyData.convoy;
            ActiveUnitIndex = activeUnitIndex;
            EncounterComponent = new EncounterPosition
            {
                EncounterNodeId = playerDataPartyData.currentEncounterNodeId,
                MovedEncounterIds = playerDataPartyData.movedEncounterIds
            };
        }
    }
}