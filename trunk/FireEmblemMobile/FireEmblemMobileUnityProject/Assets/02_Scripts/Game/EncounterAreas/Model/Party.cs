using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using _02_Scripts.Game.Dialog.DialogSystem;
using Game.GameActors.Items;
using Game.GameActors.Items.Gems;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.OnGameObject;
using Game.GameResources;
using LostGrace;
using UnityEngine;

namespace Game.WorldMapStuff.Model
{
    [System.Serializable]
    public class Party
    {

        public event Action<int> onGoldChanged;
        public event Action<int> onGraceChanged;
        [field:NonSerialized] public List<Unit> members;
        [field:NonSerialized] public List<Unit> deadMembers;

        public List<LGEventDialogSO> VisitedEvents;
        public List<BattleMap> VisitedMaps;
        public static Action<Party> PartyDied;
        [SerializeField] int maxSize = 4;
        [SerializeField] private int money = default;
        [SerializeField] private int collectedGrace = default;

        public Morality Morality;
        public int MaxSize
        {
            get => maxSize;
            set => maxSize = value;
        }

        public Convoy Convoy;
        public Convoy Storage;
        int activeUnitIndex = 0;
        public EncounterPosition EncounterComponent { get; set; }

        public GameObject GameObject { get; set; }
        public int AreaIndex { get; set; }
        public int ActiveUnitIndex
        {
            get { return activeUnitIndex; }
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

                onActiveUnitChanged?.Invoke();
            }
        }

        public event Action onActiveUnitChanged;
        public event Action<Unit> onMemberRemoved;
        public event Action<Unit> onMemberAdded;

        public int Money
        {
            get { return money; }
            set
            {
                if (value == money)
                    return;
                money = value;
                if (money <= 0)
                    money = 0;
                onGoldChanged?.Invoke(money);
            }
        }

        public int CollectedGrace
        {
            get { return collectedGrace; }
            set
            {
                if (value == collectedGrace)
                    return;
                collectedGrace = value;
                if (collectedGrace <= 0)
                    collectedGrace = 0;
                onGraceChanged?.Invoke(collectedGrace);
            }
        }

        public Party()
        {
            members = new List<Unit>();
            deadMembers = new List<Unit>();
            Convoy = new Convoy();
            Storage = new Convoy();
            EncounterComponent = new EncounterPosition();
            money = 1000;
            Morality = new Morality();
            VisitedEvents = new List<LGEventDialogSO>();
            VisitedMaps = new List<BattleMap>();

        }

        


        public Unit ActiveUnit
        {
            get
            {
                if(activeUnitIndex<members.Count)
                    return members[ActiveUnitIndex];
                return null;
            }
        }
        
       


        public void SetActiveUnit(Unit unit)
        {
            ActiveUnitIndex = members.IndexOf(unit);
        }
        private void OnEnable()
        {
            for (int i = members.Count - 1; i >= 0; i--)
            {
                if (members[i] == null)
                    members.RemoveAt(i);
            }
        }

        public bool IsAlive()
        {
            return members.Count(a => a.IsAlive()) != 0;
        }

        public void PartyMemberDied(Unit member)
        {
            var tempUnit = ActiveUnit;
            if (ActiveUnit.Equals(member))
            {
                if (ActiveUnitIndex != 0)
                    tempUnit = members[0];
                else if (members.Count >= 2)
                    tempUnit = members[1];
                else tempUnit = null;
            }
          
            
            members.Remove(member);
            deadMembers.Add(member);
            for (int i = 0; i < members.Count; i++)
            {
                if (members[i].Equals(tempUnit))
                {
                    bool change = ActiveUnitIndex == i;
                    ActiveUnitIndex = i;
                    if(change)
                        onActiveUnitChanged?.Invoke();
                  
                    break;
                }
            }
        }
        
        public void Initialize()
        {
            foreach (var member in members)
            {
                InitMember(member);
            }

            AreaIndex = 1;
            Unit.UnitDied -= PartyMemberDied;
            Unit.UnitDied += PartyMemberDied;
            Unit.OnUnequippedRelic -= AddItem;
            Unit.OnUnequippedRelic += AddItem;
            Unit.OnUnequippedCombatItem -= Convoy.AddItem;
            Unit.OnUnequippedCombatItem += Convoy.AddItem;
            Unit.OnEquippedCombatItem -= Convoy.RemoveItem;
            Unit.OnEquippedCombatItem += Convoy.RemoveItem;
            Unit.OnEquippedRelic -=  RemoveItem;
            Unit.OnEquippedRelic += RemoveItem;

        }

        public override string ToString()
        {
            string party = "";

            party += "PartySize: " + members.Count;
            foreach (var member in members)
            {
                party += member.ToString() + ",\n";
            }

            party += "Gold: " + money + "\n";
            party += Convoy.ToString() + "\n";
            party += "EncounterNode: " + EncounterComponent.EncounterNodeId + "\n";
            party += "ActiveUnit: " + ActiveUnitIndex + "\n";
            party += "Moved Encounters: ";
            foreach (var node in EncounterComponent.MovedEncounterIds)
            {
                party += node.ToString() + ", ";
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
            Debug.Log("Before Inner Init");
            InitMember(unit);
            Debug.Log("Before Inner Add");
            members.Add(unit);
            if (ActiveUnitIndex <0)
                ActiveUnitIndex = 0;
            Debug.Log("Before Inner Event");
            Debug.Log(members.Count+" "+activeUnitIndex);
            Debug.Log(ActiveUnit);
            
            onMemberAdded?.Invoke(unit);
        }

        void InitMember(Unit unit)
        {
            unit.Party = this;
            Debug.Log("INIT UNIT "+unit.name);
            unit.SkillManager.RefreshSkills();
        }

        public void AddGold(int gold)
        {

            Money += gold;
        }

        public void AddGrace(int grace)
        {
            CollectedGrace += grace;
        }
       

        public void RemoveMember(Unit unit)
        {
            int rmvIndex = members.IndexOf(unit);
            Debug.Log("Remove Member: "+unit+" "+rmvIndex+" ActiveIndex: "+ActiveUnitIndex);
            if (rmvIndex <= ActiveUnitIndex&&ActiveUnitIndex>=1)
                ActiveUnitIndex--;
            Debug.Log(" ActiveIndexAfterRemoving: "+ActiveUnitIndex);
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
            Debug.Log("Load Party Data");
            money = playerDataPartyData.money;
            members = new List<Unit>();
            maxSize = playerDataPartyData.maxSize;
            collectedGrace = playerDataPartyData.collectedGrace;
            activeUnitIndex = playerDataPartyData.activeUnitIndex;
            foreach (var data in playerDataPartyData.humanData)
            {
                Unit unit = data.Load();
                if(!data.dead)
                    members.Add(unit);
                else
                {
                    deadMembers.Add(unit);
                }
            }

            Convoy = new Convoy();
            Convoy = playerDataPartyData.convoy.LoadData();
            Storage = new Convoy();
            Storage = playerDataPartyData.storage.LoadData();
            VisitedEvents = new List<LGEventDialogSO>();
            foreach (var id in playerDataPartyData.visitedEvents)
            {
                VisitedEvents.Add(GameBPData.Instance.GetEventData().GetEventById(id));
            }
            VisitedMaps = new List<BattleMap>();
            foreach (var id in playerDataPartyData.visitedEvents)
            {
                VisitedMaps.Add(GameBPData.Instance.GetMapById(id));
            }
            ActiveUnitIndex = activeUnitIndex;
            EncounterComponent = new EncounterPosition
            {
                activatedEncounter = playerDataPartyData.activatedEncounter,
                EncounterNodeId = playerDataPartyData.currentEncounterNodeId,
                MovedEncounterIds = playerDataPartyData.movedEncounterIds
            };
            AreaIndex = playerDataPartyData.areaIndex;
            Morality.Set(playerDataPartyData.morality);

        }

        public bool CanAfford(int price)
        {
            return Money >= price;
        }
        public bool CanAfford(Item item)
        {
            return Convoy.ContainsItem(item);
        }
        public bool CanAfford(ResourceType resourceType, int amount)
        {
            switch (resourceType)
            {
                case ResourceType.Gold:
                    return CanAfford(amount);
                case ResourceType.Grace:
                    return CollectedGrace>= amount;
                case ResourceType.Morality:
                    return true;
                case ResourceType.HP_Percent:
                    return ActiveUnit.Hp> ActiveUnit.MaxHp * (amount/100f);
            }

            return false;
        }

        public void EncounterTick()
        {
            foreach (var member in members)
            {
                member.EncounterTick();
            }
        }

        public void ReviveCharacter(Unit unitToRevive)
        {
            throw new NotImplementedException();
        }


        public bool MembersContainsByBluePrintID(string reqBluePrintID)
        {
            foreach (var member in members)
            {
                if (member.bluePrintID == reqBluePrintID)
                {
                    return true;
                }
            }

            return false;
        }

        public Unit GetMembersContainsBluePrintID(string reqBluePrintID)
        {
            foreach (var member in members)
            {
                if (member.bluePrintID == reqBluePrintID)
                {
                    return member;
                }

            }

            return null;
        }


        public void ResetFoodBuffs()
        {
            foreach (var member in members)
            {
                member.Stats.BonusAttributesFromFood.Clear();
            }
        }

        public void AddStockedItem(StockedItem stockedItem)
        {
            if (stockedItem.item is Gem || stockedItem.item is Stone)
            {
                Storage.AddItem(stockedItem);
            }
            else
            {
                Convoy.AddItem(stockedItem);
            }

        }

        public void RemoveItem(Item item)
        {
            if (item is Gem || item is Stone)
            {
                Storage.RemoveItem(item);
            }
            else
            {
                Convoy.RemoveItem(item);
            }
        }

        public void AddItem(Item item)
        {
            if (item is Gem || item is Stone)
            {
                Storage.AddItem(item);
            }
            else
            {
                Convoy.AddItem(item);
            }
        }

        public Unit GetUnitByName(string characterRequirementName)
        {
            foreach (var member in members)
            {
                if (member.Name == characterRequirementName)
                    return member;
            }

            return null;
        }

        public bool CanReceiveBlessing(Unit unit, God god)
        {
            if (unit.CanReceiveBlessing(god))
            {
                foreach (var member in members)
                {
                    if (member.Blessing != null && member.Blessing.God == god)
                        return false;
                }

                return true;
            }

            return false;
           
        }

        public bool HasVisitedEvent(LGEventDialogSO randomEvent)
        {
            return VisitedEvents.Contains(randomEvent);
        }

        public bool HasVisitedMap(BattleMap battleMap)
        {
            return VisitedMaps.Contains(battleMap);
        }
    }
}