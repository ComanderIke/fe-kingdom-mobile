using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;

namespace Game.GameActors.Players
{
    [System.Serializable]
    public class HumanData: UnitData
    {
        public Weapon EquippedWeapon;
        public HumanData(Human unit) : base(unit)
        {
            EquippedWeapon = unit.EquippedWeapon;
        }

        public override void Load(Unit unit)
        {
            base.Load(unit);
            Human human = (Human) unit;
            human.EquippedWeapon = EquippedWeapon;
        }
    }
}