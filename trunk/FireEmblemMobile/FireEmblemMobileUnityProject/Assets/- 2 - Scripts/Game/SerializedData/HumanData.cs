﻿using System.Linq;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameResources;
using UnityEngine;

namespace Game.GameActors.Players
{
    [System.Serializable]
    public class HumanData: UnitData
    {
        [SerializeField]
        public string weaponId;
        public HumanData(Human unit) : base(unit)
        {
            weaponId = unit.EquippedWeapon.name;
        }

        public override void Load(Unit unit)
        {
            Debug.Log("Load Human!");
            base.Load(unit);
            Human human = (Human) unit;
            human.EquippedWeapon = GameData.Instance.Weapons.FirstOrDefault(w=>w.name == weaponId);
        }
    }
}