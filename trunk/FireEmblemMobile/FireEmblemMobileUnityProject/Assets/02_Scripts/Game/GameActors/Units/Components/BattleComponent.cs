using Game.GameActors.Items.Weapons;
using Game.GameInput;
using Game.Mechanics;
using Game.Mechanics.Battle;
using UnityEngine;

namespace Game.GameActors.Units
{
    public class BattleComponent
    {
        public BattleStats BattleStats { get;  set; }

        private readonly IBattleActor owner;
        public BattleComponent(IBattleActor actor)
        {
             BattleStats = new BattleStats(actor);
             owner = actor;

        }

      
        // public int InflictDamage(int dmg,bool magic, bool crit,bool eff, IBattleActor damageDealer)
        // {
        //     //crit = Random.Range(0, 100) <= 50;
        //     if(BattleActor is Unit unit)
        //         Unit.OnUnitDamaged?.Invoke(unit, dmg,magic, crit, eff);
        //     BattleActor.Hp -= dmg;
        //     return dmg;
        // }

        public bool IsInEffective(MoveType unitMoveType)
        {
            return owner.GetEquippedWeapon().IsInEffective(unitMoveType);
        }
        public bool IsEffective(MoveType unitMoveType)
        {
            return owner.GetEquippedWeapon().IsEffective(unitMoveType);
        }
        public bool IsInEffective(WeaponType weaponType)
        {
            return owner.GetEquippedWeapon().IsInEffective(weaponType);
        }
        public bool IsEffective(WeaponType weaponType)
        {
            return owner.GetEquippedWeapon().IsEffective(weaponType);
        }
    }
}