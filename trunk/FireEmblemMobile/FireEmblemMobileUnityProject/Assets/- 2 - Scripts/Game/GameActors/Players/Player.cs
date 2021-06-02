using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Units;
using Game.GameActors.Units.Attributes;
using Game.GameResources;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Players
{

    [System.Serializable]
    public class Player
    {
        private static Player _instance;
        public static Player Instance
        {
            get { return _instance ??= new Player(); }
        }

        [SerializeField] private int playerId = default;
        public Faction faction;
        public string Name;
        [SerializeField] public List<Item> convoy = default;
        [SerializeField] public int money = default;

        public Player()
        {
            convoy = new List<Item>();
            Name = "Player1";
            playerId = 0;
            faction = new WM_Faction(playerId,Name,true);
        
        }

        public PlayerData GetSaveData()
        {
            var playerData = new PlayerData(this);
            return playerData;
        }

        public void LoadPlayer(Player player)
        {
            _instance = player;
        }
    }

    public class PlayerData
    {
        public string Name;
        public FactionData factionData;
        public int money;
        public List<Item> convoy;
        public PlayerData (Player player)
        {
            money = player.money;
            convoy = player.convoy;
            factionData = new FactionData((WM_Faction)player.faction);

        }
    }

    public class FactionData
    {
        public int factionId;
        public string Name;
        public List<PartyData> Parties;
        
        public FactionData(WM_Faction faction)
        {
            factionId=faction.Id;
            Name = faction.Name;
            Parties = new List<PartyData>();
            foreach (var party in faction.Parties)
            {
                Parties.Add(new PartyData(party));
            }
        }
    }

    public class PartyData
    {
        public string name;
        public List<UnitData> unitData;
        public PartyData(Party party)
        {
            name = party.name;
            unitData = new List<UnitData>();
            foreach (var member in party.members)
            {
                unitData.Add(new UnitData(member));
            }
        }
    }

    public class UnitData
    {
        public string name;
        [SerializeField]
        private Stats stats;
        [SerializeField]
        private Growths growths;
        [SerializeField]
        private MoveType moveType;

        public ExperienceManager ExperienceManager;
        public TurnStateManager TurnStateManager { get; set; }
        public int VisualsID;
        public UnitData(Unit unit)
        {
            this.name = unit.name;
            ExperienceManager = unit.ExperienceManager;
            TurnStateManager = unit.TurnStateManager;
            this.moveType = unit.MoveType;
            this.growths = unit.Growths;
            this.stats = unit.Stats;
            VisualsID = unit.visuals.GetId();
        }
    }
}
