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
        [SerializeField] public List<Unit> members;
        public static Action<Party> PartyDied;
        public static int MaxSize = 4;
        [SerializeField] public List<Item> convoy = default;
        [SerializeField] private int money = default;

        public int ActiveUnitIndex = 0;
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
            convoy = new List<Item>();
            MovedEncounters = new List<EncounterNode>();
        }

        public EncounterNode EncounterNode { get; set; }
        public GameObject GameObject { get; set; }
        public List<EncounterNode> MovedEncounters { get; set; }

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
    }
}