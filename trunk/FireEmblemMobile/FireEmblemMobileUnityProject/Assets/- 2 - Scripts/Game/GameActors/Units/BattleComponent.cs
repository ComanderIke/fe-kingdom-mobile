using Game.GameInput;
using Game.Mechanics;
using Game.Mechanics.Battle;
using UnityEngine;

namespace Game.GameActors.Units
{
    public class BattleComponent
    {
        public BattleStats BattleStats { get;  set; }

        
        public BattleComponent(IBattleActor actor)
        {
             BattleStats = new BattleStats(actor);
            
        }

      
        // public int InflictDamage(int dmg,bool magic, bool crit,bool eff, IBattleActor damageDealer)
        // {
        //     //crit = Random.Range(0, 100) <= 50;
        //     if(BattleActor is Unit unit)
        //         Unit.OnUnitDamaged?.Invoke(unit, dmg,magic, crit, eff);
        //     BattleActor.Hp -= dmg;
        //     return dmg;
        // }

        
    }
}