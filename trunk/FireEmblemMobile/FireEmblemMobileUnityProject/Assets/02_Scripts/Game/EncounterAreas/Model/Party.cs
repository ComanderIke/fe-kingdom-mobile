using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Items;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using UnityEngine;

namespace Game.WorldMapStuff.Model
{
    [CreateAssetMenu(fileName = "Party", menuName = "GameData/Party")]
    public class Party:ScriptableObject
    {

        public event Action<int> onGoldChanged;
        public event Action<int> onSmithingStonesChanged;
        
        [SerializeField] public List<Unit> members;
        public static Action<Party> PartyDied;
        public static int MaxSize = 4;
        
        [SerializeField] public int money = default;

    

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
        public event Action<Unit> onAddUnit;

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
            MovedEncounters = new List<EncounterNode>();
        }

        public EncounterNode EncounterNode { get; set; }
        public GameObject GameObject { get; set; }
        public List<EncounterNode> MovedEncounters { get; set; }

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
            if(EncounterNode!=null)
                party += "EncounterNode: "+EncounterNode.ToString()+"\n";
            party += "ActiveUnit: "+ActiveUnitIndex+"\n";
            party += "Moved Encounters: ";
            foreach (var node in MovedEncounters)
            {
                party+= node.ToString()+", ";
            }
            return party;
        }

        public void AddMember(Unit unit)
        {
            members.Add(unit);
            onAddUnit?.Invoke(unit);
        }


        
    }
}