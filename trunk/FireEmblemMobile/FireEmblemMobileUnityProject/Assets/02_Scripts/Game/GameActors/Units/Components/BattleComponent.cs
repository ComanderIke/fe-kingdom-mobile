using System;
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

        public bool IsEffective(EffectType effect)
        {
            return owner.GetEquippedWeapon().IsEffective(effect);
        }
        public float GetEffectiveCoefficient(EffectType effect)
        {
            return owner.GetEquippedWeapon().GetEffectiveCoefficient(effect);
        }

        public event Action<Unit> OnEndCombat;
        public event Action<Unit> OnInitiateCombat;
        public event Action<Unit> OnCombat;
        public event Action<Unit> OnAttack;
    }
}