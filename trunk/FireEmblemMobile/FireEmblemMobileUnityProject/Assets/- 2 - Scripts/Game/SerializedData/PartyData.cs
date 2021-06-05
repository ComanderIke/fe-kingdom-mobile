﻿using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Monsters;
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

        public void Load(Party party)
        {
            party.name = name;
            party.members = new List<Unit>();
            foreach (var data in unitData)
            {
                if (data is HumanData)
                {
                    var unit = new Human();
                    data.Load(unit);
                    party.members.Add(unit);
                }
                else if (data is MonsterData)
                {
                    var unit = new Monster();
                    data.Load(unit);
                    party.members.Add(unit);
                }
             
               
            }
        }
    }
}