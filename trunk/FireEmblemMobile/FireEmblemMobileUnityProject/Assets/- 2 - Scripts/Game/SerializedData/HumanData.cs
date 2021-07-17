using System.Linq;
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
            base.Load(unit);
          
            Human human = (Human) unit;
            //Debug.Log("LOAD222222: "+unit.name+" "+unit.ExperienceManager.Exp);
            human.EquippedWeapon = GameData.Instance.Weapons.FirstOrDefault(w=>w.name == weaponId);
        }
    }
}