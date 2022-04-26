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

        public override Unit Load()
        {
            Human human = (Human)base.Load();
          
           

            // Debug.LogError("Loaded Weapon: "+GameData.Instance.Weapons.FirstOrDefault(w => w.name == weaponId).name);
            human.EquippedWeapon = GameData.Instance.GetWeapon(weaponId);
            return human;
        }
    }
}