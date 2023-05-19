﻿using System;
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
    [System.Serializable]
    public class Party
    {

        public event Action<int> onGoldChanged;
        public event Action<int> onGraceChanged;
        [field:NonSerialized] public List<Unit> members;

        public static Action<Party> PartyDied;
        [SerializeField] int maxSize = 4;
        [SerializeField] private int money = default;
        [SerializeField] private int collectedGrace = default;
   
        public int MaxSize
        {
            get => maxSize;
            set => maxSize = value;
        }

        public Convoy Convoy;
        int activeUnitIndex = 0;

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
            Convoy = new Convoy();
            EncounterComponent = new EncounterPosition();
            DeadCharacters = new List<Unit>();
            money = 1000;
            
        }

        public EncounterPosition EncounterComponent { get; set; }

        public GameObject GameObject { get; set; }


        public Unit ActiveUnit
        {
            get { return members[ActiveUnitIndex]; }
        }

        public List<Unit> DeadCharacters { get; set; }


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

        
        public void Initialize()
        {
            foreach (var member in members)
            {
                InitMember(member);
            }
            Unit.OnUnequippedRelic -= Convoy.AddItem;
            Unit.OnUnequippedRelic += Convoy.AddItem;
            Unit.OnEquippedRelic -=  Convoy.RemoveItem;
            Unit.OnEquippedRelic += Convoy.RemoveItem;

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
            InitMember(unit);
            members.Add(unit);
            onMemberAdded?.Invoke(unit);
        }

        void InitMember(Unit unit)
        {
            unit.Party = this;

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

            foreach (var data in playerDataPartyData.humanData)
            {
                Unit unit = data.Load();
                members.Add(unit);
            }

            Convoy = new Convoy();
            Convoy = playerDataPartyData.convoy.LoadData();
            ActiveUnitIndex = activeUnitIndex;
            EncounterComponent = new EncounterPosition
            {
                activatedEncounter = playerDataPartyData.activatedEncounter,
                EncounterNodeId = playerDataPartyData.currentEncounterNodeId,
                MovedEncounterIds = playerDataPartyData.movedEncounterIds
            };
        }

        public bool CanAfford(int price)
        {
            return Money >= price;
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
    }
}