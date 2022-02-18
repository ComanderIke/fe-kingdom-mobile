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
        [SerializeField] private List<StockedItem> convoy;
        [SerializeField] public int money = default;

        public event Action convoyUpdated;
        public List<StockedItem> Convoy
        {
            get
            {
                return convoy;
            }
        }
        public void AddItem(Item item)
        {
            bool instock = false;
            foreach (var stockedItem in convoy)
            {
                if (stockedItem.item == item)
                {
                    instock = true;
                    stockedItem.stock++;
                    break;
                }
            }
            if(!instock)
                convoy.Add(new StockedItem(item, 1));
            convoyUpdated?.Invoke();
        }
        public void RemoveItem(Item item)
        {
            StockedItem removeItem=null;
            foreach (var stockedItem in convoy)
            {
                if (stockedItem.item == item)
                {
                    stockedItem.stock--;
                    if (stockedItem.stock <= 0)
                        removeItem = stockedItem;
                    break;

                }
            }

            if (removeItem != null)
                convoy.Remove(removeItem);
            convoyUpdated?.Invoke();
           
        }
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
            convoy = new List<StockedItem>();
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
    }
}