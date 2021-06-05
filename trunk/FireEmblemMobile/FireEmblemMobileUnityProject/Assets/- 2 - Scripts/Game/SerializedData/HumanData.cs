﻿using System.Linq;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameResources;

namespace Game.GameActors.Players
{
    [System.Serializable]
    public class HumanData: UnitData
    {
        public int weaponId;
        public HumanData(Human unit) : base(unit)
        {
            weaponId = unit.EquippedWeapon.id;
        }

        public override void Load(Unit unit)
        {
            base.Load(unit);
            Human human = (Human) unit;
            human.EquippedWeapon = GameData.Instance.Weapons.FirstOrDefault(w=>w.id == weaponId);
        }
    }
}